using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace BlazorAppWebEcomm.Client.Shared
{
    public partial class AdminMenu : ComponentBase
    {
        [Inject]
        public AuthenticationStateProvider authenticationStateProvider { get; set; }

        bool authorized = false;
        protected override async Task OnInitializedAsync()
        {
            var role = (await authenticationStateProvider.GetAuthenticationStateAsync()).User.Claims.FirstOrDefault(p => p.Type == ClaimTypes.Role).Value;
            if ( role!=null && role.Contains("Admin"))
            {
                authorized = true;
            }
        }
    }
}
