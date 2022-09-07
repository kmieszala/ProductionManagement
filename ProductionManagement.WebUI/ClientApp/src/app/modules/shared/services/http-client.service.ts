import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpClientService {

  headers: HttpHeaders;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.headers = new HttpHeaders();
  }

  get<T>(url: string): Observable<T> {
    return this.http.get<T>(this.baseUrl + url, { headers: this.headers });
  }

  post<T>(url: string, body: any): Observable<T> {
    return this.http.post<T>(this.baseUrl + url, body, { headers: this.headers });
  }

  postHtmlResponse(url: string, body: any): Observable<string> {
    return this.http.post(this.baseUrl + url, body, { responseType: 'text' });
  }

  put<T>(url: string, body: any): Observable<T> {
    return this.http.put<T>(this.baseUrl + url, body, { headers: this.headers });
  }

  getFile<Blob>(url: string): Observable<Blob> {
    return this.http.get<Blob>(this.baseUrl + url, { headers: this.headers, responseType: 'blob' as 'json' });
  }
}
