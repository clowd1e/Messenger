export type ResetPasswordRequest = {
    userId: string;
    tokenId: string;
    token: string;
    newPassword: string;
}