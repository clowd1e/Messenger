import { Message } from "../../../models/message";

export type MessageDto = {
    message: Message;
    userIconVisible: boolean;
    userNameVisible: boolean;
    iconUri: string;
};