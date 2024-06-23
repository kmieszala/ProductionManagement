import { Component, OnDestroy, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { forkJoin, Subscription } from 'rxjs';
import { ConfirmationDialogComponent } from '../../shared/components/confirmation-dialog/confirmation-dialog.component';
import { UserStatusEnum } from '../../shared/enums/user-status.enum';
import { ConfirmationDialogOptions } from '../../shared/models/confirmation-dialog-options';
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
  useridChangeSecretCode: number; // user ID - show button change secret code
  subscriptions: Subscription[] = [];
  bsModalRef?: BsModalRef;
  stars = '****';

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
        this.hideUsersCodes(result.users);
        this.roles = result.roles;
        this.loading = false;
      });
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
    this.subscriptions = [];
  }

  regenerateCode(user: UserModel) {
    this.loading = true;
    this.users.forEach(x => x.code = this.stars);
    this.useridChangeSecretCode = user.id;
    this._usersService.changeUserSecretCode(user.id).subscribe(result => {
      if(result) {
        user.code = result;
      } else {
        this._toastrService.error("Coś poszło nie tak");
      }
      this.loading = false;
    });     
  }

  showUserSecretCode(user: UserModel) {
    if (this.useridChangeSecretCode != user.id) {
      this.users.forEach(x => x.code = this.stars);
      this.loading = true;
      this.useridChangeSecretCode = user.id;
      this._usersService.getUserSecretCode(user.id).subscribe(result => {
        if(result) {
          user.code = result;
        } else {
          this._toastrService.error("Coś poszło nie tak");
        }
        this.loading = false;
      });
    }
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
          result.code = this.stars;
          this.users.push(result);
          this.loading = false;
        } else {
          this._toastrService.error("Coś poszło nie tak - login może już istnieć");
        }
      });
    }));
  }

  activeUser(model: UserModel) {
    const initialState: ModalOptions = {
      initialState: {
        editUser: model,
        edit: false,
      }
    };

    this.bsModalRef = this._modalService.show(UsersFormComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.newPass.subscribe((newPass: string) => {
      this.loading = true;
      this._usersService.unlockUser(model.id, newPass).subscribe(result => {
        model.status = UserStatusEnum.New;
        this.loading = false;
        this._toastrService.success("Hasło zmienione");
      });

    }));
  }

  deactiveUser(model: UserModel) {
    let options = {
      noButton: true,
      yesButton: true,
      title: 'Czy na pewno chcesz zablokować konto? Odblokowanie będzie wymagało zmiany hasła.'
    } as ConfirmationDialogOptions;

    const initialState: ModalOptions = {
      initialState: {
        options: options
      }
    };
    this.bsModalRef = this._modalService.show(ConfirmationDialogComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.answear.subscribe((res: boolean) => {
      if(res) {
        this._usersService.deactiveUser(model.id).subscribe(result => {
          model.status = UserStatusEnum.TimeBlocked;
          this._toastrService.success("Użytkownik zablokowany");
        });
      }
    }));
  }

  editUser(model: UserModel) {
    const initialState: ModalOptions = {
      initialState: {
        editUser: model,
        roles: this.roles,
        edit: true,
      }
    };

    this.bsModalRef = this._modalService.show(UsersFormComponent, initialState);
    this.subscriptions.push(this.bsModalRef.content.newUser.subscribe((res: UserModel) => {
      this.loading = true;
      res.id = model.id;
      this._usersService.editUser(res).subscribe(result => {
        model.status = result.status;
        model.email = result.email;
        model.firstName = result.firstName;
        model.lastName = result.lastName;
        model.roles = result.roles;
        this.loading = false;
        this._toastrService.success("Pomyślnie zapisano zmiany");
      });
    }));
  }

  showDetails(model: UserModel) {
    model.showDetails = !model.showDetails;
  }

  private hideUsersCodes(users: UserModel[]) {
    this.users = users.map(user => {
      return {
          ...user, // kopiujemy wszystkie właściwości użytkownika
          code: this.stars // nadpisujemy właściwość Code
      };
    });
  }
}
