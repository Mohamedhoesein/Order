import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { MDBValidation } from "mdb-react-ui-kit";
import { FormState, initializeForm } from "../../components/form/types";
import { Input } from "../../components/form/input/input";
import { SubmitButton } from "../../components/form/submit-button/submit-button";
import { LogInError, isLogInError, useLogIn } from "../../hooks/APIHook";
import { defaultEmailError, defaultPasswordError, passwordRegex } from "../../utils/constant";
import "./login.css";
import { MessageContext } from "../../utils/context";

const Keys = ["email", "password"] as const;
type Keys = Array<typeof Keys[number]>;

interface State extends FormState<Keys> {
};

export const LogIn = () => {
    const navigate = useNavigate();
    const [state, setCurrentState] = useState<State>(initializeForm<Keys, State>({
        keys: Keys,
        errors: {
            email: defaultEmailError,
            password: defaultPasswordError
        },
        otherValues: {
        }
    }));
    const {isLoading, isSuccess, data} = useLogIn(state.formData.email, state.formData.password, state.disabled);
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
                if (isLogInError(data?.data)) {
                    const error = data?.data as LogInError;
                    setCurrentState({
                        ...partialState,
                        formErrors: {
                            ...partialState.formErrors,
                            email: error.Email.join("\n"),
                            password: error.Password.join("\n")
                        }
                    });
                }
                else {
                    setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                }
            }
        }
    }, [navigate, state, isLoading, isSuccess, data]);

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
                                 pattern={passwordRegex}
                                 state={state}
                                 type="password"/>
                                <SubmitButton
                                 onSubmit={setCurrentState}
                                 partialWidth={false}
                                 state={state}
                                 text="Log In"/>
                            </MDBValidation>
                            <a href="/forgot-password" className="link">Forgot Password?</a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};