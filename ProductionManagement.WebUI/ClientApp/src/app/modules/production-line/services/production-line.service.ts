import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../../shared/services/http-client.service';
import { ProductionLine } from '../models/production-line';
import { DictModel } from '../../shared/models/dict-model';

@Injectable({
  providedIn: 'root'
})
export class ProductionLineService {
  constructor(
    private _http: HttpClientService) {
  }

  getLines() : Observable<ProductionLine[]> {
    return this._http.get<ProductionLine[]>('api/productionLine/getLines');
  }

  getProductionLines() : Observable<DictModel[]> {
    return this._http.get<DictModel[]>('api/productionLine/getProductionLines');
  }

  addLine(model: ProductionLine) : Observable<number> {
    return this._http.post<number>('api/productionLine/addLine', model);
  }

  editLine(model: ProductionLine) : Observable<boolean> {
    return this._http.post<boolean>('api/productionLine/editLine', model);
  }

}
