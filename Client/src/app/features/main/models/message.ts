import { User } from "./user";

export type Message = {
    id: string;
    sender: User;
    timestamp: string;
    content: string;
}