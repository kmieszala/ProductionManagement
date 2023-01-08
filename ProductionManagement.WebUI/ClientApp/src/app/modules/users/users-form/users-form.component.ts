import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
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
  editUser: boolean = false;
  form: FormGroup;
  loading = true;
  public newUser: Subject<UserModel> = new Subject();

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
        formFirstName: new FormControl(null, [Validators.required, Validators.maxLength(100)]),
        formLastName: new FormControl(null, [Validators.required, Validators.max(100)]),
        formEmailLogin: new FormControl(null),
        formPassword: new FormControl(null),
        formRoles: new FormControl("", [Validators.required]),
      });
    } else {
      this.form = this._formBuilder.group({
        formFirstName: new FormControl(null, [Validators.required, Validators.maxLength(100)]),
        formLastName: new FormControl(null, [Validators.required, Validators.max(100)]),
        formEmailLogin: new FormControl(null),
        formPassword: new FormControl(null),
        formRoles: new FormControl("", [Validators.required]),
      });
    }
    this.loading = false;
  }

  saveUser() {
    this.form?.markAsTouched();

    if(!this.form?.valid) {
      return;
    }
    this._usersService.checkUniqueLogin(this.formEmailLogin?.value).subscribe(result => {
      if(result == false) {
        this.formEmailLogin?.setErrors({uniqueLoginError: true});
        return;
      }

      let model = {
        email: this.formEmailLogin?.value,
        firstName: this.formFirstName?.value,
        lastName: this.formLastName?.value,
        password: this.formPassword?.value,
        roles: this.formRoles?.value,
      } as UserModel;

      this.newUser.next(model);
      this.bsModalRef.hide();
    });
  }
}
