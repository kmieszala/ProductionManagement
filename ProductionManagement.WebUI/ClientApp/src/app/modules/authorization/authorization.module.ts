import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoginComponent } from './components/login/login.component';
import { SharedModule } from '../shared/shared.module';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AuthorizeGuard } from './guards/authorize.guard';

@NgModule({
  declarations: [
    LoginComponent,
    ],
  imports: [
    CommonModule,
    SharedModule,
    HttpClientModule,
    RouterModule.forChild(
      [
        { path: "authentication/login", component: LoginComponent },
      ]
    )
  ],
  exports: [
    LoginComponent]
})
export class AuthorizationModule { }
