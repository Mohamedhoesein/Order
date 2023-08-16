import { Component } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { ErrorStateMatcher } from '@angular/material/core';
import { Router } from '@angular/router';
import { BaseFormComponent, Control } from '../../shared/class/base-form-component/base-form-component';
import { AuthService } from '../../core/services/auth/auth.service';
import { MessageService } from '../../core/services/message/message.service';
import { ForgotPasswordError } from '../../core/services/auth/types';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.scss']
})
export class ForgotPasswordComponent extends BaseFormComponent {
  private _emailFormControl: Control = super.createEmailControl({
    name: 'emailAddress',
    placeHolder: 'example@example.com'
  });

  public get emailFormControl(): Control {
    return {...this._emailFormControl};
  }

  public set emailFormControl(value: Control) {
    this._emailFormControl = value;
  }

  constructor(matcher: ErrorStateMatcher, private _auth: AuthService, private _router: Router, private _message: MessageService) {
    super(matcher);
  }

  public valid(): boolean {
    return this.emailFormControl.control.valid;
  }

  public onSubmit(): void {
    this._auth.forgotPassword(this.emailFormControl.control.value).subscribe({
      next: this.success.bind(this),
      error: this.error.bind(this)
    });
  }

  private success(): void {
    this._message.show('An email has been sent.');
    this._router.navigate(['/login']);
  }

  private error(err: HttpErrorResponse): void {
    if (err.status !== 400) {
      this._message.show('An error occurred while resetting the password. Please try again later.');
    }
    else {
      const registerError = err.error as ForgotPasswordError;

      this.emailFormControl = super.setError(this.emailFormControl, registerError.Email);
    }
  }
}
