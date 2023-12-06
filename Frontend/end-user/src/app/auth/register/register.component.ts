import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ErrorStateMatcher } from '@angular/material/core';
import { Router } from '@angular/router';
import { BaseFormComponent, Control } from '../../shared/class/base-form-component/base-form-component';
import { AuthService } from '../../core/services/auth/auth.service';
import { RegisterError } from '../../core/services/auth/types';
import { MessageService } from '../../core/services/message/message.service';
import { AbstractControl, ValidationErrors } from '@angular/forms';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent extends BaseFormComponent {
  private _nameFormControl: Control = super.createTextControl({
    name: 'name',
    placeHolder: 'John Doe'
  });
  private _addressFormControl: Control = super.createTextControl({
    name: 'address',
    placeHolder: '1234 Main St'
  });
  private _emailFormControl: Control = super.createEmailControl({
    name: 'email',
    placeHolder: 'example@example.com'
  });
  private _passwordFormControl: Control = super.createPasswordControl({
    name: 'password',
    placeHolder: '**********'
  });
  private _confirmPasswordFormControl: Control = super.createExtendedRequiredControl({
    name: 'confirmPassword',
    placeHolder: '**********',
    validators: [{
      key: 'passwordMismatch',
      message: 'Please retype the password.',
      validator: this.validatePasswordConfirmation.bind(this)
    }],
    type: 'password'
  });

  public get nameFormControl(): Control {
    return {...this._nameFormControl};
  }

  public set nameFormControl(value: Control) {
    this._nameFormControl = value;
  }

  public get addressFormControl(): Control {
    return {...this._addressFormControl};
  }

  public set addressFormControl(value: Control) {
    this._addressFormControl = value;
  }

  public get emailFormControl(): Control {
    return {...this._emailFormControl};
  }

  public set emailFormControl(value: Control) {
    this._emailFormControl = value;
  }

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

  constructor(matcher: ErrorStateMatcher, private _auth: AuthService, private _router: Router, private _message: MessageService) {
    super(matcher);
  }

  public valid(): boolean {
    return this.nameFormControl.control.valid &&
          this.addressFormControl.control.valid &&
          this.emailFormControl.control.valid &&
          this.passwordFormControl.control.valid &&
          this.confirmPasswordFormControl.control.valid;
  }

  public onSubmit(): void {
    this._auth.register(
        this.nameFormControl.control.value,
        this.addressFormControl.control.value,
        this.emailFormControl.control.value,
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
      this._message.show('An error occurred while registering. Please try again later.');
    }
    else {
      const registerError = err.error as RegisterError;

      this.nameFormControl = super.setError(this.nameFormControl, registerError.Name);
      this.addressFormControl = super.setError(this.addressFormControl, registerError.Address);
      this.emailFormControl = super.setError(this.emailFormControl, registerError.Email);
      this.passwordFormControl = super.setError(this.passwordFormControl, registerError.Password);
      this.confirmPasswordFormControl = super.setError(this.confirmPasswordFormControl, registerError.ConfirmPassword);
    }
  }
}
