import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { forkJoin, Subscription } from 'rxjs';
import { ProductionDays } from '../../models/production-days';
import { ProductionDaysBasic } from '../../models/production-days-basic';
import { WorkScheduleService } from '../../services/work-schedule.service';
import { ChangeDayFormComponent } from '../shared/change-day-form/change-day-form.component';
import { ProductionLineService } from 'src/app/modules/production-line/services/production-line.service';

@Component({
  selector: 'app-production-days-list',
  templateUrl: './production-days-list.component.html',
  styleUrls: ['./production-days-list.component.scss']
})
export class ProductionDaysListComponent implements OnInit, OnDestroy {

  productionDays: ProductionDays[];
  headers: string[];
  loading = true;
  subscriptions: Subscription[] = [];
  bsModalRef?: BsModalRef;
  form: FormGroup;
  get formStartDate() { return this.form.get('formStartDate'); }
  get formStopDate() { return this.form.get('formStopDate'); }

  constructor(
    private _formBuilder: FormBuilder,
    private _workScheduleService: WorkScheduleService,
    private _productionLineService: ProductionLineService,
    private _modalService: BsModalService,) { }

  ngOnInit(): void {
    let dateFrom =  new Date();
    let dateTo =  new Date();
    dateFrom.setDate( dateFrom.getDate() - 7 );
    dateTo.setDate( dateTo.getDate() + 20 );

    this.form = this._formBuilder.group({
      formStartDate: new FormControl(dateFrom.toLocaleDateString(), [Validators.required]),
      formStopDate: new FormControl(dateTo.toLocaleDateString(), [Validators.required]),
    });

    forkJoin(
      {
        prodDays: this._workScheduleService.getProductionDays(dateFrom, dateTo),
        headers: this._productionLineService.getProductionLines()
      })
      .subscribe(result => {
        this.productionDays = result.prodDays;
        this.headers = result.headers.map(x => x.value);
        this.loading = false;
        this.formStartDate?.setValue(dateFrom);
        this.formStopDate?.setValue(dateTo);
      });
  }

  filter() {
    let dateFrom =  new Date(this.formStartDate?.value);
    let dateTo =  new Date(this.formStopDate?.value);
    this.loading = true;

    this._workScheduleService.getProductionDays(dateFrom, dateTo).subscribe(result => {
      this.productionDays = result;
      this.loading = false;
    });
  }

  changeFreeDay(item: ProductionDaysBasic) {
    const initialState: ModalOptions = {
      initialState: {
        productionDay: item
      }
    };

    this.bsModalRef = this._modalService.show(ChangeDayFormComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.editedDay.subscribe((res: boolean) => {
      this.filter();
    }));
  }

  changeWorkDay(item: ProductionDaysBasic) {
    const initialState: ModalOptions = {
      initialState: {
        productionDay: item
      }
    };

    this.bsModalRef = this._modalService.show(ChangeDayFormComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.editedDay.subscribe((res: boolean) => {
      this.filter();
    }));
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
  }

}
