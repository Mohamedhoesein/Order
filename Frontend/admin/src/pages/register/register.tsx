import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { MDBContainer, MDBValidation } from "mdb-react-ui-kit";
import { FormState, initializeForm } from "../../components/form/types";
import { Input } from "../../components/form/input/input";
import { SubmitButton } from "../../components/form/submit-button/submit-button";
import {
    defaultAddressError,
    defaultConfirmPasswordError,
    defaultEmailError,
    defaultNameError,
    defaultPasswordError,
    passwordRegex
} from "../../utils/constant";
import { RegisterError, isRegisterError, useRegister } from "../../hooks/APIHook";
import { MessageContext } from "../../utils/context";

const Keys = ["name", "address", "email", "password", "confirmPassword"] as const;
type Keys = Array<typeof Keys[number]>;

interface State extends FormState<Keys> {
}

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
        otherValues: {
        }
    }));
    const {isLoading, isSuccess, data} = useRegister(state.formData.name, state.formData.address, state.formData.email, state.formData.password, state.formData.confirmPassword, state.disabled);
    const {setState} = useContext(MessageContext);
    useEffect(() => {
        if (state.disabled && !isLoading) {
            if (isSuccess) {
                navigate("/");
            }
            else {
                const partialState: State = {
                    ...state,
                    disabled: false
                };
                if (isRegisterError(data?.data)) {
                    const error = data?.data as RegisterError;
                    setCurrentState({
                        ...partialState,
                        formErrors: {
                            ...partialState.formErrors,
                            email: error.Email.join("\n"),
                            name: error.Name.join("\n"),
                            address: error.Address.join("\n"),
                            password: error.Password.join("\n"),
                            confirmPassword: error.ConfirmPassword.join("\n")
                        }
                    });
                }
                else {
                    setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                }
            }
        }
    }, [navigate, state, isLoading, isSuccess, data]);
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
                 pattern={passwordRegex}
                 state={state}
                 type="password"/>
                <Input
                 name="confirmPassword"
                 onChange={setCurrentState}
                 pattern={passwordRegex}
                 state={state}
                 type="password"
                 additionalCondition={(text: string) => state.formData.password == text}/>
                <SubmitButton
                 onSubmit={setCurrentState}
                 state={state}
                 text="Register"/>
            </MDBValidation>
        </MDBContainer>
    );
}