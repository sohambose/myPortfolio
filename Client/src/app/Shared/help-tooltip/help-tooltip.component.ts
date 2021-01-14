import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-help-tooltip',
  templateUrl: './help-tooltip.component.html',
  styleUrls: ['./help-tooltip.component.css']
})
export class HelpTooltipComponent implements OnInit {

  isShowToolTipDiv: boolean = false;

  @Input() ToolTipHTML;

  constructor() { }

  ngOnInit(): void {
  }

  onMouseEnter() {
    this.isShowToolTipDiv = true;
  }
  onMouseLeave() {
    this.isShowToolTipDiv = false;
  }

}
