import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionLineFormComponent } from './production-line-form.component';

describe('ProductionLineFormComponent', () => {
  let component: ProductionLineFormComponent;
  let fixture: ComponentFixture<ProductionLineFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionLineFormComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductionLineFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
