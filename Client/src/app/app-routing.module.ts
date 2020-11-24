import { HomeComponent } from './home/home.component';
import { StockPortfolioListComponent } from './stock-portfolio-list/stock-portfolio-list.component';
import { StockEntryComponent } from './stock-entry/stock-entry.component';
import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'home', component: HomeComponent },
  { path: 'stock-entry', component: StockEntryComponent },
  { path: 'stock-list', component: StockPortfolioListComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
