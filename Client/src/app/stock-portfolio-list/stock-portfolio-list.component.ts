import { StockService } from './../_Services/Stock.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-stock-portfolio-list',
  templateUrl: './stock-portfolio-list.component.html',
  styleUrls: ['./stock-portfolio-list.component.css']
})
export class StockPortfolioListComponent implements OnInit {

  stocks: any[];

  constructor(private stockService: StockService, private router: Router) { }

  ngOnInit(): void {
    this.stockService.getAllStocks().subscribe(res => {
      this.stocks = res
      console.log(res);
    })
  }

  onAddNewStock() {
    this.router.navigate(['/stock-entry']);
  }

}
