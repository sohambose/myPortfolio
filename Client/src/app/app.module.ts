import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { StockEntryComponent } from './stock-entry/stock-entry.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { StockPortfolioListComponent } from './stock-portfolio-list/stock-portfolio-list.component';
import { HomeComponent } from './home/home.component';
import { StockLandingComponent } from './stock-landing/stock-landing.component';
import { FormsModule } from '@angular/forms';
import { AgGridModule } from 'ag-grid-angular';
import { ExcelUploadComponent } from './excel-upload/excel-upload.component';
import { StockFundamentalReportComponent } from './stock-fundamental-report/stock-fundamental-report.component';

@NgModule({
  declarations: [
    AppComponent,
    StockEntryComponent,
    NavMenuComponent,
    StockPortfolioListComponent,
    HomeComponent,
    StockLandingComponent,
    ExcelUploadComponent,
    StockFundamentalReportComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    AgGridModule.withComponents([])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
