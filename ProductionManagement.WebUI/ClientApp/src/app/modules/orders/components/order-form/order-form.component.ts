import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { ProductionLineTank } from '../../../products/models/production-line-tank';
import { TankModel } from '../../../products/models/tank-model';
import { OrderModel } from '../../models/order-model';
import { OrdersService } from '../../services/orders.service';

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
  public newOrder: Subject<OrderModel> = new Subject();
  get formOrderName() { return this.form.get('formOrderName'); }
  get formTank() { return this.form.get('formTank'); }
  get formDescription() { return this.form.get('formDescription'); }
  get formColor() { return this.form.get('formColor'); }

  constructor(
    private _formBuilder: FormBuilder,
    private _toastr: ToastrService,
    private _ordersService: OrdersService,
    public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
    if(this.editedOrder) {
      this.form = this._formBuilder.group({
        formOrderName: new FormControl(this.editedOrder.orderName, [Validators.required, Validators.maxLength(30)]),
        formTank: new FormControl(this.editedOrder.tankId),
        formDescription: new FormControl(this.editedOrder.description, [Validators.maxLength(500)]),
        formColor: new FormControl(this.editedOrder.color),
      });
    } else {
      this.form = this._formBuilder.group({
        formOrderName: new FormControl('', [Validators.required, Validators.maxLength(30)]),
        formTank: new FormControl(null),
        formDescription: new FormControl('', [Validators.maxLength(500)]),
        formColor: new FormControl(this.getRandomColor()),
      });
    }

    this.loading = false;
  }

  getRandomColor() {
    var color = Math.floor(0x1000000 * Math.random()).toString(16);
    return '#' + ('000000' + color).slice(-6);
  }

  saveOrder() {
    if(!this.form?.valid) {
      return;
    }

    let tank = this.tanks.filter(x => x.id == this.formTank?.value)[0];
    let productionLines = tank.productionLines != null ? (tank.productionLines as ProductionLineTank[]).map(x => x.productionLineName).join(', ') : null;
    //let productionLines = this.formTank?.value.productionLines != null ? (this.formTank?.value.productionLines as ProductionLineTank[]).map(x => x.productionLineName).join(', ') : null;

    let model = {
      id: this.editedOrder?.id,
      orderName: this.formOrderName?.value,
      description: this.formDescription?.value,
      tankId: tank.id,
      tankName: tank.name,
      productionDays: tank.productionDays,
      productionLinesNames: productionLines,
      color: this.formColor?.value,
    } as OrderModel;

    if(this.editedOrder) {
      this._ordersService.editOrder(model).subscribe(result => {
        this.newOrder.next(model);
        this.bsModalRef.hide();
      });
    } else {
      this._ordersService.addOrder(model).subscribe(result => {
        if(result) {
          model.id = result;
          this.newOrder.next(model);
          this.bsModalRef.hide();
        } else {
          this._toastr.error("Coś poszło nie tak, proszę spróbować za później")
        }
      });
    }
  }
}
