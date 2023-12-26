import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { MDBContainer, MDBValidation } from "mdb-react-ui-kit";
import { FormState, initializeForm, submitValidate } from "../../components/form/types";
import { defaultPasswordError, passwordRegex } from "../../utils/constant";
import { Input } from "../../components/form/input/input";
import { SubmitButton } from "../../components/form/submit-button/submit-button";
import { PasswordError, isPasswordError, useDeleteAccount } from "../../hooks/APIHook";
import { MessageContext } from "../../utils/context";

const Keys = ["deletePassword"] as const;
type Keys = Array<typeof Keys[number]>;

type State = FormState<Keys>

export const Delete = () => {
    const navigate = useNavigate();
    const [state, setCurrentState] = useState<State>(initializeForm<Keys, State>({
        keys: Keys,
        errors: {
            deletePassword: defaultPasswordError
        },
        required: Keys,
        pattern: {
            deletePassword: passwordRegex,
        },
        submitId: 'delete-submit'
    }));
    const {isLoading, isSuccess, data, error} = useDeleteAccount(state.formData.deletePassword, state.disabled);
    const {setState} = useContext(MessageContext);
    useEffect(() => {
        if (state.disabled && !isLoading) {
            if (isSuccess) {
                navigate("/login");
            }
            else {
                let partialState: State = {
                    ...state,
                    disabled: false
                };
                if (error?.response?.data != undefined) {
                    if (isPasswordError(error?.response?.data)) {
                        const errorContent = error?.response?.data as PasswordError;
                        partialState = {
                            ...partialState,
                            formErrors: {
                                ...partialState.formErrors,
                                deletePassword: errorContent.password.length > 0 ? errorContent.password[0] : ""
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
    }, [state, isLoading, isSuccess, data, navigate, error, setState]);

    const submit = () => {
        submitValidate(Keys, state, setCurrentState);
    };

    return (
        <MDBContainer>
            <h1 className="text-center">Delete Account</h1>
            <MDBValidation className="mt-3 g-3" isValidated={true}>
                <Input
                 name="deletePassword"
                 onChange={setCurrentState}
                 state={state}
                 partialWidth={false}
                 type="password"/>
                <SubmitButton
                 onSubmit={submit}
                 state={state}
                 partialWidth={false}
                 text="Delete Account"/>
            </MDBValidation>
        </MDBContainer>
    );
}