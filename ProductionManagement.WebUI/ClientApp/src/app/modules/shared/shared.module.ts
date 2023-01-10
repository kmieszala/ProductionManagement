import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { FormErrorComponent } from './components/form-error/form-error.component';
import { NgSelectModule } from '@ng-select/ng-select';
import { StatusDirective } from './common/directives/status-active.directive';
import { UserStatusDirective } from './common/directives/user-status.directive';
import { ToastrModule } from 'ngx-toastr';
import { ConfirmationDialogComponent } from './components/confirmation-dialog/confirmation-dialog.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { DragDropModule} from '@angular/cdk/drag-drop';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { DatePickerComponent } from './components/date-picker/date-picker.component';
import { MatRadioModule, MAT_RADIO_DEFAULT_OPTIONS} from '@angular/material/radio';
import { DateSelectorComponent } from './components/date-selector/date-selector.component';
import { UserStatusPipe } from './common/pipes/user-status.pipe';


@NgModule({
  declarations: [
    FormErrorComponent,
    ConfirmationDialogComponent,
    DatePickerComponent,
    DateSelectorComponent,

    StatusDirective,
    UserStatusDirective,

    UserStatusPipe],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 4000,
      positionClass: 'toast-top-right',
      preventDuplicates: true,
    }),
    NgSelectModule,
    BrowserAnimationsModule,
    DragDropModule,
    BsDatepickerModule,
    MatRadioModule
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
    BsDatepickerModule,
    DatePickerComponent,
    MatRadioModule,
    DateSelectorComponent,

    UserStatusPipe,

    UserStatusDirective,
    StatusDirective
  ],
  entryComponents: [
    ConfirmationDialogComponent,
  ],
  providers: [{
    provide: MAT_RADIO_DEFAULT_OPTIONS,
    useValue: { color: 'warn' },
  }]
})
export class SharedModule { }
