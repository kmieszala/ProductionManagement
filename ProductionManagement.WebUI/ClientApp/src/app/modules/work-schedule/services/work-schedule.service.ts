import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../../shared/services/http-client.service';
import { ProductionDays } from '../models/production-days';
import { ProductionDaysBasic } from '../models/production-days-basic';

@Injectable({
  providedIn: 'root'
})
export class WorkScheduleService {

  constructor(
    private _http: HttpClientService) {
  }
  getProductionDays(date: Date, dateTo: Date) : Observable<ProductionDays[]> {
    return this._http.post<ProductionDays[]>('api/workschedule/getProductionDays', {dateFrom: date, dateTo: dateTo});
  }

  getCalendarHeaders() : Observable<string[]> {
    return this._http.get<string[]>('api/workschedule/getCalendarHeaders');
  }

  changeWorkDay(productionDay: ProductionDaysBasic): Observable<boolean> {
    return this._http.post<boolean>('api/workschedule/changeWorkDay', productionDay);
  }
}
