import { Message } from "./message";
import { User } from "../components/add-chat/models/user";

export type Chat = {
    id: string;
    creationDate: string;
    users: User[];
    lastMessage: Message;
}