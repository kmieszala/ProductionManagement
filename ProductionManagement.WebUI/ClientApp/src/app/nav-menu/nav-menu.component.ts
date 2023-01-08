import { Component } from '@angular/core';
import { AuthorizationService } from '../modules/shared/services/authorization.service';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  isLogged: boolean;
  userName = '';

  constructor(
    private _authService: AuthorizationService) {
    this.isLogged = false;
  }

  ngOnInit() {
    this._authService.isAuthenticated.subscribe(result => this.isLogged = result);

    this._authService.currentUser.subscribe(user => {
      if (user != null) {
        this.userName = `${user.firstName} ${user.lastName}`;
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
}
