import { JSX, useState } from "react";
import { MessageContext } from "../../utils/context";
import { Toast } from "../../components/toast/toast";

interface ContentState {
    message: string;
    showMessage: boolean;
    timeout: boolean;
}

export const UnprotectedContent = ({children}: {children: JSX.Element}) => {
    const [state, setCurrentState] = useState<ContentState>({
        message: "",
        showMessage: false,
        timeout: false
    });
    const handleSetState = (newState: ContentState) => {
        setCurrentState({...state, ...newState});
    };
    const ele = ({...children});
    return (
        <MessageContext.Provider value={{...state, setState: handleSetState}}>
            {ele}
            <Toast/>
        </MessageContext.Provider>
    );
};