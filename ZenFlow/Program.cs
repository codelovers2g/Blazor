
using ZenFlow.Components;
using ZenFlow.Client.Services;


var builder = WebApplication.CreateBuilder(args);

//Registering domain state as Scoped ensures each user session maintains isolated task data in Blazor Server.
builder.Services.AddScoped<ITaskState, TaskState>();

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

var app = builder.Build();

// Pipeline configuration prioritizes WASM debugging locally and secure error handling in production.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // HSTS is critical for enforcing HTTPS in production to prevent man-in-the-middle attacks.
    app.UseHsts();
}

app.UseHttpsRedirection();


app.UseAntiforgery();

app.MapStaticAssets();
// Leveraging Blazor Interactive Auto mode to combine Server's fast initial load with WebAssembly's rich client-side interactivity.
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ZenFlow.Client._Imports).Assembly);

app.Run();
