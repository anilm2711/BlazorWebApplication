using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAppWebEcomm.Client.Shared
{
    public partial class UserButton : ComponentBase
    {
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

    }
}
