import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../../shared/services/http-client.service';
import { TankModel } from '../models/tank-model';

@Injectable({
  providedIn: 'root'
})
export class TanksService {

  constructor(
    private _http: HttpClientService) {
  }

  addTank(model: TankModel): Observable<number> {
    return this._http.post<number>('api/tanks/addTank', model);
  }

  getTanks(): Observable<TankModel[]> {
    return this._http.get<TankModel[]>('api/tanks/getTanks');
  }

  editTank(model: TankModel) : Observable<boolean> {
    return this._http.post<boolean>('api/tanks/editTank', model);
  }
}
