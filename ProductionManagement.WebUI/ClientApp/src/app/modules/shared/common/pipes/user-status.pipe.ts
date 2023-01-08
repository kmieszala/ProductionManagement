import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'userStatus'
})
export class UserStatusPipe implements PipeTransform {
  transform(value: number): string {
    if (value == null)
      return "";

    switch (value) {
      case 1:
        return `Dodany`;
      case 2:
        return `Aktywny`;
      case 3:
        return `Zablokowany`;
      case 4:
        return `Usuniety`;
      default:
        return ``;
    }
  }
}
