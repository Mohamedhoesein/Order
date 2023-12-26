import { BASE_URL, DEFAULT_PASSWORD, NEW_PASSWORD } from "../util/constants.ts";

describe('login.cy.ts', () => {
    it('should login', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);
    });

    it('should show invalid credentials', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(NEW_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/login`);
        cy.contains('Invalid credentials.');
    });

    it('should show an error when typing an email without an @', () => {
        cy.visit('/login');

        cy.get('#email').type('Abc.example.com');
        cy.get('#login-submit');

        cy.contains('Please give an email.');
    });

    it('should show an error when typing an email with multiple @', () => {
        cy.visit('/login');

        cy.get('#email').type('A@b@c@example.com ');
        cy.get('#login-submit');

        cy.contains('Please give an email.');
    });

    it('should show an error when typing an email with invalid characters', () => {
        cy.visit('/login');

        cy.get('#email').type('a"b(c)d,e:f;g<h>i[j\\k]l@example.com');
        cy.get('#login-submit');

        cy.contains('Please give an email.');
    });

    it('should show an error when typing a password without lowercase letters', () => {
        cy.visit('/login');

        cy.get('#password').type('TESTPASSWORD123!@#');
        cy.get('#login-submit');

        cy.contains('Please give a password.');
    });

    it('should show an error when typing a password without uppercase letters', () => {
        cy.visit('/login');

        cy.get('#password').type('testpassword123!@#');
        cy.get('#login-submit');

        cy.contains('Please give a password.');
    });

    it('should show an error when typing a password without numbers', () => {
        cy.visit('/login');

        cy.get('#password').type('TestPassword!@#');
        cy.get('#login-submit');

        cy.contains('Please give a password.');
    });

    it('should show an error when typing a password without special characters', () => {
        cy.visit('/login');

        cy.get('#password').type('TestPassword123');
        cy.get('#login-submit');

        cy.contains('Please give a password.');
    });

    it('should show an error when typing a password that is to short', () => {
        cy.visit('/login');

        cy.get('#password').type('T1!e2@s3#');
        cy.get('#login-submit');

        cy.contains('Please give a password.');
    });

    it('should be disabled', () => {
        cy.visit('/login');

        cy.get('#login-submit').should('be.disabled');
    });

    afterEach(() => {
        cy.visit('/logout');
        cy.task('resetDatabase');
    });
});