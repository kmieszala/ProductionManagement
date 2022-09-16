import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductionLineMainComponent } from './production-line-main.component';

describe('ProductionLineMainComponent', () => {
  let component: ProductionLineMainComponent;
  let fixture: ComponentFixture<ProductionLineMainComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProductionLineMainComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductionLineMainComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
