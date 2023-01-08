import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './components/home/home.component';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { AuthorizeGuard } from '../authorization/guards/authorize.guard';



@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forRoot([
      { path: '',
        canActivate: [AuthorizeGuard],
        component: HomeComponent,
        pathMatch: 'full'
      },
    ]),
  ]
})
export class HomeModule { }
