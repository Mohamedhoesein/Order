import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

interface Message {
  message: string;
}

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  public showMessage: Subject<Message> = new Subject<Message>();

  constructor() { }

  public show(message: string): void {
    this.showMessage.next({
      message
    });
  }
}
