import { Message } from "./message";

export type ChatItem = {
    id: string;
    creationDate: string;
    messages: Message[];
    type: 'private' | 'group';
    selected?: boolean;
}