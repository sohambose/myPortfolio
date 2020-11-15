import { StockService } from './../_Services/Stock.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-stock-entry',
  templateUrl: './stock-entry.component.html',
  styleUrls: ['./stock-entry.component.css']
})
export class StockEntryComponent implements OnInit {

  constructor(private stockService: StockService) { }
  stocks: any[];

  ngOnInit(): void {
    this.stockService.getAllStocks().subscribe(res => {
      this.stocks = res
      console.log(res);
    })
  }
}
