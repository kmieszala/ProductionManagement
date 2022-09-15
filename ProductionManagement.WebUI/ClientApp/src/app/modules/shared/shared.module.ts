import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ReactiveFormsModule } from '@angular/forms';
import { FormErrorComponent } from './components/form-error/form-error.component';
import { NgSelectModule } from '@ng-select/ng-select';


@NgModule({
  declarations: [FormErrorComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    NgSelectModule
  ],
  exports: [
    ModalModule,
    ReactiveFormsModule,
    FormErrorComponent,
    NgSelectModule
  ]
})
export class SharedModule { }
