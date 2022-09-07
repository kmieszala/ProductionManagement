import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ReactiveFormsModule } from '@angular/forms';
import { FormErrorComponent } from './components/form-error/form-error.component';


@NgModule({
  declarations: [FormErrorComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    ModalModule.forRoot()
  ],
  exports: [
    ModalModule,
    ReactiveFormsModule,
    FormErrorComponent
  ]
})
export class SharedModule { }
