import { Component, OnDestroy, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent implements OnInit, OnDestroy {
  public loggedIn: boolean = false;
  private subscription: Subscription;
  private tempLogIn: Subscription | null = null;

  constructor(private _auth: AuthService) {
    this.subscription = this._auth.loggedInChanged.subscribe((loggedIn: boolean) => {
      this.loggedIn = loggedIn;
    });
  }

  public ngOnInit(): void {
    this.checkLoggedIn();
  }

  public ngOnDestroy(): void {
    this.subscription.unsubscribe();
    if (this.tempLogIn !== null) {
      this.tempLogIn.unsubscribe();
    }
  }

  private checkLoggedIn(): void {
    this.tempLogIn = this._auth.loggedIn().subscribe({
      next: (value) => {
        this.loggedIn = value.loggedIn;
        this.tempLogIn = null;
      }
    });
  }
}
