import { BASE_URL, DEFAULT_PASSWORD, NEW_PASSWORD } from "../util/constants";
import { LastEmail } from "../util/tasks";

describe('forgot-password.cy.ts', () => {
  it('should change password', () => {
    cy.visit('/');

    cy.get('#register-link').click();
    cy.url().should('eq', `${BASE_URL}/register`);

    cy.get('#name').type('TestUser');
    cy.get('#email').type('test1@test.com');
    cy.get('#address').type('address');
    cy.get('#password').type(DEFAULT_PASSWORD);
    cy.get('#confirmPassword').type(DEFAULT_PASSWORD);
    cy.get('#register-submit').click();
  
    cy.task<LastEmail>('getLastEmail', {name: 'TestUser',email: 'test1@test.com'}).then(data => {
      cy.wrap(data).its('type').should('eq', 'verify');
      cy.wrap(data).its('id').should('not.eq', '1');
      cy.wrap(data).its('code').should('not.eq', '');

      cy.visit(`/verify/${data.id}/${data.code}`);
    
      cy.get('#login-link').click();
      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#forgot-password').click();
      cy.url().should('eq', `${BASE_URL}/forgot-password`);
  
      cy.get('#email').type('test1@test.com');

      cy.get('#forgot-password-submit').click();
      cy.url().should('eq', `${BASE_URL}/login`);

      cy.task<LastEmail>('getLastEmail', {name: 'TestUser',email: 'test1@test.com'}).then(data => {	
        cy.wrap(data).its('type').should('eq', 'change-password');
        cy.wrap(data).its('id').should('eq', '2');
        cy.wrap(data).its('code').should('not.eq', '');

        cy.visit(`/change-password/${data.id}/${data.code}`);

        cy.get('#password').type(NEW_PASSWORD);
        cy.get('#confirmPassword').type(NEW_PASSWORD);
        cy.get('#change-password-submit').click();
      });
    });
  });

  it('should be disabled', () => {
    cy.visit('/');

    cy.get('#login-link').click();
    cy.url().should('eq', `${BASE_URL}/login`);

    cy.get('#forgot-password').click();
    cy.url().should('eq', `${BASE_URL}/forgot-password`);

    cy.get('#forgot-password-submit').should('be.disabled');
  });

  it('should show an error when typing an email without an @', () => {
    cy.visit('/');

    cy.get('#login-link').click();
    cy.url().should('eq', `${BASE_URL}/login`);

    cy.get('#forgot-password').click();
    cy.url().should('eq', `${BASE_URL}/forgot-password`);

    cy.get('#email').type('Abc.example.com');
    cy.get('#forgot-password-submit');

    cy.contains('Please enter a valid email address.');
  });

  it('should show an error when typing an email with multiple @', () => {
    cy.visit('/');

    cy.get('#login-link').click();
    cy.url().should('eq', `${BASE_URL}/login`);

    cy.get('#forgot-password').click();
    cy.url().should('eq', `${BASE_URL}/forgot-password`);

    cy.get('#email').type('A@b@c@example.com ');
    cy.get('#forgot-password-submit');

    cy.contains('Please enter a valid email address.');
  });

  it('should show an error when typing an email with invalid characters', () => {
    cy.visit('/');

    cy.get('#login-link').click();
    cy.url().should('eq', `${BASE_URL}/login`);

    cy.get('#forgot-password').click();
    cy.url().should('eq', `${BASE_URL}/forgot-password`);

    cy.get('#email').type("a'b(c)d,e:f;g<h>i[j\\k]l@example.com");
    cy.get('#forgot-password-submit');

    cy.contains('Please enter a valid email address.');
  });

  afterEach(() => {
      cy.visit('/logout');
      cy.task('resetDatabase');
  });
});