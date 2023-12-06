import {
    MDBNavbar,
    MDBNavbarNav,
    MDBNavbarItem,
    MDBNavbarLink,
    MDBContainer
} from "mdb-react-ui-kit";

export const Header = () => {
    return (
        <header>
            <MDBNavbar expand="lg" light bgColor="white">
                <MDBContainer fluid>
                    <MDBNavbarNav right className="mr-auto mb-2 mb-lg-0">
                        <MDBNavbarItem>
                            <MDBNavbarLink id="home-link" href="/">
                                Home
                            </MDBNavbarLink>
                        </MDBNavbarItem>
                        <MDBNavbarItem>
                            <MDBNavbarLink id="register-link" href="register">
                                Register
                            </MDBNavbarLink>
                        </MDBNavbarItem>
                    </MDBNavbarNav>
                    <MDBNavbarNav className='w-auto mb-2 mb-lg-0'>
                        <MDBNavbarItem>
                            <MDBNavbarLink id="account-link" href="account">
                                Account
                            </MDBNavbarLink>
                        </MDBNavbarItem>
                        <MDBNavbarItem>
                            <MDBNavbarLink id="logout-link" href="logout">
                                Logout
                            </MDBNavbarLink>
                        </MDBNavbarItem>
                    </MDBNavbarNav>
                </MDBContainer>
            </MDBNavbar>
        </header>
    );
};