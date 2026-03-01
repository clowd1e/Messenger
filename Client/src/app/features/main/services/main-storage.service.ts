import { Injectable, Signal, signal } from '@angular/core';
import { Message } from '../models/message';
import { Chat } from '../models/chat';
import { ChatMessagesData } from '../models/chat-messages-data';
import { ChatMessagesMetadata as ChatMessagesMetadata } from '../models/chat-messages-chunks-metadata';
import { environment } from '../../../../environments/environment';
import { MessageHubResponse } from '../models/message-hub-response';

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
  private lastReceivedMessage = signal<MessageHubResponse | null>(null);

  get MessagesPageSize(): number {
    return MainStorageService.MessagesPageSize;
  }

  get LastReceivedMessage(): Signal<MessageHubResponse | null> {
    return this.lastReceivedMessage;
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

  GetCurrentChatCreatedAt() : string {
    if (!this.SelectedChat()) {
      throw new Error('No chat is selected.');
    }
    return this.SelectedChat()!.creationDate;
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

  appendMessageToChat(messageResponse: MessageHubResponse) {
    const chatMessagesData = this.chatMessages()[messageResponse.chatId];
    const chat = this.chats().find(c => c.id === messageResponse.chatId);
    if (!chatMessagesData) {
      throw new Error(`Chat with id ${messageResponse.chatId} not found in storage.`);
    }
    if (!chat) {
      throw new Error(`Chat with id ${messageResponse.chatId} not found in chats list.`);
    }

    const message = messageResponse.message;
    chat.lastMessage = message;
    this.chats.update(chats => {
      const otherChats = chats.filter(c => c.id !== chat.id);
      return [chat, ...otherChats];
    });

    chatMessagesData.messages.update(messages => [...messages, message]);
    this.chatMessages.update(messagesRecord => ({
      ...messagesRecord,
      [messageResponse.chatId]: chatMessagesData
    }));

    this.lastReceivedMessage.set(messageResponse);
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

  removeMessage(chatId: string, messageId: string) {
    const chatMessagesData = this.chatMessages()[chatId];
    if (!chatMessagesData) {
      // deletion of a message belonging to not loaded chat
      return;
    }

    chatMessagesData.messages.update(messages => 
      messages.filter(m => m.id !== messageId)
    );
  }

  updateMessage(chatId: string, messageId: string, newContent: string, updatedAt: string) {
    const chatMessagesData = this.chatMessages()[chatId];
    if (!chatMessagesData) {
      // update of a message belonging to not loaded chat
      return;
    }

    chatMessagesData.messages.update(messages =>
      messages.map(m => 
        m.id === messageId 
          ? { ...m, content: newContent, updatedAt: updatedAt }
          : m
      )
    );
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
