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

  addPart(filters: PartModel): Observable<number> {
    return this._http.post<number>('api/parts/addPart', filters);
  }
}
