import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { PatternRegexConst } from '../../shared/common/consts/pattern-regex.const';
import { UserStatusEnum } from '../../shared/enums/user-status.enum';
import { DictModel } from '../../shared/models/dict-model';
import { UserModel } from '../models/user-model';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-users-form',
  templateUrl: './users-form.component.html',
  styleUrls: ['./users-form.component.scss']
})
export class UsersFormComponent implements OnInit {

  roles: DictModel[];
  selectedRoles: DictModel[];
  editUser: UserModel;
  form: FormGroup;
  loading = true;
  edit: boolean | null = null;
  public newUser: Subject<UserModel> = new Subject();
  public newPass: Subject<string> = new Subject();

  get formFirstName() { return this.form.get('formFirstName'); }
  get formLastName() { return this.form.get('formLastName'); }
  get formEmailLogin() { return this.form.get('formEmailLogin'); }
  get formRoles() { return this.form.get('formRoles'); }
  get formPassword() { return this.form.get('formPassword'); }

  constructor(
    private _formBuilder: FormBuilder,
    public bsModalRef: BsModalRef,
    private _usersService: UsersService) { }

  ngOnInit(): void {
    if(this.editUser) {
      this.form = this._formBuilder.group({
        formFirstName: new FormControl(this.editUser.firstName, !this.edit ? null : [Validators.required, Validators.maxLength(100)]),
        formLastName: new FormControl(this.editUser.lastName, !this.edit ? null :[Validators.required, Validators.maxLength(100)]),
        formEmailLogin: new FormControl(this.editUser.email, !this.edit ? null :[Validators.required, Validators.maxLength(100)]),
        formPassword: new FormControl(null, this.edit ? null : [Validators.required, Validators.maxLength(50), Validators.pattern(PatternRegexConst.password)]),
        formRoles: new FormControl("", !this.edit ? null :[Validators.required]),
      });
      let tmp = this.editUser.roles.map(x => x.id);
      this.formRoles?.setValue(tmp);
    } else {
      this.form = this._formBuilder.group({
        formFirstName: new FormControl(null, [Validators.required, Validators.maxLength(100)]),
        formLastName: new FormControl(null, [Validators.required, Validators.maxLength(100)]),
        formEmailLogin: new FormControl(null, [Validators.required, Validators.maxLength(100)]),
        formPassword: new FormControl(null, [Validators.required, Validators.maxLength(50), Validators.pattern(PatternRegexConst.password)]),
        formRoles: new FormControl("", [Validators.required]),
      });
    }
    this.loading = false;
  }

  saveUser() {
    this.form?.markAsTouched();
    this.form.markAllAsTouched();
    if(!this.form?.valid) {
      return;
    }

    // add new or edit user
    if(this.edit != false) {
      if(this.editUser && this.editUser.email == this.formEmailLogin?.value) {
        // edit user without login
        let model = this.preapareUserModel(this.editUser.status);
        this.newUser.next(model);
        this.bsModalRef.hide();
      } else if(this.editUser) {
        // edit user with login
        this.checkLoginAndHide(this.editUser.status);
      } else {
        // add new user
        this.checkLoginAndHide(UserStatusEnum.New);
      }
    } else {
      // unblock user/set password
      this.newPass.next(this.formPassword?.value);
      this.bsModalRef.hide();
    }
  }

  checkLoginAndHide(status: UserStatusEnum) {
    this._usersService.checkUniqueLogin(this.formEmailLogin?.value).subscribe(result => {
      if(result == true) {
        this.formEmailLogin?.setErrors({uniqueLoginError: true});
        return;
      }
      let model = this.preapareUserModel(status);

      this.newUser.next(model);
      this.bsModalRef.hide();
    });
  }

  preapareUserModel(status: UserStatusEnum) {
    let tmp = this.roles.filter(x => this.formRoles?.value.includes(x.id));

    let model = {
      email: this.formEmailLogin?.value,
      firstName: this.formFirstName?.value,
      lastName: this.formLastName?.value,
      password: this.formPassword?.value,
      status: status,
      roles: tmp,
    } as UserModel;

    return model;
  }
}
