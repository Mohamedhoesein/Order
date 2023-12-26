import { LastEmail } from "../util/tasks.ts";
import { BASE_URL, NEW_PASSWORD } from "../util/constants.ts";

describe('forgot-password.cy.ts', () => {
    it('should login with new credentials', () => {
        cy.visit('/login');

        cy.get('#forgot-password').click();
        cy.get('#email').type('test@test.com');
        cy.get('#forgot-password-submit').click();

        cy.task<LastEmail>('getLastEmail', {name: 'TempAdmin', email: 'test@test.com'}).then(data => {
            cy.wrap(data).its('type').should('eq', 'change-password');
            cy.wrap(data).its('id').should('eq', '1');
            cy.wrap(data).its('code').should('not.eq', '');

            cy.visit('/change-password/' + data.id + '/' + data.code);

            cy.get('#password').type(NEW_PASSWORD);
            cy.get('#confirmPassword').type(NEW_PASSWORD);
            cy.get('#change-password-submit').click();

            cy.visit('/login');

            cy.get('#email').type('test@test.com');
            cy.get('#password').type(NEW_PASSWORD);
            cy.get('#login-submit').click();

            cy.url().should('eq', `${BASE_URL}/`);
        });
    });

    it('should show invalid credentials', () => {
        cy.visit('/login');

        cy.get('#forgot-password').click();
        cy.get('#email').type('test@test.com');
        cy.get('#forgot-password-submit').click();

        cy.task<LastEmail>('getLastEmail', {name: 'TempAdmin', email: 'test@test.com'}).then(data => {
            cy.wrap(data).its('type').should('eq', 'change-password');
            cy.wrap(data).its('id').should('eq', '1');
            cy.wrap(data).its('code').should('not.eq', '');

            cy.visit('/change-password/' + data.id + '/' + data.code);
            cy.get('#password').type(NEW_PASSWORD);
            cy.get('#confirmPassword').type(NEW_PASSWORD);
            cy.get('#change-password-submit').click();

            cy.visit('/login');

            cy.get('#email').type('test@test.com');
            cy.get('#password').type('TestPassword123!@#');
            cy.get('#login-submit').click();

            cy.url().should('eq', `${BASE_URL}/login`);
            cy.contains('Invalid credentials.');
        });
    });

    it('should not send email', () => {
        cy.visit('/login');

        cy.get('#forgot-password').click();
        cy.get('#email').type('test@test.com');
        cy.get('#forgot-password-submit').click();

        cy.task<LastEmail>('getLastEmail', {name: 'TempAdmin', email: 'test1@test.com'}).then(data => {
            cy.wrap(data).its('type').should('eq', 'unknown');
        });
    });

    it('should show an error when typing an email without an @', () => {
        cy.visit('/login');

        cy.get('#forgot-password').click();
        cy.get('#email').type('Abc.example.com');
        cy.get('#forgot-password-submit');

        cy.contains('Please give an email.');
    });

    it('should show an error when typing an email with multiple @', () => {
        cy.visit('/login');

        cy.get('#forgot-password').click();
        cy.get('#email').type('A@b@c@example.com ');
        cy.get('#forgot-password-submit');

        cy.contains('Please give an email.');
    });

    it('should show an error when typing an email with invalid characters', () => {
        cy.visit('/login');

        cy.get('#forgot-password').click();
        cy.get('#email').type('a"b(c)d,e:f;g<h>i[j\\k]l@example.com');
        cy.get('#forgot-password-submit');

        cy.contains('Please give an email.');
    });

    afterEach(() => {
        cy.visit('/logout');
        cy.task('resetDatabase');
    });
});