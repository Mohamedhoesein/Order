import { MDBInput, MDBValidationItem } from "mdb-react-ui-kit";
import { FormState, InputProps } from "../types";
import "./input.css";

export const Input = <Property extends string, State extends FormState<[Property]>>
    ({name, partialWidth = true, pattern, required = true, state, onChange, type, additionalCondition, disabled = false}: InputProps<Property, State>) => {
    let classes = (partialWidth ? "offset-md-4 col-md-4 col-12 " : "col-12 ") + "mb-2";
    classes += state.formHasError[name] ? " invalid" : "";
    return (
        <MDBValidationItem
         className={classes}
         feedback={state.formHasError[name] ? state.formErrors[name] : ""}
         invalid={state.formHasError[name]}>
           <MDBInput
            id={name}
            type={type}
            label={name[0].toUpperCase() + name.slice(1)}
            value={state.formData[name]}
            onChange={e => {
            onChange({
                ...state,
                formData: {
                    ...state.formData,
                    [name]: e.target.value
                },
                formHasError: {
                    ...state.formHasError,
                    [name]: (required && !e.target.value) || (pattern !== undefined && !e.target.value.match(pattern)) || (additionalCondition != undefined && !additionalCondition(e.target.value))
                }
            })}}
            readOnly={state.disabled || disabled}/>
       </MDBValidationItem>
    );
};