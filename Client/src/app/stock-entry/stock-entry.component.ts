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

  selectedStockID: any;
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
    /* if (this.isAddNewMode) {
      this.stock.stockID = -1;
    }
    else {
      this.stock.stockID = this.stockForm.value.txtstockID;
    } */
    if (!this.isAddNewMode) {
      this.stock.stockID = this.stockForm.value.txtstockID;
    }
    this.stock.stockSymbol = this.stockForm.value.txtStockSymbol;
    this.stock.companyName = this.stockForm.value.txtCompanyName;
    this.stock.industry = this.stockForm.value.txtIndustry;
    this.stock.quantity = this.stockForm.value.numQty;
    this.stockService.SaveStock(this.stock, this.isAddNewMode).subscribe(res => {
      this.isAddNewMode = false;
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
