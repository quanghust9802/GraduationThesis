﻿using Domain.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.AuthProvide
{
    public sealed class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public static TokenValidationParameters GetTokenValidationParameters(IConfiguration configuration) =>
            new()
            {
                ValidateIssuer = true,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                IssuerSigningKey = GetSecurityKey(configuration)
            };

        public string GenerateJWT(IEnumerable<Claim>? additionalClaims = null)
        {
            var securityKey = GetSecurityKey(_configuration);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var expireInMinutes = Convert.ToInt32(_configuration["Jwt:ExpireMinutes"]);

            var claims = new List<Claim> {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            if (additionalClaims?.Any() == true)
                claims.AddRange(additionalClaims!);

            var token = new JwtSecurityToken(issuer: _configuration["Jwt:Issuer"],
                audience: "*",
              claims: claims,
              expires: DateTime.Now.AddMinutes(expireInMinutes),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string GenerateJWTWithUser(User user, IEnumerable<Claim>? additionalClaims = null)
        {
            var claims = new List<Claim>
                {
                    new("UserId", user.Id.ToString()),
                    new("Username", user.Username),
                    new("Address", user.Address.ToString()),
                    new(ClaimTypes.Role, user.UserRole.RoleName),
                    new ("ModifyAt", user.ModifiedAt.ToString()!)
                };
            if (additionalClaims?.Any() == true)
                claims.AddRange(additionalClaims!);

            return GenerateJWT(claims);
        }


        private static SymmetricSecurityKey GetSecurityKey(IConfiguration _configuration) =>
            new(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));

    }

}
