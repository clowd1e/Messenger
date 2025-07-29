import { Chat } from "../models/chat";
import { ChatItem } from "../models/chat-item";

export function MapChatToChatItem(chat: Chat): ChatItem {
    return {
        id: chat.id,
        creationDate: chat.creationDate,
        messages: [chat.lastMessage],
        users: chat.users,
    }
}