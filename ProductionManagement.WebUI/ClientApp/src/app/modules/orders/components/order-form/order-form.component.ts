import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { TankModel } from '../../../products/models/tank-model';
import { OrderModel } from '../../models/order-model';

@Component({
  selector: 'app-order-form',
  templateUrl: './order-form.component.html',
  styleUrls: ['./order-form.component.scss']
})
export class OrderFormComponent implements OnInit {

  editedOrder: OrderModel;
  loading = true;
  addDisable = false;
  form: FormGroup;
  tanks: TankModel[];
  public newPart: Subject<OrderModel> = new Subject();
  get formOrderName() { return this.form.get('formOrderName'); }
  get formTank() { return this.form.get('formTank'); }
  get formDescription() { return this.form.get('formDescription'); }

  constructor(
    private _formBuilder: FormBuilder,
    public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
    if(this.editedOrder) {
      this.form = this._formBuilder.group({
        formOrderName: new FormControl(this.editedOrder.orderName, [Validators.required, Validators.maxLength(30)]),
        formTank: new FormControl(null),
        formDescription: new FormControl(this.editedOrder.description),
      });
    } else {
      this.form = this._formBuilder.group({
        formOrderName: new FormControl('', [Validators.required, Validators.maxLength(30)]),
        formTank: new FormControl(null),
        formDescription: new FormControl(null),
      });
    }
    this.loading = false;
  }

  saveOrder(){

  }
}
