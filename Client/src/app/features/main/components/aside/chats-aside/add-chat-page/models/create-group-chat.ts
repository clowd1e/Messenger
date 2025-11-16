export type CreateGroupChat = {
    name: string;
    description: string | null;
    icon: File | null;
    participantIds: string[];
}