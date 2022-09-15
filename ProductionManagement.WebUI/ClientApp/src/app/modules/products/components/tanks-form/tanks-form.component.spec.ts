import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TanksFormComponent } from './tanks-form.component';

describe('TanksFormComponent', () => {
  let component: TanksFormComponent;
  let fixture: ComponentFixture<TanksFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TanksFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TanksFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
