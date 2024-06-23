import { AfterViewInit, Component, ElementRef, HostListener, OnInit, ViewChild } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-get-user-code',
  templateUrl: './get-user-code.component.html',
  styleUrls: ['./get-user-code.component.scss']
})
export class GetUserCodeComponent implements OnInit, AfterViewInit  {

  @ViewChild('inputElement') inputElement: ElementRef;
  code: string = '';
  public answear: Subject<string> = new Subject();

  constructor(
    public bsModalRef: BsModalRef
  ) { }

  ngOnInit(): void {    
  }

  close(item: boolean) {
    if(item && this.code.trim().length == 4) {
      this.answear.next(this.code);
    }
    
    this.bsModalRef.hide();
  }

  /// ustawienie kursora w danym polu po załadowaniu komponentu
  ngAfterViewInit(): void {
    setTimeout(() => {
      this.inputElement.nativeElement.focus();
    }, 10);
  }

  /// nasłuchiwanie na klawiszu ENTER
  @HostListener('document:keydown.enter', ['$event'])
  handleEnterKey(event: KeyboardEvent) {
    this.close(true);
  }
}
