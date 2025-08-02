import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthErrorBoxComponent } from './auth-error-box.component';

describe('AuthErrorBoxComponent', () => {
  let component: AuthErrorBoxComponent;
  let fixture: ComponentFixture<AuthErrorBoxComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AuthErrorBoxComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthErrorBoxComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
