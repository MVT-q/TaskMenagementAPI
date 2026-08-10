namespace TaskMenagementAPI.Exceptions
{
    public class InvalidDueDateException : Exception
    {
        public InvalidDueDateException(string message)
            : base(message) { }
    }
}
