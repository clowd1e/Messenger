namespace Messenger.Application.Abstractions.Emails
{
    public sealed record Letter(
        string Subject,
        string Body);
}
