import { Component, OnDestroy, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';
import { ChangePasswordComponent } from '../modules/shared/components/change-password/change-password.component';
import { AuthorizationService } from '../modules/shared/services/authorization.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements  OnInit, OnDestroy {
  isExpanded = false;
  isLogged: boolean;
  userName = '';
  userId: number;
  bsModalRef?: BsModalRef;
  subscriptions: Subscription[] = [];

  constructor(
    private _authService: AuthorizationService,
    private _modalService: BsModalService) {
    this.isLogged = false;
  }

  ngOnDestroy(): void {
    this.subscriptions.forEach(x => x.unsubscribe());
    this.subscriptions = [];
  }

  ngOnInit() {
    this._authService.isAuthenticated.subscribe(result => this.isLogged = result);

    this._authService.currentUser.subscribe(user => {
      if (user != null) {
        this.userName = `${user.firstName} ${user.lastName}`;
        this.userId = user.id;
        this.isLogged = true;
      } else {
        this.isLogged = false;
      }
    });
  }

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logout() {
    this._authService.logout();
  }

  changePass( ) {
    if(this.userId > 0) {
      const initialState: ModalOptions = {
        initialState: {
          userId: this.userId,
        }
      };

      this.bsModalRef = this._modalService.show(ChangePasswordComponent, initialState);
    }
  }
}
