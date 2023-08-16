import { HttpContext, HttpHeaders, HttpParams } from "@angular/common/http";

export interface LoggedIn {
    loggedIn: boolean
};

export interface LogInError {
    Password: string[],
    Email: string[]
};

export interface LogIn {};

export interface RegisterError {
    Name: string[],
    Address: string[],
    Email: string[],
    Password: string[],
    ConfirmPassword: string[]
};

export interface Register {};

export interface ForgotPasswordError {
    Email: string[]
}

export interface ForgotPassword {};

export interface ChangePasswordError {
    Password: string[],
    ConfirmPassword: string[]
};

export interface ChangePassword {};

export interface UpdateAccountError {
    Name: string[],
    Address: string[],
    Email: string[],
    Password: string[]
};

export interface UpdateAccount {};

export interface DeleteAccountError {
    Password: string[]
};

export interface DeleteAccount {};

export interface Account {
    name: string,
    address: string,
    email: string
}

export interface HttpOptions {
    headers?: HttpHeaders | {
        [header: string]: string | string[];
    };
    context?: HttpContext;
    observe?: 'body';
    params?: HttpParams | {
        [param: string]: string | number | boolean | ReadonlyArray<string | number | boolean>;
    };
    reportProgress?: boolean;
    responseType?: 'json';
    withCredentials?: boolean;
};