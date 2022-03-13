using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
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
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private UserLogin user = new UserLogin();

        private string returnUrl { get; set; } = String.Empty;


        string message = string.Empty;
        string messageCssClass = string.Empty;

        [Inject]
        ICartService cartService { get; set; }

        protected override void OnInitialized()
        {
            var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
            if(QueryHelpers.ParseQuery(uri.Query).TryGetValue("returnUrl",out var url))
            {
                returnUrl=url;
            }
        }
        private async Task Handlelogin()
        {
            var result = await authService.Login(user);
            message = result.Message;
            if (result.Success)
            {
                message = string.Empty;
                messageCssClass = "text-success";
                await localStorageService.SetItemAsStringAsync("authToken", result.Data);
                await AuthenticationStateProvider.GetAuthenticationStateAsync();
                await cartService.GetCartItemsCount();
                await cartService.StoreCartItems(true);
                navigationManager.NavigateTo(returnUrl);
            }
            else
            {
                messageCssClass = "text-danger";
            }

        }
    }
}
