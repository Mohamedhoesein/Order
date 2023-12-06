import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { MDBValidation } from "mdb-react-ui-kit";
import {FormState, initializeForm, submitValidate} from "../../components/form/types";
import { Input } from "../../components/form/input/input";
import { SubmitButton } from "../../components/form/submit-button/submit-button";
import {LogInModelError, isLogInModelError, useLogIn, isLogInError, LogInError} from "../../hooks/APIHook";
import { defaultEmailError, defaultPasswordError, emailRegex, passwordRegex } from "../../utils/constant";
import { MessageContext } from "../../utils/context";
import "./login.css";

const Keys = ["email", "password"];
type Keys = Array<typeof Keys[number]>;

type State = FormState<Keys>;

export const LogIn = () => {
    const navigate = useNavigate();
    const [state, setCurrentState] = useState<State>(initializeForm<Keys, State>({
        keys: Keys,
        errors: {
            email: defaultEmailError,
            password: defaultPasswordError
        },
        required: Keys,
        pattern: {
            email: emailRegex,
            password: passwordRegex
        },
        disabled: false,
        submitId: 'login-submit'
    }));
    const {isLoading, isSuccess, data, error} = useLogIn(state.formData.email, state.formData.password, state.disabled);
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
                    if (isLogInModelError(error?.response?.data)) {
                        const errorContent = error.response.data as LogInModelError;
                        partialState = {
                            ...partialState,
                            formErrors: {
                                ...partialState.formErrors,
                                email: errorContent.email.length > 0 ? errorContent.email[0] : "",
                                password: errorContent.password.length > 0 ? errorContent.password[0] : ""
                            },
                            formHasError: {
                                ...partialState.formHasError,
                                email: errorContent.email.length > 0,
                                password: errorContent.password.length > 0
                            }
                        };
                    }
                    else if (isLogInError(error?.response?.data) && (error?.response?.data as LogInError).error === "Invalid") {
                        setState({message: "Invalid credentials.", showMessage: true, timeout: false});
                    }
                    else {
                        setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                    }
                }
                else {
                    setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                }
                setCurrentState(partialState)
            }
        }
    }, [navigate, state, isLoading, isSuccess, data, error, setState, setCurrentState]);

    const submit = () => {
        submitValidate(Keys, state, setCurrentState);
    };

    return (
        <div
         className="d-flex align-items-center justify-content-center stripes"
         style={{height: "100vh", width: "100vw"}}>
            <div className="modal-dialog modal-dialog-centered">
                <div className="modal-dialog">
                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title">Log In</h5>
                        </div>
                        <div className="modal-body">
                            <MDBValidation isValidated={true}>
                                <Input
                                 name="email"
                                 onChange={setCurrentState}
                                 partialWidth={false}
                                 state={state}
                                 type="email"/>
                                <Input
                                 name="password"
                                 onChange={setCurrentState}
                                 partialWidth={false}
                                 state={state}
                                 type="password"/>
                                <SubmitButton
                                 onSubmit={submit}
                                 partialWidth={false}
                                 state={state}
                                 text="Log In"/>
                            </MDBValidation>
                            <a id="forgot-password" href="/forgot-password" className="link">Forgot Password?</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};