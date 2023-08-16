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
                            <MDBNavbarLink href="#">
                                Home
                            </MDBNavbarLink>
                        </MDBNavbarItem>
                        <MDBNavbarItem>
                            <MDBNavbarLink href="register">
                                Register
                            </MDBNavbarLink>
                        </MDBNavbarItem>
                    </MDBNavbarNav>
                    <MDBNavbarNav className='w-auto mb-2 mb-lg-0'>
                        <MDBNavbarItem>
                            <MDBNavbarLink href="account">
                                Account
                            </MDBNavbarLink>
                        </MDBNavbarItem>
                        <MDBNavbarItem>
                            <MDBNavbarLink href="logout">
                                Logout
                            </MDBNavbarLink>
                        </MDBNavbarItem>
                    </MDBNavbarNav>
                </MDBContainer>
            </MDBNavbar>
        </header>
    );
};