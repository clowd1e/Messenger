import { Message } from "./message";

export type Chat = {
    id: string;
    type: 'private' | 'group';
    creationDate: string;
    lastMessage: Message;
}