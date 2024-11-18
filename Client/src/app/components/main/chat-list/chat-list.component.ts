import { Component } from '@angular/core';
import { ChatItemComponent } from "../chat-item/chat-item.component";

@Component({
  selector: 'app-chat-list',
  standalone: true,
  imports: [ChatItemComponent],
  templateUrl: './chat-list.component.html',
  styleUrl: './chat-list.component.scss'
})
export class ChatListComponent {
  chatItems = [
    { name: 'Chat 1', isChecked: false },
    { name: 'Chat 2', isChecked: false },
    { name: 'Chat 3', isChecked: false },
    { name: 'Chat 4', isChecked: false },
    { name: 'Chat 5', isChecked: false }
  ]
}
