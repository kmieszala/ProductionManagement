import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersListComponent } from './components/orders-list/orders-list.component';
import { OrderFormComponent } from './components/order-form/order-form.component';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';



@NgModule({
  declarations: [
    OrdersListComponent,
    OrderFormComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forRoot([
      { path: '', component: OrdersListComponent, pathMatch: 'full' },
    ]),
  ],
  entryComponents: [OrderFormComponent],
})
export class OrdersModule { }
