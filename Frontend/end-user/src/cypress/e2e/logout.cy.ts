import { BASE_URL, DEFAULT_PASSWORD } from "../util/constants";
import { LastEmail } from "../util/tasks";

describe('logout.cy.ts', () => {
  it('should logout', () => {
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

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#logout-link').click();
      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Register');
      cy.contains('Log in');
    });
  });
});