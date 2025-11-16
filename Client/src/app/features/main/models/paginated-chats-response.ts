import { Chat } from "./chat"

export type PaginatedChatsResponse = {
    chats: Chat[],
    isLastPage: boolean
}