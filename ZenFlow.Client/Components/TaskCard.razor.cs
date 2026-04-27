using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using ZenFlow.Client.Domain.Models;

namespace ZenFlow.Client.Components;

public partial class TaskCard : ComponentBase
{
    [Parameter] 
    public TaskItem Task { get; set; } = default!;
    
    [Parameter] 
    public EventCallback OnToggle { get; set; }
    
    [Parameter] 
    public EventCallback<string> OnUpdate { get; set; }
    
    [Parameter] 
    public EventCallback OnRemove { get; set; }

    private bool isEditing;
    private string editTitle = "";

    private async System.Threading.Tasks.Task Toggle() => await OnToggle.InvokeAsync();
    
    private void StartEdit()
    {
        isEditing = true;
        editTitle = Task.Title;
    }
    
    private async System.Threading.Tasks.Task SaveEdit()
    {
        if (isEditing)
        {
            isEditing = false;
            if (!string.IsNullOrWhiteSpace(editTitle) && editTitle != Task.Title)
            {
                await OnUpdate.InvokeAsync(editTitle);
            }
        }
    }
    
    private void CancelEdit() => isEditing = false;
    
    private async System.Threading.Tasks.Task HandleKeyUp(KeyboardEventArgs e)
    {
        if (e.Key == "Enter") await SaveEdit();
        else if (e.Key == "Escape") CancelEdit();
    }
    
    private async System.Threading.Tasks.Task Remove() => await OnRemove.InvokeAsync();
}
