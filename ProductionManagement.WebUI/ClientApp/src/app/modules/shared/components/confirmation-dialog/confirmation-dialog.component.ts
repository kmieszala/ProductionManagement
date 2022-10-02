import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { ConfirmationDialogOptions } from '../../models/confirmation-dialog-options';

@Component({
  selector: 'app-confirmation-dialog',
  templateUrl: './confirmation-dialog.component.html',
  styleUrls: ['./confirmation-dialog.component.scss']
})
export class ConfirmationDialogComponent implements OnInit {

  public options: ConfirmationDialogOptions;

  public answear: Subject<boolean> = new Subject();

  constructor(
    public bsModalRef: BsModalRef) {}

  public confirmMessage:string;

  ngOnInit(): void {
  }

  close(value: boolean) {
    this.answear.next(value);
    this.bsModalRef.hide();
  }

}
