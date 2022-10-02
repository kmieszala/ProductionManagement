import { ComponentFixture, TestBed } from '@angular/core/testing';

import { StorekeeperDocumentComponent } from './storekeeper-document.component';

describe('StorekeeperDocumentComponent', () => {
  let component: StorekeeperDocumentComponent;
  let fixture: ComponentFixture<StorekeeperDocumentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ StorekeeperDocumentComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(StorekeeperDocumentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
