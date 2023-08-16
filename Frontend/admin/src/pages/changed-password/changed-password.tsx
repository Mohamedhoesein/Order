import { useContext, useEffect, useState } from "react";
import { Navigate, useNavigate, useParams } from "react-router-dom";
import { MDBValidation } from "mdb-react-ui-kit";
import { ChangePasswordError, isChangePasswordError, useChangedPassword } from "../../hooks/APIHook";
import { MessageContext } from "../../utils/context";
import { Input } from "../../components/form/input/input";
import { FormState, initializeForm } from "../../components/form/types";
import { defaultPasswordError, passwordRegex } from "../../utils/constant";
import { SubmitButton } from "../../components/form/submit-button/submit-button";

type Params = {
    id: string;
    code: string;
};

const Keys = ["password", "confirmPassword"] as const;
type Keys = Array<typeof Keys[number]>;

interface State extends FormState<Keys> {
};

export const ChangedPassword = () => {
    const navigate = useNavigate();
    const params = useParams<Params>();
    if (!params.id || !params.code || isNaN(Number(params.id)))
        return (<Navigate to="/not-found"/>);

    const [state, setCurrentState] = useState<State>(initializeForm<Keys, State>({
        keys: Keys,
        errors: {
            password: defaultPasswordError,
            confirmPassword: defaultPasswordError
        },
        otherValues: {
        }
    }));
    const {isLoading, isSuccess, data} = useChangedPassword(params.id, params.code, state.formData.password, state.formData.confirmPassword, state.disabled);
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
                if (isChangePasswordError(data?.data)) {
                    const error = data?.data as ChangePasswordError;
                    setCurrentState({
                        ...partialState,
                        formErrors: {
                            ...partialState.formErrors,
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
    }, [isLoading, isSuccess, setState, state.disabled]);

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
                                 name="password"
                                 onChange={setCurrentState}
                                 partialWidth={false}
                                 pattern={passwordRegex}
                                 state={state}
                                 type="password"/>
                                <Input
                                 name="confirmPassword"
                                 onChange={setCurrentState}
                                 partialWidth={false}
                                 pattern={passwordRegex}
                                 state={state}
                                 type="password"/>
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