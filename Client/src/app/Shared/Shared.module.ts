import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { CommonModule } from '@angular/common';
import { HelpTooltipComponent } from './help-tooltip/help-tooltip.component';

const commonRoutes: Routes = [
];

@NgModule({
    declarations: [
        NavMenuComponent,       
        HelpTooltipComponent
    ],
    imports: [
        CommonModule,
        FormsModule,
        RouterModule.forChild(commonRoutes)
    ],
    exports: [
        HelpTooltipComponent,
        NavMenuComponent
    ]
})
export class SharedModule {
}