import { Message } from "../../../../models/message";

export type MessageDto = {
    message: Message;
    userNameVisible: boolean;
    userIconVisible: boolean;
    iconUri: string;
    updatedAt: string | null;
}