namespace Messenger.Application.Exceptions
{
    public sealed class NullTokenClaimException : Exception
    {
        public NullTokenClaimException(string claimName)
            : base($"Token claim of type {claimName} is null.")
        { }
    }
}
