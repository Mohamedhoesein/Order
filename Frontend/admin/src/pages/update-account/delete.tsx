import { useContext, useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { MDBContainer, MDBValidation } from "mdb-react-ui-kit";
import { FormState, initializeForm } from "../../components/form/types";
import { defaultPasswordError, passwordRegex } from "../../utils/constant";
import { Input } from "../../components/form/input/input";
import { SubmitButton } from "../../components/form/submit-button/submit-button";
import { PasswordError, isPasswordError, useDeleteaccount } from "../../hooks/APIHook";
import { MessageContext } from "../../utils/context";

const Keys = ["password"] as const;
type Keys = Array<typeof Keys[number]>;

interface State extends FormState<Keys> {
}

export const Delete = () => {
    const navigate = useNavigate();
    const [state, setCurrentState] = useState<State>(initializeForm<Keys, State>({
        keys: Keys,
        errors: {
            password: defaultPasswordError
        },
        otherValues: {
        }
    }));
    const {isLoading, isSuccess, data} = useDeleteaccount(state.formData.password, state.disabled);
    const {setState} = useContext(MessageContext);
    useEffect(() => {
        if (state.disabled && !isLoading) {
            if (isSuccess) {
                navigate("/login");
            }
            else {
                const partialState: State = {
                    ...state,
                    disabled: false
                };
                if (isPasswordError(data?.data)) {
                    const error = data?.data as PasswordError;
                    setCurrentState({
                        ...partialState,
                        formErrors: {
                            ...partialState.formErrors,
                            password: error.Password.join("\n")
                        }
                    });
                }
                else {
                    setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                }
            }
        }
    }, [state, isLoading, isSuccess, data]);

    return (
        <MDBContainer>
            <h1 className="text-center">Delete Account</h1>
            <MDBValidation className="mt-3 g-3" isValidated={true}>
                <Input
                 name="password"
                 onChange={setCurrentState}
                 pattern={passwordRegex}
                 state={state}
                 partialWidth={false}
                 type="password"/>
                <SubmitButton
                 onSubmit={setCurrentState}
                 state={state}
                 partialWidth={false}
                 text="Delete Account"/>
            </MDBValidation>
        </MDBContainer>
    );
}