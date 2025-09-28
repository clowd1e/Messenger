import { ChatItem } from "./chat-item";
import { GroupMember } from "./group-member";

export type GroupChatItem = ChatItem & {
    type: 'group';
    name: string;
    description: string | null;
    iconUri: string | null;
    participants: GroupMember[];
}