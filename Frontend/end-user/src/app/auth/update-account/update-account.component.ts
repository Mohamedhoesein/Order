import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ErrorStateMatcher } from '@angular/material/core';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { AuthService } from '../../core/services/auth/auth.service';
import { Account, RegisterError } from '../../core/services/auth/types';
import { MessageService } from '../../core/services/message/message.service';
import { BaseFormComponent, Control } from '../../shared/class/base-form-component/base-form-component';
import { DeleteAccountComponent } from '../delete-account/delete-account.component';

@Component({
  selector: 'app-update-account',
  templateUrl: './update-account.component.html',
  styleUrls: ['./update-account.component.scss']
})
export class UpdateAccountComponent extends BaseFormComponent implements OnInit {
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

  constructor(matcher: ErrorStateMatcher, private _auth: AuthService, private _message: MessageService, private _dialog: MatDialog, private _router: Router) {
    super(matcher);
  }

  public ngOnInit(): void {
    this._auth.getAccount().subscribe({
      next: this.accountSuccess.bind(this),
      error: this.accountError.bind(this)
    });
  }

  private accountSuccess({name, address, email}: Account): void {
    this.nameFormControl.control.setValue(name);
    this.addressFormControl.control.setValue(address);
    this.emailFormControl.control.setValue(email);
  }

  private accountError(): void {
    this._message.show('An error occurred while retrieving some information. Please try again later.');
    this._router.navigate(['/']);
  }

  public openDialog(): void {
    this._dialog.open(DeleteAccountComponent, {});
  }

  public changePassword(): void {
    this._auth.forgotPasswordUser().subscribe({
      next: this.passwordChangeSuccess.bind(this),
      error: this.passwordChangeError.bind(this)
    });
  }

  public valid(): boolean {
    return this.nameFormControl.control.valid &&
          this.addressFormControl.control.valid &&
          this.emailFormControl.control.valid &&
          this.passwordFormControl.control.valid;
  }

  public onSubmit(): void {
    this._auth.updateAccount(
        this.nameFormControl.control.value,
        this.addressFormControl.control.value,
        this.emailFormControl.control.value,
        this.passwordFormControl.control.value
      ).subscribe({
        next: this.success.bind(this),
        error: this.error.bind(this)
      });
  }

  private passwordChangeSuccess(): void {
    this._message.show('An email has been send.');
  }

  private passwordChangeError(err: HttpErrorResponse): void {
    this._message.show('Password change failed.');
  }

  private success(): void {
    this._message.show('Account updated successfully.');
  }

  private error(err: HttpErrorResponse): void {
    if (err.status !== 400) {
      this._message.show('An error occurred while updating. Please try again later.');
    }
    else {
      const registerError = err.error as RegisterError;

      this.nameFormControl = super.setError(this.nameFormControl, registerError.Name);
      this.addressFormControl = super.setError(this.addressFormControl, registerError.Address);
      this.emailFormControl = super.setError(this.emailFormControl, registerError.Email);
      this.passwordFormControl = super.setError(this.passwordFormControl, registerError.Password);
    }
  }
}

