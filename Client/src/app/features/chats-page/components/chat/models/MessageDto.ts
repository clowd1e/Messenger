import { Message } from "../../../models/Message";

export type MessageDto = {
    message: Message;
    userIconVisible: boolean;
    iconUri: string;
};