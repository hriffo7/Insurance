using System;

namespace Insurance.DTO.Model.Client
{
    public class ClientDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}