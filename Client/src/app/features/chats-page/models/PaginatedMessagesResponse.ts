import { Message } from "./Message"

export type PaginatedMessagesResponse = {
    messages: Message[],
    isLastPage: boolean
}