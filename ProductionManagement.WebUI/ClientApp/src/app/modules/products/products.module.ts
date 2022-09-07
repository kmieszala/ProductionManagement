import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PartsListComponent } from './components/parts-list/parts-list.component';
import { PartsFormComponent } from './components/parts-form/parts-form.component';
import { RouterModule } from '@angular/router';
import { PartsMainComponent } from './components/parts-main/parts-main.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    PartsListComponent,
    PartsFormComponent,
    PartsMainComponent
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
          { path: 'add', component: PartsFormComponent, data: { breadcrumb: 'add' } },
        ]
      },
    ])
  ]
})
export class ProductsModule { }
