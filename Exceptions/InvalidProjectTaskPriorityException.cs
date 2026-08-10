namespace TaskMenagementAPI.Exceptions
{
    public class InvalidProjectTaskPriorityException : Exception
    {
        public InvalidProjectTaskPriorityException(string message)
            : base(message) { }
    }
}
