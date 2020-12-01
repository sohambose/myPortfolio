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
      { path: 'edit/:stockID', component: StockEntryComponent }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
