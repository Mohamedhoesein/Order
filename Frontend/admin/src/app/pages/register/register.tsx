import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { MDBContainer, MDBValidation } from "mdb-react-ui-kit";
import { FormState, initializeForm, submitValidate } from "../../components/form/types";
import { Input } from "../../components/form/input/input";
import { SubmitButton } from "../../components/form/submit-button/submit-button";
import {
    defaultAddressError,
    defaultConfirmPasswordError,
    defaultEmailError,
    defaultNameError,
    defaultPasswordError, emailRegex, passwordRegex
} from "../../utils/constant";
import { RegisterError, isRegisterError, useRegister } from "../../hooks/APIHook";
import { MessageContext } from "../../utils/context";

const Keys = ["name", "address", "email", "password", "confirmPassword"] as const;
type Keys = Array<typeof Keys[number]>;

type State = FormState<Keys>

export const Register = () => {
    const navigate = useNavigate();
    const [state, setCurrentState] = useState<State>(initializeForm<Keys, State>({
        keys: Keys,
        errors: {
            name: defaultNameError,
            address: defaultAddressError,
            email: defaultEmailError,
            password: defaultPasswordError,
            confirmPassword: defaultPasswordError
        },
        required: Keys,
        pattern: {
            email: emailRegex,
            password: passwordRegex,
            confirmPassword: passwordRegex
        },
        additionalCondition: {
            confirmPassword: (text: string): boolean => state.formData.password == text
        },
        submitId: 'register-submit'
    }));
    const {isLoading, isSuccess, data, error} = useRegister(state.formData.name, state.formData.address, state.formData.email, state.formData.password, state.formData.confirmPassword, state.disabled);
    const {setState} = useContext(MessageContext);
    useEffect(() => {
        if (state.disabled && !isLoading) {
            if (isSuccess) {
                navigate("/");
            }
            else {
                let partialState: State = {
                    ...state,
                    disabled: false
                };
                if (error?.response?.data != undefined) {
                    if (isRegisterError(error?.response?.data)) {
                        const errorContent = error?.response?.data as RegisterError;
                        partialState = {
                            ...partialState,
                            formErrors: {
                                ...partialState.formErrors,
                                email: errorContent.email.length > 0 ? errorContent.email[0] : "",
                                name: errorContent.name.length > 0 ? errorContent.name[0] : "",
                                address: errorContent.address.length > 0 ? errorContent.address[0] : "",
                                password: errorContent.password.length > 0 ? errorContent.password[0] : "",
                                confirmPassword: errorContent.confirmPassword.length > 0 ? errorContent.confirmPassword[0] : ""
                            }
                        };
                    }
                    else {
                        setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                    }
                }
                else {
                    setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                }
                setCurrentState(partialState);
            }
        }
    }, [navigate, state, isLoading, isSuccess, data, error, setState]);

    if (
        state.formData.password !== state.formData.confirmPassword &&
        state.formErrors.confirmPassword === defaultPasswordError
    ) {
        setCurrentState({
            ...state,
            formErrors: {
                ...state.formErrors,
                confirmPassword: defaultConfirmPasswordError
            }
        });
    }
    if (
        state.formData.password === state.formData.confirmPassword &&
        state.formErrors.confirmPassword !== defaultPasswordError
    ) {
        setCurrentState({
            ...state,
            formErrors: {
                ...state.formErrors,
                confirmPassword: defaultPasswordError
            }
        });
    }

    const submit = () => {
        submitValidate(Keys, state, setCurrentState);
    };

    return (
        <MDBContainer>
            <h1 className="text-center">Register</h1>
            <MDBValidation className="mt-3 g-3" isValidated={true}>
                <Input
                 name="name"
                 onChange={setCurrentState}
                 state={state}
                 type="text"/>
                <Input
                 name="address"
                 onChange={setCurrentState}
                 state={state}
                 type="text"/>
                <Input
                 name="email"
                 onChange={setCurrentState}
                 state={state}
                 type="email"/>
                <Input
                 name="password"
                 onChange={setCurrentState}
                 state={state}
                 type="password"/>
                <Input
                 name="confirmPassword"
                 onChange={setCurrentState}
                 state={state}
                 type="password"/>
                <SubmitButton
                 onSubmit={submit}
                 state={state}
                 text="Register"/>
            </MDBValidation>
        </MDBContainer>
    );
}