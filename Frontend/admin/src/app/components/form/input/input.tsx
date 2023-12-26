import { MDBInput, MDBValidationItem } from "mdb-react-ui-kit";
import {
    BoolRequired,
    FormState,
    InputProps,
    StringRequired,
    validate
} from "../types";
import "./input.css";

export const Input = <All extends string[], Property extends All[number], FullState extends FormState<All>>
    ({name, partialWidth = true, state, onChange, type, disabled = false}: InputProps<All, Property, FullState>) => {
    const usableRequiredKeyBool = name as keyof BoolRequired<[Property]>;
    const usableRequiredKeyString = name as keyof StringRequired<[Property]>;
    const invalid = state.formHasError[usableRequiredKeyString] && state.formDirty[usableRequiredKeyBool];
    let classes = (partialWidth ? "offset-md-4 col-md-4 col-12 " : "col-12 ") + "mb-2";
    classes += invalid ? " invalid" : "";

    return (
        <MDBValidationItem
         className={classes}
         feedback={invalid ? state.formErrors[usableRequiredKeyString] : ""}
         invalid={invalid}>
           <MDBInput
            id={name.toString()}
            type={type}
            label={name[0].toUpperCase() + name.slice(1)}
            value={state.formData[usableRequiredKeyString]}
            onChange={e => {
                state.formData[usableRequiredKeyString] = e.target.value;
                state.formDirty[usableRequiredKeyBool] = true;
                validate(
                    name,
                    state,
                    onChange
                );
            }}
            readOnly={state.disabled || disabled}/>
       </MDBValidationItem>
    );
};