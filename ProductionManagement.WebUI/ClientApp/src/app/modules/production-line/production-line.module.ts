import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductionLineListComponent } from './components/production-line-list/production-line-list.component';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { ProductionLineMainComponent } from './components/production-line-main/production-line-main.component';
import { ProductionLineFormComponent } from './components/production-line-form/production-line-form.component';
import { AuthorizeGuard } from '../authorization/guards/authorize.guard';



@NgModule({
  declarations: [
    ProductionLineMainComponent,
    ProductionLineListComponent,
    ProductionLineFormComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forRoot([
      {
        path: 'production-line',
        canActivate: [AuthorizeGuard],
        component: ProductionLineMainComponent,
        data: { breadcrumb: 'production' },
        children: [
          { path: '', component: ProductionLineListComponent, pathMatch: 'full', data: { breadcrumb: 'list' } },
        ]
      },
    ]),
  ],
  entryComponents: [ProductionLineFormComponent],
})
export class ProductionLineModule { }
