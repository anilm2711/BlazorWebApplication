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

        [Inject]
        ILocalStorageService localStorageService { get; set; }
        [Inject]
        NavigationManager navigationManager { get; set; }

        private UserLogin user = new UserLogin();

        string message = string.Empty;
        string messageCssClass = string.Empty;
        private async Task Handlelogin()
        {
            var result = await authService.Login(user);
            message = result.Message;
            if (result.Success)
            {
                message = string.Empty;
                messageCssClass = "text-success";
                await localStorageService.SetItemAsStringAsync("authToken", result.Data);
                navigationManager.NavigateTo("");
            }
            else
            {
                messageCssClass = "text-danger";
            }

        }
    }
}
