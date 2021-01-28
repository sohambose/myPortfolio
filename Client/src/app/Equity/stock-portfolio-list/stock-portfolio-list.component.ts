import { StockService } from '../EquityServices/Stock.service';
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
  lastSortedColumn: string;

  isShowUploadModal: boolean = false;
  isShowStockEditModal: boolean = false;

  isShowCompareBtn: boolean = false;

  selectedStockID: any;
  selectedStockSymbol: any;



  gridColumns: any[] = [
    {
      "displayName": "Select",
      "columnName": "Select",
      "sortable": "false"
    },
    {
      "displayName": "Symbol",
      "columnName": "stockSymbol",
      "sortable": "true"
    },
    {
      "displayName": "Company Name",
      "columnName": "companyName",
      "sortable": "true"
    },
    {
      "displayName": "Industry",
      "columnName": "industry",
      "sortable": "true"
    },
    {
      "displayName": "Quantity",
      "columnName": "quantity",
      "sortable": "true"
    },
    {
      "displayName": "Action",
      "columnName": "",
      "sortable": "false"
    }
  ];

  arrPageNos: number[] = [];
  totalItems: number;
  ItemsPerPage: number = 5;


  arrselectedStockIDs: string[] = [];

  @ViewChild('grdStocks') grdStocks;

  constructor(private stockService: StockService, private router: Router,
    private activatedRoute: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.stockService.getAllStocks().subscribe(res => {
      this.stocks = res;
      this.oldStocks = this.stocks;
      this.HandlePaginationForGrid();
    })

    this.stockService.arrStocksModified.subscribe(res => {
      this.stocks = res;
    })
  }

  HandlePaginationForGrid() {
    this.totalItems = this.stocks.length;
    var NoPages = Math.ceil(this.totalItems / this.ItemsPerPage);
    for (let i = 1; i <= NoPages; i++) {
      this.arrPageNos.push(i);
    }
    this.Paginate(1);
  }

  Paginate(pageNo, event = null) {
    //event.srcElement.className = 'active';    
    var minIndex: number = (this.ItemsPerPage * pageNo) - this.ItemsPerPage;
    var maxIndex: number = minIndex + this.ItemsPerPage - 1;
    this.stockService.filterArrayForPagination(minIndex, maxIndex);
  }

  onViewReport(stockID) {
    console.log(stockID);
    this.selectedStockID = stockID;
    this.router.navigate(['stock-fundamentals/' + this.selectedStockID]);
  }

  onEdit(stockID) {
    this.selectedStockID = stockID;
    this.isShowStockEditModal = true;
    //this.router.navigate(['edit/' + this.selectedStockID], { relativeTo: this.activatedRoute });
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

  sortColumn(column) {
    if (this.lastSortedColumn != column.columnName) {
      this.isAsc = true;
    }

    if (column.sortable == 'true') {
      this.stockService.sortStocksArrayByColumn(this.stocks, column.columnName, this.isAsc);
      this.isAsc = !this.isAsc;
      this.lastSortedColumn = column.columnName;
    }
  }

  onSelectRow(stockID: string, event: any) {
    if (event.target.checked == false) {
      var selectedStockIndex = this.arrselectedStockIDs.indexOf(stockID);
      this.arrselectedStockIDs.splice(selectedStockIndex, 1);
    }
    if (event.target.checked &&
      !this.arrselectedStockIDs.includes(stockID)) {
      this.arrselectedStockIDs.push(stockID);
    }
    if (this.arrselectedStockIDs.length > 1) {
      this.isShowCompareBtn = true;
    }
    else {
      this.isShowCompareBtn = false;
    }
    console.log(this.arrselectedStockIDs);
  }

  onCompare() {
    this.router.navigate(['compare-stocks/' + this.arrselectedStockIDs.toString()]);
    /* this.stockService.CompareStocks(this.arrselectedStockIDs.toString()).subscribe(res => {
      console.log(res);
    }); */
  }

  onSearchKeyUp(event, field: string) {
    this.stockService.searchArraybyAnyField(event.target.value, field);
  }

  onUploadData(stockID: any, stockSymbol: any) {
    this.selectedStockID = stockID;
    this.selectedStockSymbol = stockSymbol;
    this.isShowUploadModal = true;
  }

  closeModal() {
    this.isShowUploadModal = false;
  }

  closeStockEntry() {
    this.isShowStockEditModal = false;
  }

  onAddNewStock() {
    //this.router.navigate(['edit'], { relativeTo: this.activatedRoute });
    this.isShowStockEditModal = true;
  }

}
