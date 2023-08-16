import { Component } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MessageService } from '../../services/message/message.service';

@Component({
  selector: 'app-toast',
  templateUrl: './toast.component.html',
  styleUrls: ['./toast.component.scss']
})
export class ToastComponent {
  constructor(private _snackbar: MatSnackBar, private _message: MessageService) {
    this._message.showMessage.subscribe((message) => {
      this._snackbar.open(message.message, 'X', {
        duration: 3000
      });
    });
  }
}
