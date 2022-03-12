using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAppWebEcomm.Client.Shared
{
    public partial class UserButton : ComponentBase
    {
        [Inject]
        ILocalStorageService localStorageService { get; set; }
        [Inject]
        NavigationManager NavigationManager { get; set; }
        [Inject]
        AuthenticationStateProvider AuthenticationStateProvider { get; set; }

        private bool showUserMenu = false;

        private string UserMenuCssClass => showUserMenu ? "show-menu" : null;
        private void ToggleUserMenu()
        {
            showUserMenu = !showUserMenu;
        }

        private async Task HideUserMenu()
        {
            await Task.Delay(200);
            showUserMenu = false;
        }

        private async Task Logout()
        {
            await localStorageService.RemoveItemAsync("authToken");
            await AuthenticationStateProvider.GetAuthenticationStateAsync();
            NavigationManager.NavigateTo("");
        }

    }
}
