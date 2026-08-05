namespace TaskMenagementAPI.Exceptions
{
    public class CurrentUserNotFoundException : Exception
    {
        public CurrentUserNotFoundException(string message)
            : base(message) { }
    }
}
