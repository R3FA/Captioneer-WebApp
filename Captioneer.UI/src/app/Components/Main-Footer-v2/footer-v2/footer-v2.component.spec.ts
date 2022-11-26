import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FooterV2Component } from './footer-v2.component';

describe('FooterV2Component', () => {
  let component: FooterV2Component;
  let fixture: ComponentFixture<FooterV2Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FooterV2Component ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FooterV2Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
