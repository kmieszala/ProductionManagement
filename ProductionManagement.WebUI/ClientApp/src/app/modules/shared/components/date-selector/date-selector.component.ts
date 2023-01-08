import { Component, OnInit, Input, Self, Optional, ViewChild, ElementRef } from '@angular/core';
import { BsDatepickerConfig, BsDaterangepickerDirective, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ControlValueAccessor, NgControl, FormControl } from '@angular/forms';
import { plLocale } from 'ngx-bootstrap/locale';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { FormFormatHelper } from '../../helpers/form-format-helper';

@Component({
  selector: 'date-selector',
  templateUrl: './date-selector.component.html',
  styleUrls: ['./date-selector.component.scss']
})
export class DateSelectorComponent implements OnInit, ControlValueAccessor {
  @ViewChild('datepicker', { static: false }) datepicker: BsDaterangepickerDirective;
  @ViewChild('datepickerControl', { static: false }) datepickerControl: ElementRef;

  @Input('label') label: string;
  @Input('name') name: string;
  @Input('allowPast') allowPast: boolean;
  @Input('allowPastYear') allowPastYear: number = 5;
  @Input('minDate') minDate: Date;
  @Input('allowFuture') allowFuture: boolean = true;

  focused = false;
  calendarConfig: Partial<BsDatepickerConfig>;
  dateInput: FormControl;

  constructor(@Self() @Optional() public control: NgControl,
    private localeService: BsLocaleService) {
    control.valueAccessor = this;

    defineLocale('pl', plLocale);
    this.localeService.use('pl');

  }

  propagateChange = (_: any) => { };
  onTouched = () => { };

  writeValue(obj: any): void {

  }

  registerOnChange(fn: any): void {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {

  }

  markAsTouched() {
    this.onTouched();
  }

  ngOnInit(): void {
    this.dateInput = this.control.control as FormControl;
    this.dateInput.valueChanges.subscribe(value => {
      const date = FormFormatHelper.getDateWithoutOffset(value);
      this.dateInput.setValue(date, { emitEvent: false });
      this.onTouched();
    });

    let minDate = new Date();
    if (this.minDate) {
      minDate = this.minDate;
    }

    if (this.allowPast) {
      minDate.setFullYear(minDate.getFullYear() - this.allowPastYear);
    }

    this.calendarConfig = {
      minDate: minDate,
      showWeekNumbers: false,
      isAnimated: true,
      dateInputFormat: 'DD.MM.YYYY'
    }

    if (!this.allowFuture) {
      this.calendarConfig.maxDate = new Date();
    }
  }

  focusControl() {
    this.datepickerControl.nativeElement.focus();
    this.datepicker.show();
  }
}

