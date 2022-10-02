import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { forkJoin, Subscription } from 'rxjs';
import { ProductionLine } from '../../../production-line/models/production-line';
import { ProductionLineService } from '../../../production-line/services/production-line.service';
import { PartModel } from '../../models/part-model';
import { TankModel } from '../../models/tank-model';
import { PartsService } from '../../services/parts.service';
import { TanksService } from '../../services/tanks.service';
import { TanksFormComponent } from '../tanks-form/tanks-form.component';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationDialogComponent } from '../../../shared/components/confirmation-dialog/confirmation-dialog.component';
import { ConfirmationDialogOptions } from '../../../shared/models/confirmation-dialog-options';

@Component({
  selector: 'app-tanks-list',
  templateUrl: './tanks-list.component.html',
  styleUrls: ['./tanks-list.component.scss']
})
export class TanksListComponent implements OnInit {

  statusFilter: boolean = true;
  subscriptions: Subscription[] = [];
  bsModalRef?: BsModalRef;
  tanks: TankModel[];
  parts: PartModel[];
  productionLines: ProductionLine[];
  loading = true;
  constructor(
    private _modalService: BsModalService,
    private _tanksService: TanksService,
    private _partsService: PartsService,
    private _productionLineService: ProductionLineService,
    private _toastr: ToastrService,
  ) {}

  ngOnInit(): void {
    forkJoin(
    {
      tanks: this._tanksService.getTanks(true),
      parts: this._partsService.getParts(),
      productionLines: this._productionLineService.getLines()
    })
    .subscribe(result => {
      this.tanks = result.tanks;
      this.productionLines = result.productionLines;
      this.parts = result.parts;
      this.loading = false;
    });
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
    this.subscriptions = [];
  }

  refresh(model: boolean) {
    this.loading = true;
    this._tanksService.getTanks(model)
    .subscribe(result => {
      this.tanks = result;
      this.loading = false;
    });
  }

  showDetails(model: TankModel) {
    model.showDetails = !model.showDetails;
  }

  activeTank(model: TankModel) {
    this._tanksService.activeTank(model.id).subscribe(result => {
      if(result) {
      this.tanks = this.tanks.filter(x => x.id != model.id);
      this._toastr.success('Zbiornik aktywny.');
      } else {
        this._toastr.error('Wystąpił błąd podczas aktywacji zbiornika, skontaktuj się z administratorem');
      }
    });
  }

  deactiveTank(model: TankModel) {
    const initialState: ModalOptions = {
      initialState: {
        options: {
          title: 'Czy na pewno chcesz deaktywować zbiornik ' + model.name,
          yesButton: true,
          noButton: true
        } as ConfirmationDialogOptions
      }
    };
    this.bsModalRef = this._modalService.show(ConfirmationDialogComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.answear.subscribe((res: boolean) => {
      if(res) {
        this._tanksService.deactiveTank(model.id).subscribe(result => {
          if(result) {
          this.tanks = this.tanks.filter(x => x.id != model.id);
          this._toastr.success('Zbiornik deaktywowany.');
          } else {
            this._toastr.error('Wystąpił błąd podczas deaktywacji zbiornika, skontaktuj się z administratorem');
          }
        });
      }
    }));
  }

  editTank(model: TankModel) {
    const initialState: ModalOptions = {
      initialState: {
        parts: this.parts,
        editTank: model,
        productionLines: this.productionLines
      },
      class: 'modal-lg',
    };

    this.bsModalRef = this._modalService.show(TanksFormComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.newTank.subscribe((res: TankModel) => {
      let tmp = this.tanks.filter(x => x.id == res.id)[0];
      tmp.name = res.name;
      tmp.productionDays = res.productionDays;
      tmp.parts = res.parts;
      tmp.description = res.description;
      tmp.productionLines = res.productionLines;
    }));
  }

  addNewTank() {
    const initialState: ModalOptions = {
      initialState: {
        parts: this.parts,
        productionLines: this.productionLines
      },
      class: 'modal-lg',
    };

    this.bsModalRef = this._modalService.show(TanksFormComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.newTank.subscribe((res: TankModel) => {
      this.tanks.push(res); // add new tank to tanks list
    }));
  }
}
