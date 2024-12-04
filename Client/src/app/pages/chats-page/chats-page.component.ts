import { Component, inject } from '@angular/core';
import { ChatListComponent } from "../../components/main/chat-list/chat-list.component";
import { ChatComponent } from "../../components/main/chat/chat.component";
import { SignalrService } from '../../services/signalr/signalr.service';
import { ChatItem } from '../../Models/ChatItem';
import { Error } from '../../Models/error/Error';
import { Message } from '../../Models/Message';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-chats-page',
  standalone: true,
  imports: [ChatListComponent, ChatComponent],
  providers: [SignalrService],
  templateUrl: './chats-page.component.html',
  styleUrl: './chats-page.component.scss'
})
export class ChatsPageComponent {
  userChats: Array<ChatItem> = [];

  selectedChat: ChatItem | undefined = undefined;

  signalrService = inject(SignalrService);
  route = inject(ActivatedRoute);

  ngOnInit() {
    this.connectSignalR();

    this.route.paramMap.subscribe(params => {
      const chatId = params.get('chatId');
      if (chatId) {
        this.selectedChat = this.userChats.find(chat => chat.id === chatId);
      }
    });
  }

  private connectSignalR() : void {
    this.signalrService.connect().then(() => {
      this.signalrService.getHubConnection().on('ReceiveUserChats',
        (chats: Array<ChatItem>) => {
          this.userChats = chats;
          console.log(chats);
      });

      this.signalrService.getHubConnection().on('ReceiveError',
        (error: Error) => {
          console.log(error);
        }
      )
    })
  }

}
