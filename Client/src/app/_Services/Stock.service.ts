import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class StockService {

  constructor(private http: HttpClient) { }

  baseURL: string = 'https://localhost:5001';

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
}
