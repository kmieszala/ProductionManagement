import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { OrdersModule } from './modules/orders/orders.module';
import { OrdersListComponent } from './modules/orders/components/orders-list/orders-list.component';
import { ProductsModule } from './modules/products/products.module';
import { SharedModule } from './modules/shared/shared.module';
import { ProductionLineModule } from './modules/production-line/production-line.module';
import { WorkScheduleModule } from './modules/work-schedule/work-schedule.module';
import { AuthorizationModule } from './modules/authorization/authorization.module';
import { AuthorizeInterceptor } from './modules/authorization/interceptors/authorize.interceptor';
import { HomeModule } from './modules/home/home.module';
import { UsersModule } from './modules/users/users.module';
import { NgxPermissionsModule } from 'ngx-permissions';
import { TaskAcceptorModule } from './modules/task-acceptor/task-acceptor.module';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    OrdersModule,
    TaskAcceptorModule,
    HomeModule,
    ProductsModule,
    SharedModule,
    UsersModule,
    ProductionLineModule,
    WorkScheduleModule,
    AuthorizationModule,
    RouterModule.forRoot([
      { path: '', component: OrdersListComponent, pathMatch: 'full' },
    ]),
    NgxPermissionsModule.forRoot()
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: AuthorizeInterceptor, multi: true },
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
