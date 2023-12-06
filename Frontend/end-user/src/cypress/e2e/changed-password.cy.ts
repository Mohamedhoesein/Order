import { BASE_URL, DEFAULT_PASSWORD, NEW_PASSWORD } from "../util/constants";
import { LastEmail } from "../util/tasks";

describe('changed-password.cy.ts', () => {
  it('should change password', () => {	
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
  
          cy.url().should('eq', `${BASE_URL}/login`);
  
          cy.get('#email').type('test1@test.com');
          cy.get('#password').type(NEW_PASSWORD);
    
          cy.get('#login-submit').click();
  
          cy.url().should('eq', `${BASE_URL}/not-found`);
  
          cy.contains('Account');
          cy.contains('Log out');
        });
      });
    });
  });

  it('should be disabled', () => {
    cy.visit('/change-password/1/1');

    cy.get('#change-password-submit').should('be.disabled');
  });


  it('should show an error when using invalid id', () => {
    cy.visit('/change-password/1/Q2ZESjhIbkE3U1J0TUoxRnYxaGhodDUzbGFmNzU3SGoxOGVnK3FEM1Z6cmhBSEJUM253T1J5OFJVbGlJT3h6cGhXNEU1eHJEanNrd3pSVHVWeHBUU1Q0c2s3SXVHVjgrM2l0K1AwczZ3bFNNNnpFaEIxZG5Rb1FzNFhjbW1yK3dsQ1l0WDhMR21sUTJLbDdxYWVScVhkbTZaMjhMakR5TjUvZXhHOUZYY3JmTHAwMDIwckF1NTVKb0ZxdHJhMjl1VHUrYkJRPT0');

    cy.get('#password').type(NEW_PASSWORD);
    cy.get('#confirmPassword').type(NEW_PASSWORD);
    cy.get('#change-password-submit').click();

    cy.contains('An error occurred while changing the password. Please try again later.');
  });

  it('should show an error when using different password', () => {
    cy.visit('/change-password/1/Q2ZESjhIbkE3U1J0TUoxRnYxaGhodDUzbGFmNzU3SGoxOGVnK3FEM1Z6cmhBSEJUM253T1J5OFJVbGlJT3h6cGhXNEU1eHJEanNrd3pSVHVWeHBUU1Q0c2s3SXVHVjgrM2l0K1AwczZ3bFNNNnpFaEIxZG5Rb1FzNFhjbW1yK3dsQ1l0WDhMR21sUTJLbDdxYWVScVhkbTZaMjhMakR5TjUvZXhHOUZYY3JmTHAwMDIwckF1NTVKb0ZxdHJhMjl1VHUrYkJRPT0');

    cy.get('#password').type(NEW_PASSWORD);
    cy.get('#confirmPassword').type('TestPassword322!@#');
    cy.get('#change-password-submit');

    cy.get('div:has(#confirmPassword)').contains('Passwords do not match.');
  });

  it('should show an error when typing a password without lowercase letters', () => {
    cy.visit('/change-password/1/1');

    cy.get('#password').type('TESTPASSWORD123!@#');
    cy.get('#change-password-submit');

    cy.get('div:has(#password)').contains('Password must have a lower and upper case letter, digit, and special character.');
  });

  it('should show an error when typing a password without uppercase letters', () => {
    cy.visit('/change-password/1/1');

    cy.get('#password').type('testpassword123!@#');
    cy.get('#change-password-submit');

    cy.get('div:has(#password)').contains('Password must have a lower and upper case letter, digit, and special character.');
  });

  it('should show an error when typing a password without numbers', () => {
    cy.visit('/change-password/1/1');

    cy.get('#password').type('TestPassword!@#');
    cy.get('#change-password-submit');

    cy.get('div:has(#password)').contains('Password must have a lower and upper case letter, digit, and special character.');
  });

  it('should show an error when typing a password without special characters', () => {
    cy.visit('/change-password/1/1');

    cy.get('#password').type('TestPassword123');
    cy.get('#change-password-submit');

    cy.get('div:has(#password)').contains('Password must have a lower and upper case letter, digit, and special character.');
  });

  it('should show an error when typing a password that is to short', () => {
    cy.visit('/change-password/1/1');

    cy.get('#password').type('T1!e2@s3#');
    cy.get('#change-password-submit');

    cy.get('div:has(#password)').contains('Password must be at least 10 characters long.');
  });
});