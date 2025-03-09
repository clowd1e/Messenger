import { User } from "../components/add-chat/models/User";

export type Message = {
    id: string;
    sender: User;
    timestamp: string;
    content: string;
}