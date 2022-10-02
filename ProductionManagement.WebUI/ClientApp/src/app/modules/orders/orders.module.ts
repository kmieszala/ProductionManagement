import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrdersListComponent } from './components/orders-list/orders-list.component';
import { OrderFormComponent } from './components/order-form/order-form.component';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { StorekeeperDocumentComponent } from './components/storekeeper-document/storekeeper-document.component';

@NgModule({
  declarations: [
    OrdersListComponent,
    OrderFormComponent,
    StorekeeperDocumentComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forRoot([
      { path: '', component: OrdersListComponent, pathMatch: 'full' },
    ]),
  ],
  entryComponents: [OrderFormComponent, StorekeeperDocumentComponent],
})
export class OrdersModule { }
