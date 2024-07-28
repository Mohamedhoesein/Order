import { useContext, useEffect, useState } from "react"
import { WholeCategory, useDeleteCategory, useGetCategories, useRestoreCategory } from "../../hooks/APIHook"
import { MDBAccordion, MDBAccordionItem, MDBBtn, MDBContainer } from "mdb-react-ui-kit";
import { AddCategory } from "./add-category";
import { MessageContext } from "../../utils/context";
import { OpenSpecifications } from "./open-specifications";
import { ClosedSpecifications } from "./closed-specifications";

interface State {
    categories: WholeCategory[],
    loading: boolean,
    categoryId: number | undefined,
    add: boolean,
    refetch: boolean,
    restore: number,
    delete: number
}

const defaultState: State = {
    categories: [],
    loading: true,
    categoryId: undefined,
    add: false,
    refetch: false,
    restore: -1,
    delete: -1
}

export const Categories = () => {
    const [state, setCurrentState] = useState<State>(defaultState);

    const {setState} = useContext(MessageContext);
    const {isLoading: isLoadingCategories, isSuccess: isSuccessCategories, data: dataCategories, refetch} = useGetCategories(state.loading);
    useEffect(() => {
        if (state.refetch)
            refetch().then(data => {
                if (data.isSuccess && data.data) {
                    console.log(JSON.stringify(data.data.data));
                    setCurrentState({
                        ...state,
                        categories: data.data.data,
                        loading: false,
                        refetch: false
                    });
                }
                else {
                    setState({message: "An error occurred while loading the data, please try again.", showMessage: true, timeout: false});
                    setCurrentState({
                        ...state,
                        loading: false,
                        refetch: false
                    });
                }
            })
    }, [state]);
    useEffect(() => {
        if (state.loading && !isLoadingCategories) {
            if (isSuccessCategories && dataCategories) {
                console.log(3)
                setCurrentState({
                    ...state,
                    categories: dataCategories.data,
                    loading: false
                });
            }
            else {
                setState({message: "An error occurred while loading the data, please try again.", showMessage: true, timeout: false});
                setCurrentState({
                    ...state,
                    loading: false
                });
            }
        }
    }, [state, isLoadingCategories, isSuccessCategories, dataCategories]);
    const {isLoading: isLoadingDelete, isSuccess: isSuccessDelete} = useDeleteCategory(
        state.delete === -1 ? "" : state.categories[state.delete].name,
        state.delete !== -1
    )
    useEffect(() => {
        if (state.delete !== -1 && !isLoadingDelete) {
            if (isSuccessDelete) {
                setCurrentState({
                    ...state,
                    refetch: true,
                    delete: -1
                });
            }
            else {
                setState({message: "An error occurred while, please try again.", showMessage: true, timeout: false});
                setCurrentState({
                    ...state,
                    delete: -1
                });
            }
        }
    }, [state, isLoadingDelete, isLoadingDelete]);
    const {isLoading: isLoadingRestore, isSuccess: isSuccessRestore} = useRestoreCategory(
        state.restore === -1 ? "" : state.categories[state.restore].name,
        state.restore !== -1
    )
    useEffect(() => {
        if (state.restore !== -1 && !isLoadingRestore) {
            if (isSuccessRestore) {
                setCurrentState({
                    ...state,
                    refetch: true,
                    restore: -1
                });
            }
            else {
                setState({message: "An error occurred while, please try again.", showMessage: true, timeout: false});
                setCurrentState({
                    ...state,
                    restore: -1
                });
            }
        }
    }, [state, isLoadingRestore, isSuccessRestore]);

    return (
        <>
            <MDBContainer>
                <MDBBtn id='add-category' color='success' onClick={() => setCurrentState({...state, add: true})}>Add</MDBBtn>
                <MDBAccordion onChange={(id) => setCurrentState({...state, categoryId: Array.isArray(id) ? id[0] : id})}>
                    {
                        state.categories.map((c, i) =>
                            <MDBAccordionItem collapseId={i} headerTitle={c.name} id={c.name} key={c.name}>
                                {
                                    <>
                                        <MDBBtn className='rename-category' color='link'>Rename</MDBBtn>
                                        {
                                            c.deleted ?
                                                <MDBBtn className='restore-category' color='success' onClick={() => setCurrentState({...state,restore:i})}>Restore</MDBBtn> :
                                                <MDBBtn className='delete-category' color='danger' onClick={() => setCurrentState({...state,delete:i})}>Delete</MDBBtn>
                                        }
                                        <OpenSpecifications category={c.name} specifications={c.openSpecifications.map(specification => {return {...specification}})} refetch={() => {setCurrentState({...state,refetch:true})}}/>
                                        <ClosedSpecifications category={c.name} specifications={c.closedSpecifications}/>
                                    </>
                                }
                            </MDBAccordionItem>
                        )
                    }
                </MDBAccordion>
            </MDBContainer>
            {state.add ? <AddCategory hide={() => {setCurrentState({...state, add: false, refetch: true})}}/> : <></>}
        </>
    )
}