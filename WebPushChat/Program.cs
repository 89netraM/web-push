using System.Net.Http;
using Blazored.LocalStorage;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using WebPushChat;
using WebPushChat.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddScoped(sp => new PushServiceClient(new HttpClient()));

builder.Services.AddScoped<VapidKeyService>();
builder.Services.AddScoped<SubscriptionService>();
builder.Services.AddScoped<PushService>();

await using var host = builder.Build();

await host.RunAsync();
