using BlazorApp2.Models;
using BlazorApp2.Models.Responses;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BlazorApp2.Services.TokenValidators
{
    public class TokenValidator
    {
        private readonly AuthenticationConfiguration _configuration;

        public TokenValidator(AuthenticationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public SecurityToken Validate(string token)
        {
            TokenValidationParameters validationParameters = new TokenValidationParameters()
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.AccessTokenSecret)),
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration.Issuer,
                ValidateIssuer = true,
                ValidAudience = _configuration.Audience,
                ValidateAudience = true,
                ClockSkew = TimeSpan.Zero
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, validationParameters,out SecurityToken validatedToken);
            return validatedToken;
        }

    }
}
