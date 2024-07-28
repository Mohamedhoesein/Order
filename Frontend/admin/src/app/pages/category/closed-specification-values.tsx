import { useContext, useEffect, useState } from "react";
import { ClosedSpecification, useAddOverwriteClosedSpecification, useRenameClosedSpecificationValue } from "../../hooks/APIHook";
import { MDBBtn, MDBListGroup, MDBListGroupItem } from "mdb-react-ui-kit";
import { MessageContext } from "../../utils/context";
import { RenameClosedSpecificationValue } from "./rename-closed-specification-value";

interface State {
    category: string,
    closedSpecification: ClosedSpecification,
    restore: number,
    delete: number,
    rename: number,
    renameName: string | null,
    newName: string | null,
    new: boolean
}

interface Props {
    category: string,
    closedSpecification: ClosedSpecification
}

const defaultState: (props: Props) => State = (props: Props) => {
    return {
        category: props.category,
        closedSpecification: props.closedSpecification,
        restore: -1,
        delete: -1,
        rename: -1,
        renameName: null,
        new: false,
        newName: null
    }
}

export const ClosedSpecificationValue = (props: Props) => {
    const [state, setCurrentState] = useState<State>(defaultState(props));
    var specification = {
        ...state.closedSpecification
    };
    specification.values = specification.values.map((value, i) => {
        if (i === state.restore)
            value.deleted = false;
        if (i === state.delete)
            value.deleted = true;
        return value;
    });
    if (state.new && state.newName !== null)
        specification.values.push({
            deleted: false,
            value: state.newName
        });
    const {isLoading: isLoadingAdd, isSuccess: isSuccessAdd, error: errorAdd} = useAddOverwriteClosedSpecification(
        state.category,
        specification.name,
        specification.filter,
        specification.deleted,
        specification.values,
        state.restore !== -1 || state.delete !== -1 || (state.new && state.newName !== null)
    );
    const {isLoading: isLoadingRename, isSuccess: isSuccessRename, error: errorRename} = useRenameClosedSpecificationValue(
        state.category,
        state.closedSpecification.name,
        state.rename !== -1 ? state.closedSpecification.values[state.rename].value : "",
        state.renameName !== null ? state.renameName : "",
        state.rename !== -1 && state.renameName !== null
    )
    const {setState} = useContext(MessageContext);
    useEffect(() => {
        if ((state.restore !== -1 || state.delete !== -1 || (state.new && state.newName !== null)) && !isLoadingAdd) {
            if (!isSuccessAdd && errorAdd?.status !== 400) {
                setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
            }
            setCurrentState(defaultState(props));
        }
    }, [state, isLoadingAdd, isSuccessAdd, errorAdd, setState]);
    useEffect(() => {
        if ((state.restore !== -1 || state.delete !== -1) && !isLoadingRename) {
            if (!isSuccessRename && errorRename?.status !== 400) {
                setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
            }
            setCurrentState(defaultState(props));
        }
    }, [state, isLoadingRename, isSuccessRename, errorRename, setState]);
    return (
        <>
            <MDBListGroup>
                {
                    state.closedSpecification.values.map(
                        (value, i) => {
                            return (<MDBListGroupItem>
                                {value.value}
                                {value.deleted ? <MDBBtn id='restore-closed-value' size='sm' rounded color='success' onClick={() => setCurrentState({...state,restore: i})}>
                                    Restore
                                </MDBBtn> : <MDBBtn id='add-closed-value' size='sm' rounded color='danger' onClick={() => setCurrentState({...state,delete: i})}>
                                    Delete
                                </MDBBtn>}
                                <MDBBtn id='rename-closed-value' size='sm' rounded color='link' onClick={() => setCurrentState({...state,rename: i})}>
                                    Rename
                                </MDBBtn>
                            </MDBListGroupItem>);
                        }
                    )
                }
            </MDBListGroup>
            <MDBBtn id='add-closed-value' color='success' onClick={() => setCurrentState({...state,new: true})}>Add</MDBBtn>
            {state.rename !== -1 ?
                <RenameClosedSpecificationValue
                    new={false}
                    hide={
                        (name: string | null) =>
                            setCurrentState({
                                ...state,
                                renameName: name
                            })
                    }/> :
                <></>}
            {state.new ?
                <RenameClosedSpecificationValue
                    new={true}
                    hide={
                        (name: string | null) =>
                            setCurrentState({
                                ...state,
                                newName: name
                            })
                    }/> :
                <></>}
        </>
    );
}