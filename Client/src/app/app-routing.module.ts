import { StockFundamentalReportComponent } from './stock-fundamental-report/stock-fundamental-report.component';
import { ExcelUploadComponent } from './excel-upload/excel-upload.component';
import { StockLandingComponent } from './stock-landing/stock-landing.component';
import { HomeComponent } from './home/home.component';
import { StockEntryComponent } from './stock-entry/stock-entry.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  {
    path: 'portfolio', component: StockLandingComponent, children: [
      { path: 'edit', component: StockEntryComponent },
      { path: 'edit/:stockID', component: StockEntryComponent },
      { path: 'stock-fundamentals/:stockID', component: StockFundamentalReportComponent },
    ]
  },
  { path: 'excel-upload', component: ExcelUploadComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
