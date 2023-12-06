import { MDBSpinner } from "mdb-react-ui-kit";
import "./spinner.css";

export const Spinner = () => {
    return (
        <div className='center'>
            <MDBSpinner color="primary">
                <span className="visually-hidden">Loading...</span>
            </MDBSpinner>
        </div>
    );
}