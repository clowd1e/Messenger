import { Message } from "../Message";

export type MessageDto = {
    message: Message;
    userIconVisible: boolean;
    uniqueId: number;
    iconUri: string;
};