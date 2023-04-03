import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageNotFoundErrorComponent } from './page-not-found-error.component';

describe('PageNotFoundErrorComponent', () => {
  let component: PageNotFoundErrorComponent;
  let fixture: ComponentFixture<PageNotFoundErrorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageNotFoundErrorComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PageNotFoundErrorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
