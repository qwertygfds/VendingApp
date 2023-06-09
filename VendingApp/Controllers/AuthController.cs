using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using VendingApp.Domain;
using VendingApp.Infrastructure;
using VendingApp.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using VendingApp.Model.Models;
using VendingApp.Extensions;

namespace VendingApp.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly int DaysForRefreshToken;
        private readonly int HoursForToken;

        public AuthController(ILogger<AuthController> logger,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            DaysForRefreshToken = int.TryParse(_configuration["DaysForRefreshToken"], out int refreshToken)
                ? refreshToken
                : 7;
            HoursForToken = int.TryParse(_configuration["HoursForToken"], out int token)
                ? token
                : 24;
        }


        [HttpPost("/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel data)
        {
            ApplicationUser? user = await _userManager.FindByNameAsync(data.UserName ?? "");
            if (user == null || !await _userManager.CheckPasswordAsync(user, data.Password ?? ""))
            {
                return Unauthorized("Введён неверный логин/пароль");
            }

            List<Claim>? authClaims = new()
            {
                new Claim(ClaimTypes.Name, user.UserName ?? ""),
                new Claim("UserId", user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("FullName", user.FullName ?? ""),
            };

            IList<string> roles = await _userManager.GetRolesAsync(user);
            foreach (string userRole in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                authClaims.Add(new Claim("Role", userRole));
            }

            JwtSecurityToken? token = GetToken(authClaims, DateTime.UtcNow.AddHours(HoursForToken));

            string userRefreshToken = GenerateRefreshToken();
            user.RefreshToken = userRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(DaysForRefreshToken);
            await _userManager.UpdateAsync(user);

            Console.WriteLine($"User {data.UserName} logged in");
            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token),
                refreshToken = userRefreshToken,
                expiration = token.ValidTo,
            });
        }


        [HttpPost("/refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel == null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;
            if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
            {
                return BadRequest("Invalid access token or refresh token (cannot be null or empty)");
            }

            ClaimsPrincipal? principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token");
            }

            string? username = principal.Identity?.Name;
            if (username == null)
            {
                return BadRequest("User not found in token");
            }

            ApplicationUser? user = await _userManager.FindByNameAsync(username);
            if (user == null)
            {
                return BadRequest("User not found in database");
            }
            if (user.RefreshToken != refreshToken)
            {
                return BadRequest("Invalid refresh token for user");
            }
            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return BadRequest("Refresh token is expired");
            }


            JwtSecurityToken? newAccessToken = GetToken(principal.Claims.ToList(),
                                                        DateTime.UtcNow.AddHours(HoursForToken));

            string? newRefreshToken = GenerateRefreshToken();
            user.RefreshToken = newRefreshToken;
            _ = await _userManager.UpdateAsync(user);

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken,
                expiration = newAccessToken.ValidTo,
            });
        }


        #region privates

        private JwtSecurityToken GetToken(List<Claim> authClaims, DateTime validTo)
        {
            SymmetricSecurityKey? authSigningKey
                = new(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            JwtSecurityToken? token = new(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: validTo,
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey,
                                                           SecurityAlgorithms.HmacSha256));

            return token;
        }

        private static string GenerateRefreshToken()
        {
            byte[]? randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            TokenValidationParameters? tokenValidationParameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                    .GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler? tokenHandler = new();
            ClaimsPrincipal? principal = tokenHandler
                .ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);

            return securityToken is not JwtSecurityToken jwtSecurityToken
                    || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase)
                ? null
                : principal;
        }

        #endregion
    }
}
