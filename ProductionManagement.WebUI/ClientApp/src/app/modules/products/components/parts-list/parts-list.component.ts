import { Component, OnDestroy, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { PartModel } from '../../models/part-model';
import { PartsService } from '../../services/parts.service';
import { PartsFormComponent } from '../parts-form/parts-form.component';

@Component({
  selector: 'app-parts-list',
  templateUrl: './parts-list.component.html',
  styleUrls: ['./parts-list.component.scss']
})
export class PartsListComponent implements OnInit, OnDestroy {

  subscriptions: Subscription[] = [];
  bsModalRef?: BsModalRef;
  parts: PartModel[];
  loading = true;
  constructor(
    private modalService: BsModalService,
    private _partsService: PartsService
  ) {}

  ngOnInit(): void {
    this._partsService.getParts().subscribe(result => {
      this.parts = result;
      this.loading = false;
    })
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
    this.subscriptions = [];
  }

  editPart(model: PartModel) {
    const initialState: ModalOptions = {
      initialState: {
        editPart: model
      }
    };

    this.bsModalRef = this.modalService.show(PartsFormComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.newPart.subscribe((res: PartModel) => {
      let tmp = this.parts.filter(x => x.id == res.id)[0];
      tmp.name = res.name;
      tmp.description = res.description;
    }));
  }

  addNewPart() {
    this.bsModalRef = this.modalService.show(PartsFormComponent);
    this.subscriptions.push(this.bsModalRef.content.newPart.subscribe((res: PartModel) => {
      this.parts.push(res); // add new part to parts list
    }));
  }
}
