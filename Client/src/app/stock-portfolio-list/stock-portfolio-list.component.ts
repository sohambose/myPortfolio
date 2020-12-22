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
  oldStocks: any[] = [];
  isAsc: boolean = true;

  rowData: any;
  selectedStockID: any;

  gridColumns: any[] = [
    {
      "displayName": "Symbol",
      "columnName": "stockSymbol"
    },
    {
      "displayName": "Company Name",
      "columnName": "companyName"
    },
    {
      "displayName": "Industry",
      "columnName": "industry"
    },
    {
      "displayName": "Quantity",
      "columnName": "quantity"
    },
    {
      "displayName": "Action",
      "columnName": ""
    }
  ];

  @ViewChild('grdStocks') grdStocks;

  constructor(private stockService: StockService, private router: Router,
    private activatedRoute: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.stockService.getAllStocks().subscribe(res => {
      this.stocks = res;
      this.oldStocks = this.stocks;
    })

    this.stockService.arrStocksModified.subscribe(res => {
      this.stocks = res;
      this.rowData = this.stocks;
    })
  }
  onViewReport(stockID) {
    console.log(stockID);
    this.selectedStockID = stockID;
    this.router.navigate(['stock-fundamentals/' + this.selectedStockID], { relativeTo: this.activatedRoute });
  }

  onEdit(stockID) {
    console.log(stockID);
    this.selectedStockID = stockID;
    this.router.navigate(['edit/' + this.selectedStockID], { relativeTo: this.activatedRoute });
  }

  onDelete(stockID) {
    if (confirm("Delete this Stock from List?")) {
      this.selectedStockID = stockID;
      this.stockService.DeleteStock(this.selectedStockID).subscribe(res => {
        alert('Data Deleted Successfully!');
        this.router.navigate(['/portfolio']);
      },
        err => {
          console.log('error= ');
          console.log(err);
        }
      );
    }
  }

  sortColumn(columnName) {
    this.stockService.sortStocksArrayByColumn(this.stocks, columnName, this.isAsc);
    this.isAsc = !this.isAsc;
  }



  /* columnDefs = [
    {
      field: 'stockSymbol', filter: true, sortable: true, checkboxSelection: true,
      cellRenderer: 'btnCellRenderer',
      cellRendererParams: {
        clicked: function (field: any) {
          alert(`${field} was clicked`);
        }
      },
    },
    { field: 'companyName', sortable: true },
    { field: 'quantity', sortable: true }
  ]; */

  /* onRowClicked(event: any) {
    this.selectedStockID = event.data.stockID;
    this.router.navigate(['edit/' + this.selectedStockID], { relativeTo: this.activatedRoute });
  } */

  //onRowSelected(event: any) {
  /* alert(this.grdStocks.api.getSelectedRows());
  alert(this.grdStocks.api.getSelectedRows()[0].stockID); */
  //} 
}
