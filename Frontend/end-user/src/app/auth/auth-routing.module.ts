import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { LogoutComponent } from './logout/logout.component';
import { RegisterComponent } from './register/register.component';
import { ForgotPasswordComponent } from './forgot-password/forgot-password.component';
import { ChangedPasswordComponent } from './changed-password/changed-password.component';
import { VerifyComponent } from './verify/verify.component';
import { UpdateAccountComponent } from './update-account/update-account.component';
import { UpdateEmailComponent } from './update-email/update-email.component';

const routes: Routes = [
  {path: 'login', component: LoginComponent},
  {path: 'logout', component: LogoutComponent},
  {path: 'register', component: RegisterComponent},
  {path: 'forgot-password', component: ForgotPasswordComponent},
  {path: 'account', component: UpdateAccountComponent},
  {path: 'change-password/:id/:code', component: ChangedPasswordComponent},
  {path: 'verify/:id/:code', component: VerifyComponent},
  {path: 'email/:id/:code', component: UpdateEmailComponent}
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AuthRoutingModule { }
