import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../../shared/services/http-client.service';
import { PartModel } from '../models/part-model';

@Injectable({
  providedIn: 'root'
})
export class PartsService {

  constructor(
    private _http: HttpClientService) {
  }

  addPart(model: PartModel): Observable<number> {
    return this._http.post<number>('api/parts/addPart', model);
  }

  getParts(): Observable<PartModel[]> {
    return this._http.get<PartModel[]>('api/parts/getParts');
  }

  editPart(model: PartModel) : Observable<boolean> {
    return this._http.post<boolean>('api/parts/editPart', model);
  }
}
