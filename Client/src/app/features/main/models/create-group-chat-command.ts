export type CreateGroupChatCommand = {
    invitees: string[];
    name: string;
    description: string | null;
    icon: File | null;
    message: string | null;
}