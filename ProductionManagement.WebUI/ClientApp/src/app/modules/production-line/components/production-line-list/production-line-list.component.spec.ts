import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionLineListComponent } from './production-line-list.component';

describe('ProductionLineListComponent', () => {
  let component: ProductionLineListComponent;
  let fixture: ComponentFixture<ProductionLineListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionLineListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductionLineListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
