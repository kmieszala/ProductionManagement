import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormErrorComponent } from './components/form-error/form-error.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { StatusDirective } from './common/directives/status-active.directive';
import { ToastrModule } from 'ngx-toastr';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {DragDropModule} from '@angular/cdk/drag-drop';


@NgModule({
  declarations: [FormErrorComponent, StatusDirective, ConfirmationDialogComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    ToastrModule.forRoot(),
    NgSelectModule,
    BrowserAnimationsModule,
    DragDropModule
  ],
  exports: [
    ModalModule,
    FormsModule,
    ReactiveFormsModule,
    FormErrorComponent,
    NgSelectModule,
    ToastrModule,
    ConfirmationDialogComponent,
    BrowserAnimationsModule,
    DragDropModule,

    StatusDirective
  ],
  entryComponents: [
    ConfirmationDialogComponent,
  ]
})
export class SharedModule { }
