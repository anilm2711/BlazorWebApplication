using System;
using System.Collections.Generic;

namespace BlazorAppWebEcomm.Server.Models
{
    public partial class User
    {
        public User()
        {
            Addresses = new HashSet<Address>();
        }

        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public DateTime DateCreated { get; set; }
        public int? AddressId { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
    }
}
