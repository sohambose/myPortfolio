import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class StockService {

  constructor(private http: HttpClient) { }

  baseURL: string = 'https://localhost:5001';

  private options = { headers: new HttpHeaders().set('Content-Type', 'application/json') };

  getAllStocks() {
    return this.http.get(this.baseURL + '/api/Stock')
      .pipe(
        map(responseData => {
          const stockArray = [];
          for (const key in responseData) {
            if (responseData.hasOwnProperty(key)) {
              stockArray.push({ ...responseData[key], id: key });
            }
          }
          return stockArray;
        }));
  }

  SaveStock(stock: any): Observable<any> {
    console.log(stock);
    console.log(this.baseURL + '/api/Stock');

    return this.http.post(this.baseURL + '/api/Stock',
      {
        "StockSymbol": "aaaa",
        "CompanyName": "aaaa",
        "Industry": "aaaa"
      },
      this.options      
    );
  }

  handleError() {
    console.log("Error");

  }
}
