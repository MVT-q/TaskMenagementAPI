namespace TaskMenagementAPI.Services
{
    public class ConsoleNotificationService : INotificationService
    {
        public Task NotifyTaskAssignedAsync(int userId, int taskId)
        {
            Console.WriteLine($"User {userId} was assigned to task {taskId}");

            return Task.CompletedTask;
        }
    }
}
