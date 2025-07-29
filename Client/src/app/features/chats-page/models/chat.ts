import { User } from "../components/add-chat/models/user";
import { Message } from "./Message";

export type Chat = {
    id: string;
    creationDate: string;
    users: User[];
    lastMessage: Message;
}