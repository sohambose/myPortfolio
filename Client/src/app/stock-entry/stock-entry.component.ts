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

  constructor(private stockService: StockService) { }


  ngOnInit(): void {
  }

  OnSave() {
    this.stock.StockSymbol = this.stockForm.value.txtStockSymbol;
    this.stock.CompanyName = this.stockForm.value.txtCompanyName;
    this.stock.Industry = this.stockForm.value.txtIndustry;
    this.stockService.SaveStock(this.stock).subscribe(res => {
      console.log(res);
    });
  }

}
