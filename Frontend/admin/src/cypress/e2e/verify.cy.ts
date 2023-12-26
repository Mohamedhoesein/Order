import { BASE_URL } from "../util/constants.ts";

describe('verify.cy.ts', () => {
    it('should show an error when using invalid code', () => {
        cy.visit('/verify/1/t');

        cy.contains('Please try again to verify.');
        cy.url().should('eq', `${BASE_URL}/login`);
    });

    it('should show an error when using invalid id', () => {
        cy.visit('/verify/99/Q2ZESjhIbkE3U1J0TUoxRnYxaGhodDUzbGFlUmgvS2NwTi9zdXdhem81K2FFM3R4akpiNVpUbDNGdHN5ZHlKR3hJbHRiRVhheFJmM3FNUWhZQXNRQ2NsK2NZSEZSa2Vqc3N1ejMycFBPcVZ5SjlHNk5FSWpIcGZNcXBiTjNrUUZGU1U5bkw1SEdhLy9UT3ZYTFJONzZjcnZPV1VOMWpVbWFzSFpWNWR6bUxPWEgvMVhmc1ZQazhBbDh0RDF2K3I2WVg4MWhnPT0');

        cy.contains('Please try again to verify.');
        cy.url().should('eq', `${BASE_URL}/login`);
    });

    afterEach(() => {
        cy.visit('/logout');
        cy.task('resetDatabase');
    });
});