import { MDBSpinner } from "mdb-react-ui-kit";
import { Navigate } from "react-router-dom";
import { useLoggedIn } from "../../hooks/APIHook";

export const ProtectedRoute = ({children}: {children: JSX.Element}) => {
    const {isLoading, isSuccess, data} = useLoggedIn();
    if (isLoading) {
        return (
            <MDBSpinner color="primary">
                <span className="visually-hidden">Loading...</span>
            </MDBSpinner>
        );
    }
    if (isSuccess && data.data.loggedIn === false)
        return (<Navigate to="/login" replace />);
    return ({...children});
};