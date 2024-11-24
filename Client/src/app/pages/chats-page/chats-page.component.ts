import { Component } from '@angular/core';
import { ChatListComponent } from "../../components/main/chat-list/chat-list.component";
import { ChatComponent } from "../../components/main/chat/chat.component";

@Component({
  selector: 'app-chats-page',
  standalone: true,
  imports: [ChatListComponent, ChatComponent],
  templateUrl: './chats-page.component.html',
  styleUrl: './chats-page.component.scss'
})
export class ChatsPageComponent {

}
