using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Events_system.BusinessServices.BusinessInterfaces;
using Events_system.DTOs;
using Events_system.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Events_system.BusinessServices
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userMgr;
        private readonly SignInManager<User> _signInMgr;
        private readonly RoleManager<IdentityRole> _roleMgr;
        private readonly IConfiguration _cfg;

        public AuthenticationService(
            UserManager<User> userMgr,
            SignInManager<User> signInMgr,
            RoleManager<IdentityRole> roleMgr,
            IConfiguration cfg)
        {
            _userMgr = userMgr;
            _signInMgr = signInMgr;
            _roleMgr = roleMgr;
            _cfg = cfg;
        }

        // ---------- REGISTER ----------
        public async Task<AuthResultDTO> RegisterAsync(AuthRegisterDTO dto)
        {
            var existingUser = await _userMgr.FindByEmailAsync(dto.Email);
            if (existingUser != null)
                return Fail("E-mail already registered.");

            var user = new User
            {
                UserName = dto.Email,
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DateJoined = DateTime.UtcNow
            };

            var createRes = await _userMgr.CreateAsync(user, dto.Password);
            if (!createRes.Succeeded)
                return Fail(createRes.Errors.Select(e => e.Description));

            // default улога – Customer
            await EnsureRoleExistsAsync("Customer");

            var roleRes = await _userMgr.AddToRoleAsync(user, "Customer");
            if (!roleRes.Succeeded)
                return Fail(roleRes.Errors.Select(e => e.Description));

            return await SuccessAsync(user);

        }

        // ---------- LOGIN ----------
        public async Task<AuthResultDTO> LoginAsync(AuthLoginDTO dto)
        {
            var user = await _userMgr.FindByEmailAsync(dto.Email);
            if (user == null)
                return Fail("Bad credentials.");

            var pwRes = await _signInMgr.CheckPasswordSignInAsync(user, dto.Password, false);
            if (!pwRes.Succeeded)
                return Fail("Bad credentials.");

            return await SuccessAsync(user);
        }

        // ---------- HELPERS ----------
        private async Task<AuthResultDTO> SuccessAsync(User user)
        {
            var roles = await _userMgr.GetRolesAsync(user);
            var token = GenerateJwt(user, roles);

            return new AuthResultDTO
            {
                IsSuccess = true,
                Token = token.TokenString,
                ExpiresAt = token.ExpiresAt,
                Roles = roles
            };
        }

        private AuthToken GenerateJwt(User user, IEnumerable<string> roles)
        {
            var key = Encoding.UTF8.GetBytes(_cfg["Jwt:Key"]!);
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id),
                new(JwtRegisteredClaimNames.Email, user.Email!),
                new("firstName", user.FirstName ?? "")
            };
            claims.AddRange(roles.Select(r => new Claim(ClaimTypes.Role, r)));

            var expires = DateTime.UtcNow.AddMinutes(int.Parse(_cfg["Jwt:ExpiresInMinutes"]!));

            var jwt = new JwtSecurityToken(
                issuer: _cfg["Jwt:Issuer"],
                audience: _cfg["Jwt:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            var tokenStr = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new AuthToken(tokenStr, expires);
        }

        private AuthResultDTO Fail(IEnumerable<string> errors)
            => new() { IsSuccess = false, Errors = errors };

        private AuthResultDTO Fail(string error)
            => Fail(new[] { error });

        private async Task EnsureRoleExistsAsync(string roleName)
        {
            IdentityResult res = IdentityResult.Success;

            if (!await _roleMgr.RoleExistsAsync(roleName))
                res = await _roleMgr.CreateAsync(new IdentityRole(roleName));

            if (!res.Succeeded)
                throw new Exception("Role creation failed: " +
                    string.Join(", ", res.Errors.Select(e => e.Description)));
        }

        private record AuthToken(string TokenString, DateTimeOffset ExpiresAt);
    }
}

