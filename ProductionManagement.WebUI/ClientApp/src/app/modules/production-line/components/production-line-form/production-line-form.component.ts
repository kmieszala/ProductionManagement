import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { TankModel } from '../../../products/models/tank-model';
import { LineTank } from '../../models/line-tank';
import { ProductionLine } from '../../models/production-line';
import { ProductionLineService } from '../../services/production-line.service';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { defineLocale, plLocale } from 'ngx-bootstrap/chronos';

@Component({
  selector: 'app-production-line-form',
  templateUrl: './production-line-form.component.html',
  styleUrls: ['./production-line-form.component.scss']
})
export class ProductionLineFormComponent implements OnInit {

  editLine: ProductionLine;
  tanks: TankModel[];
  loading = true;
  form: FormGroup;
  addDisable = false;
  listTanks: LineTank[] = [];
  dateToShow: string;

  public newTank: Subject<ProductionLine> = new Subject();

  get formLineName() { return this.form.get('formLineName'); }
  get formActiveLine() { return this.form.get('formActiveLine'); }
  get formTank() { return this.form.get('formTank'); }
  get formStartDate() { return this.form.get('formStartDate'); }

  constructor(
    private _productionLineService: ProductionLineService,
    private _formBuilder: FormBuilder,
    public bsModalRef: BsModalRef) {}

  ngOnInit(): void {
    if(this.editLine) {
      this.dateToShow = new Date(this.editLine.startDate).toLocaleDateString();
      this.form = this._formBuilder.group({
        formLineName: new FormControl(this.editLine.name, [Validators.required, Validators.maxLength(30)]),
        formActiveLine: new FormControl(this.editLine.active, [Validators.required, Validators.max(100)]),
        formTank: new FormControl(null),
        formStartDate: new FormControl(this.editLine.startDate.toString(), [Validators.required]),
      });
      this.listTanks = this.editLine.tanks;
    } else {
      this.dateToShow = new Date().toLocaleDateString();
      this.form = this._formBuilder.group({
        formLineName: new FormControl('', [Validators.required, Validators.maxLength(30)]),
        formActiveLine: new FormControl(true, [Validators.required, Validators.max(100)]),
        formTank: new FormControl(null),
        formStartDate: new FormControl(null, [Validators.required]),
      });
    }
    this.loading = false;
  }

  dateChange(date: Date | null) {
    this.formStartDate?.setValue(date);
  }

  addTank() {
    this.formTank?.setValidators([Validators.required]);
    this.formTank?.updateValueAndValidity();
    this.formTank?.markAsTouched();

    if(!this.formTank?.valid) {
      return;
    }

    let model = {
      tankName: this.formTank?.value.name,
      tankId: +this.formTank?.value.id,
    } as LineTank;

    // chcek if tank is on list, I don't add new record
    let test = this.listTanks.filter(x => x.tankId == model.tankId);
    if(test.length == 0) {
      this.listTanks.push(model);
    }

    this.tankUntouched();
  }

  tankUntouched() {
    this.formTank?.setValue(null);
    this.formTank?.setValidators(null);
    this.formTank?.updateValueAndValidity();
    this.formTank?.markAsUntouched();
  }

  deleteTank(model: LineTank) {
    this.listTanks = this.listTanks.filter(x => x.tankId != model.tankId);
  }

  saveLine() {
    this.tankUntouched();
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }

    this.addDisable = true;

    let model = {
      id: this.editLine?.id,
      name: this.formLineName?.value,
      active: this.formActiveLine?.value,
      tanks: this.listTanks,
      startDate: this.formStartDate?.value,
    } as ProductionLine;

    if(this.editLine) {
      this._productionLineService.editLine(model).subscribe(result => {
        this.newTank.next(model);
        this.bsModalRef.hide();
      });
    } else {
      this._productionLineService.addLine(model).subscribe(result => {
        model.id = result;
        this.newTank.next(model);
        this.bsModalRef.hide();
      });
    }
  }
}
