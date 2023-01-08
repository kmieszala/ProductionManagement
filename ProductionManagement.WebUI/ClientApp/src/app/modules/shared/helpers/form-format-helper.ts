import { AbstractControl } from "@angular/forms";
import * as moment from 'moment';

export class FormFormatHelper {

  public static getValue(formControl: AbstractControl): string | null {
    if (formControl.value && formControl.value.toString().length > 0) {
      return formControl.value;
    }
    return null;
  }

  public static getDateWithoutTime(formControl: AbstractControl): Date | null {

    const value = this.getValue(formControl);

    if (value == null) {
      return null;
    }

    var date = new Date(value);

    return moment(new Date(date.getFullYear(), date.getMonth(), date.getDate())).utcOffset(0, true).toDate();
  }

  public static getDateWithoutTimeWithDateValue(value: Date): Date | null {

    if (value == null) {
      return null;
    }

    var date = new Date(value);

    return moment(new Date(date.getFullYear(), date.getMonth(), date.getDate())).utcOffset(0, true).toDate();
  }

  public static getNumberValue(formControl: AbstractControl): number | null {
    if (formControl.value) {
      const value = String(formControl.value).replace(',', '.');

      return Number(value);
    }
    return null;
  }

  public static checkAmount(formControl: AbstractControl) {
    if (formControl.value) {
      const tmp = String(formControl.value).replace(',', '.');
      if (!isNaN(Number(tmp)) && isFinite(Number(tmp)) && tmp !== "") {
        formControl.setValue(Number(tmp).toFixed(2).replace('.', ','));
      }
    }
  }

  public static getCurrentDate() {
    const now = new Date();
    return FormFormatHelper.getDateWithoutOffset(now.toISOString());
  }

  public static getDateWithoutOffset(stringDate: string) {
    if (!stringDate) {
      return;
    }
    const now = new Date();
    const temp = new Date(stringDate);

    const hourOffset = Math.max(Math.min(now.getHours() - temp.getTimezoneOffset() / 60, 23), 0);

    temp.setHours(hourOffset);
    temp.setMinutes(now.getMinutes());
    temp.setSeconds(now.getSeconds());

    return temp;
  }

  public static transferNipFormat(formControl: AbstractControl): string {
    let value = formControl.value;
    let resultFormat = value.substring(0, 3) + '-' + value.substring(3, 6) + '-' + value.substring(6, 8) + '-' + value.substring(8, 10);

    return resultFormat;
  }
}
