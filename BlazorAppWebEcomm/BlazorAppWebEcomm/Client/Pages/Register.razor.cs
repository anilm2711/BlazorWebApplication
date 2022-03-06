using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAppWebEcomm.Client.Pages
{
    public partial class Register : ComponentBase
    {
        public UserRegister user { get; set; } = new UserRegister();

        void HandleRegistration()
        {
            Console.WriteLine("register");
        }
    }
}
