import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent implements OnInit {

  constructor() { }
  productName = 'My Portfolio Manager';

  ngOnInit(): void {
  }

  onClickMenuItem(){
    alert("hello mobile user");
  }


}
