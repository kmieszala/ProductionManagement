import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductionDaysListComponent } from './components/production-days-list/production-days-list.component';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { ProductionDaysMainComponentComponent } from './components/production-days-main-component/production-days-main-component.component';
import { ChangeDayFormComponent } from './components/shared/change-day-form/change-day-form.component';

@NgModule({
  declarations: [
    ProductionDaysListComponent,
    ProductionDaysMainComponentComponent,
    ChangeDayFormComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forRoot([
      {
        path: 'production-days', component: ProductionDaysMainComponentComponent, data: { breadcrumb: 'production-days' },
        children: [
          { path: '', component: ProductionDaysListComponent, pathMatch: 'full', data: { breadcrumb: 'list' } },
        ]
      },
    ]),
  ],
  entryComponents: [ChangeDayFormComponent]
})
export class WorkScheduleModule { }
