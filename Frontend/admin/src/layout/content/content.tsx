import { useState } from "react";
import { Outlet } from "react-router-dom";
import { Header } from "../../components/header/header";
import { MessageContext } from "../../utils/context";
import { Toast } from "../../components/toast/toast";

interface ContentState {
    message: string;
    showMessage: boolean;
    timeout: boolean;
};

export const Content = () => {
    const [state, setCurrentState] = useState<ContentState>({
        message: "",
        showMessage: false,
        timeout: false
    });
    const handleSetState = (newState: ContentState) => {
        setCurrentState({...state, ...newState});
    };
    return (
        <MessageContext.Provider value={{...state, setState: handleSetState}}>
            <Header/>
            <Outlet/>
            <Toast/>
        </MessageContext.Provider>
    );
};