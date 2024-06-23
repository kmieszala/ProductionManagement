import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetUserCodeComponent } from './get-user-code.component';

describe('GetUserCodeComponent', () => {
  let component: GetUserCodeComponent;
  let fixture: ComponentFixture<GetUserCodeComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GetUserCodeComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GetUserCodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
