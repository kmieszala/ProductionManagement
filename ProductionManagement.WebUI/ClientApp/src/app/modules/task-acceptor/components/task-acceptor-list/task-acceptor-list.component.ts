import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Subscription, forkJoin } from 'rxjs';
import { OrdersService } from 'src/app/modules/orders/services/orders.service';
import { ProductionLineService } from 'src/app/modules/production-line/services/production-line.service';
import { DictModel } from 'src/app/modules/shared/models/dict-model';
import { WorkScheduleService } from 'src/app/modules/work-schedule/services/work-schedule.service';
import { GetUserCodeComponent } from '../get-user-code/get-user-code.component';

@Component({
  selector: 'app-task-acceptor-list',
  templateUrl: './task-acceptor-list.component.html',
  styleUrls: ['./task-acceptor-list.component.scss']
})
export class TaskAcceptorListComponent implements OnInit {

  loading = true;
  headers: string[];
  tasks: any[];
  colSize: number;
  bsModalRef?: BsModalRef;
  subscriptions: Subscription[] = [];
  
  constructor(
    private _productionLineService: ProductionLineService,
    private _toastr: ToastrService,
    private _ordersService: OrdersService,
    private _modalService: BsModalService,) { }

  ngOnInit(): void {
    forkJoin(
      {
        headers: this._productionLineService.getProductionLines(),
        tasks: this._ordersService.getCurrentOrders()
      })
      .subscribe(result => {
        this.tasks = result.tasks;
        this.headers = result.headers.map(x => x.value);
        this.colSize = 12 / this.headers.length;
        this.loading = false;
      });
  }

  endTask(item: DictModel) {
    this.bsModalRef = this._modalService.show(GetUserCodeComponent);
    this.subscriptions.push(this.bsModalRef.content.answear.subscribe((res: string) => {
      if(res) {
        this.loading = true;
        var prop = { id: item.id, value: res } as DictModel;
        this._ordersService.markOrderAsDone(prop).subscribe(result => {
          if(result) {
            this._ordersService.getCurrentOrders().subscribe(tasks => {
              this.tasks = tasks;
              this.loading = false;
              this._toastr.success(`Zadanie ${item.value} oznaczone jako wykonane`);
            });
          } else {
            this.loading = false;
            this._toastr.error('Wystąpił błąd, skontaktuj się z administratorem');
          }
        });
      }
    }));
  }

}
