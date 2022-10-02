import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { TankParts } from '../../products/models/tank-parts';
import { HttpClientService } from '../../shared/services/http-client.service';
import { OrderModel } from '../models/order-model';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {

  constructor(
    private _http: HttpClientService) {
  }

  getOrders(): Observable<OrderModel[]> {
    return this._http.get<OrderModel[]>('api/orders/getOrders');
  }

  addOrder(model: OrderModel) : Observable<number> {
    return this._http.post<number>('api/orders/addOrder', model);
  }

  editOrder(model: OrderModel) : Observable<boolean> {
    return this._http.post<boolean>('api/orders/editOrder', model);
  }

  downloadFile(ordersIds: number[], parts: TankParts[]): Observable<Blob> {
    return this._http.postFile(`api/orders/prepareStorekeeperDocument/`, { ordersIds: ordersIds, parts: parts });
  }
}
