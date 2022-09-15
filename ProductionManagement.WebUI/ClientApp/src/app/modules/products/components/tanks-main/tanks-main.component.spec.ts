import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TanksMainComponent } from './tanks-main.component';

describe('TanksMainComponent', () => {
  let component: TanksMainComponent;
  let fixture: ComponentFixture<TanksMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TanksMainComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TanksMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
