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
  arrStocks: any[] = [];
  arrStocksModified = new Subject<any[]>();

  arrStockFundamentalAttributes: any[] = [];

  arrYAxisData: any[] = [];

  //-----------------------------Stock Methods-----------------------------------
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

  SaveStock(stock: any) {
    if (stock.stockID < 1) {
      this.arrStocks.push(stock); //insert case
    }
    else {
      //update case
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

  DeleteStock(stockID: number) {
    console.log('delete= ' + stockID);

    var deleteStockIndex = this.arrStocks.findIndex(s => s.stockID == stockID);
    this.arrStocks.splice(deleteStockIndex, 1);
    this.arrStocksModified.next(this.arrStocks.slice());

    //----Call API-----------  
    console.log('before API Call');
    return this.http.delete(this.baseURL + '/api/Stock/' + stockID);
  }


  //-----------------Stock Fundamental Methods--------------------------------

  getStockFundamentalAttributes(stockID: number) {
    //console.log('in service:');
    return this.http.get(this.baseURL + '/api/StockFundamentalAttribute/' + stockID)
      .pipe(
        map(responseData => {
          const arrSFA: any[] = [];
          let arrGraphData: any[] = [];
          for (const key in responseData) {
            if (responseData.hasOwnProperty(key)) {
              arrGraphData = this.processGraphData(responseData, key);
              responseData[key].graphData = arrGraphData;
              arrSFA.push({ ...responseData[key], id: key });
            }
          }

          this.arrStockFundamentalAttributes = arrSFA;
          return this.arrStockFundamentalAttributes;
        }));
  }

  processGraphData(responseData: any, key: any) {
    const arrRetVal: any[] = [];
    arrRetVal.push(responseData[key].y0);
    arrRetVal.push(responseData[key].y1);
    arrRetVal.push(responseData[key].y2);
    arrRetVal.push(responseData[key].y3);
    arrRetVal.push(responseData[key].y4);
    arrRetVal.push(responseData[key].y5);
    arrRetVal.push(responseData[key].y6);
    arrRetVal.push(responseData[key].y7);
    arrRetVal.push(responseData[key].y8);
    arrRetVal.push(responseData[key].y9);

    return arrRetVal;
  }
}
