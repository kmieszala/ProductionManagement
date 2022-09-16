import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { TankModel } from '../../../products/models/tank-model';
import { TanksService } from '../../../products/services/tanks.service';
import { OrderModel } from '../../models/order-model';
import { OrderFormComponent } from '../order-form/order-form.component';

@Component({
  selector: 'app-orders-list',
  templateUrl: './orders-list.component.html',
  styleUrls: ['./orders-list.component.scss']
})
export class OrdersListComponent implements OnInit {

  subscriptions: Subscription[] = [];
  bsModalRef?: BsModalRef;
  orders: OrderModel[];
  tanks: TankModel[];
  form: FormGroup;
  get formOrderName() { return this.form.get('formOrderName'); }
  get formTank() { return this.form.get('formTank'); }
  get formDescription() { return this.form.get('formDescription'); }

  constructor(
    private _modalService: BsModalService,
    private _formBuilder: FormBuilder,
    private _tanksService: TanksService,
    ) { }

  ngOnInit(): void {
    this._tanksService.getTanks().subscribe(result => this.tanks = result);

    this.form = this._formBuilder.group({
      formOrderName: new FormControl('', [Validators.required, Validators.maxLength(150)]),
      formTank: new FormControl(null),
      formDescription: new FormControl(null),
    });
  }

  addNewOrder() {
    const initialState: ModalOptions = {
      initialState: {
        tanks: this.tanks
      }
    };
    this.bsModalRef = this._modalService.show(OrderFormComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.newPart.subscribe((res: OrderModel) => {
      this.orders.push(res); // add new order to orders list
    }));
  }
}
