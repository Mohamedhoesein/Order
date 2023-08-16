import { MDBSpinner } from "mdb-react-ui-kit";
import { Navigate } from "react-router-dom";
import { useLoggedIn } from "../../hooks/APIHook";

export const UnprotectedRoute = ({children}: {children: JSX.Element}) => {
    const {isLoading, isSuccess, data} = useLoggedIn();
    if (isLoading) {
        return (
            <MDBSpinner color="primary">
                <span className="visually-hidden">Loading...</span>
            </MDBSpinner>
        );
    }
    if (isSuccess && data.data.loggedIn === true)
        return (<Navigate to="/" replace />);
    return ({...children});
};