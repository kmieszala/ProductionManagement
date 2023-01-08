import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AuthorizationService } from '../../shared/services/authorization.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizeGuard implements CanActivate {
  constructor(
    private authorize: AuthorizationService,
    private router: Router) {
  }

  async canActivate(
    _next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) {
    const isAuthenticated = await this.authorize.checkIsAuthenticated();

    if (isAuthenticated != null && isAuthenticated) {
      return true;
    } else {
      this.router.navigate(['authentication/login'], { queryParams: { returnUrl: state.url } });
      return false;
    }
  }
}
