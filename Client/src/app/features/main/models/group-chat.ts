import { Chat } from "./chat";
import { GroupMember } from "./group-member";

export type GroupChat = Chat & {
    type: 'group';
    name: string;
    description: string | null;
    iconUri: string | null;
    participants: GroupMember[];
}