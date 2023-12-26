import { Navigate, useParams } from "react-router-dom";
import { useContext, useEffect, useState } from "react";
import { useVerify } from "../../hooks/APIHook.ts";
import { MessageContext } from "../../utils/context.ts";
import { Spinner } from "../../components/spinner/spinner.tsx";

type Params = {
    id: string;
    code: string;
};

export const Verify = () => {
    const params = useParams<Params>();
    const [enabled, setEnabled] = useState(true);
    const {isLoading, isSuccess} = useVerify(
        params.id ?? "",
        params.code ?? "",
        !enabled && params.id !== undefined && params.code !== undefined && !isNaN(Number(params.id))
    );
    const {setState} = useContext(MessageContext);

    useEffect(() => {
        if (!isSuccess && !isLoading)
            setState({message: "Please try again to verify.", showMessage: true, timeout: false});
    }, [isLoading, isSuccess, setState, enabled]);


    if (!params.id || !params.code || isNaN(Number(params.id)))
        return (<Navigate to="/not-found"/>);

    if (isLoading) {
        if (enabled)
            setEnabled(false);
        return (
            <Spinner/>
        );
    }

    return (<Navigate to="/login" />);
}