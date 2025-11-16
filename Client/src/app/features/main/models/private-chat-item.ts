import { ChatItem } from "./chat-item";
import { User } from "./user";

export type PrivateChatItem = ChatItem & {
    type: 'private';
    participants: User[];
}