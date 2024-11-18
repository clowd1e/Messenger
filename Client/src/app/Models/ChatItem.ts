import { Message } from "./Message";
import { User } from "./User";

export type ChatItem = {
    chatId: string;
    chatCreationDate: Date;
    users: Array<User>;
    messages: Array<Message>;
}