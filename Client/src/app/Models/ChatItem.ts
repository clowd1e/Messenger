import { Message } from "./Message";
import { User } from "./User";

export type ChatItem = {
    id: string;
    creationDate: Date;
    users: Array<User>;
    messages: Array<Message>;
}