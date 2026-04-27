using Microsoft.AspNetCore.Components;
using ZenFlow.Client.Services;

namespace ZenFlow.Client.Layout;

public partial class MainLayout : LayoutComponentBase
{
    [Inject] 
    private ITaskState TaskState { get; set; } = default!;

    protected override void OnInitialized()
    {
        TaskState.OnChange += StateHasChanged;
    }
}
