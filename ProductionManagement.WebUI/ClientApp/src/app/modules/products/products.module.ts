import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PartsListComponent } from './components/parts-list/parts-list.component';
import { PartsFormComponent } from './components/parts-form/parts-form.component';
import { RouterModule } from '@angular/router';
import { PartsMainComponent } from './components/parts-main/parts-main.component';
import { SharedModule } from '../shared/shared.module';
import { TanksListComponent } from './components/tanks-list/tanks-list.component';
import { TanksFormComponent } from './components/tanks-form/tanks-form.component';
import { TanksMainComponent } from './components/tanks-main/tanks-main.component';

@NgModule({
  declarations: [
    PartsListComponent,
    PartsFormComponent,
    PartsMainComponent,
    TanksListComponent,
    TanksFormComponent,
    TanksMainComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forRoot([
      {
        path: 'parts', component: PartsMainComponent, data: { breadcrumb: 'parts' },
        children: [
          { path: '', component: PartsListComponent, pathMatch: 'full', data: { breadcrumb: 'list' } },
          { path: 'list', component: PartsListComponent, data: { breadcrumb: 'list' } },
        ]
      },
      {
        path: 'tanks', component: TanksMainComponent, data: { breadcrumb: 'tanks' },
        children: [
          { path: '', component: TanksListComponent, pathMatch: 'full', data: { breadcrumb: 'list' } },
          { path: 'list', component: TanksListComponent, data: { breadcrumb: 'list' } },
        ]
      },
    ]),
  ],
  entryComponents: [TanksFormComponent, PartsFormComponent],
})
export class ProductsModule { }
