import { BASE_URL, DEFAULT_PASSWORD, NEW_PASSWORD } from '../util/constants.ts';
import { LastEmail } from '../util/tasks.ts';

describe('register.cy.ts', () => {
    it('should verify', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#register-link').click();
        cy.url().should('eq', `${BASE_URL}/register`);

        cy.get('#name').type('TestAccount');
        cy.get('#address').type('Address');
        cy.get('#email').type('test1@test.com');
        cy.get('#password').type(NEW_PASSWORD);
        cy.get('#confirmPassword').type(NEW_PASSWORD);
        cy.get('#register-submit').click();

        cy.visit('/logout');

        cy.task<LastEmail>('getLastEmail', {name: 'TestAccount', email: 'test1@test.com'}).then(data => {
            cy.wrap(data).its('type').should('eq', 'verify');
            cy.wrap(data).its('id').should('not.eq', '1');
            cy.wrap(data).its('code').should('not.eq', '');

            cy.visit(`/verify/${data.id}/${data.code}`);

            cy.url().should('eq', `${BASE_URL}/login`);

            cy.get('#email').type('test1@test.com');
            cy.get('#password').type(NEW_PASSWORD);
            cy.get('#login-submit').click();

            cy.url().should('eq', `${BASE_URL}/`);
        });
    });

    it('should be disabled', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#register-link').click();
        cy.url().should('eq', `${BASE_URL}/register`);

        cy.get('#register-submit').should('be.disabled');
    });

    it('should show an error when typing an email without an @', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#register-link').click();
        cy.url().should('eq', `${BASE_URL}/register`);

        cy.get('#email').type('Abc.example.com');
        cy.get('#register-submit');

        cy.contains('Please give an email.');
    });

    it('should show an error when typing an email with multiple @', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#register-link').click();
        cy.url().should('eq', `${BASE_URL}/register`);

        cy.get('#email').type('A@b@c@example.com ');
        cy.get('#register-submit');

        cy.contains('Please give an email.');
    });

    it('should show an error when typing an email with invalid characters', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#register-link').click();
        cy.url().should('eq', `${BASE_URL}/register`);

        cy.get('#email').type("a'b(c)d,e:f;g<h>i[j\\k]l@example.com");
        cy.get('#register-submit');

        cy.contains('Please give an email.');
    });

    it('should show an error when typing a password without lowercase letters', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#register-link').click();
        cy.url().should('eq', `${BASE_URL}/register`);

        cy.get('#password').type('TESTPASSWORD123!@#');
        cy.get('#register-submit');

        cy.get('div:has(#password)').contains('Please give a password.');
    });

    it('should show an error when typing a password without uppercase letters', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#register-link').click();
        cy.url().should('eq', `${BASE_URL}/register`);

        cy.get('#password').type('testpassword123!@#');
        cy.get('#register-submit');

        cy.get('div:has(#password)').contains('Please give a password.');
    });

    it('should show an error when typing a password without numbers', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#register-link').click();
        cy.url().should('eq', `${BASE_URL}/register`);

        cy.get('#password').type('TestPassword!@#');
        cy.get('#register-submit');

        cy.get('div:has(#password)').contains('Please give a password.');
    });

    it('should show an error when typing a password without special characters', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#register-link').click();
        cy.url().should('eq', `${BASE_URL}/register`);

        cy.get('#password').type('TestPassword123');
        cy.get('#register-submit');

        cy.get('div:has(#password)').contains('Please give a password.');
    });

    it('should show an error when typing a password that is to short', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#register-link').click();
        cy.url().should('eq', `${BASE_URL}/register`);

        cy.get('#password').type('T1!e2@s3#');
        cy.get('#register-submit');

        cy.get('div:has(#password)').contains('Please give a password.');
    });

    it('should show an error when typing a different password', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#register-link').click();
        cy.url().should('eq', `${BASE_URL}/register`);

        cy.get('#password').type(NEW_PASSWORD);
        cy.get('#confirmPassword').type(DEFAULT_PASSWORD);
        cy.get('#register-submit');

        cy.get('div:has(#confirmPassword)').contains('Please retype the password.');
    });

    afterEach(() => {
        cy.visit('/logout');
        cy.task('resetDatabase');
    });
});