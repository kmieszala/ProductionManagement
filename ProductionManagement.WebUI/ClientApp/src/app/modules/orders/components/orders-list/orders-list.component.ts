import { Component, OnInit } from '@angular/core';
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
import { CdkDragDrop, moveItemInArray } from '@angular/cdk/drag-drop';
import { Sequence } from '../../models/sequence';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

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
  ordersToMove: OrderModel[];
  tanks: TankModel[];
  parts: PartModel[];
  checkedButton = false;
  updateSequenceButton = false;
  generateButton = false;
  generating = false;

  constructor(
    private _modalService: BsModalService,
    private _ordersService: OrdersService,
    private _partsService: PartsService,
    private _toastr: ToastrService,
    private _router: Router,
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
        this.orders = result.orders.filter(x => x.startDate != null);
        this.ordersToMove = result.orders.filter(x => x.startDate == null);
      });
  }

  checkAll() {
    this.checkedButton = !this.checkedButton;
    this.orders.forEach(x => x.checked = this.checkedButton);
    this.ordersToMove.forEach(x => x.checked = this.checkedButton);
    if(!this.checkedButton) {
      this.generateButton = false;
    }
  }

  checkItem() {
    setTimeout(() => {
      this.generateButton = this.ordersToMove.find(x => x.checked) != null;
      if(!this.generateButton) {
        this.checkedButton = false;
      }
    }, 10);
  }

  generateCalendar() {
    let orders = this.ordersToMove.filter(x => x.checked);
    this._ordersService.generateCalendar(orders).subscribe(result => {
      this._router.navigate(['production-days']);
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
      let tmp = res.startDate == null
        ? this.ordersToMove.filter(x => x.id == res.id)[0]
        : this.orders.filter(x => x.id == res.id)[0];
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
      this.ordersToMove.push(res); // add new order to orders list
    }));
  }

  updateSequence() {
    var model = this.ordersToMove.filter(x => x.startDate == null).map(x => {
      return { id: x.id, sequence: x.sequence } as Sequence;
    });

    this._ordersService.updateSequenceOrders(model).subscribe(result => {
      if(result) {
        this.updateSequenceButton = false;
        this._toastr.success("Kolejność zaktualizowana");
      } else {
        this._toastr.error("Coś poszło nie tak, proszę spróbować za później")
      }
    });
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
    let tmp1 = this.orders.filter(x => x.checked);
    let tmp2 = this.ordersToMove.filter(x => x.checked);

    let checkedOrders = [ ...tmp1, ...tmp2];
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
    moveItemInArray(this.ordersToMove, event.previousIndex, event.currentIndex);

    this.ordersToMove.forEach((group, idx) => {
      group.sequence = idx + 1;
    });
    this.updateSequenceButton = true;
  }
}
