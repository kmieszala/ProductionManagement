import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { PartModel } from '../../models/part-model';
import { PartsService } from '../../services/parts.service';

@Component({
  selector: 'app-parts-form',
  templateUrl: './parts-form.component.html',
  styleUrls: ['./parts-form.component.scss']
})
export class PartsFormComponent implements OnInit {

  loading = true;
  addDisable = false;
  form: FormGroup;
  get formPartName() { return this.form.get('formPartName'); }
  get formPartDesc() { return this.form.get('formPartDesc'); }

  constructor(
    private _formBuilder: FormBuilder,
    private _partsService: PartsService,
    public bsModalRef: BsModalRef) {
    this.form = this._formBuilder.group({
      formPartName: new FormControl('', [Validators.required, Validators.maxLength(30)]),
      formPartDesc: new FormControl('', [Validators.required, Validators.maxLength(500)]),
    });
  }

  ngOnInit(): void {
    this.form = this._formBuilder.group({
      formPartName: new FormControl('', [Validators.required, Validators.maxLength(30)]),
      formPartDesc: new FormControl('', [Validators.required, Validators.maxLength(500)]),
    });
    this.loading = false;
  }

  addPart() {
    this.form.markAllAsTouched();
    console.log(this.formPartName?.errors)
    if (!this.form.valid) {
      return;
    }

    this.addDisable = true;
    let model = {
      description: this.formPartDesc?.value,
      name: this.formPartName?.value,
    } as PartModel;

    this._partsService.addPart(model).subscribe(res => {
      this.bsModalRef.hide();
    });
  }

}
