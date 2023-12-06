import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject, retry, tap } from 'rxjs';
import {
  Account,
  ChangePassword,
  ForgotPassword,
  HttpOptions,
  LogIn,
  LoggedIn,
  Register
} from './types';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl: string = `${environment.apiUrl}auth/`;
  private defaultOptions: HttpOptions = {
    withCredentials: true
  };
  private defaultPostOptions: HttpOptions = {
    ...this.defaultOptions,
    headers: {
        "Content-Type": "application/json"
    }
  };
  public loggedInChanged: Subject<boolean> = new Subject<boolean>();

  constructor(private _http: HttpClient) {
  }

  public loggedIn(): Observable<LoggedIn> {
    return this._http.get<LoggedIn>(
      `${this.apiUrl}logged-In`,
      this.defaultOptions
    ).pipe(
      retry(1),
      tap({
        next: (data: LoggedIn) => {
          console.log('loggedIn', data.loggedIn);
          this.loggedInChanged.next(data.loggedIn);
        }
      })
    );
  }

  public login(email: string, password: string): Observable<LogIn> {
    return this._http.post<LogIn>(
      `${this.apiUrl}login/user`,
      {
        Email: email,
        Password: password
      },
      this.defaultPostOptions
    ).pipe(
      retry(1),
      tap({
        next: () => {
          this.loggedInChanged.next(true);
        }
      })
    );
  }

  public register(name: string, address: string, email: string, password: string, confirmPassword: string): Observable<Register> {
    return this._http.post<Register>(
      `${this.apiUrl}register/user`,
      {
        Name: name,
        Address: address,
        Email: email,
        Password: password,
        ConfirmPassword: confirmPassword
      },
      this.defaultPostOptions
    ).pipe(retry(1));
  }

  public logout(): Observable<{}> {
    return this._http.post<{}>(
      `${this.apiUrl}logout`,
      {},
      this.defaultPostOptions
    ).pipe(
      retry(1),
      tap({
        next: () => {
          this.loggedInChanged.next(false);
        }
      })
    );
  }

  public forgotPassword(email: string): Observable<ForgotPassword> {
    return this._http.post<ForgotPassword>(
      `${this.apiUrl}forgot-Password/user`,
      {
        Email: email
      },
      this.defaultPostOptions
    ).pipe(retry(1));
  }

  public forgotPasswordUser(): Observable<ForgotPassword> {
    return this._http.post<ForgotPassword>(
      `${this.apiUrl}forgot-Password/current/user`,
      {},
      this.defaultPostOptions
    ).pipe(retry(1));
  }

  public changePassword(id: string, code: string, password: string, confirmPassword: string): Observable<ChangePassword> {
    return this._http.post<ChangePassword>(
      `${this.apiUrl}change-Password/${id}/${code}`,
      {
        Password: password,
        ConfirmPassword: confirmPassword
      },
      this.defaultPostOptions
    ).pipe(retry(1));
  }

  public getAccount(): Observable<Account> {
    return this._http.get<Account>(
      `${this.apiUrl}`,
      this.defaultOptions
    ).pipe(retry(1));
  }

  public updateAccount(name: string, address: string, email: string, password: string): Observable<Register> {
    return this._http.post<Register>(
      `${this.apiUrl}user`,
      {
        Name: name,
        Address: address,
        Email: email,
        Password: password
      },
      this.defaultPostOptions
    ).pipe(retry(1));
  }

  public deleteAccount(password: string): Observable<{}> {
    return this._http.delete<{}>(
      `${this.apiUrl}`,
      {
        ...this.defaultOptions,
        body: {
          Password: password
        }
      }
    ).pipe(retry(1));
  }

  public verify(id: string, code: string): Observable<{}> {
    return this._http.post<{}>(
      `${this.apiUrl}verify/${id}/${code}`,
      {},
      this.defaultPostOptions
    ).pipe(retry(1));
  }

  public updateEmail(id: string, code: string): Observable<{}> {
    return this._http.post<{}>(
      `${this.apiUrl}email/${id}/${code}`,
      {},
      this.defaultPostOptions
    ).pipe(
      retry(1),
      tap({
        next: () => {
          this.loggedIn().subscribe();
        }
      })
    );
  }
}
