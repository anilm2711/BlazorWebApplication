using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAppWebEcomm.Client.Pages
{
    public partial class Profile : ComponentBase
    {
        [Inject]
        IAuthService authService { get; set; }

        public UserChangePassword userChangePassword { get; set; } = new UserChangePassword();

        public string message { get; set; } = string.Empty;

        private async Task ChangePassword()
        {
           var response= await authService.ChangePassword(userChangePassword);
            if(response.Success==false)
            {
                message = response.Message;
            }
            else
            {
                message = "Password has been changed successfully.";
            }
        }
    }
}
