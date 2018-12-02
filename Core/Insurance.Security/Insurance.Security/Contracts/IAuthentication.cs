using System;
using Insurance.DTO.Model.Client;

namespace Insurance.Security.Contracts
{
    public interface IAuthentication
    {
        string GenerateJwtToken(ClientDto user);
    }
}
