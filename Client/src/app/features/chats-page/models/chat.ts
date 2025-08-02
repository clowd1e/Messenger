import { User } from "../components/add-chat/models/user";
import { Message } from "./message";

export type Chat = {
    id: string;
    creationDate: string;
    users: User[];
    lastMessage: Message;
}