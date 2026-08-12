namespace TaskMenagementAPI.Exceptions
{
    public class CannotChangeOwnRoleException : Exception
    {
        public CannotChangeOwnRoleException(string message)
            : base(message) { }
    }
}
