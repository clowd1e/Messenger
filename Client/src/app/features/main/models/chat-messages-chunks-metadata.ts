export type ChatMessagesMetadata = {
    currentPage: number;
    isLastPage: boolean;
    retrieveCutoff: Date;
    chatScrollPosition?: number;
}