using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ZenFlow.Client.Services;
using ZenFlow.Client.Domain.Models;

namespace ZenFlow.Client.Pages;

public partial class Home : ComponentBase, IDisposable
{
    [Inject] 
    private ITaskState TaskState { get; set; } = default!;

    private string newTaskTitle = "";
    private string searchTerm = "";
    private bool showCompleted = false;

    private IEnumerable<TaskItem> FilteredTasks =>
        TaskState.Tasks
            .Where(t => string.IsNullOrWhiteSpace(searchTerm) || t.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

    private IEnumerable<TaskItem> ActiveTasks =>
        FilteredTasks
            .Where(t => !t.IsCompleted)
            .OrderByDescending(t => t.CreatedAt);

    private IEnumerable<TaskItem> CompletedTasks =>
        FilteredTasks
            .Where(t => t.IsCompleted)
            .OrderByDescending(t => t.CreatedAt);

    protected override void OnInitialized()
    {
        // Any change triggers automatic UI re-render (interactive SPA behavior)
        TaskState.OnChange += StateHasChanged;
    }

    private void ToggleCompleted() => showCompleted = !showCompleted;

    private void AddTask()
    {
        if (!string.IsNullOrWhiteSpace(newTaskTitle))
        {
            TaskState.AddTask(newTaskTitle);
            newTaskTitle = "";
        }
    }

    private void HandleKeyUp(KeyboardEventArgs e)
    {
        if (e.Key == "Enter") AddTask();
    }

    // Explicitly unsubscribing from global state events in Dispose is critical to prevent memory leaks in long-lived Blazor SPA sessions.
    public void Dispose()
    {
        TaskState.OnChange -= StateHasChanged;
        GC.SuppressFinalize(this);
    }
}
