import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { PartModel } from '../../models/part-model';
import { PartsService } from '../../services/parts.service';

@Component({
  selector: 'app-parts-form',
  templateUrl: './parts-form.component.html',
  styleUrls: ['./parts-form.component.scss']
})
export class PartsFormComponent implements OnInit {

  editPart: PartModel;

  loading = true;
  addDisable = false;
  form: FormGroup;
  public newPart: Subject<PartModel> = new Subject();
  get formPartName() { return this.form.get('formPartName'); }
  get formPartDesc() { return this.form.get('formPartDesc'); }

  constructor(
    private _formBuilder: FormBuilder,
    private _partsService: PartsService,
    public bsModalRef: BsModalRef) {
  }

  ngOnInit(): void {
    if(this.editPart) {
      this.form = this._formBuilder.group({
        formPartName: new FormControl(this.editPart.name, [Validators.required, Validators.maxLength(30)]),
        formPartDesc: new FormControl(this.editPart.description, [Validators.required, Validators.maxLength(500)]),
      });
    } else {
      this.form = this._formBuilder.group({
        formPartName: new FormControl('', [Validators.required, Validators.maxLength(30)]),
        formPartDesc: new FormControl('', [Validators.required, Validators.maxLength(500)]),
      });
    }
    this.loading = false;
  }

  savePart() {
    this.form.markAllAsTouched();
    if (!this.form.valid) {
      return;
    }

    this.addDisable = true;
    let model = {
      description: this.formPartDesc?.value,
      name: this.formPartName?.value,
    } as PartModel;

    if(this.editPart) {
      model.id = this.editPart.id;
      this._partsService.editPart(model).subscribe(res => {
        this.newPart.next(model);
        this.bsModalRef.hide();
      });
    } else {
      this._partsService.addPart(model).subscribe(res => {
        model.id = res;
        this.newPart.next(model);
        this.bsModalRef.hide();
      });
    }
  }

}
