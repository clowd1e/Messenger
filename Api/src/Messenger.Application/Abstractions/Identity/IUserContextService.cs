namespace Messenger.Application.Abstractions.Identity
{
    public interface IUserContextService<TUserId>
    {
        TUserId GetAuthenticatedUserId();
    }
}
