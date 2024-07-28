import { useState } from "react";
import { FormState, initializeForm, submitValidate } from "../../components/form/types";
import { defaultNameError } from "../../utils/constant";
import { MDBBtn, MDBModal, MDBModalBody, MDBModalContent, MDBModalDialog, MDBModalHeader, MDBModalTitle, MDBValidation } from "mdb-react-ui-kit";
import { Input } from "../../components/form/input/input";
import { SubmitButton } from "../../components/form/submit-button/submit-button";

const Keys = ["name"] as const;
type Keys = Array<typeof Keys[number]>;

interface State extends FormState<Keys> {
    new: boolean,
    hide: (name: string | null) => void
}

interface Props {
    new: boolean,
    hide: (name: string | null) => void
}

export const RenameClosedSpecificationValue = (props: Props) => {
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
            new: props.new,
            hide: props.hide
        },
        submitId: 'rename-open-specification-submit'
    }));
    if (state.disabled)
        state.hide(state.formData.name);
    const submit = () => {
        submitValidate(Keys, state, setCurrentState);
    };

    return (
        <>
            <MDBModal open={true} onClose={() => state.hide(null)}>
                <MDBModalDialog>
                    <MDBModalContent>
                        <MDBModalHeader>
                            <MDBModalTitle>
                                Rename Closed Specification Value
                            </MDBModalTitle>
                            <MDBBtn id="close" className="btn-close" color="none" onClick={() => state.hide(null)}></MDBBtn>
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
                                 text={props.new ? "Create Value" : "Rename Value"}/>
                            </MDBValidation>
                        </MDBModalBody>
                    </MDBModalContent>
                </MDBModalDialog>
            </MDBModal>
        </>
    );
}