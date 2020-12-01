import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-stock-landing',
  templateUrl: './stock-landing.component.html',
  styleUrls: ['./stock-landing.component.css']
})
export class StockLandingComponent implements OnInit {

  constructor(private router: Router,
    private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
  }

  onAddNewStock() {
    this.router.navigate(['edit'], { relativeTo: this.activatedRoute });
  }

}
