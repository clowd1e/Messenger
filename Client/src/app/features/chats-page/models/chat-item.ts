import { Message } from "./message";
import { User } from "../components/add-chat/models/user";

export type ChatItem = {
    id: string;
    creationDate: string;
    users: User[];
    messages: Message[];
}