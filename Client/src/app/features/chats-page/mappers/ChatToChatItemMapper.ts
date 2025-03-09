import { Chat } from "../models/Chat";
import { ChatItem } from "../models/ChatItem";

export function MapChatToChatItem(chat: Chat): ChatItem {
    return {
        id: chat.id,
        creationDate: chat.creationDate,
        messages: [chat.lastMessage],
        users: chat.users,
    }
}