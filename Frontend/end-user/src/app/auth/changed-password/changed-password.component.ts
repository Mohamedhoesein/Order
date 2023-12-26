import { Component } from '@angular/core';
import { ErrorStateMatcher } from '@angular/material/core';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpErrorResponse } from '@angular/common/http';
import { AbstractControl, ValidationErrors } from '@angular/forms';
import { BaseFormComponent, Control } from '../../shared/class/base-form-component/base-form-component';
import { AuthService } from '../../core/services/auth/auth.service';
import { MessageService } from '../../core/services/message/message.service';
import { ChangePasswordError } from '../../core/services/auth/types';

@Component({
  selector: 'app-changed-password',
  templateUrl: './changed-password.component.html',
  styleUrls: ['./changed-password.component.scss']
})
export class ChangedPasswordComponent extends BaseFormComponent {
  private _passwordFormControl: Control = super.createPasswordControl({
    name: 'password',
    placeHolder: '**********'
  });
  private _confirmPasswordFormControl: Control = super.createExtendedRequiredControl({
    name: 'confirmPassword',
    placeHolder: '**********',
    validators: [{
      key: 'passwordMismatch',
      message: 'Passwords do not match.',
      validator: this.validatePasswordConfirmation.bind(this)
    }],
    type: 'password'
  });
  private _id: string;
  private _code: string;

  public get passwordFormControl(): Control {
    return {...this._passwordFormControl};
  }

  public set passwordFormControl(value: Control) {
    this._passwordFormControl = value;
  }

  public get confirmPasswordFormControl(): Control {
    return {...this._confirmPasswordFormControl};
  }

  public set confirmPasswordFormControl(value: Control) {
    this._confirmPasswordFormControl = value;
  }

  constructor(matcher: ErrorStateMatcher, private _route: ActivatedRoute, private _auth: AuthService, private _router: Router, private _message: MessageService) {
    super(matcher);
    const id = this._route.snapshot.paramMap.get('id');
    const code = this._route.snapshot.paramMap.get('code');
    if (id == null || Number.isNaN(Number(id)) || code === null)
      this._router.navigate(['/notfound']);
    this._id = id ?? '';
    this._code = code ?? '';
  }

  public valid(): boolean {
    return this.passwordFormControl.control.valid &&
          this.confirmPasswordFormControl.control.valid;
  }

  public onSubmit(): void {
    this._auth.changePassword(
        this._id,
        this._code,
        this.passwordFormControl.control.value,
        this.confirmPasswordFormControl.control.value
      ).subscribe({
        next: this.success.bind(this),
        error: this.error.bind(this)
      });
  }

  private validatePasswordConfirmation(_control: AbstractControl): ValidationErrors | null {
    const passwordFormControl = this.passwordFormControl;
    const confirmPasswordFormControl = this.confirmPasswordFormControl;
    if (passwordFormControl.control.value !== confirmPasswordFormControl.control.value)
      return {
        passwordMismatch: true
      };
    else
      return null;
  }

  private success(): void {
    this._router.navigate(['/login']);
  }

  private error(err: HttpErrorResponse): void {
    if (err.status !== 400) {
      this._message.show('An error occurred while changing the password. Please try again later.');
    }
    else {
      const registerError = err.error as ChangePasswordError;

      this.passwordFormControl = super.setError(this.passwordFormControl, registerError.Password);
      this.confirmPasswordFormControl = super.setError(this.confirmPasswordFormControl, registerError.ConfirmPassword);
    }
  }
}
