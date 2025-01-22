import { Component, ElementRef, inject, input, Renderer2, SimpleChanges, ViewChild } from '@angular/core';
import { ChatMessageComponent } from './chat-message/chat-message.component';
import { Message } from '../models/Message';
import { User } from '../../add-chat/models/User';
import { UserContextService } from '../../../../../shared/services/user-context/user-context.service';
import { MessageDto } from '../models/MessageDto';

@Component({
  selector: 'app-chat-message-list',
  standalone: true,
  imports: [ChatMessageComponent],
  templateUrl: './chat-message-list.component.html',
  styleUrl: './chat-message-list.component.scss'
})
export class ChatMessageListComponent {
  @ViewChild('messageListContainer', {static: false}) 
  private messageList? : ElementRef;

  messages = input<Message[]>();

  users = input.required<User[]>();
  
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
      iconUri: this.users().find(user => user.id === message.userId)?.iconUri || "https://cdn-icons-png.flaticon.com/512/149/149071.png"
    }));
  }

  ngAfterViewInit() {
    this.scrollToBottom();
  }

  ngOnChanges(changes: SimpleChanges) {
    // if (changes['messages']) {
    //   console.log("messages changed");
    //   this.scrollToBottom();
    // }
  }

  private scrollToBottom(): void {
    if (!this.messageList) {
      return;
    }

    const scrollContainer = this.messageList.nativeElement;

    this.renderer.setStyle(scrollContainer, 'opacity', '0');

    setTimeout(() => {

      scrollContainer.scrollTop = scrollContainer.scrollHeight;

      this.renderer.setStyle(scrollContainer, 'opacity', '100');
    }, 0);
  }
}
