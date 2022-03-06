using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAppWebEcomm.Client.Pages
{
    public partial class Login : ComponentBase
    {
        [Inject]
        IAuthService authService { get; set; }
        public UserRegister user { get; set; } = new UserRegister();

        string message = string.Empty;
        string messageCssClass = string.Empty;
        private async Task Handlelogin()
        {
            Console.WriteLine("Login ");
            //var result = await authService.Register(user);
            //message = result.Message;
            //if (result.Success)
            //{
            //    messageCssClass = "text-success";
            //}
            //else
            //{
            //    messageCssClass = "text-danger";
            //}

        }
    }
}
