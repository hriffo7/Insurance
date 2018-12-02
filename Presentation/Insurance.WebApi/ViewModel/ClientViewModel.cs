using System;
namespace Insurance.WebApi.ViewModel
{
    public class ClientViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }
    }
}
