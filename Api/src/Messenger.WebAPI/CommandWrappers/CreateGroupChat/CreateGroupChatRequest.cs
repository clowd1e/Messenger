namespace Messenger.WebAPI.CommandWrappers.CreateGroupChat
{
    public sealed record CreateGroupChatRequest(
        List<Guid> Invitees,
        string Name,
        string? Description,
        string Message);
}
