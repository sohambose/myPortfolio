import { Router, ActivatedRoute } from '@angular/router';
import { StockService } from './../_Services/Stock.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-stock-entry',
  templateUrl: './stock-entry.component.html',
  styleUrls: ['./stock-entry.component.css']
})
export class StockEntryComponent implements OnInit {
  stock: any = {};
  @ViewChild('stockForm', { static: true }) stockForm: NgForm;

  selectedStockID: any = -1;
  isAddNewMode: boolean;

  constructor(private stockService: StockService,
    private router: Router, private activatedRoute: ActivatedRoute) { }


  ngOnInit(): void {
    this.activatedRoute.params.subscribe(p => {
      this.selectedStockID = p['stockID'];
      if (this.selectedStockID > 0) {
        this.isAddNewMode = false;
        this.loadStockDetails();
      }
      else {
        this.selectedStockID = -1;
        this.isAddNewMode = true;
      }
    })
  }

  loadStockDetails() {
    this.stockService.getStockByID(this.selectedStockID).subscribe(res => {
      this.stock = res;
      this.stockForm.setValue({
        txtstockID: this.stock.stockID,
        txtStockSymbol: this.stock.stockSymbol,
        txtCompanyName: this.stock.companyName,
        txtIndustry: this.stock.industry,
        numQty: this.stock.quantity
      })
    });
  }

  OnSave() {
    this.stock.stockID = this.selectedStockID;
    this.stock.stockSymbol = this.stockForm.value.txtStockSymbol;
    this.stock.companyName = this.stockForm.value.txtCompanyName;
    this.stock.industry = this.stockForm.value.txtIndustry;
    this.stock.quantity = this.stockForm.value.numQty;
    console.log(this.stock);

    this.stockService.SaveStock(this.stock).subscribe(res => {
      this.isAddNewMode = false;
      console.log("response at page level= " + res);
      this.selectedStockID = res;
      alert('Data saved Successfully!');
    },
      err => {
        console.log(err);
      });
  }

  onCancel() {
    this.router.navigate(['/portfolio']);
  }

}
