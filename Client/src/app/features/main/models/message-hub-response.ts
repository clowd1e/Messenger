import { Message } from "./message";

export type MessageHubResponse = {
    chatId: string;
    message: Message;
}