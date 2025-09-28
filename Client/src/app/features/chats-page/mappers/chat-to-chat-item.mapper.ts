import { User } from "../components/add-chat/models/user";
import { Chat } from "../models/chat";
import { ChatItem } from "../models/chat-item";
import { GroupChatItem } from "../models/group-chat-item";
import { GroupMember } from "../models/group-member";
import { PrivateChatItem } from "../models/private-chat-item";

export function MapChatToChatItem(chat: Chat): ChatItem {
    if (chat.type === 'private') {
        return {
            id: chat.id,
            creationDate: chat.creationDate,
            messages: [chat.lastMessage],
            participants: (chat as any).participants as User[],
            type: 'private',
        } as PrivateChatItem;
    } else if (chat.type === 'group') {
        return {
            id: chat.id,
            creationDate: chat.creationDate,
            name: (chat as any).name as string,
            description: (chat as any).description as string | null,
            iconUri: (chat as any).iconUri as string | null,
            messages: [chat.lastMessage],
            type: 'group',
            participants: (chat as any).participants as GroupMember[]
        } as GroupChatItem;
    } else {
        throw new Error('Unknown chat type');
    }
}