import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PartsMainComponent } from './parts-main.component';

describe('PartsMainComponent', () => {
  let component: PartsMainComponent;
  let fixture: ComponentFixture<PartsMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PartsMainComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PartsMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
