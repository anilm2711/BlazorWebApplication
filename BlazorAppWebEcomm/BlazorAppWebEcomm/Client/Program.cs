global using BlazorAppWebEcomm.Shared;
global using System.Net.Http.Json;
global using BlazorAppWebEcomm.Client.Services.ProductServices;
using BlazorAppWebEcomm.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorAppWebEcomm.Client.Services.CategoryServices;
using Blazored.LocalStorage;
using BlazorAppWebEcomm.Client.Services.CartService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();

await builder.Build().RunAsync();
