import { useContext, useEffect, useState } from "react";
import { FormState, initializeForm, submitValidate } from "../../components/form/types";
import { defaultNameError } from "../../utils/constant";
import { MDBBtn, MDBModal, MDBModalBody, MDBModalContent, MDBModalDialog, MDBModalHeader, MDBModalTitle, MDBValidation } from "mdb-react-ui-kit";
import { Input } from "../../components/form/input/input";
import { SubmitButton } from "../../components/form/submit-button/submit-button";
import { useAddOverwriteOpenSpecification, useRenameOpenSpecification } from "../../hooks/APIHook";
import { MessageContext } from "../../utils/context";

const Keys = ["name"] as const;
type Keys = Array<typeof Keys[number]>;

interface State extends FormState<Keys> {
    new: boolean,
    category: string,
    oldName: string,
    restore: number,
    delete: number,
    rename: number,
    hide: (name: string | null) => void
}

interface Props {
    new: boolean,
    category: string,
    oldName: string,
    hide: (name: string | null) => void
}

export const RenameOpenSpecification = (props: Props) => {
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
            category: props.category,
            oldName: props.oldName,
            hide: props.hide
        },
        submitId: 'rename-open-specification-submit'
    }));
    const {isLoading: isLoadingRename, isSuccess: isSuccessRename, error: errorRename} = useRenameOpenSpecification(
        state.category,
        state.oldName,
        state.formData.name,
        state.disabled && !state.new
    );
    const {isLoading: isLoadingNew, isSuccess: isSuccessNew, error: errorNew} = useAddOverwriteOpenSpecification(
        state.category,
        state.formData.name,
        false,
        state.disabled && !state.new
    );
    const {setState} = useContext(MessageContext);
    useEffect(() => {
        if (state.disabled && !isLoadingRename) {
            if (isSuccessRename) {
                state.hide(state.formData.name);
            }
            else {
                let partialState: State = {
                    ...state,
                    disabled: false
                };
                if (errorRename?.status !== 400) {
                    setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                }
                else {
                    state.hide(null);
                }
                setCurrentState(partialState);
            }
        }
    }, [state, isLoadingRename, isSuccessRename, errorRename, setState]);
    useEffect(() => {
        if (state.disabled && !isLoadingNew) {
            if (isSuccessNew) {
                state.hide(state.formData.name);
            }
            else {
                let partialState: State = {
                    ...state,
                    disabled: false
                };
                if (errorNew?.status !== 400) {
                    setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                }
                else {
                    state.hide(null);
                }
                setCurrentState(partialState);
            }
        }
    }, [state, isLoadingNew, isSuccessNew, errorNew, setState]);
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
                                Rename Open Specification
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
                                 text={props.new ? "Create Specification" : "Rename Specification"}/>
                            </MDBValidation>
                        </MDBModalBody>
                    </MDBModalContent>
                </MDBModalDialog>
            </MDBModal>
        </>
    );
}