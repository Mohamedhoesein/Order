import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { MDBValidation } from "mdb-react-ui-kit";
import { FormState, initializeForm } from "../../components/form/types";
import { defaultEmailError } from "../../utils/constant";
import { ForgotPasswordError, isForgotPasswordError, useForgotPassword } from "../../hooks/APIHook";
import { Input } from "../../components/form/input/input";
import { SubmitButton } from "../../components/form/submit-button/submit-button";
import { MessageContext } from "../../utils/context";

const Keys = ["email"] as const;
type Keys = Array<typeof Keys[number]>;

interface State extends FormState<Keys> {
};

export const ForgotPassword = () => {
    const navigate = useNavigate();
    const [state, setCurrentState] = useState<State>(initializeForm<Keys, State>({
        keys: Keys,
        errors: {
            email: defaultEmailError
        },
        otherValues: {
        }
    }));
    const {isLoading, isSuccess, data} = useForgotPassword(state.formData.email, state.disabled);
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
                if (isForgotPasswordError(data?.data)) {
                    const error = data?.data as ForgotPasswordError;
                    setCurrentState({
                        ...partialState,
                        formErrors: {
                            ...partialState.formErrors,
                            email: error.Email.join("\n")
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
                                 onSubmit={setCurrentState}
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