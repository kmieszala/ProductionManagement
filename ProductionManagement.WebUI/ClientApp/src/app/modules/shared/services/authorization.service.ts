import { Injectable } from "@angular/core";
import { Router } from "@angular/router";
import { BehaviorSubject, Observable } from "rxjs";
import { map } from "rxjs/operators";
import { UserInfo } from "../models/user-info";
import { HttpClientService } from "./http-client.service";

@Injectable({ providedIn: 'root' })
export class AuthorizationService {
  currentUser = new BehaviorSubject<UserInfo | null>(null);
  //public currentUser: BehaviorSubject<UserInfo | null> = new BehaviorSubject(null);
  public isAuthenticated: BehaviorSubject<any> = new BehaviorSubject(null);

  constructor(
    private router: Router,
    private _http: HttpClientService) {
    this.init();
  }

  private init() {
    this.isAuthenticated.subscribe(result => {
      if (result == null) {
        this.checkIsAuthenticated();
      } else if (result) {
        if(this.currentUser.value == null){
          this.getUserInfoPromise(0);
        }
        // this.getUserRoles().subscribe(roles => {
        //   //this._permissionsService.loadPermissions(roles);
        //   this.loadRoles();
        // });
      } else {
        //this._permissionsService.loadPermissions([]);
      }
    });
  }

  getUserRoles(): Observable<string[]> {
    return this._http.get<string[]>('api/user/GetRoles');
  }

  getUserInfo(nr: number): Observable<any> {
    return this._http.get<any>(`api/auth/getusername/${nr}`);
  }

  // guard puka tu
  async checkIsAuthenticated(): Promise<boolean | undefined> {
    if (this.isAuthenticated.value == null) {
      return await this.isUserAuthenticated().then(res => {
        if(res){
          let tmp = localStorage.getItem('user');
          this.currentUser.next(JSON.parse(tmp??''));
        } else {
          localStorage.removeItem('user');
          this.currentUser.next(null);
        }
        this.isAuthenticated.next(res??false);
        return res;
      });
    }

    return this.isAuthenticated.value;
  }

  private async isUserAuthenticated(): Promise<boolean | undefined> {
    let res = await this._http.get<boolean>(`api/auth/IsAuthenticated`).toPromise();
    return res;
  }

  userHasRoles(roles: string[]) {
    for (let role of roles) {
      // let permission = this._permissionsService.getPermission(role);
      // if (permission === undefined || permission === null) {
      //   return false;
      // }
    }

    return true;
  }

  isUserInRoles(roles: string[]) {
    for (let role of roles) {
      // let permission = this._permissionsService.getPermission(role);
      // if (permission != null) {
      //   return true;
      // }
    }

    return false;
  }

  private async getUserInfoPromise(nr: number): Promise<any> {
    return await this._http.get<any>(`api/auth/getusername/${nr}`)
    .pipe(map(user => {
      if(user != null){
        this.currentUser.next(user);
        localStorage.setItem('user', JSON.stringify(user));
        this.isAuthenticated.next(true);
      }
      return user;
    })).toPromise();
  }

  public login(username: string, password: string, repeatPassword: string) { //: Observable<boolean> {
    return this._http.post<UserInfo>(`api/auth/login`, { username, password, repeatPassword })
      .pipe(map(user => {
        if(user != null && user.status == 2){
          this.currentUser.next(user);
          localStorage.setItem('user', JSON.stringify(user));
          this.isAuthenticated.next(true);
        }
        return user;
      }));
  }

  public async logout(): Promise<void> {
    await this._http.get<boolean>(`api/auth/logout`).toPromise();
    // remove user from local storage to log user out
    this.currentUser.next(null);
    localStorage.removeItem('user');
    this.router.navigate(['authentication/login']);
  }

  private loadRoles(): void {
    // jeżeli jakiś komponent wymaga posiadania na raz kilku przełączników
    // należy dodać role na podstawie przykładu poniżej. Role można potem
    // wykorzystać poprzez użycie ngxPermissionsOnly='exampleNameAccess'
    //this._rolesService.addRole('exampleNameAccess', ['user_9997', 'user_9998', 'user_9999']);
  }
}
