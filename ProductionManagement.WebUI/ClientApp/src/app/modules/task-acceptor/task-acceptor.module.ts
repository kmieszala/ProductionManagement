import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TaskAcceptorMainComponent } from './components/task-acceptor-main/task-acceptor-main.component';
import { TaskAcceptorListComponent } from './components/task-acceptor-list/task-acceptor-list.component';
import { SharedModule } from '../shared/shared.module';
import { RouterModule } from '@angular/router';
import { GetUserCodeComponent } from './components/get-user-code/get-user-code.component';



@NgModule({
  declarations: [
    TaskAcceptorMainComponent,
    TaskAcceptorListComponent,
    GetUserCodeComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    RouterModule.forRoot([
      {
        path: 'tasks',
        component: TaskAcceptorMainComponent,
        data: { breadcrumb: 'tasks' },
        children: [
          { path: '', component: TaskAcceptorListComponent, pathMatch: 'full', data: { breadcrumb: 'list' } },
          { path: 'list', component: TaskAcceptorListComponent, data: { breadcrumb: 'list' } },
        ]
      }
    ]),
  ],
  entryComponents: [GetUserCodeComponent],
})
export class TaskAcceptorModule { }
