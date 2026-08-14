namespace TaskMenagementAPI.Services
{
    public interface INotificationService
    {
        Task NotifyTaskAssignedAsync(int userId, int taskId);
    }
}
