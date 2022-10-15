import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionDaysMainComponentComponent } from './production-days-main-component.component';

describe('ProductionDaysMainComponentComponent', () => {
  let component: ProductionDaysMainComponentComponent;
  let fixture: ComponentFixture<ProductionDaysMainComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionDaysMainComponentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductionDaysMainComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
