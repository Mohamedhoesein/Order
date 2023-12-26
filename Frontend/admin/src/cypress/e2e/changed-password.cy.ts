import { NEW_PASSWORD } from "../util/constants";

describe('changed-password.cy.ts', () => {
    it('should show an error when using invalid code', () => {
        cy.visit('/change-password/1/t');

        cy.get('#password').type(NEW_PASSWORD);
        cy.get('#confirmPassword').type(NEW_PASSWORD);
        cy.get('#change-password-submit').click();

        cy.contains('An error occurred, please try again.');
    });

    it('should show an error when using invalid id', () => {
        cy.visit('/change-password/2/Q2ZESjhIbkE3U1J0TUoxRnYxaGhodDUzbGFmNzU3SGoxOGVnK3FEM1Z6cmhBSEJUM253T1J5OFJVbGlJT3h6cGhXNEU1eHJEanNrd3pSVHVWeHBUU1Q0c2s3SXVHVjgrM2l0K1AwczZ3bFNNNnpFaEIxZG5Rb1FzNFhjbW1yK3dsQ1l0WDhMR21sUTJLbDdxYWVScVhkbTZaMjhMakR5TjUvZXhHOUZYY3JmTHAwMDIwckF1NTVKb0ZxdHJhMjl1VHUrYkJRPT0');

        cy.get('#password').type(NEW_PASSWORD);
        cy.get('#confirmPassword').type(NEW_PASSWORD);
        cy.get('#change-password-submit').click();

        cy.contains('An error occurred, please try again.');
    });

    it('should show an error when using different password', () => {
        cy.visit('/change-password/2/Q2ZESjhIbkE3U1J0TUoxRnYxaGhodDUzbGFmNzU3SGoxOGVnK3FEM1Z6cmhBSEJUM253T1J5OFJVbGlJT3h6cGhXNEU1eHJEanNrd3pSVHVWeHBUU1Q0c2s3SXVHVjgrM2l0K1AwczZ3bFNNNnpFaEIxZG5Rb1FzNFhjbW1yK3dsQ1l0WDhMR21sUTJLbDdxYWVScVhkbTZaMjhMakR5TjUvZXhHOUZYY3JmTHAwMDIwckF1NTVKb0ZxdHJhMjl1VHUrYkJRPT0');

        cy.get('#password').type(NEW_PASSWORD);
        cy.get('#confirmPassword').type('TestPassword322!@#');
        cy.get('#change-password-submit');

        cy.get('div:has(#confirmPassword)').contains('Please give a password.');
    });

    it('should show an error when typing a password without lowercase letters', () => {
        cy.visit('/change-password/1/1');

        cy.get('#password').type('TESTPASSWORD123!@#');
        cy.get('#change-password-submit');

        cy.get('div:has(#password)').contains('Please give a password.');
    });

    it('should show an error when typing a password without uppercase letters', () => {
        cy.visit('/change-password/1/1');

        cy.get('#password').type('testpassword123!@#');
        cy.get('#change-password-submit');

        cy.get('div:has(#password)').contains('Please give a password.');
    });

    it('should show an error when typing a password without numbers', () => {
        cy.visit('/change-password/1/1');

        cy.get('#password').type('TestPassword!@#');
        cy.get('#change-password-submit');

        cy.get('div:has(#password)').contains('Please give a password.');
    });

    it('should show an error when typing a password without special characters', () => {
        cy.visit('/change-password/1/1');

        cy.get('#password').type('TestPassword123');
        cy.get('#change-password-submit');

        cy.get('div:has(#password)').contains('Please give a password.');
    });

    it('should show an error when typing a password that is to short', () => {
        cy.visit('/change-password/1/1');

        cy.get('#password').type('T1!e2@s3#');
        cy.get('#change-password-submit');

        cy.get('div:has(#password)').contains('Please give a password.');
    });

    it('should show an error when typing a confirmation password without lowercase letters', () => {
        cy.visit('/change-password/1/1');

        cy.get('#confirmPassword').type('TESTPASSWORD123!@#');
        cy.get('#change-password-submit');

        cy.get('div:has(#confirmPassword)').contains('Please give a password.');
    });

    it('should show an error when typing a confirmation password without uppercase letters', () => {
        cy.visit('/change-password/1/1');

        cy.get('#confirmPassword').type('testpassword123!@#');
        cy.get('#change-password-submit');

        cy.get('div:has(#confirmPassword)').contains('Please give a password.');
    });

    it('should show an error when typing a confirmation password without numbers', () => {
        cy.visit('/change-password/1/1');

        cy.get('#confirmPassword').type('TestPassword!@#');
        cy.get('#change-password-submit');

        cy.get('div:has(#confirmPassword)').contains('Please give a password.');
    });

    it('should show an error when typing a confirmation password without special characters', () => {
        cy.visit('/change-password/1/1');

        cy.get('#confirmPassword').type('TestPassword123');
        cy.get('#change-password-submit');

        cy.get('div:has(#confirmPassword)').contains('Please give a password.');
    });

    it('should show an error when typing a confirmation password that is to short', () => {
        cy.visit('/change-password/1/1');

        cy.get('#confirmPassword').type('T1!e2@s3#');
        cy.get('#change-password-submit');

        cy.get('div:has(#confirmPassword)').contains('Please give a password.');
    });

    afterEach(() => {
        cy.visit('/logout');
        cy.task('resetDatabase');
    });
});