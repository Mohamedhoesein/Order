import { useContext, useEffect, useState } from "react";
import { FormState, initializeForm, submitValidate } from "../../components/form/types";
import { defaultNameError } from "../../utils/constant";
import { MDBBtn, MDBModal, MDBModalBody, MDBModalContent, MDBModalDialog, MDBModalHeader, MDBModalTitle, MDBValidation } from "mdb-react-ui-kit";
import { Input } from "../../components/form/input/input";
import { SubmitButton } from "../../components/form/submit-button/submit-button";
import { useAddCategory } from "../../hooks/APIHook";
import { MessageContext } from "../../utils/context";

const Keys = ["name"] as const;
type Keys = Array<typeof Keys[number]>;

interface State extends FormState<Keys> {
    hide: () => void
}

interface Props {
    hide: () => void
}

export const AddCategory = (props: Props) => {
    const [state, setCurrentState] = useState<State>(initializeForm<Keys, State>({
        keys: Keys,
        errors: {
            name: defaultNameError
        },
        required: Keys,
        pattern: {
        },
        additionalCondition: {
        },
        otherValues: {
            hide: props.hide
        },
        submitId: 'add-category-submit'
    }));
    const {isLoading, isSuccess, data, error} = useAddCategory(state.formData.name, state.disabled);
    const {setState} = useContext(MessageContext);
    useEffect(() => {
        if (state.disabled && !isLoading) {
            if (isSuccess) {
                state.hide();
            }
            else {
                let partialState: State = {
                    ...state,
                    disabled: false
                };
                if (error?.status !== 400) {
                    setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                }
                else {
                    state.hide();
                }
                setCurrentState(partialState);
            }
        }
    }, [state, isLoading, isSuccess, data, error, setState]);
    const submit = () => {
        submitValidate(Keys, state, setCurrentState);
    };

    return (
        <>
            <MDBModal open={true} onClose={state.hide}>
                <MDBModalDialog>
                    <MDBModalContent>
                        <MDBModalHeader>
                            <MDBModalTitle>
                                Add Category
                            </MDBModalTitle>
                            <MDBBtn id="close" className="btn-close" color="none" onClick={state.hide}></MDBBtn>
                        </MDBModalHeader>
                        <MDBModalBody>
                            <MDBValidation isValidated={true}>
                                <Input
                                 name="name"
                                 onChange={setCurrentState}
                                 state={state}
                                 partialWidth={false}
                                 type="text"/>
                                <SubmitButton
                                 onSubmit={submit}
                                 state={state}
                                 partialWidth={false}
                                 text="Create Category"/>
                            </MDBValidation>
                        </MDBModalBody>
                    </MDBModalContent>
                </MDBModalDialog>
            </MDBModal>
        </>
    );
}