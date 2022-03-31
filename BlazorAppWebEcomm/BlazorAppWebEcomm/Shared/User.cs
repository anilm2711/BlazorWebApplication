using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorAppWebEcomm.Shared
{
    public partial class User
    {
        public int Id { get; set; }
        public string Email { get; set; } = String.Empty;
        public byte[] PasswordHash { get; set; } = new byte[0];
        public byte[] PasswordSalt { get; set; }= new byte[0];
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public int? AddressId { get; set; }
        public string Role { get; set; } = "Customer";
    }
}
