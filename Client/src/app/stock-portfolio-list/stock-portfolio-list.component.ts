import { StockService } from './../_Services/Stock.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-stock-portfolio-list',
  templateUrl: './stock-portfolio-list.component.html',
  styleUrls: ['./stock-portfolio-list.component.css']
})
export class StockPortfolioListComponent implements OnInit {

  stocks: any[] = [];
  rowData: any;
  selectedStockID: any;

  constructor(private stockService: StockService, private router: Router,
    private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.stockService.getAllStocks().subscribe(res => {
      this.stocks = res;
      this.rowData = this.stocks;
    })

    this.stockService.arrStocksModified.subscribe(res => {
      this.stocks = res;
      this.rowData = this.stocks;
    })
  }

  onRowClicked(event: any) {
    this.selectedStockID = event.data.stockID;    
    this.router.navigate(['edit/' + this.selectedStockID], { relativeTo: this.activatedRoute });
  }


  columnDefs = [
    { field: 'stockSymbol', filter: true, sortable: true },
    { field: 'companyName', sortable: true },
    { field: 'quantity', sortable: true }
  ];
}
