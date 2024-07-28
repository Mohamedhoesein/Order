import { useContext, useEffect, useState } from "react";
import { MDBModal, MDBModalDialog, MDBModalContent, MDBModalBody, MDBValidation, MDBBtn, MDBModalHeader, MDBModalTitle } from "mdb-react-ui-kit";
import { Input } from "../../components/form/input/input";
import { FormState, initializeForm, submitValidate } from "../../components/form/types";
import { SubmitButton } from "../../components/form/submit-button/submit-button";
import { defaultNameError } from "../../utils/constant";
import { useUpdateCategory } from "../../hooks/APIHook";
import { MessageContext } from "../../utils/context";

const Keys = ["name"] as const;
type Keys = Array<typeof Keys[number]>;

interface State extends FormState<Keys> {
    hide: () => void,
    name: string
}

interface Props {
    hide: () => void,
    name: string
}

export const RenameClosedSpecificationFilter = (props: Props) => {
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
            hide: props.hide,
            name: props.name
        },
        submitId: 'rename-category-submit'
    }));
    const {isLoading, isSuccess} = useUpdateCategory(state.name, state.formData.name, state.disabled);
    const {setState} = useContext(MessageContext);
    useEffect(() => {
        if (state.disabled && !isLoading) {
            if (isSuccess) {
                state.hide();
            }
            else {
                setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
            }
        }
    }, [isLoading, isLoading, state, setCurrentState]);

    const submit = () => {
        submitValidate(Keys, state, setCurrentState);
    };

    return (
        <>
            <MDBModal open={true} onClose={() => state.hide()}>
                <MDBModalDialog>
                    <MDBModalContent>
                        <MDBModalHeader>
                            <MDBModalTitle>
                                Rename Category
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
                                 text={"Set Filter"}/>
                            </MDBValidation>
                        </MDBModalBody>
                    </MDBModalContent>
                </MDBModalDialog>
            </MDBModal>
        </>
    );
}