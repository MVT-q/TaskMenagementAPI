namespace TaskMenagementAPI.Exceptions
{
    public class InvalidProjectTaskStatusException : Exception
    {
        public InvalidProjectTaskStatusException(string message)
            : base(message) { }
    }
}
