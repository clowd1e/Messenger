import { CommonModule } from '@angular/common';
import { Component, input, Input } from '@angular/core';

@Component({
  selector: 'app-common-button',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './common-button.component.html',
  styleUrl: './common-button.component.scss'
})
export class CommonButtonComponent {
  @Input() borderRadius: number = 0;
  @Input() textColor: string = '#000';
  @Input() backgroundColor: string = '#6799EF';
  @Input() fontSize: number = 12;
  @Input() padding: string = '0px';
  @Input() fontWeight: number = 400;
  
  text = input.required<string>();
  
  getStyles() {
    return {
      'color': this.textColor,
      'background-color': this.backgroundColor,
      'border-radius': `${this.borderRadius}px`,
      'padding': this.padding,
      'font-size': `${this.fontSize}px`,
      'font-weight': this.fontWeight,
    };
  }
}
