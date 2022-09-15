import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TanksListComponent } from './tanks-list.component';

describe('TanksListComponent', () => {
  let component: TanksListComponent;
  let fixture: ComponentFixture<TanksListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TanksListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TanksListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
