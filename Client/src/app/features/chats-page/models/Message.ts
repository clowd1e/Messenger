import { User } from "../components/add-chat/models/user";

export type Message = {
    id: string;
    sender: User;
    timestamp: string;
    content: string;
}