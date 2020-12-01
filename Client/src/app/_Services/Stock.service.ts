import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class StockService {

  constructor(private http: HttpClient) { }

  baseURL: string = 'https://localhost:5001';
  //private options = { headers: new HttpHeaders().set('Content-Type', 'application/json') };

  arrStocks: any[] = [];
  arrStocksModified = new Subject<any[]>();

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
          this.arrStocks = stockArray
          this.arrStocksModified.next(this.arrStocks.slice());  //Notify change in array.....
          return this.arrStocks;
        }));
  }

  getStockByID(stockID: number) {
    return this.http.get(this.baseURL + '/api/Stock/' + stockID)
      .pipe(
        map(responseData => {
          const stockArray = [];
          for (const key in responseData) {
            if (responseData.hasOwnProperty(key)) {
              stockArray.push({ ...responseData[key], id: key });
            }
          }
          return responseData;
        }));
  }

  SaveStock(stock: any, isAddNew: boolean): Observable<any> {
    if (isAddNew) {
      this.arrStocks.push(stock); //insert
    }
    else {
      //update      
      var updatedStock = this.arrStocks.find(s => s.stockID == stock.stockID);
      updatedStock.stockSymbol = stock.stockSymbol;
      updatedStock.companyName = stock.companyName;
      updatedStock.industry = stock.industry;
      updatedStock.quantity = stock.quantity;
    }
    this.arrStocksModified.next(this.arrStocks.slice());
    //----Call API-----------    
    return this.http.post(this.baseURL + '/api/Stock',
      {
        "stockID": stock.stockID,
        "stockSymbol": stock.stockSymbol,
        "companyName": stock.companyName,
        "industry": stock.industry,
        "quantity": stock.quantity
      }
    );
  }
}
