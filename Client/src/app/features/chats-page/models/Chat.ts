import { Message } from "./Message";
import { User } from "../components/add-chat/models/User";

export type Chat = {
    id: string;
    creationDate: string;
    users: User[];
    lastMessage: Message;
}