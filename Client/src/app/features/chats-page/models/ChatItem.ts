import { Message } from "../components/chat/models/Message";
import { User } from "../components/add-chat/models/User";

export type ChatItem = {
    id: string;
    creationDate: string;
    users: Array<User>;
    messages: Array<Message>;
}