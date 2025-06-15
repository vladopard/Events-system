using System.Reflection.Emit;
using Events_system.BusinessServices;
using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.DbContexts;
using Events_system.Entities;
using Events_system.Helpers;
using Events_system.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<EventDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddDbContext<EventDbContext>(options => 
//    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpClient<ITickermasterService, TicketmasterService>();

builder.Services.AddScoped<ISystemRepository, SystemRepository>();
builder.Services.AddScoped<IEventService, EventService>();
builder.Services.AddScoped<ITicketService, TicketService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IQueueService, QueueService>();

builder.Services.AddIdentity<User, IdentityRole>(opts =>
{
    opts.Password.RequireDigit = true;
    opts.Password.RequiredLength = 6;
    opts.Password.RequireNonAlphanumeric = false;
    opts.User.RequireUniqueEmail = true;
})
    .AddEntityFrameworkStores<EventDbContext>()
    .AddDefaultTokenProviders();

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

    await dbContext.Database.MigrateAsync();

    await sp.EnsureSeedDataAsync();


    if (!dbContext.Events.Any())
    {
        var tmService = scope.ServiceProvider.GetRequiredService<ITickermasterService>();
        var events = await tmService.FetchEventsAsync();

        dbContext.Events.AddRange(events);
        await dbContext.SaveChangesAsync();
    }

    if (!dbContext.Tickets.Any())
    {
        var ticketTypes = await dbContext.TicketTypes.ToDictionaryAsync(t => t.Id); // 1: Regular, 2: VIP …
        foreach (var ev in await dbContext.Events.ToListAsync())
        {
            dbContext.Tickets.Add(new Ticket
            {
                Seat = $"A{ev.Id}",
                EventId = ev.Id,
                TicketTypeId = ticketTypes[1].Id          // Regular
            });
            dbContext.Tickets.Add(new Ticket
            {
                Seat = $"B{ev.Id}",
                EventId = ev.Id,
                TicketTypeId = ticketTypes[2].Id          // VIP
            });
        }
        await dbContext.SaveChangesAsync();
    }

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

app.UseAuthorization();

app.MapControllers();

app.Run();
