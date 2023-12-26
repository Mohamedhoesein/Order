import { useContext, useEffect } from 'react';
import { MessageContext } from '../../utils/context';

export const Toast = () => {
    const { message, showMessage, timeout, setState } = useContext(MessageContext);

    useEffect(() => {
        if (timeout || !showMessage) {
            return;
        }
        setState({message, showMessage, timeout: true});
        setTimeout(() => {
            setState({message: "", showMessage: false, timeout: true});
        }, 3000);
    }, [setState, message, showMessage, timeout]);

    if (!showMessage) {
        return (<></>);
    }

    return (
        <div className="toast align-items-center position-absolute text-white bg-primary border-0 bottom-0 end-0 show" role="alert" aria-live="assertive" aria-atomic="true">
            <div className="d-flex">
                <div className="toast-body">
                    {message}
                </div>
            <button id="toast-close" type="button" className="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
            </div>
        </div>
    )
};