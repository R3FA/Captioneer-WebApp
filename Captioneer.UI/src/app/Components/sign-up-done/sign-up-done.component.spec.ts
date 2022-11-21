import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SignUpDoneComponent } from './sign-up-done.component';

describe('SignUpDoneComponent', () => {
  let component: SignUpDoneComponent;
  let fixture: ComponentFixture<SignUpDoneComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SignUpDoneComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(SignUpDoneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
