import { Component, OnDestroy, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { PartsFormComponent } from '../parts-form/parts-form.component';

@Component({
  selector: 'app-parts-list',
  templateUrl: './parts-list.component.html',
  styleUrls: ['./parts-list.component.scss']
})
export class PartsListComponent implements OnInit, OnDestroy {

  subscriptions: Subscription[] = [];
  bsModalRef?: BsModalRef;
  constructor(private modalService: BsModalService) {}

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
    this.subscriptions = [];
  }

  addNewPart() {
    this.bsModalRef = this.modalService.show(PartsFormComponent);
    this.bsModalRef.onHide?.subscribe(x => console.log(x));
  }
}
