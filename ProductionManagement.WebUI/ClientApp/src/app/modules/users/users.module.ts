import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { UsersMainComponent } from './users-main/users-main.component';
import { UsersListComponent } from './users-list/users-list.component';
import { UsersFormComponent } from './users-form/users-form.component';


@NgModule({
  declarations: [
    UsersMainComponent,
    UsersListComponent,
    UsersFormComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forRoot([
      {
        path: 'users', component: UsersMainComponent, data: { breadcrumb: 'users' },
        children: [
          { path: '', component: UsersListComponent, pathMatch: 'full', data: { breadcrumb: 'list' } },
        ]
      },
    ]),
  ],
  entryComponents: [UsersFormComponent]
})
export class UsersModule { }
