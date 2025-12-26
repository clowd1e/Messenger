import { Component, input } from '@angular/core';

@Component({
  selector: 'app-messages-date-badge',
  standalone: true,
  imports: [],
  templateUrl: './messages-date-badge.component.html',
  styleUrl: './messages-date-badge.component.scss'
})
export class MessagesDateBadgeComponent {
  label = input.required<string>();
}
