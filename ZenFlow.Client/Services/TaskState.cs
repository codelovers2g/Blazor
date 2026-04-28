using ZenFlow.Client.Domain.Models;

namespace ZenFlow.Client.Services;

public class TaskState : ITaskState
{
    public List<TaskItem> Tasks { get; private set; } = new();
    
    // Implementing the Observer pattern via events forces one-way data flow, preventing hard-to-trace state mutations across components.
    public event Action? OnChange;

    public void AddTask(string title)
    {
        Tasks.Add(new TaskItem { Title = title });
        NotifyStateChanged();
    }

    public void ToggleTask(Guid id)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);
        if (task != null)
        {
            task.IsCompleted = !task.IsCompleted;
            NotifyStateChanged();
        }
    }

    public void RemoveTask(Guid id)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);
        if (task != null)
        {
            Tasks.Remove(task);
            NotifyStateChanged();
        }
    }

    public void UpdateTask(Guid id, string newTitle)
    {
        var task = Tasks.FirstOrDefault(t => t.Id == id);
        if (task != null && !string.IsNullOrWhiteSpace(newTitle))
        {
            task.Title = newTitle;
            NotifyStateChanged();
        }
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
