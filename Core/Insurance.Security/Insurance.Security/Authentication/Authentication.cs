﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Insurance.DTO.Model.Client;
using Insurance.Security.Contracts;
using Microsoft.IdentityModel.Tokens;

namespace Insurance.Security.Authentication
{
    public class Authentication : IAuthentication
    {
        public string GenerateJwtToken(ClientDto client)
        {
            byte[] secret = Convert.FromBase64String("856FECBA3B06519C8DDDBC80BB080557");
            SymmetricSecurityKey credentials = new SymmetricSecurityKey(secret);

            List<Claim> claimsList = new List<Claim>();
            var userClaimName = new Claim(JwtRegisteredClaimNames.Sub, client.Name);
            claimsList.Add(userClaimName);

            Claim roleClaim = new Claim(ClaimTypes.Role, client.Role);
            claimsList.Add(roleClaim);

            Claim idClaim = new Claim("id", client.Id.ToString());
            claimsList.Add(idClaim);

            JwtSecurityToken token = new JwtSecurityToken(
                claims: claimsList,
                expires: DateTime.UtcNow.AddMinutes(30),
                signingCredentials: new SigningCredentials(credentials, SecurityAlgorithms.HmacSha256)
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
