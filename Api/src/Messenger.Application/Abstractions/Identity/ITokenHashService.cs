namespace Messenger.Application.Abstractions.Identity
{
    public interface ITokenHashService
    {
        string Hash(string token);

        bool Verify(string token, string? hash);
    }
}
