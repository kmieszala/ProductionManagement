import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../../../shared/services/http-client.service';
import { OrderModel } from '../../models/order-model';

@Injectable({
  providedIn: 'root'
})
export class OrdersService {

  constructor(
    private _http: HttpClientService) {
  }

  getParts(): Observable<OrderModel[]> {
    return this._http.get<OrderModel[]>('api/orders/getOrders');
  }
}
