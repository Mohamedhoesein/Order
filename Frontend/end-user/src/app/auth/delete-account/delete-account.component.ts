import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ErrorStateMatcher } from '@angular/material/core';
import { MatDialogRef } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { AuthService } from '../../core/services/auth/auth.service';
import { MessageService } from '../../core/services/message/message.service';
import { BaseFormComponent, Control } from '../../shared/class/base-form-component/base-form-component';
import { DeleteAccountError } from 'src/app/core/services/auth/types';

@Component({
  selector: 'app-delete-account',
  templateUrl: './delete-account.component.html',
  styleUrls: ['./delete-account.component.scss']
})
export class DeleteAccountComponent extends BaseFormComponent {
  private _passwordFormControl: Control = super.createPasswordControl({
    id: 'deletePassword',
    name: 'password',
    placeHolder: '**********'
  });

  public get passwordFormControl(): Control {
    return this._passwordFormControl;
  }

  public set passwordFormControl(value: Control) {
    this._passwordFormControl = value;
  }

  constructor(public dialogRef: MatDialogRef<DeleteAccountComponent>, matcher: ErrorStateMatcher, private _auth: AuthService, private _router: Router, private _message: MessageService) {
    super(matcher);
  }

  public onNoClick(): void {
    this.dialogRef.close();
  }

  public valid(): boolean {
    return this.passwordFormControl.control.valid;
  }

  public onSubmit(): void {
    this._auth.deleteAccount(this.passwordFormControl.control.value)
      .subscribe({
        next: this.success.bind(this),
        error: this.error.bind(this)
      });
  }

  private success(): void {
    this.dialogRef.close();
    this._auth.loggedInChanged.next(false);
    this._router.navigate(['/']);
  }

  private error(err: HttpErrorResponse): void {
    if (err.status !== 400) {
      this._message.show('An error occurred while deleting the account. Please try again later.');
    }
    else {
      const deleteAccountError = err.error as DeleteAccountError;

      this.passwordFormControl = super.setError(this.passwordFormControl, deleteAccountError.Password);
    }
  }
}
