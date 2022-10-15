import { Component, OnDestroy, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { forkJoin, Subscription } from 'rxjs';
import { ProductionDays } from '../../models/production-days';
import { ProductionDaysBasic } from '../../models/production-days-basic';
import { WorkScheduleService } from '../../services/work-schedule.service';
import { ChangeDayFormComponent } from '../shared/change-day-form/change-day-form.component';

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

  constructor(
    private _workScheduleService: WorkScheduleService,
    private _modalService: BsModalService,) { }

  ngOnInit(): void {
    let dateFrom =  new Date(2022, 7, 30, 12);
    let dateTo =  new Date(2022, 11, 1, 12);

    forkJoin(
      {
        prodDays: this._workScheduleService.getProductionDays(dateFrom, dateTo),
        headers: this._workScheduleService.getCalendarHeaders()
      })
      .subscribe(result => {
        this.productionDays = result.prodDays;
        this.headers = result.headers;
        this.loading = false;
      });
  }

  filter() {
    let dateFrom =  new Date(2022, 7, 30, 12);
    let dateTo =  new Date(2022, 11, 1, 12);
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
