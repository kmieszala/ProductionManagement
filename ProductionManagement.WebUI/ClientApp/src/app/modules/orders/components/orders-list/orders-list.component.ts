import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { forkJoin, Subscription } from 'rxjs';
import { TankModel } from '../../../products/models/tank-model';
import { TanksService } from '../../../products/services/tanks.service';
import { OrderModel } from '../../models/order-model';
import { OrderFormComponent } from '../order-form/order-form.component';
import { OrdersService } from '../../services/orders.service';
import { StorekeeperDocumentComponent } from '../storekeeper-document/storekeeper-document.component';
import { PartsService } from '../../../products/services/parts.service';
import { PartModel } from '../../../products/models/part-model';
import {CdkDragDrop, moveItemInArray} from '@angular/cdk/drag-drop';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.scss']
})
export class OrdersListComponent implements OnInit {

  test = 'red';
  subscriptions: Subscription[] = [];
  bsModalRef?: BsModalRef;
  orders: OrderModel[];
  tanks: TankModel[];
  parts: PartModel[];

  constructor(
    private _modalService: BsModalService,
    private _ordersService: OrdersService,
    private _partsService: PartsService,
    private _tanksService: TanksService,
    ) { }

  ngOnInit(): void {
    forkJoin(
      {
        tanks: this._tanksService.getTanks(true),
        orders: this._ordersService.getOrders()
      })
      .subscribe(result => {
        this.tanks = result.tanks;
        this.orders = result.orders;
      });
  }

  editOrder(model: OrderModel) {
    const initialState: ModalOptions = {
      initialState: {
        tanks: this.tanks,
        editedOrder: model
      },
      ignoreBackdropClick: true
    };

    this.bsModalRef = this._modalService.show(OrderFormComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.newOrder.subscribe((res: OrderModel) => {
      let tmp = this.orders.filter(x => x.id == res.id)[0];
      tmp.color = res.color;
      tmp.description = res.description;
      tmp.orderName = res.orderName;
      tmp.productionDays = res.productionDays;
      tmp.tankName = res.tankName;
      tmp.productionLinesNames = res.productionLinesNames;
    }));
  }

  addNewOrder() {
    const initialState: ModalOptions = {
      initialState: {
        tanks: this.tanks
      },
      ignoreBackdropClick: true
    };
    this.bsModalRef = this._modalService.show(OrderFormComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.newOrder.subscribe((res: OrderModel) => {
      this.orders.push(res); // add new order to orders list
    }));
  }

  forStorekeeper() {

    if(this.parts == null) {
      this._partsService.getParts().subscribe(result => {
        this.parts = result;
        this.openStorekeeperDocumentComponent();
      });
    } else {
      this.openStorekeeperDocumentComponent();
    }
  }

  openStorekeeperDocumentComponent() {
    let checkedOrders = this.orders.filter(x => x.checked);

    const initialState: ModalOptions = {
      initialState: {
        checkedOrders: checkedOrders,
        parts: this.parts
      },
      ignoreBackdropClick: true
    };

    this.bsModalRef = this._modalService.show(StorekeeperDocumentComponent, initialState);
  }

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.orders, event.previousIndex, event.currentIndex);
    this.orders.forEach((group, idx) => {
      group.order = idx + 1;
    });
  }
}
