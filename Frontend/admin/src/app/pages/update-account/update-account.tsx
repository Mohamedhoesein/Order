import { useContext, useEffect, useState } from "react";
import { FormState, initializeForm, submitValidate } from "../../components/form/types";
import {
    defaultAddressError,
    defaultEmailError,
    defaultNameError,
    defaultPasswordError, emailRegex,
    passwordRegex
} from "../../utils/constant";
import { MDBBtn, MDBContainer, MDBModal, MDBModalBody, MDBModalContent, MDBModalDialog, MDBModalHeader, MDBValidation } from "mdb-react-ui-kit";
import { Input } from "../../components/form/input/input";
import { SubmitButton } from "../../components/form/submit-button/submit-button";
import {
    UpdateAccountError,
    isUpdateAccountError,
    useGetAccount,
    useUpdateAccount,
    useForgotPasswordUser
} from "../../hooks/APIHook";
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
        required: Keys,
        pattern: {
            email: emailRegex,
            password: passwordRegex
        },
        otherValues: {
            loading: true,
            forgot: false
        },
        submitId: 'update-submit'
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
                    formHasError: {
                        ...state.formHasError,
                        name: false,
                        address: false,
                        email: false,
                    },
                    loading: false
                });
            }
        }
    }, [state, isLoadingAccount, isSuccessAccount, account]);
    const {isLoading: isLoadingChange, isSuccess: isSuccessChange, data: change} = useForgotPasswordUser(state.forgot);
    const {setState} = useContext(MessageContext);
    useEffect(() => {
        if (state.forgot && !isLoadingChange && !state.loading) {
            if (isSuccessChange)
                setState({message: "An email has been send.", showMessage: true, timeout: false});
            else
                setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
            setCurrentState({
                ...state,
                forgot: false
            });

        }
    }, [state, isLoadingChange, isSuccessChange, change, setState]);
            
    const {isLoading, isSuccess, data, error} = useUpdateAccount(state.formData.name, state.formData.address, state.formData.password, state.disabled);
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
                let partialState: State = {
                    ...state,
                    disabled: false
                };

                if (error?.response?.data != undefined) {
                    if (isUpdateAccountError(error?.response?.data)) {
                        const errorContent = error?.response?.data as UpdateAccountError;
                        partialState = {
                            ...partialState,
                            formErrors: {
                                ...partialState.formErrors,
                                email: errorContent.email.length > 0 ? errorContent.email[0] : "",
                                name: errorContent.name.length > 0 ? errorContent.name[0] : "",
                                address: errorContent.address.length > 0 ? errorContent.address[0] : "",
                                password: errorContent.password.length > 0 ? errorContent.password[0] : ""
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
    }, [state, isLoading, isSuccess, data, error, setState]);

    const submit = () => {
        submitValidate(Keys, state, setCurrentState);
    };

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
                 state={state}
                 type="password"/>
                <SubmitButton
                 onSubmit={submit}
                 state={state}
                 text="Update Account"/>
            </MDBValidation>
            <MDBBtn id="delete-account" className="offset-md-4 col-md-2 col-6 mt-4" color="danger" onClick={swapDelete}>Delete Account</MDBBtn>
            <MDBModal show={showDelete} setShow={setShowDelete} tabIndex="-1">
                <MDBModalDialog>
                    <MDBModalContent>
                        <MDBModalHeader>
                            <MDBBtn id="close-delete" className="btn-close" color="none" onClick={() => setShowDelete(false)}/>
                        </MDBModalHeader>
                        <MDBModalBody>
                            <Delete/>
                        </MDBModalBody>
                    </MDBModalContent>
                </MDBModalDialog>
            </MDBModal>
            <MDBBtn id="forgot-password" className="col-md-2 col-6" color="warning" onClick={() => setCurrentState({...state, forgot: true})}>Forgot Password</MDBBtn>
        </MDBContainer>
    );
}