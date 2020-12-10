import { StockService } from './../_Services/Stock.service';
import { Component, OnInit, ViewChild } from '@angular/core';
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
  @ViewChild('grdStocks') grdStocks;

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

  columnDefs = [
    { field: 'stockSymbol', filter: true, sortable: true, checkboxSelection: true },
    { field: 'companyName', sortable: true },
    { field: 'quantity', sortable: true }
  ];

  onRowClicked(event: any) {
    this.selectedStockID = event.data.stockID;
    this.router.navigate(['edit/' + this.selectedStockID], { relativeTo: this.activatedRoute });
  }

  onRowSelected(event: any) {
    /* alert(this.grdStocks.api.getSelectedRows());
    alert(this.grdStocks.api.getSelectedRows()[0].stockID); */
  }

  onCellClicked(event: any) {
  }
}
