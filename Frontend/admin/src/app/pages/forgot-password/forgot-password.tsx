import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { MDBValidation } from "mdb-react-ui-kit";
import {FormState, initializeForm, submitValidate} from "../../components/form/types";
import {defaultEmailError, emailRegex} from "../../utils/constant";
import { ForgotPasswordError, isForgotPasswordError, useForgotPassword } from "../../hooks/APIHook";
import { Input } from "../../components/form/input/input";
import { SubmitButton } from "../../components/form/submit-button/submit-button";
import { MessageContext } from "../../utils/context";

const Keys = ["email"] as const;
type Keys = Array<typeof Keys[number]>;

type State = FormState<Keys>;

export const ForgotPassword = () => {
    const navigate = useNavigate();
    const [state, setCurrentState] = useState<State>(initializeForm<Keys, State>({
        keys: Keys,
        errors: {
            email: defaultEmailError
        },
        required: Keys,
        pattern: {
            email: emailRegex
        },
        submitId: 'forgot-password-submit'
    }));
    const {isLoading, isSuccess, error} = useForgotPassword(state.formData.email, state.disabled);
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
                if (error?.response?.data !== undefined && isForgotPasswordError(error.response.data)) {
                    const errorContent = error.response.data as ForgotPasswordError;
                    partialState = {
                        ...partialState,
                        formErrors: {
                            ...partialState.formErrors,
                            email: errorContent.email.length > 0 ? errorContent.email[0] : ''
                        },
                        formHasError: {
                            ...partialState.formHasError,
                            email: errorContent.email.length > 0
                        }
                    };
                    setState({message: "Email send.", showMessage: true, timeout: false});
                }
                else {
                    setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                }
                setCurrentState(partialState);
            }
        }
    }, [navigate, state, isLoading, isSuccess, error, setState]);

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
                            <h5 className="modal-title">Change Password</h5>
                        </div>
                        <div className="modal-body">
                            <MDBValidation isValidated={true}>
                                <Input
                                 name="email"
                                 onChange={setCurrentState}
                                 partialWidth={false}
                                 state={state}
                                 type="email"/>
                                <SubmitButton
                                 onSubmit={submit}
                                 partialWidth={false}
                                 state={state}
                                 text="Change Password"/>
                            </MDBValidation>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
};