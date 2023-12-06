import { BASE_URL, DEFAULT_PASSWORD, NEW_PASSWORD } from '../util/constants';

describe('delete.cy.ts', () => {
    it('should delete account', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);
    
        cy.get('#name').invoke('val').should('equal', 'TempAdmin');
        cy.get('#address').invoke('val').should('equal', 'Address');

        cy.get('#delete-account').click();

        cy.contains('Delete Account');

        cy.get('#deletePassword').type(DEFAULT_PASSWORD);
        cy.get('#delete-submit').click();

        cy.url().should('eq', `${BASE_URL}/login`);
        
        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.contains('Invalid credentials.')
    });

    it('should not delete account', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);
    
        cy.get('#name').invoke('val').should('equal', 'TempAdmin');
        cy.get('#address').invoke('val').should('equal', 'Address');

        cy.get('#delete-account').click();

        cy.contains('Delete Account');

        cy.get('#deletePassword').type(NEW_PASSWORD);
        cy.get('#delete-submit').click();

        cy.contains('An error occurred, please try again.');
        cy.url().should('eq', `${BASE_URL}/account`);

        cy.get('#close-delete').click();
        cy.get('#toast-close').click();
        cy.get('#logout-link').click();
        cy.url().should('eq', `${BASE_URL}/logout`);
        
        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);
    });

    it('should show an error when typing a password without lowercase letters', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);
    
        cy.get('#name').invoke('val').should('equal', 'TempAdmin');
        cy.get('#address').invoke('val').should('equal', 'Address');

        cy.get('#delete-account').click();

        cy.contains('Delete Account');

        cy.get('#deletePassword').type(DEFAULT_PASSWORD);
        cy.get('#delete-submit');

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
    
        cy.get('#name').invoke('val').should('equal', 'TempAdmin');
        cy.get('#address').invoke('val').should('equal', 'Address');

        cy.get('#delete-account').click();

        cy.contains('Delete Account');

        cy.get('#deletePassword').type(DEFAULT_PASSWORD);
        cy.get('#delete-submit');

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
    
        cy.get('#name').invoke('val').should('equal', 'TempAdmin');
        cy.get('#address').invoke('val').should('equal', 'Address');

        cy.get('#delete-account').click();

        cy.contains('Delete Account');

        cy.get('#deletePassword').type('TestPassword!@#');
        cy.get('#delete-submit');

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
    
        cy.get('#name').invoke('val').should('equal', 'TempAdmin');
        cy.get('#address').invoke('val').should('equal', 'Address');

        cy.get('#delete-account').click();

        cy.contains('Delete Account');

        cy.get('#deletePassword').type('TestPassword123');
        cy.get('#delete-submit');

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
    
        cy.get('#name').invoke('val').should('equal', 'TempAdmin');
        cy.get('#address').invoke('val').should('equal', 'Address');

        cy.get('#delete-account').click();

        cy.contains('Delete Account');

        cy.get('#deletePassword').type('T1!e2@s3#');
        cy.get('#delete-submit');

        cy.contains('Please give a password.');
    });

    afterEach(() => {
        cy.visit('/logout');
        cy.exec('npm run reload')
    });
});