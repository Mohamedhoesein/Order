import { BASE_URL, DEFAULT_PASSWORD } from "../util/constants.ts";
import { LastEmail } from "../util/tasks.ts";

describe('update-account.cy.ts', () => {
    it('should send an email for forgot password', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);

        cy.get('#forgot-password').click();
        cy.contains('An email has been send.');

        cy.task<LastEmail>('getLastEmail', {name: 'TestAccount', email: 'test1@test.com'}).then(data => {
            cy.wrap(data).its('type').should('eq', 'forgot-password');
        });
    });

    it('should update account', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);

        cy.get('#name').clear().type('TempAdmin2');
        cy.get('#address').clear().type('Address2');
        cy.get('#password').type(DEFAULT_PASSWORD);

        cy.get('#update-submit').click();

        cy.contains('Account updated successfully.');

        cy.get('#home-link').click();
        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);
    
        cy.get('#name').invoke('val').should('equal', 'TempAdmin2');
        cy.get('#address').invoke('val').should('equal', 'Address2');
    });

    it('should show an error when using an invald password', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);

        cy.get('#name').clear().type('TempAdmin2');
        cy.get('#address').clear().type('Address2');
        cy.get('#password').type('TestPassword321!@#');

        cy.get('#update-submit').click();

        cy.contains('An error occurred, please try again.');

        cy.get('#home-link').click();
        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);

        cy.get('#name').invoke('val').should('equal', 'TempAdmin');
        cy.get('#address').invoke('val').should('equal', 'Address');
    });

    it('should be disabled', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);

        cy.get('#update-submit').should('be.disabled');
    });

    it('should show an error when typing a password without lowercase letters', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);

        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#update-submit');

        cy.contains('Please give a password.');
    });

    it('should show an error when typing a password without uppercase letters', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);

        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#update-submit');

        cy.contains('Please give a password.');
    });

    it('should show an error when typing a password without numbers', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);

        cy.get('#password').type('TestPassword!@#');
        cy.get('#update-submit');

        cy.contains('Please give a password.');
    });

    it('should show an error when typing a password without special characters', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);

        cy.get('#password').type('TestPassword123');
        cy.get('#update-submit');

        cy.contains('Please give a password.');
    });

    it('should show an error when typing a password that is to short', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);

        cy.get('#password').type('T1!e2@s3#');
        cy.get('#update-submit');

        cy.contains('Please give a password.');
    });

    afterEach(() => {
        cy.visit('/logout');
        cy.task('resetDatabase');
    });
});