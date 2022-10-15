import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeDayFormComponent } from './change-day-form.component';

describe('ChangeDayFormComponent', () => {
  let component: ChangeDayFormComponent;
  let fixture: ComponentFixture<ChangeDayFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChangeDayFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ChangeDayFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
