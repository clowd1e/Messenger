import { Chat } from "./chat";
import { User } from "./user";

export type PrivateChat = Chat & {
    type: 'private';
    participants: User[];
}