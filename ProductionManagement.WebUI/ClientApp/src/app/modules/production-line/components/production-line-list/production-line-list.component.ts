import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { forkJoin, Subscription } from 'rxjs';
import { TankModel } from '../../../products/models/tank-model';
import { TanksService } from '../../../products/services/tanks.service';
import { ProductionLine } from '../../models/production-line';
import { ProductionLineService } from '../../services/production-line.service';
import { ProductionLineFormComponent } from '../production-line-form/production-line-form.component';

@Component({
  selector: 'app-production-line-list',
  templateUrl: './production-line-list.component.html',
  styleUrls: ['./production-line-list.component.scss']
})
export class ProductionLineListComponent implements OnInit {

  subscriptions: Subscription[] = [];
  bsModalRef?: BsModalRef;
  productionLines: ProductionLine[];
  tanks: TankModel[];

  constructor(
    private _modalService: BsModalService,
    private _productionLineService: ProductionLineService,
    private _tanksService: TanksService,) { }

  ngOnInit(): void {
    forkJoin(
      {
        tanks: this._tanksService.getTanks(),
        lines: this._productionLineService.getLines()
      })
      .subscribe(result => {
        this.tanks = result.tanks;
        this.productionLines = result.lines;
        //this.loading = false;
      });
  }

  showDetails(model: ProductionLine) {
    model.showDetails = !model.showDetails;
  }

  addNewLine() {
    const initialState: ModalOptions = {
      initialState: {
        tanks: this.tanks
      }
    };

    this.bsModalRef = this._modalService.show(ProductionLineFormComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.newTank.subscribe((res: TankModel) => {
      this.tanks.push(res); // add new tank to tanks list
    }));
  }

  editLine(model: ProductionLine) {

  }
}
