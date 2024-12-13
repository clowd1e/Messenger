import { AfterViewInit, Component, effect, ElementRef, inject, Input, input, OnChanges, Renderer2, ViewChild } from '@angular/core';
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
    const msgs = this.messages();
    if (!msgs) return [];
    
    return msgs.map((message, i) => ({
      message,
      userIconVisible: i === msgs.length - 1 || msgs[i + 1]?.userId !== message.userId,
      uniqueId: i,
    }));
  }

  ngAfterViewInit() {
    effect(() => {
      console.log(this.messages())
    })

    this.scrollToBottom();
  }

  private scrollToBottom(): void {
    if (!this.messageList) {
      return;
    }

    this.messageList.nativeElement.scrollTop = this.messageList.nativeElement.scrollHeight;
  }
}
