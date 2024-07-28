import { useContext, useEffect, useState } from "react"
import { ClosedSpecification, useAddOverwriteClosedSpecification } from "../../hooks/APIHook"
import { MDBBtn, MDBListGroup, MDBListGroupItem } from "mdb-react-ui-kit"
import { MessageContext } from "../../utils/context"
import { RenameClosedSpecification } from "./rename-closed-specification"
import { RenameClosedSpecificationFilter } from "./rename-closed-specification-filter"
import { ClosedSpecificationValue } from "./closed-specification-values"

interface State {
    category: string,
    specifications: ClosedSpecification[],
    restore: number,
    delete: number,
    rename: number,
    removeFilter: number,
    setFilter: number,
    filterName: string | null,
    new: boolean
}

interface Props {
    category: string,
    specifications: ClosedSpecification[]
}

const defaultState: (props: Props) => State = (props: Props) => {
    return {
        category: props.category,
        specifications: props.specifications,
        restore: -1,
        delete: -1,
        rename: -1,
        removeFilter: -1,
        setFilter: -1,
        filterName: null,
        new: false
    }
}

const getSpecification =
(index: number, specifications: ClosedSpecification[], deleted: boolean | null = null, setDeleted: boolean = false, filter: string | null = null, setFilter: boolean = false) => {
    return {
        name: index === -1 ? "" : specifications[index].name,
        filter: setFilter ? filter : index === -1 ? null : specifications[index].filter,
        deleted: setDeleted && deleted !== null ? deleted : index === -1 ? false : specifications[index].deleted,
        values: index === -1 ? [] : specifications[index].values,
    }
}

const newSpecification: (state: State, setCurrentState: React.Dispatch<React.SetStateAction<State>>, name: string | null) => void = (state: State, setCurrentState: React.Dispatch<React.SetStateAction<State>>, name: string | null) => {
    if (name === null)
        return;
    const specifications = state.specifications.map(specification => {
        return {...specification}
    });
    specifications.push({name: name, deleted: false, filter: null, values: []})
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

export const ClosedSpecifications = (props: Props) => {
    const [state, setCurrentState] = useState<State>(defaultState(props));

    const {setState} = useContext(MessageContext);
    const restoreSpecification = getSpecification(state.restore, state.specifications, false);
    const {isLoading: isLoadingRestore, isSuccess: isSuccessRestore} = useAddOverwriteClosedSpecification(
        state.category,
        restoreSpecification.name,
        restoreSpecification.filter,
        restoreSpecification.deleted,
        restoreSpecification.values,
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
    const deleteSpecification = getSpecification(state.restore, state.specifications, true);
    const {isLoading: isLoadingDelete, isSuccess: isSuccessDelete} = useAddOverwriteClosedSpecification(
        state.category,
        deleteSpecification.name,
        deleteSpecification.filter,
        deleteSpecification.deleted,
        deleteSpecification.values,
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
    const removeFilterSpecification = getSpecification(state.removeFilter, state.specifications, undefined, undefined, null, true);
    const {isLoading: isLoadingRemovefilter, isSuccess: isSuccessRemovefilter} = useAddOverwriteClosedSpecification(
        state.category,
        removeFilterSpecification.name,
        removeFilterSpecification.filter,
        removeFilterSpecification.deleted,
        removeFilterSpecification.values,
        state.removeFilter !== -1
    );
    useEffect(() => {
        if (state.removeFilter !== -1 && !isLoadingRemovefilter) {
            if (isSuccessRemovefilter) {
                setCurrentState({
                    ...state,
                    removeFilter: -1,
                    specifications: state.specifications.map(specification => {
                        if (specification.name === state.specifications[state.delete].name)
                            return {...specification, filter: null}
                        return {...specification}
                    })
                });
            }
            else {
                setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                setCurrentState(defaultState(props));
            }
        }
    }, [state, isLoadingRemovefilter, isSuccessRemovefilter, setCurrentState, setState]);
    const setFilterSpecification = getSpecification(state.removeFilter, state.specifications, undefined, undefined, state.filterName, true);
    const {isLoading: isLoadingSetfilter, isSuccess: isSuccessSetfilter} = useAddOverwriteClosedSpecification(
        state.category,
        setFilterSpecification.name,
        setFilterSpecification.filter,
        setFilterSpecification.deleted,
        setFilterSpecification.values,
        state.setFilter !== -1
    );
    useEffect(() => {
        if (state.setFilter !== -1 && !isLoadingSetfilter) {
            if (isSuccessSetfilter) {
                setCurrentState({
                    ...state,
                    setFilter: -1,
                    specifications: state.specifications.map(specification => {
                        if (specification.name === state.specifications[state.delete].name)
                            return {...specification, filter: state.filterName}
                        return {...specification}
                    })
                });
            }
            else {
                setState({message: "An error occurred, please try again.", showMessage: true, timeout: false});
                setCurrentState(defaultState(props));
            }
        }
    }, [state, isLoadingSetfilter, isSuccessSetfilter, setCurrentState, setState]);

    return (
        <>
            <h1>Closed Specification</h1>
            <MDBListGroup>
                {
                    state.specifications.map(
                        (closed, i) => {
                            return (<MDBListGroupItem id={"closed_" + state.category + "_" + open.name} key={closed.name}>
                                {closed.name}
                                <MDBBtn className='rename-closed-specification' size='sm' color='link' onClick={() => setCurrentState({...state,rename: i})}>
                                    Rename
                                </MDBBtn>
                                <MDBBtn className='delete-filter-closed-specification' size='sm' color='link' onClick={() => setCurrentState({...state,removeFilter: i})}>
                                    Delete Filter
                                </MDBBtn>
                                <MDBBtn className='set-filter-closed-specification' size='sm' color='link' onClick={() => setCurrentState({...state,setFilter: i})}>
                                    Set Filter
                                </MDBBtn>
                                {closed.deleted ? <MDBBtn className='restore-closed-specification' size='sm' color='success' onClick={() => setCurrentState({...state,restore: i})}>
                                    Restore
                                </MDBBtn> : <MDBBtn className='delete-closed-specification' size='sm' color='danger' onClick={() => setCurrentState({...state,delete: i})}>
                                    Delete
                                </MDBBtn>}
                            </MDBListGroupItem>);
                        }
                    )
                }
                <ClosedSpecificationValue category={state.category} closedSpecification={state.specifications[state.rename]}/>
            </MDBListGroup>
            <MDBBtn id='add-closed-specification' color='success' onClick={() => setCurrentState({...state,new: true})}>Add</MDBBtn>
            {
                state.rename !== -1 ?
                <RenameClosedSpecification
                    new={false}
                    category={state.category}
                    oldName={state.specifications[state.rename].name}
                    filter={state.specifications[state.rename].filter}
                    values={state.specifications[state.rename].values}
                    hide={(name: string | null) => renameSpecification(state, setCurrentState, name)}/> :
                <></>
            }
            {
                state.new ?
                <RenameClosedSpecification
                    new={true}
                    category={state.category}
                    oldName={""}
                    filter={""}
                    values={[]}
                    hide={(name: string | null) => newSpecification(state, setCurrentState, name)}/> :
                <></>
            }
            {
                state.setFilter !== -1 ?
                <RenameClosedSpecificationFilter hide={(filter: string | null) => {
                    if (filter == null) {
                        setCurrentState({
                            ...state,
                            setFilter: -1
                        });
                    }
                    else {
                        setCurrentState({
                            ...state,
                            filterName: filter
                        })
                    }
                }}/> :
                <></>
            }
        </>
    )
}