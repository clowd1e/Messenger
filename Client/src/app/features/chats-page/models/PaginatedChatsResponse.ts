import { Chat } from "./Chat"

export type PaginatedChatsResponse = {
    chats: Chat[],
    isLastPage: boolean
}