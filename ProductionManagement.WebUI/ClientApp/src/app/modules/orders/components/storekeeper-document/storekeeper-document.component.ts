import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { PartModel } from '../../../products/models/part-model';
import { TankParts } from '../../../products/models/tank-parts';
import { HttpClientService } from '../../../shared/services/http-client.service';
import { OrderModel } from '../../models/order-model';
import { OrdersService } from '../../services/orders.service';

@Component({
  selector: 'app-storekeeper-document',
  templateUrl: './storekeeper-document.component.html',
  styleUrls: ['./storekeeper-document.component.scss']
})
export class StorekeeperDocumentComponent implements OnInit {

  checkedOrders: OrderModel[];
  parts: PartModel[];
  addedParts: TankParts[] = [];

  form: FormGroup;
  get formPart() { return this.form.get('formPart'); }
  get formPartsAmount() { return this.form.get('formPartsAmount'); }

  constructor(
    private _formBuilder: FormBuilder,
    private _ordersService: OrdersService,
    private _toastr: ToastrService,
    private _httpClient: HttpClientService,
    public _bsModalRef: BsModalRef) { }

  ngOnInit(): void {
    this.form = this._formBuilder.group({
      formPart: new FormControl(null),
      formPartsAmount: new FormControl(1, [Validators.min(1)]),
    });
  }

  deletePart(id: number) {
    this.addedParts = this.addedParts.filter(x => x.id != id);
  }

  addPart() {
    this.formPart?.setValidators([Validators.required]);
    this.formPart?.updateValueAndValidity();
    this.formPart?.markAsTouched();
    this.formPartsAmount?.markAsTouched();

    if(!this.formPart?.valid || !this.formPartsAmount?.valid) {
      return;
    }

    let model = {
      partsId: +this.formPart?.value.id,
      partsName: this.formPart?.value.name,
      partsNumber: +this.formPartsAmount?.value,
    } as TankParts;


    let test = this.addedParts.filter(x => x.partsId == model.partsId);
    if(test.length > 0) {
      test[0].partsNumber += model.partsNumber;
    } else {
      this.addedParts.push(model);
    }

    this.formPart?.setValue(null);
    this.formPartsAmount?.setValue(1);
    this.formPart?.setValidators(null);
    this.formPart?.updateValueAndValidity();
    this.formPart?.markAsUntouched();
    this.formPartsAmount?.markAsUntouched();
  }

  prepareDocument() {
    let ordersIds: number[];
    if((this.checkedOrders && this.checkedOrders.length >= 0) || (this.addedParts && this.addedParts.length >= 0)) {
      ordersIds = this.checkedOrders.map(x => x.id);
    } else {
      this._toastr.warning("Nie wybrano żadnej części ani zamówienia!");
      return;
    }

    this._ordersService.downloadFile(ordersIds ,this.addedParts).subscribe(result => {
      console.log(result);
      this._httpClient.saveXlsmFile(result, "DokumentDlaMagazyniera.xls");
    });

  }
}
