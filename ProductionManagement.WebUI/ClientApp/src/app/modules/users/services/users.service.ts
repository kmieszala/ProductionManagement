import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { DictModel } from '../../shared/models/dict-model';
import { HttpClientService } from '../../shared/services/http-client.service';
import { UserModel } from '../models/user-model';

@Injectable({
  providedIn: 'root'
})
export class UsersService {

  constructor(
    private _http: HttpClientService) {
  }

  getUsers() : Observable<UserModel[]> {
    return this._http.get<UserModel[]>('api/users/getUsers');
  }

  getRoles(): Observable<DictModel[]> {
    return this._http.get<DictModel[]>('api/users/getRoles');
  }

  addUser(model: UserModel) : Observable<UserModel>{
    return this._http.post<UserModel>('api/users/addUser', model);
  }

  editUser(model: UserModel) : Observable<UserModel>{
    return this._http.post<UserModel>('api/users/editUser', model);
  }

  checkUniqueLogin(login: string) : Observable<boolean>{
    return this._http.get<boolean>(`api/users/checkUniqueLogin/${login}`);
  }

  deactiveUser(id: number) : Observable<boolean>{
    return this._http.post<boolean>('api/users/deactiveUser', {id: id, value: ''});
  }

  unlockUser(id: number, newPass: string) : Observable<boolean>{
    return this._http.post<boolean>('api/users/unlockUser', {id: id, value: newPass});
  }
}
