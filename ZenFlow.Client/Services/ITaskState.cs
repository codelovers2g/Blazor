using ZenFlow.Client.Domain.Models;

namespace ZenFlow.Client.Services;

public interface ITaskState
{
    List<TaskItem> Tasks { get; }

    event Action? OnChange;

    void AddTask(string title);
    void ToggleTask(Guid id);
    void RemoveTask(Guid id);
    void UpdateTask(Guid id, string newTitle);
}
