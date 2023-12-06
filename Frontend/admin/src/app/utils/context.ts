import { createContext } from "react";

interface MessageContextPropsData {
    message: string;
    showMessage: boolean;
    timeout: boolean;
}

interface MessageContextProps extends MessageContextPropsData {
    setState: (state: MessageContextPropsData) => void;
}

export const MessageContext = createContext<MessageContextProps>({message: "", timeout: false, showMessage: false, setState: () => {}});