import { Message } from "./message"

export type PaginatedMessagesResponse = {
    messages: Message[],
    isLastPage: boolean
}