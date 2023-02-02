using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Task.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;

namespace Task.Interfaces
{
    public  interface ITokenService
    {
        
        SecurityToken GetToken(List<Claim> claims);
        TokenValidationParameters GetTokenValidationParameters();
        string WriteToken(SecurityToken token) ;
    }
}