import { useContext, useEffect, useState } from "react";
import { Navigate, useNavigate, useParams } from "react-router-dom";
import { MDBValidation } from "mdb-react-ui-kit";
import { ChangePasswordError, isChangePasswordError, useChangedPassword } from "../../hooks/APIHook";
import { MessageContext } from "../../utils/context";
import { Input } from "../../components/form/input/input";
import {FormState, initializeForm, submitValidate, update} from "../../components/form/types";
import { defaultPasswordError, passwordRegex } from "../../utils/constant";
import { SubmitButton } from "../../components/form/submit-button/submit-button";

type Params = {
    id: string;
    code: string;
};

const Keys = ["password", "confirmPassword"] as const;
type Keys = Array<typeof Keys[number]>;

type State = FormState<Keys>;

export const ChangedPassword = () => {
    const navigate = useNavigate();
    const params = useParams<Params>();

    const [state, setCurrentState] = useState<State>(initializeForm<Keys, State>({
        keys: Keys,
        errors: {
            password: defaultPasswordError,
            confirmPassword: defaultPasswordError
        },
        required: Keys,
        pattern: {
            password: passwordRegex,
            confirmPassword: passwordRegex
        },
        additionalCondition: {
            confirmPassword: (text: string, currentState: State): boolean => currentState.formData.password === text
        },
        submitId: 'change-password-submit'
    }));
    const {isLoading, isSuccess, error} = useChangedPassword(
        params.id ?? '',
        params.code ?? '',
        state.formData.password,
        state.formData.confirmPassword,
        state.disabled && params.id !== undefined && params.code !== undefined && !isNaN(Number(params.id))
    );
    const {setState} = useContext(MessageContext);
    useEffect(() => {
        if (state.disabled && !isLoading) {
            if (isSuccess) {
                navigate("/");
                setState({message: 'Password changed.', showMessage: true, timeout: false});
            }
            else {
                let partialState: State = {
                    ...state,
                    disabled: false
                };

                if (error?.response?.data !== undefined && isChangePasswordError(error.response.data)) {
                    const errorContent = error.response.data as ChangePasswordError;
                    partialState = {
                        ...partialState,
                        formErrors: {
                            ...partialState.formErrors,
                            password: errorContent.password.length > 0 ? errorContent.password[0] : '',
                            confirmPassword: errorContent.confirmPassword.length > 0 ? errorContent.confirmPassword[0] : ''
                        },
                        formHasError: {
                            ...partialState.formHasError,
                            password: errorContent.password.length > 0,
                            confirmPassword: errorContent.confirmPassword.length > 0,
                        }
                    }
                    setState({message: 'An error occurred, please try again.', showMessage: true, timeout: false});
                }
                else {
                    setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                }
                setCurrentState(partialState);
            }
        }
    }, [error, isLoading, isSuccess, navigate, setState, state, state.disabled]);

    if (!params.id || !params.code || isNaN(Number(params.id)))
        return (<Navigate to="/not-found"/>);

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
                                 name="password"
                                 onChange={update(state, setCurrentState)}
                                 partialWidth={false}
                                 state={state}
                                 type="password"/>
                                <Input
                                 name="confirmPassword"
                                 onChange={update(state, setCurrentState)}
                                 partialWidth={false}
                                 state={state}
                                 type="password"/>
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