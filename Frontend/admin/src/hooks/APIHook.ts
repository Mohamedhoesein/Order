import { useQuery } from "@tanstack/react-query";
import axios from "axios";

const endpoint = import.meta.env.VITE_APP_ENDPOINT;
const authEndpoint = `${endpoint}auth/`;
const instance = axios.create({
    baseURL: endpoint
});

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
};

export interface ForgotPassword {};

export interface UpdateAccountError {
    Name: string[],
    Address: string[],
    Email: string[],
    Password: string[]
};

export interface UpdateAccount {};

export interface Account {
    name: string,
    address: string,
    email: string
};

export interface PasswordError {
    Password: string[]
};

export interface Password {};

export interface ChangePasswordError {
    Password: string[],
    ConfirmPassword: string[]
};

export interface ChangePassword {};

export const isLogInError = (data: LogInError | LogIn | undefined) => {
    return data != undefined && "Password" in data && "Username" in data;
};

export const isRegisterError = (data: RegisterError | Register | undefined) => {
    return data != undefined && "Name" in data && "Address" in data && "Email" in data && "Password" in data && "ConfirmPassword" in data;
};

export const isForgotPasswordError = (data: ForgotPasswordError | ForgotPassword | undefined) => {
    return data != undefined && "Email" in data;
};

export const isUpdateAccountError = (data: UpdateAccountError | UpdateAccount | undefined) => {
    return data != undefined && "Name" in data && "Address" in data && "Email" in data && "Password" in data;
};

export const isPasswordError = (data: PasswordError | Password | undefined) => {
    return data != undefined && "Password" in data;
};

export const isChangePasswordError = (data: ChangePasswordError | ChangePassword | undefined) => {
    return data != undefined && "Password" in data && "ConfirmPassword" in data;
};

export const useRegister = (name: string, address: string, email: string, password: string, confirmPassword: string, enabled: boolean) => {
    return useQuery({
        queryKey: ["register", name, email],
        queryFn: async () => {
            return instance.post<Register | RegisterError>(
                `${authEndpoint}register/employee`,
                JSON.stringify({
                    Name: name,
                    Address: address,
                    Email: email,
                    Password: password,
                    ConfirmPassword: confirmPassword
                }),
                {
                    withCredentials: true,
                    headers: {
                        "Content-Type": "application/json"
                    }
                }
            );
        },
        enabled: enabled,
        retry: 2
    });
};

export const useLogIn = (email: string, password: string, enabled: boolean) => {
    return useQuery({
        queryKey: ["login", email],
        queryFn: async () => {
            return instance.post<LogIn | LogInError>(
                `${authEndpoint}login/employee`,
                JSON.stringify({
                    Email: email,
                    Password: password
                }),
                {
                    withCredentials: true,
                    headers: {
                        "Content-Type": "application/json"
                    }
                }
            );
        },
        enabled: enabled,
        retry: 2
    });
};

export const useLoggedIn = () => {
    return useQuery({
        queryKey: ["loggedin"],
        queryFn: async () => {
            return instance.get<LoggedIn>(
                `${authEndpoint}logged-in`,
                {
                    withCredentials: true
                }
            )
        },
        retry: 2
    });
};

export const useLogOut = (enabled: boolean) => {
    return useQuery({
        queryKey: ["logout"],
        queryFn: async () => {
            return instance.post(
                `${authEndpoint}logout`,
                {},
                {
                    withCredentials: true
                }
            );
        },
        enabled: enabled,
        retry: 2
    });
};

export const useForgotPassword = (email: string, enabled: boolean) => {
    return useQuery({
        queryKey: ["forgotpassword", email],
        queryFn: async () => {
            return instance.post<ForgotPasswordError | ForgotPassword>(
                `${authEndpoint}forgot-password/employee`,
                JSON.stringify({
                    Email: email
                }),
                {
                    withCredentials: true,
                    headers: {
                        "Content-Type": "application/json"
                    }
                }
            );
        },
        enabled: enabled,
        retry: 2
    });
};

export const useForgotPasswordUser = (enabled: boolean) => {
    return useQuery({
        queryKey: ["forgotpassworduser"],
        queryFn: async () => {
            return instance.post(
                `${authEndpoint}forgot-password/current/employee`,
                {},
                {
                    withCredentials: true
                }
            );
        },
        enabled: enabled,
        retry: 2
    });
};

export const useChangedPassword = (id: string, token: string, password: string, confirmPassword: string, enabled: boolean) => {
    return useQuery({
        queryKey: ["changepassword", token],
        queryFn: async () => {
            return instance.post<ChangePasswordError | ChangePassword>(
                `${authEndpoint}change-password/${id}/${token}`,
                {
                    Password: password,
                    ConfirmPassword: confirmPassword
                },
                {
                    withCredentials: true,
                }
            );
        },
        enabled: enabled,
        retry: 2
    });
};

export const useUpdateAccount = (name: string, address: string, password: string, enabled: boolean) => {
    return useQuery({
        queryKey: ["updateaccount", name],
        queryFn: async () => {
            return instance.post<UpdateAccountError | UpdateAccount>(
                `${authEndpoint}employee`,
                JSON.stringify({
                    Name: name,
                    Address: address,
                    Password: password
                }),
                {
                    withCredentials: true,
                    headers: {
                        "Content-Type": "application/json"
                    }
                }
            );
        },
        enabled: enabled,
        retry: 2
    });
}

export const useGetAccount = (enabled: boolean) => {
    return useQuery({
        queryKey: ["getaccount"],
        queryFn: async () => {
            return instance.get<Account>(
                `${authEndpoint}`,
                {
                    withCredentials: true
                }
            );
        },
        enabled: enabled,
        retry: 2
    });
}

export const useDeleteaccount = (password: string, enabled: boolean) => {
    return useQuery({
        queryKey: ["deleteaccount"],
        queryFn: async () => {
            return instance.delete<PasswordError | Password>(
                `${authEndpoint}`,
                {
                    withCredentials: true,
                    headers: {
                        "Content-Type": "application/json"
                    },
                    data: JSON.stringify({
                        Password: password
                    })
                }
            );
        },
        enabled: enabled,
        retry: 2
    });
}