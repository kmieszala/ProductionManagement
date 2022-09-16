import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { forkJoin, Subscription } from 'rxjs';
import { PartModel } from '../../models/part-model';
import { TankModel } from '../../models/tank-model';
import { PartsService } from '../../services/parts.service';
import { TanksService } from '../../services/tanks.service';
import { TanksFormComponent } from '../tanks-form/tanks-form.component';

@Component({
  selector: 'app-tanks-list',
  templateUrl: './tanks-list.component.html',
  styleUrls: ['./tanks-list.component.scss']
})
export class TanksListComponent implements OnInit {

  subscriptions: Subscription[] = [];
  bsModalRef?: BsModalRef;
  tanks: TankModel[];
  parts: PartModel[];
  loading = true;
  constructor(
    private _modalService: BsModalService,
    private _tanksService: TanksService,
    private _partsService: PartsService
  ) {}

  ngOnInit(): void {
    forkJoin(
    {
      tanks: this._tanksService.getTanks(),
      parts: this._partsService.getParts()
    })
    .subscribe(result => {
      this.tanks = result.tanks;
      this.parts = result.parts;
      this.loading = false;
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
    this.subscriptions = [];
  }

  showDetails(model: TankModel) {
    model.showDetails = !model.showDetails;
  }

  editTank(model: TankModel) {
    const initialState: ModalOptions = {
      initialState: {
        parts: this.parts,
        editTank: model
      }
    };

    this.bsModalRef = this._modalService.show(TanksFormComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.newTank.subscribe((res: TankModel) => {
      let tmp = this.tanks.filter(x => x.id == res.id)[0];
      tmp.name = res.name;
      tmp.productionDays = res.productionDays;
      tmp.parts = res.parts;
    }));
  }

  addNewTank() {
    const initialState: ModalOptions = {
      initialState: {
        parts: this.parts
      }
    };

    this.bsModalRef = this._modalService.show(TanksFormComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.newTank.subscribe((res: TankModel) => {
      this.tanks.push(res); // add new tank to tanks list
    }));
  }
}
