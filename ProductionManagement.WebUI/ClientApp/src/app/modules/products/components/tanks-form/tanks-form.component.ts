import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { PartModel } from '../../models/part-model';
import { TankModel } from '../../models/tank-model';
import { TankParts } from '../../models/tank-parts';
import { TanksService } from '../../services/tanks.service';

@Component({
  selector: 'app-tanks-form',
  templateUrl: './tanks-form.component.html',
  styleUrls: ['./tanks-form.component.scss']
})
export class TanksFormComponent implements OnInit {

  parts: PartModel[];
  editTank: TankModel;
  tankParts: TankParts[] = [];
  loading = true;
  form: FormGroup;
  addDisable = false;
  showValidError = false;

  public newTank: Subject<TankModel> = new Subject();

  get formTankName() { return this.form.get('formTankName'); }
  get formDaysNumber() { return this.form.get('formDaysNumber'); }
  get formPart() { return this.form.get('formPart'); }
  get formPartsAmount() { return this.form.get('formPartsAmount'); }

  constructor(
    private _formBuilder: FormBuilder,
    private _tanksService: TanksService,
    public bsModalRef: BsModalRef) {
    }

  ngOnInit(): void {
    if(this.editTank) {
      this.form = this._formBuilder.group({
        formTankName: new FormControl(this.editTank.name, [Validators.required, Validators.maxLength(30)]),
        formDaysNumber: new FormControl(this.editTank.productionDays, [Validators.required, Validators.max(100)]),
        formPart: new FormControl(null),
        formPartsAmount: new FormControl(1, [Validators.min(1)]),
      });
      this.tankParts = this.editTank.parts;
    } else {
      this.form = this._formBuilder.group({
        formTankName: new FormControl('', [Validators.required, Validators.maxLength(30)]),
        formDaysNumber: new FormControl('', [Validators.required, Validators.max(100)]),
        formPart: new FormControl(null),
        formPartsAmount: new FormControl(1, [Validators.min(1)]),
      });
    }
    this.loading = false;
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
      tankId: 0,
    } as TankParts;
    // for new part I add 1000, for deleting
    model.id = 90000 + model.partsId;

    // chcek if part is on list, I don't add new record
    let test = this.tankParts.filter(x => x.partsId == model.partsId);
    if(test.length > 0){
      test[0].partsNumber += model.partsNumber;
    } else {
      this.tankParts.push(model);
      this.showValidError = false;
    }

    this.partUntouched();
  }

  partUntouched() {
    this.formPart?.setValue(null);
    this.formPartsAmount?.setValue(1);
    this.formPart?.setValidators(null);
    this.formPart?.updateValueAndValidity();
    this.formPart?.markAsUntouched();
    this.formPartsAmount?.markAsUntouched();
  }

  deletePart(model: TankParts) {
    this.tankParts = this.tankParts.filter(x => x.id != model.id);
  }

  saveTank() {
    this.partUntouched();
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }
    if (this.tankParts.length == 0) {
      this.showValidError = true;
      return;
    }

    this.addDisable = true;
    this.tankParts.forEach(x => {
      if (x.id > 90000) {
        x.id = 0;
      }
    })

    let model = {
      id: this.editTank?.id,
      description: this.editTank?.description,
      name: this.formTankName?.value,
      productionDays: +this.formDaysNumber?.value,
      active: true,
      parts: this.tankParts,
    } as TankModel;

    if(this.editTank) {
      this._tanksService.editTank(model).subscribe(result => {
        this.newTank.next(model);
        this.bsModalRef.hide();
      });
    } else {
      this._tanksService.addTank(model).subscribe(result => {
        model.id = result;
        this.newTank.next(model);
        this.bsModalRef.hide();
      });
    }
  }
}
