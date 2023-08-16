import { useContext, useEffect, useState } from "react";
import { FormState, initializeForm } from "../../components/form/types";
import {
    defaultAddressError,
    defaultEmailError,
    defaultNameError,
    defaultPasswordError,
    passwordRegex
} from "../../utils/constant";
import { MDBBtn, MDBContainer, MDBModal, MDBModalBody, MDBModalContent, MDBModalDialog, MDBValidation } from "mdb-react-ui-kit";
import { Input } from "../../components/form/input/input";
import { SubmitButton } from "../../components/form/submit-button/submit-button";
import { UpdateAccountError, isUpdateAccountError, useForgotPassword, useGetAccount, useUpdateAccount } from "../../hooks/APIHook";
import { Delete } from "./delete";
import { MessageContext } from "../../utils/context";

const Keys = ["name", "address", "email", "password"] as const;
type Keys = Array<typeof Keys[number]>;

interface State extends FormState<Keys> {
    loading: boolean;
    forgot: boolean;
}

export const UpdateAccount = () => {
    const [state, setCurrentState] = useState<State>(initializeForm<Keys, State>({
        keys: Keys,
        values: {
            name: "",
            address: "",
            email: "",
            password: ""
        },
        errors: {
            name: defaultNameError,
            address: defaultAddressError,
            email: defaultEmailError,
            password: defaultPasswordError
        },
        otherValues: {
            loading: true,
            forgot: false
        }
    }));
    const [showDelete, setShowDelete] = useState(false);
    const swapDelete = () => setShowDelete(!showDelete);
    const {isLoading: isLoadingAccount, isSuccess: isSuccessAccount, data: account} = useGetAccount(state.loading);
    useEffect(() => {
        if (state.loading && !isLoadingAccount) {
            if (isSuccessAccount && account) {
                setCurrentState({
                    ...state,
                    formData: {
                        name: account.data.name,
                        address: account.data.address,
                        email: account.data.email,
                        password: ""
                    },
                    loading: false
                });
            }
        }
    }, [state, isLoadingAccount, isSuccessAccount, account]);
    const {isLoading: isLoadingChange, isSuccess: isSuccessChange, data: change} = useForgotPassword(state.formData.email, state.forgot);
    const {setState} = useContext(MessageContext);
    useEffect(() => {
        if (state.forgot && !isLoadingChange && !isSuccessChange && !state.loading) {
            if (isSuccess)
                setState({message: "An email has been send.", showMessage: true, timeout: false});
            else
                setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
            setCurrentState({
                ...state,
                forgot: false
            });

        }
    }, [state, isLoadingChange, isSuccessChange, change]);
            
    const {isLoading, isSuccess, data} = useUpdateAccount(state.formData.name, state.formData.address, state.formData.password, state.disabled);
    useEffect(() => {
        if (state.disabled && !isLoading && !state.loading) {
            if (isSuccess) {
                setState({message: "Account updated successfully.", showMessage: true, timeout: false});
                setCurrentState({
                    ...state,
                    disabled: false
                });
            }
            else {
                const partialState: State = {
                    ...state,
                    disabled: false
                };
    
                if (isUpdateAccountError(data?.data)) {
                    const error = data?.data as UpdateAccountError;
                    setCurrentState({
                        ...partialState,
                        formErrors: {
                            ...partialState.formErrors,
                            email: error.Email.join("\n"),
                            name: error.Name.join("\n"),
                            address: error.Address.join("\n"),
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
            <h1 className="text-center">Update Account</h1>
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
                 type="email"
                 disabled={true}/>
                <Input
                 name="password"
                 onChange={setCurrentState}
                 pattern={passwordRegex}
                 state={state}
                 type="password"/>
                <SubmitButton
                 onSubmit={setCurrentState}
                 state={state}
                 text="Update Account"/>
            </MDBValidation>
            <MDBBtn className="offset-md-4 col-md-2 col-6 mt-4" color="danger" onClick={swapDelete}>Delete Account</MDBBtn>
            <MDBModal show={showDelete} setShow={setShowDelete} tabIndex="-1">
                <MDBModalDialog>
                    <MDBModalContent>
                        <MDBModalBody>
                            <Delete/>
                        </MDBModalBody>
                    </MDBModalContent>
                </MDBModalDialog>
            </MDBModal>
            <MDBBtn className="col-md-2 col-6" color="warning" onClick={() => setCurrentState({...state, forgot: true})}>Forgot Password</MDBBtn>
        </MDBContainer>
    );
}