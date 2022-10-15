import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionDaysListComponent } from './production-days-list.component';

describe('ProductionDaysListComponent', () => {
  let component: ProductionDaysListComponent;
  let fixture: ComponentFixture<ProductionDaysListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionDaysListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductionDaysListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
