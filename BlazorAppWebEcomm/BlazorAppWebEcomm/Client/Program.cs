global using BlazorAppWebEcomm.Shared;
global using System.Net.Http.Json;
global using BlazorAppWebEcomm.Client.Services.ProductServices;
global using BlazorAppWebEcomm.Client;
global using Microsoft.AspNetCore.Components.Web;
global using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
global using BlazorAppWebEcomm.Client.Services.CategoryServices;
global using Blazored.LocalStorage;
global using BlazorAppWebEcomm.Client.Services.CartService;
global using BlazorAppWebEcomm.Client.Services.AuthServices;
global using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IAuthService, AuthService>();

await builder.Build().RunAsync();
