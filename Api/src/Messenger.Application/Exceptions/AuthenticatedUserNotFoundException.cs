namespace Messenger.Application.Exceptions
{
    public sealed class AuthenticatedUserNotFoundException : Exception
    {
        public AuthenticatedUserNotFoundException()
            : base("Authenticated user is not found.")
        { }
    }
}
