import { User } from "../components/add-chat/models/user"

export type GroupMember = {
    user: User;
    role: 'Admin' | 'Member';
}