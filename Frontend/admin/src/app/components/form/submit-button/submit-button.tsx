import { MDBBtn } from "mdb-react-ui-kit"
import { FormState, SubmitProps } from "../types"

export const SubmitButton = <State extends FormState<[]>>({onSubmit, partialWidth = true, state, text}: SubmitProps<State>) => {
    const classes = (partialWidth ? "offset-md-4 col-md-4 " : "") + "col-12";

    return (
        <MDBBtn
         id={state.submitId}
         className={classes}
         type="submit"
         block={!partialWidth}
         onClick={_ => onSubmit()}
         disabled={state.disabled || Object.values(state.formHasError).some(error => error)}>
            {text}
        </MDBBtn>
    )
}