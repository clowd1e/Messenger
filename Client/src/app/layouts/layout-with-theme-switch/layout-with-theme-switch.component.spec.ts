import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LayoutWithThemeSwitchComponent } from './layout-with-theme-switch.component';

describe('LayoutWithThemeSwitchComponent', () => {
  let component: LayoutWithThemeSwitchComponent;
  let fixture: ComponentFixture<LayoutWithThemeSwitchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [LayoutWithThemeSwitchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LayoutWithThemeSwitchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
