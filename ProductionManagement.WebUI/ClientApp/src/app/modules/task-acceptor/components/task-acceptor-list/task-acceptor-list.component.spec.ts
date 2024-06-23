import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskAcceptorListComponent } from './task-acceptor-list.component';

describe('TaskAcceptorListComponent', () => {
  let component: TaskAcceptorListComponent;
  let fixture: ComponentFixture<TaskAcceptorListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TaskAcceptorListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TaskAcceptorListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
