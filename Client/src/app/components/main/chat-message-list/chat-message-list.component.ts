import { AfterViewInit, Component, ElementRef, inject, input, Renderer2, ViewChild } from '@angular/core';
import { Message } from '../../../Models/Message';
import { ChatMessageComponent } from "../chat-message/chat-message.component";
import { UserContextService } from '../../../services/auth/user-context.service';
import { MessageDto } from '../../../Models/DTO/MessageDto';

@Component({
  selector: 'app-chat-message-list',
  standalone: true,
  imports: [ChatMessageComponent],
  templateUrl: './chat-message-list.component.html',
  styleUrl: './chat-message-list.component.scss'
})
export class ChatMessageListComponent implements AfterViewInit {
  @ViewChild('messageListContainer', {static: false}) 
  private messageList? : ElementRef;

  messages = input<Message[]>();
  
  userContextService = inject(UserContextService);
  renderer = inject(Renderer2);

  currentUserId = this.userContextService.getCurrentUserId();

  messageDtos(): MessageDto[] {
    if (this.messages() === undefined || this.messages() === null) {
      return [];
    }

    let messageDtos: MessageDto[] = [];

    for (let i = 0; i < this.messages()!.length; i++) {
      let userIconVisible: boolean = false;
      let message = this.messages()![i];

      if (i != this.messages()!.length - 1) {
        userIconVisible = !(message.userId == this.messages()![i + 1].userId);
      } else {
        userIconVisible = true;
      }

      let messageDto: MessageDto = {
        message: message,
        userIconVisible: userIconVisible,
        uniqueId: i
      };

      messageDtos.push(messageDto);
    }

    return messageDtos;
  }

  ngAfterViewInit() {
    this.scrollToBottom();
  }

  private scrollToBottom(): void {
    if (!this.messageList) {
      return;
    }

    this.messageList.nativeElement.scrollTop = this.messageList.nativeElement.scrollHeight;
  }
}
