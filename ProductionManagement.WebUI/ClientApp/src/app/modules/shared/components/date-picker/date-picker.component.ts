import { Component, ElementRef, EventEmitter, Input, OnInit, Output, ViewChild } from '@angular/core';
import { defineLocale, isDateValid, plLocale } from 'ngx-bootstrap/chronos';
import { BsDatepickerConfig, BsDaterangepickerDirective, BsLocaleService } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'date-picker',
  templateUrl: './date-picker.component.html',
  styleUrls: ['./date-picker.component.scss']
})
export class DatePickerComponent implements OnInit {
  @ViewChild('datepicker', { static: false }) datepicker: BsDaterangepickerDirective;
  @ViewChild('datepickerControl', { static: false }) datepickerControl: ElementRef;

  @Input() name: string = '';
  @Input() placeholder: string = 'Wybierz datę';
  @Input() dateValue: string = '';
  @Input() isDisabled: boolean = false;
  @Input() maxDate?: Date = undefined;
  @Input() minDate?: Date = undefined;
  @Input() withTimePicker: boolean = false;
  @Input() dateInputFormat: string = 'DD-MM-YYYY';
  @Input() formControlSize: string = '';
  @Input() isValid?: boolean = true;
  @Input() ngTouched?: boolean = false;
  @Input() minMode: string = '';

  @Output() dateChange: EventEmitter<Date | null> = new EventEmitter<Date | null>();

  bsConfig: Partial<BsDatepickerConfig>;
  formControlSizeClass = '';

  constructor(private _localeService: BsLocaleService) {
    defineLocale('pl', plLocale);
    this._localeService.use('pl');
  }

propagateChange = (_: any) => { };
  onTouched = () => { };

  writeValue(value: any): void {
    if (value !== undefined) {
      this.dateValue = value;
    }
  }

  registerOnChange(fn: any): void {
    this.propagateChange = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {

  }

  ngOnInit(): void {
    this.bsConfig = {
      minDate: this.minDate,
      containerClass: 'theme-dark-blue',
      maxDate: this.maxDate,
      showWeekNumbers: false,
      isAnimated: true,
      rangeInputFormat: 'DD-MM-YYYY',
      dateInputFormat: this.dateInputFormat,
      withTimepicker: this.withTimePicker,
      customTodayClass: 'calendar-today',
      adaptivePosition: true,
    };

    if (this.minMode == 'month') {
      this.bsConfig.minMode = 'month';
    }

    if (this.formControlSize == 'sm') {
      this.formControlSizeClass = 'form-control-sm';
    }
    if (this.formControlSize == 'lg') {
      this.formControlSizeClass = 'form-control-lg';
    }
  }

  onModelChange() {
    let date = this.getDateWithoutOffset(this.dateValue);
    this.propagateChange(date);
    this.dateChange.emit(date);
  }

  getDateWithoutOffset(stringDate: string) {
    if (!stringDate) {
      return;
    }
    const temp = new Date(stringDate);
    temp.setHours(temp.getHours() - temp.getTimezoneOffset() / 60);
    return temp;
  }

  focusControl() {
    if (this.isDisabled) {
      return;
    }
    this.datepickerControl.nativeElement.focus();
    this.datepicker.show();
  }
}
