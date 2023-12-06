import { Component } from '@angular/core';
import { ErrorStateMatcher } from '@angular/material/core';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth/auth.service';
import { LogInError } from '../../core/services/auth/types';
import { MessageService } from '../../core/services/message/message.service';
import { BaseFormComponent, Control } from '../../shared/class/base-form-component/base-form-component';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent extends BaseFormComponent {
  private _emailFormControl: Control = super.createEmailControl({
    name: 'email',
    placeHolder: 'example@example.com'
  });
  private _passwordFormControl: Control = super.createPasswordControl({
    name: 'password',
    placeHolder: '**********'
  });

  public get emailFormControl(): Control {
    return {...this._emailFormControl};
  }

  public set emailFormControl(value: Control) {
    this._emailFormControl = value;
  }

  public get passwordFormControl(): Control {
    return this._passwordFormControl;
  }

  public set passwordFormControl(value: Control) {
    this._passwordFormControl = value;
  }

  constructor(matcher: ErrorStateMatcher, private _auth: AuthService, private _router: Router, private _message: MessageService) {
    super(matcher);
  }

  public valid(): boolean {
    return this.emailFormControl.control.valid && this.passwordFormControl.control.valid;
  }

  public onSubmit(): void {
    this._auth.login(this.emailFormControl.control.value, this.passwordFormControl.control.value)
      .subscribe({
        next: this.success.bind(this),
        error: this.error.bind(this)
      });
  }

  private success(): void {
    this._router.navigate(['/']);
  }

  private error(err: HttpErrorResponse): void {
    if (err.status === 400) {
      const loginError = err.error as LogInError;

      this.emailFormControl = super.setError(this.emailFormControl, loginError.Email);
      this.passwordFormControl = super.setError(this.passwordFormControl, loginError.Password);
    }
    else if (err.status === 401) {
      this._message.show('Invalid email or password.');
    }
    else {
      this._message.show('An error occurred while login. Please try again later.');
    }
  }
}
