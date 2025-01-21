import { Message } from "./Message";
import { User } from "./User";

export type ChatItem = {
    id: string;
    creationDate: string;
    users: Array<User>;
    messages: Array<Message>;
}