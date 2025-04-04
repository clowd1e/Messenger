import { Message } from "./Message";
import { User } from "../components/add-chat/models/User";

export type ChatItem = {
    id: string;
    creationDate: string;
    users: User[];
    messages: Message[];
}