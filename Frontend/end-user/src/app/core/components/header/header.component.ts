import { Component, OnDestroy } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnDestroy {
  public loggedIn: boolean = false;
  private subscription: Subscription;

  constructor(private _auth: AuthService) {
    this.checkLoggedIn();
    this.subscription = this._auth.loggedInChanged.subscribe(() => {
      this.checkLoggedIn();
    });
  }

  public ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  private checkLoggedIn(): void {
    this._auth.loggedIn().subscribe((value) => {
      this.loggedIn = value.loggedIn;
    });
  }
}
