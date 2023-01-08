import { Component, OnDestroy, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { forkJoin, Subscription } from 'rxjs';
import { DictModel } from '../../shared/models/dict-model';
import { UserModel } from '../models/user-model';
import { UsersService } from '../services/users.service';
import { UsersFormComponent } from '../users-form/users-form.component';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
  styleUrls: ['./users-list.component.scss']
})
export class UsersListComponent implements OnInit, OnDestroy {
  users: UserModel[];
  roles: DictModel[];
  loading = true;
  subscriptions: Subscription[] = [];
  bsModalRef?: BsModalRef;

  constructor(
    private _modalService: BsModalService,
    private _toastrService: ToastrService,
    private _usersService: UsersService) { }

  ngOnInit(): void {
    forkJoin(
      {
        users: this._usersService.getUsers(),
        roles: this._usersService.getRoles()
      })
      .subscribe(result => {
        this.users = result.users;
        this.roles = result.roles;
        this.loading = false;
      });
  }
  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
    this.subscriptions = [];
  }

  addNewUser() {

    const initialState: ModalOptions = {
      initialState: {
        roles: this.roles
      },
      class: 'modal-lg',
    };

    this.bsModalRef = this._modalService.show(UsersFormComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.newUser.subscribe((res: UserModel) => {
      this.loading = true;
      this._usersService.addUser(res).subscribe(result => {
        if(result) {
          this.users.push(result);
          this.loading = false;
        } else {
          this._toastrService.error("Coś poszło nie tak - login może już istnieć");
        }
      });
    }));
  }

  showDetails(model: UserModel) {
    model.showDetails = !model.showDetails;
  }

}
