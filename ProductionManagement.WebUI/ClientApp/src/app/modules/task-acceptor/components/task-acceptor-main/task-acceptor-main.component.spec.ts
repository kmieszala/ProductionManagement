import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskAcceptorMainComponent } from './task-acceptor-main.component';

describe('TaskAcceptorMainComponent', () => {
  let component: TaskAcceptorMainComponent;
  let fixture: ComponentFixture<TaskAcceptorMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TaskAcceptorMainComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskAcceptorMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
