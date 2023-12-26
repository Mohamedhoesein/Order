import { useContext, useEffect, useState } from "react";
import { Navigate } from "react-router-dom";
import { useLogOut } from "../../hooks/APIHook";
import { Spinner } from "../../components/spinner/spinner";
import { MessageContext } from "../../utils/context";

export const Logout = () => {
    const [enabled, setEnabled] = useState(true);
    const {isLoading, isSuccess} = useLogOut(!enabled);
    const {setState} = useContext(MessageContext);

    useEffect(() => {
        if (!isSuccess && !isLoading)
            setState({message: "Please try again to log out.", showMessage: true, timeout: false});
    }, [isLoading, isSuccess, setState, enabled]);

    if (isLoading) {
        if (enabled)
            setEnabled(false);
        return (
            <Spinner/>
        );
    }

    if (!isSuccess)
        return (<Navigate to="/"/>);

    return (<Navigate to="/login" />);
};