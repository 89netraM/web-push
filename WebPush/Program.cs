using Lib.AspNetCore.WebPush;
using Lib.Net.Http.WebPush;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebPush;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpLogging(o => { });

builder.Services.AddOptions<PushServiceClientOptions>().BindConfiguration("Vapid").ValidateOnStart();

builder.Services.AddMemoryCache();
builder.Services.AddMemoryVapidTokenCache();
builder.Services.AddPushServiceClient();

var app = builder.Build();

app.UseHttpLogging();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapGet(
    "/vapidPublicKey",
    ([FromServices] IOptions<PushServiceClientOptions> vapidSettings) => vapidSettings.Value.PublicKey
);

app.MapPost(
    "/sendMessage",
    async ([FromServices] PushServiceClient pushServiceClient, [FromBody] SendMessageRequest request) =>
    {
        await pushServiceClient.RequestPushMessageDeliveryAsync(
            request.Subscription,
            new(
                $$"""
                {
                    "sender": "{{request.Sender}}",
                    "message": "{{request.Message}}"
                }
                """
            )
            {
                Urgency = PushMessageUrgency.High
            }
        );
        return Results.Ok();
    }
);

app.Run();
