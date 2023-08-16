import { AfterViewInit, Component } from '@angular/core';
import { AuthService } from '../../core/services/auth/auth.service';
import { MessageService } from '../../core/services/message/message.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.scss']
})
export class LogoutComponent implements AfterViewInit {
  constructor(private _auth: AuthService, private _message: MessageService, private _router: Router) {}

  public ngAfterViewInit(): void {
    this._auth.logout()
      .subscribe({
        next: () => {
          this._router.navigate(['/']);
        },
        error: () => {
          this._message.show("Please try to log out again.");
          this._router.navigate(['/']);
        }
      });
  }
}
