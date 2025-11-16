import { User } from "./user";

export type GroupMember = {
    user: User;
    role: 'Admin' | 'Member';
}