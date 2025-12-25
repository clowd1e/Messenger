import { Injectable, Signal, signal } from '@angular/core';
import { Message } from '../models/message';
import { Chat } from '../models/chat';
import { ChatMessagesData } from '../models/chat-messages-data';
import { ChatMessagesMetadata as ChatMessagesMetadata } from '../models/chat-messages-chunks-metadata';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MainStorageService {
  private static readonly MessagesPageSize = environment.MESSAGES_PAGE_SIZE;

  private chats = signal<Chat[]>([]);
  private selectedChatId = signal<string | null>(null);
  private selectedChat = signal<Chat | null>(null);
  private currentUserId: string | null = null;
  private chatMessages = signal<Record<string, ChatMessagesData>>({});

  get MessagesPageSize(): number {
    return MainStorageService.MessagesPageSize;
  }

  //#region CurrentUserId
  get CurrentUserId(): string | null {
    return this.currentUserId;
  }
  set CurrentUserId(value: string | null) {
    this.currentUserId = value;
  }
  //#endregion

  //#regionse Chats
  get Chats(): Signal<Chat[]> {
    return this.chats;
  }

  setChats(chats: Chat[]) {
    this.chats.set(chats);
    for (const chat of chats) {
      this.chatMessages.update(messagesRecord => ({
        ...messagesRecord,
        [chat.id]: { messages: signal<Message[]>([]), metadata: this.defaultMessagesMetadata() }
      }));
    }
  }

  insertAtStart(chat: Chat) {
    this.chats.update(chats => [chat, ...chats]);
    this.chatMessages.update(messagesRecord => ({
      ...messagesRecord,
      [chat.id]: { messages: signal<Message[]>([]), metadata: this.defaultMessagesMetadata() }
    }));
  }

  appendChats(externalChats: Chat[]) {
    this.chats.update(chats => [...chats, ...externalChats]);
    for (const chat of externalChats) {
      if (!this.chatMessages()[chat.id]) {
        this.chatMessages.update(messagesRecord => ({
          ...messagesRecord,
          [chat.id]: { messages: signal<Message[]>([]), metadata: this.defaultMessagesMetadata() }
        }));
      }
    }
  }
  //#endregion

  //#region SelectedChatId
  get SelectedChatId(): Signal<string | null> {
    return this.selectedChatId;
  }
  //#endregion

  //#region SelectedChat
  get SelectedChat(): Signal<Chat | null> {
    return this.selectedChat;
  }

  selectChat(id: string) {
    const chat = this.chats().find(c => c.id === id) || null;
    this.selectedChatId.set(id);
    this.selectedChat.set(chat);
  }

  selectChatByItem(chat: Chat) {
    this.selectedChat.set(chat);
    if (!this.chatMessages()[chat.id]) {
      this.chatMessages.update(messagesRecord => ({
        ...messagesRecord,
        [chat.id]: { messages: signal<Message[]>([]), metadata: this.defaultMessagesMetadata() }
      }));
    }
  }

  unsetSelectedChat() {
    this.selectedChatId.set(null);
    this.selectedChat.set(null);
  }
  //#endregion

  //#region Messages
  getCurrentMessagesMetadata() : ChatMessagesMetadata | null {
    if (!this.SelectedChatId()) {
      throw new Error('No chat is selected.');
    }
    return this.chatMessages()[this.SelectedChatId()!]?.metadata || null;
  }

  getCurrentMessages() : Signal<Message[]> {
    if (!this.SelectedChatId()) {
      throw new Error('No chat is selected.');
    }
    return this.chatMessages()[this.SelectedChatId()!]?.messages || [];
  }

  loadPreviousMessages(externalMessages: Message[], isLastPage: boolean) {
    if (!this.SelectedChatId()) {
      throw new Error('No chat is selected.');
    }

    const chatMessagesData = this.chatMessages()[this.SelectedChatId()!];
    if (!chatMessagesData) {
      throw new Error(`Chat with id ${this.SelectedChatId()} not found in storage.`);
    }
    chatMessagesData.messages.update(messages => [...externalMessages, ...messages]);
    chatMessagesData.metadata.currentPage += 1;
    chatMessagesData.metadata.isLastPage = isLastPage;
    this.chatMessages.update(messagesRecord => ({
      ...messagesRecord,
      [this.SelectedChatId()!]: chatMessagesData
    }));
  }

  appendMessageToSelectedChat(message: Message) {
    if (!this.SelectedChatId()) {
      throw new Error('No chat is selected.');
    }

    const chatMessagesData = this.chatMessages()[this.SelectedChatId()!];
    if (!chatMessagesData) {
      throw new Error(`Chat with id ${this.SelectedChatId()!} not found in storage.`);
    }

    chatMessagesData.messages.update(messages => [...messages, message]);
    this.chatMessages.update(messagesRecord => ({
      ...messagesRecord,
      [this.SelectedChatId()!]: chatMessagesData
    }));
  }

  saveChatScroll(chatId: string, scrollPosition: number) {
    const chatMessagesData = this.chatMessages()[chatId];
    if (!chatMessagesData) {
      throw new Error(`Chat with id ${chatId} not found in storage.`);
    }

    chatMessagesData.metadata.chatScrollPosition = scrollPosition;
    this.chatMessages.update(messagesRecord => ({
      ...messagesRecord,
      [chatId]: chatMessagesData
    }));
  }
  //#endregion

  private defaultMessagesMetadata(): ChatMessagesMetadata {
    return {
      currentPage: 1,
      isLastPage: false,
      retrieveCutoff: new Date()
    };
  }
}
