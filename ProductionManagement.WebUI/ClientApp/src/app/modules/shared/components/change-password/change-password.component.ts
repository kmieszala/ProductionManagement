import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Subject } from 'rxjs';
import { UsersService } from '../../../users/services/users.service';
import { PatternRegexConst } from '../../common/consts/pattern-regex.const';
import { ChangePasswordStatusEnum } from '../../enums/change-password-status.enum';
import { ChangePasswordModel } from '../../models/change-password-model';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  userId: number;
  public newPass: Subject<string> = new Subject();
  form: FormGroup;

  get formOldPassword() { return this.form.get('formOldPassword'); }
  get formPassword() { return this.form.get('formPassword'); }
  get formRepeatPassword() { return this.form.get('formRepeatPassword'); }

  constructor(
    private _formBuilder: FormBuilder,
    public bsModalRef: BsModalRef,
    private _toastrService: ToastrService,
    private _usersService: UsersService) { }

  ngOnInit(): void {
    this.form = this._formBuilder.group({
      formPassword: new FormControl(null, [Validators.required, Validators.maxLength(50), Validators.pattern(PatternRegexConst.password)]),
      formOldPassword: new FormControl(null, [Validators.required]),
      formRepeatPassword: new FormControl(null)
    });

    this.formRepeatPassword?.setValidators([Validators.required, Validators.maxLength(22), this.createCompareValidator(this.formPassword!, this.formRepeatPassword!)]);
  }

  createCompareValidator(controlOne: AbstractControl, controlTwo: AbstractControl) {
    return () => {
      if (controlOne.value !== controlTwo.value)
        return { match_error: 'Value does not match' };
      return null;
    };
  }

  savePass() {
    this.form?.markAsTouched();
    this.form.markAllAsTouched();
    if(!this.form?.valid) {
      return;
    }

    let model = {
      userId: this.userId,
      oldPassword: this.formOldPassword?.value,
      password: this.formPassword?.value,
      repeatPassword: this.formRepeatPassword?.value,

    } as ChangePasswordModel;
    this._usersService.changePassword(model).subscribe(result => {
      switch(result) {
        case ChangePasswordStatusEnum.PasswordEqualOldPass:
          this._toastrService.warning("Nowe hasło musi się różnić od poprzedniego");
          break;
        case ChangePasswordStatusEnum.PasswordNotEqualRepetPass:
          this._toastrService.warning("Powtórzone hasło jest nieprawidłowe");
          break;
        case ChangePasswordStatusEnum.WrongOldPassword:
          this._toastrService.warning("Stare hasło jest nieprawidłowe");
          break;
        default:
          this._toastrService.success("Hasło zmienione");
          this.bsModalRef.hide();
      }
    });

  }
}
