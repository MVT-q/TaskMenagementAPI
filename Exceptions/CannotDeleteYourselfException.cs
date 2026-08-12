namespace TaskMenagementAPI.Exceptions
{
    public class CannotDeleteYourselfException : Exception
    {
        public CannotDeleteYourselfException(string message)
            : base(message) { }
    }
}
