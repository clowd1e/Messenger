import { CommonModule } from '@angular/common';
import { Component, Input, input } from '@angular/core';

@Component({
  selector: 'app-default-button',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './default-button.component.html',
  styleUrl: './default-button.component.scss'
})
export class DefaultButtonComponent {
  @Input() borderRadius: number = 0;
  @Input() textColor: string = '#000';
  @Input() backgroundColor: string = '#6799EF';
  @Input() fontSize: number = 12;
  @Input() padding: string = '0px';

  text = input.required<string>();

  getStyles() {
    return {
      'color': this.textColor,
      'background-color': this.backgroundColor,
      'border-radius': `${this.borderRadius}px`,
      'padding': this.padding,
      'font-size': `${this.fontSize}px`
    };
  }
}
