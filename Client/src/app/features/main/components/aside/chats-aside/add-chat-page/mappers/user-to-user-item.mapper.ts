import { User } from "../../../../../models/user";
import { UserItem } from "../models/user-item";

export function MapUserToUserItem(user: User, isSelected: boolean): UserItem {
    return {
        id: user.id,
        name: user.name,
        username: user.username,
        iconUri: user.iconUri,
        isSelected: isSelected
    };
}