import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PageCommentComponent } from './page-comment.component';

describe('PageCommentComponent', () => {
  let component: PageCommentComponent;
  let fixture: ComponentFixture<PageCommentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PageCommentComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PageCommentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
