import { WritableSignal } from "@angular/core";
import { ChatMessagesMetadata } from "./chat-messages-chunks-metadata";
import { Message } from "./message";

export type ChatMessagesData = {
    messages: WritableSignal<Message[]>;
    metadata: ChatMessagesMetadata;
}