import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddChatButtonComponent } from './add-chat-button.component';

describe('AddChatButtonComponent', () => {
  let component: AddChatButtonComponent;
  let fixture: ComponentFixture<AddChatButtonComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AddChatButtonComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddChatButtonComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
