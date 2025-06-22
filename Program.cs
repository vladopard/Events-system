using System.Reflection.Emit;
using System.Text;
using Events_system.BusinessServices;
using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.DbContexts;
using Events_system.Entities;
using Events_system.Helpers;
using Events_system.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EventDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<ITickermasterService, TicketmasterService>();

builder.Services.AddScoped<ISystemRepository, SystemRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<ITicketTypeService, TicketTypeService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IQueueService, QueueService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

//IDentity
builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.Password.RequireDigit = true;
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = false;
    opts.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<EventDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});

builder.Services.AddAuthorization();

builder.Services.AddControllers(configure =>
{
    configure.ReturnHttpNotAcceptable = true;
})
    .AddNewtonsoftJson(setupAction =>
    {
        setupAction.SerializerSettings.ContractResolver =
        new CamelCasePropertyNamesContractResolver();
    });
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(
    AppDomain.CurrentDomain.GetAssemblies());

//FLUENT
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var sp = scope.ServiceProvider;
    var dbContext = sp.GetRequiredService<EventDbContext>();

    // 1) миграције
    await dbContext.Database.MigrateAsync();

    // 2) SEED → улоге + Admin + Customer
    await sp.EnsureSeedDataAsync();   // <<– померено најгоре

    // 3) SEED Business података – Events, TicketTypes, Tickets
    if (!dbContext.Events.Any())
    {
        var tmService = sp.GetRequiredService<ITickermasterService>();
        var events = await tmService.FetchEventsAsync();

        dbContext.Events.AddRange(events);
        await dbContext.SaveChangesAsync();

        foreach (var ev in events)
        {
            dbContext.TicketTypes.AddRange(
                new TicketType { Name = "Regular", Price = 50, EventId = ev.Id },
                new TicketType { Name = "VIP", Price = 120, EventId = ev.Id }
            );
        }
        await dbContext.SaveChangesAsync();
    }

    if (!dbContext.Tickets.Any())
    {
        var allEvents = await dbContext.Events
                                       .Include(e => e.TicketTypes)
                                       .ToListAsync();

        foreach (var ev in allEvents)
        {
            var regular = ev.TicketTypes.First(tt => tt.Name == "Regular");
            var vip = ev.TicketTypes.First(tt => tt.Name == "VIP");

            dbContext.Tickets.AddRange(
                new Ticket { Seat = $"A{ev.Id}", EventId = ev.Id, TicketTypeId = regular.Id },
                new Ticket { Seat = $"B{ev.Id}", EventId = ev.Id, TicketTypeId = vip.Id }
            );
        }
        await dbContext.SaveChangesAsync();
    }

    // BusinessSeed (Orders/Queues) – ако га имаш
    await sp.EnsureAsync();
}


app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
