import { useContext, useEffect, useState } from "react"
import { OpenSpecification, useAddOverwriteOpenSpecification } from "../../hooks/APIHook"
import { MDBBtn, MDBListGroup, MDBListGroupItem } from "mdb-react-ui-kit"
import { RenameOpenSpecification } from "./rename-open-specification"
import { MessageContext } from "../../utils/context"

interface State {
    category: string,
    specifications: OpenSpecification[],
    restore: number,
    delete: number,
    rename: number,
    new: boolean,
    refetch: () => void
}

interface Props {
    category: string,
    specifications: OpenSpecification[],
    refetch: () => void
}

const defaultState: (props: Props) => State = (props: Props) => {
    return {
        category: props.category,
        specifications: props.specifications.map(specification => {return {...specification}}),
        refetch: props.refetch,
        restore: -1,
        delete: -1,
        rename: -1,
        new: false
    }
}

const newSpecification: (state: State, setCurrentState: React.Dispatch<React.SetStateAction<State>>, name: string | null) => void = (state: State, setCurrentState: React.Dispatch<React.SetStateAction<State>>, name: string | null) => {
    if (name === null)
        return;
    const specifications = state.specifications.map(specification => {
        return {...specification}
    });
    specifications.push({name: name, deleted: false})
    setCurrentState({
        ...state,
        new: false,
        specifications: specifications
    });
}

const renameSpecification: (state: State, setCurrentState: React.Dispatch<React.SetStateAction<State>>, name: string | null) => void = (state: State, setCurrentState: React.Dispatch<React.SetStateAction<State>>, name: string | null) => {
    if (name === null)
        return;
    setCurrentState({
        ...state,
        rename: -1,
        specifications: state.specifications.map(specification => {
            if (specification.name === state.specifications[state.rename].name)
                return {...specification, name: name}
            return {...specification}
        })
    });
}

export const OpenSpecifications = (props: Props) => {
    const [state, setCurrentState] = useState<State>(defaultState(props));
    const {setState} = useContext(MessageContext);
    const {isLoading: isLoadingRestore, isSuccess: isSuccessRestore} = useAddOverwriteOpenSpecification(
        state.category,
        state.restore === -1 ? "" : state.specifications[state.restore].name,
        false,
        state.restore !== -1
    );
    useEffect(() => {
        if (state.restore !== -1 && !isLoadingRestore) {
            if (isSuccessRestore) {
                setCurrentState({
                    ...state,
                    restore: -1,
                    specifications: state.specifications.map(specification => {
                        if (specification.name === state.specifications[state.restore].name)
                            return {...specification, deleted: false}
                        return {...specification}
                    })
                });
            }
            else {
                setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                setCurrentState(defaultState(props));
            }
        }
    }, [state, isLoadingRestore, isSuccessRestore, setCurrentState, setState]);
    const {isLoading: isLoadingDelete, isSuccess: isSuccessDelete} = useAddOverwriteOpenSpecification(
        state.category,
        state.delete === -1 ? "" : state.specifications[state.delete].name,
        true,
        state.delete !== -1
    );
    useEffect(() => {
        if (state.delete !== -1 && !isLoadingDelete) {
            if (isSuccessDelete) {
                setCurrentState({
                    ...state,
                    delete: -1,
                    specifications: state.specifications.map(specification => {
                        if (specification.name === state.specifications[state.delete].name)
                            return {...specification, deleted: true}
                        return {...specification}
                    })
                });
            }
            else {
                setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                setCurrentState(defaultState(props));
            }
        }
    }, [state, isLoadingDelete, isSuccessDelete, setCurrentState, setState]);

    return (
        <>
            <h1>Open Specification</h1>
            <MDBListGroup>
                {
                    state.specifications.map(
                        (open, i) => {
                            return (<MDBListGroupItem id={"open_" + state.category + "_" + open.name} key={open.name}>
                                {open.name}
                                <MDBBtn className='rename-open-specification' size='sm' color='link' onClick={() => setCurrentState({...state,rename: i})}>
                                    Rename
                                </MDBBtn>
                                {open.deleted ? <MDBBtn className='restore-open-specification' size='sm' color='success' onClick={() => setCurrentState({...state,restore: i})}>
                                    Restore
                                </MDBBtn> : <MDBBtn className='delete-open-specification' size='sm' color='danger' onClick={() => setCurrentState({...state,delete: i})}>
                                    Delete
                                </MDBBtn>}
                            </MDBListGroupItem>);
                        }
                    )
                }
            </MDBListGroup>
            <MDBBtn id='add-open-specification' color="success" onClick={() => setCurrentState({...state,new: true})}>Add</MDBBtn>
            {
                state.rename !== -1 ?
                <RenameOpenSpecification
                    new={false}
                    category={state.category}
                    oldName={state.specifications[state.rename].name}
                    hide={(name: string | null) => renameSpecification(state, setCurrentState, name)}/> :
                <></>
            }
            {
                state.new ?
                <RenameOpenSpecification
                    new={true}
                    category={state.category}
                    oldName={""}
                    hide={(name: string | null) => newSpecification(state, setCurrentState, name)}/> :
                <></>
            }
        </>
    )
}