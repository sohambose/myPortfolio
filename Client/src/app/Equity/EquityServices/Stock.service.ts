import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, Subject, throwError } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class StockService {

  constructor(private http: HttpClient) { }

  baseURL: string = environment.baseAPIURL;
  arrStocks: any[] = [];
  arrStocksModified = new Subject<any[]>();

  arrStockFundamentalAttributes: any[] = [];
  arrStockQuarterlyData: any[] = [];

  arrStockCompareDetails: any[] = [];

  arrYAxisData: any[] = [];

  typeAheadStockSearch: any;


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

  sortStocksArrayByColumn(arrStocks: any[], PropertyName: any, isAscending: boolean) {
    var ascFactor: number = isAscending ? 1 : -1;
    arrStocks.sort((a, b) => {
      if (a[PropertyName] > b[PropertyName]) {
        return 1 * ascFactor;
      }
      else {
        return -1 * ascFactor;
      }
    });
    this.arrStocksModified.next(arrStocks.slice());  //Notify change in array.....
  }

  filterArrayForPagination(minIndex, maxIndex) {
    this.arrStocksModified.next(this.arrStocks.slice(minIndex, maxIndex + 1));
  }

  searchArraybyStockSymbol(stockSymbol) {
    this.typeAheadStockSearch = stockSymbol.toUpperCase();
    this.arrStocksModified.next(this.arrStocks.filter(s => s.stockSymbol.includes(this.typeAheadStockSearch)));
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
              arrGraphData = this.processFundamentalGraphData(responseData, key);
              responseData[key].graphData = arrGraphData;
              arrSFA.push({ ...responseData[key], id: key });
            }
          }

          this.arrStockFundamentalAttributes = arrSFA;
          return this.arrStockFundamentalAttributes;
        }));
  }

  getStockQuarterlyData(stockID: number) {
    return this.http.get(this.baseURL + '/api/StockQuarterlyData/' + stockID)
      .pipe(
        map(responseData => {
          const arrSQD: any[] = [];
          let arrGraphData: any[] = [];
          for (const key in responseData) {
            if (responseData.hasOwnProperty(key)) {
              arrGraphData = this.processQuarterlyGraphData(responseData, key);
              responseData[key].graphData = arrGraphData;
              arrSQD.push({ ...responseData[key], id: key });
            }
          }
          this.arrStockQuarterlyData = arrSQD;
          return this.arrStockQuarterlyData;
        }));
  }

  processQuarterlyGraphData(responseData: any, key: any) {
    const arrRetVal: any[] = [];

    arrRetVal.push(responseData[key].q0);
    arrRetVal.push(responseData[key].q1);
    arrRetVal.push(responseData[key].q2);
    arrRetVal.push(responseData[key].q3);
    arrRetVal.push(responseData[key].q4);
    arrRetVal.push(responseData[key].q5);
    arrRetVal.push(responseData[key].q6);
    arrRetVal.push(responseData[key].q7);
    arrRetVal.push(responseData[key].q8);
    arrRetVal.push(responseData[key].q9);

    return arrRetVal;
  }

  processFundamentalGraphData(responseData: any, key: any) {
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

  CompareStocks(SelectedStocks: string) {
    console.log("In Service: " + SelectedStocks);
    return this.http.get(this.baseURL + '/api/StockFundamentalAttribute/compare/' + SelectedStocks)
      .pipe(
        map(responseData => {
          const arrSFA: any[] = [];
          for (const key in responseData) {
            if (responseData.hasOwnProperty(key)) {
              arrSFA.push({ ...responseData[key], id: key });
            }
          }
          this.arrStockFundamentalAttributes = arrSFA;
          return this.arrStockFundamentalAttributes;
        }));
  }

  searchArraybyAnyField(searchString, searchField) {
    if (searchField == 'stockSymbol') {
      this.arrStocksModified.next(this.arrStocks.filter(s => s.stockSymbol.toUpperCase().includes(searchString.toUpperCase())));
    }
    else if (searchField == 'companyName') {
      this.arrStocksModified.next(this.arrStocks.filter(s => s.companyName.toUpperCase().includes(searchString.toUpperCase())));
    }
    else if (searchField == 'industry') {
      this.arrStocksModified.next(this.arrStocks.filter(s => s.industry.toUpperCase().includes(searchString.toUpperCase())));
    }
  }

  //Get Stock Comparison Detailed Data
  getStockComparisonDetailedData() {
    return this.http.get(this.baseURL + '/api/StockFundamentalAttribute/compareDetails')
      .pipe(
        map(responseData => {
          const arrSFA: any[] = [];
          for (const key in responseData) {
            if (responseData.hasOwnProperty(key)) {
              arrSFA.push({ ...responseData[key], id: key });
            }
          }
          this.arrStockCompareDetails = arrSFA;
          return this.arrStockCompareDetails;
        }));
  }

}
