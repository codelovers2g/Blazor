using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ZenFlow.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// In WebAssembly, Scoped services act as Singletons because there is only one user session per browser tab.
builder.Services.AddScoped<ITaskState, TaskState>();

await builder.Build().RunAsync();
