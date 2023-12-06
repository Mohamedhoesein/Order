import { useQuery } from "@tanstack/react-query";
import axios, {AxiosError} from "axios";

const endpoint = import.meta.env.VITE_APP_ENDPOINT;
const authEndpoint = `${endpoint}auth/`;
const instance = axios.create({
    baseURL: endpoint
});

export interface LoggedIn {
    loggedIn: boolean
}

export interface LogInModelError {
    password: string[],
    email: string[]
}

export interface LogInError {
    error: string
}

export interface LogIn {}

export interface RegisterError {
    name: string[],
    address: string[],
    email: string[],
    password: string[],
    confirmPassword: string[]
}

export interface Register {}

export interface ForgotPasswordError {
    email: string[]
}

export interface ForgotPassword {}

export interface UpdateAccountError {
    name: string[],
    address: string[],
    email: string[],
    password: string[]
}

export interface UpdateAccount {}

export interface Account {
    name: string,
    address: string,
    email: string
}

export interface PasswordError {
    password: string[]
}

export interface Password {}

export interface ChangePasswordError {
    password: string[],
    confirmPassword: string[]
}

export interface ChangePassword {}

export interface VerifyResult {}

export const isLogInModelError = (data: LogInModelError | LogInError | undefined) => {
    return data !== undefined && typeof data === "object" && "password" in data && "email" in data;
};

export const isLogInError = (data: LogInModelError | LogInError | undefined) => {
    return data !== undefined && typeof data === "object" && "error" in data;
}

export const isRegisterError = (data: RegisterError | undefined) => {
    return data !== undefined && typeof data === "object" && "Name" in data && "address" in data && "email" in data && "password" in data && "confirmPassword" in data;
};

export const isForgotPasswordError = (data: ForgotPasswordError | undefined) => {
    return data !== undefined && typeof data === "object" && "email" in data;
};

export const isUpdateAccountError = (data: UpdateAccountError | undefined) => {
    return data !== undefined && typeof data === "object" && "Name" in data && "address" in data && "email" in data && "password" in data;
};

export const isPasswordError = (data: PasswordError | undefined) => {
    return data !== undefined && typeof data === "object" && "password" in data;
};

export const isChangePasswordError = (data: ChangePasswordError | ChangePassword | undefined) => {
    return data !== undefined && typeof data === "object" && "password" in data && "confirmPassword" in data;
};

export const useRegister = (name: string, address: string, email: string, password: string, confirmPassword: string, enabled: boolean) => {
    return useQuery<Register, AxiosError<RegisterError>>({
        queryKey: ["register", name, email],
        queryFn: async () => {
            return instance.post<Register>(
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
    return useQuery<LogIn, AxiosError<LogInModelError | LogInError>>({
        queryKey: ["login", email],
        queryFn: async () => {
            return instance.post<LogIn>(
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
    return useQuery<ForgotPassword, AxiosError<ForgotPasswordError>>({
        queryKey: ["forgotpassword", email],
        queryFn: async () => {
            return instance.post<ForgotPassword>(
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

export const useVerify = (id: string, token: string, enabled: boolean) => {
    return useQuery<VerifyResult, AxiosError<VerifyResult>>({
        queryKey: ["verify", token],
        queryFn: async () => {
            return instance.post<VerifyResult>(
                `${authEndpoint}verify/${id}/${token}`,
                {},
                {
                    withCredentials: true
                }
            )
        },
        enabled: enabled,
        retry: 2
    });
}

export const useChangedPassword = (id: string, token: string, password: string, confirmPassword: string, enabled: boolean) => {
    return useQuery<ChangePassword, AxiosError<ChangePasswordError>>({
        queryKey: ["changepassword", token],
        queryFn: async () => {
            return instance.post<ChangePassword>(
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
    return useQuery<UpdateAccount, AxiosError<UpdateAccountError>>({
        queryKey: ["updateaccount", name],
        queryFn: async () => {
            return instance.post<UpdateAccount>(
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

export const useDeleteAccount = (password: string, enabled: boolean) => {
    return useQuery<Password, AxiosError<PasswordError>>({
        queryKey: ["deleteaccount"],
        queryFn: async () => {
            return instance.delete<Password>(
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