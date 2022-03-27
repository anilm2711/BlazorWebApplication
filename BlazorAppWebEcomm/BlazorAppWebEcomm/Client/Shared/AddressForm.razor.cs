using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlazorAppWebEcomm.Client.Shared
{
    public partial class AddressForm : ComponentBase
    {
        [Inject]
        public IAddressService addressService { get; set; }

        Address address = null;
        bool editAddress = false;
        protected override async Task OnInitializedAsync()
        {
            address =await addressService.GetAddress();
        }
        private async Task SubmitAddress()
        {
            editAddress = false;
            address = await addressService.AddOrUpdateAddress(address);
        }
        private void InitAddress()
        {
            address = new Address();
            editAddress = true;
        }
        private void EditAddress()
        {
            editAddress = true;
        }


    }
}
