import { User } from "../components/add-chat/models/user";
import { ChatItem } from "./chat-item";

export type PrivateChatItem = ChatItem & {
    type: 'private';
    participants: User[];
}