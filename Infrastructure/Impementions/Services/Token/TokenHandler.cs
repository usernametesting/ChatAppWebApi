﻿using Application.DTOs.Tokens;
using Domain.Entities;
using ETicaretAPI.Domain.Entities.Identity;
using Google.Api.Gax;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Abstractions.Services.Token;

public class TokenHandler : ITokenHandler
{
    readonly IConfiguration _configuration;
    
    public TokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Application.DTOs.Token CreateAccessToken(AppUser user, int second=1500000000)
    {
        Application.DTOs.Token token = new();

        SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]!));

        SigningCredentials signingCredentials = new(securityKey, SecurityAlgorithms.HmacSha256);

        token.Expiration = DateTime.Now.AddMinutes(15);
        //token.Expiration = DateTime.Now.AddSeconds(10);
        Console.WriteLine(token.Expiration);

        JwtSecurityToken securityToken = new(
            audience: _configuration["Token:Audience"],
            issuer: _configuration["Token:Issuer"],
            expires: token.Expiration,
            signingCredentials: signingCredentials,
            claims: new List<Claim> { new(ClaimTypes.Name, user.UserName),new(ClaimTypes.NameIdentifier,user.Id.ToString())}
            );

        token.AccessToken = new JwtSecurityTokenHandler().WriteToken(securityToken);

        token.RefreshToken = CreateRefreshToken();
        return token;
    }



    public string CreateRefreshToken()
    {
        byte[] number = new byte[32];
        using RandomNumberGenerator random = RandomNumberGenerator.Create();
        random.GetBytes(number);
        return Convert.ToBase64String(number);
    }

   
}
