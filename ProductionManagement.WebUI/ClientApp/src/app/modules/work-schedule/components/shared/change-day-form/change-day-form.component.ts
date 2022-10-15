import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { ProductionDaysBasic } from '../../../models/production-days-basic';
import { WorkScheduleService } from '../../../services/work-schedule.service';

@Component({
  selector: 'app-change-day-form',
  templateUrl: './change-day-form.component.html',
  styleUrls: ['./change-day-form.component.scss']
})
export class ChangeDayFormComponent implements OnInit {

  productionDay: ProductionDaysBasic;
  option: number;

  public editedDay: Subject<boolean> = new Subject();

  constructor(
    private _workScheduleService: WorkScheduleService,
    public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }

  save() {
    this.productionDay.productionLineName = this.option.toString();
    this._workScheduleService.changeWorkDay(this.productionDay).subscribe(res => {
      this.editedDay.next(true);
    });

    this.bsModalRef.hide();
  }


}
