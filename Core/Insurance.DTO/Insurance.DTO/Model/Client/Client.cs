using System;
using System.Collections.Generic;

namespace Insurance.DTO.Model.Client
{
    public class Client
    {
        public ICollection<ClientDto> Clients { get; set; }
    }
}