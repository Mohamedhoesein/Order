import { BASE_URL, DEFAULT_PASSWORD, NEW_PASSWORD } from "../util/constants";
import { LastEmail } from "../util/tasks";

describe('register.cy.ts', () => {
  it('should register', () => {
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
    });
  });

  it('should be disabled', () => {
    cy.visit('/');

    cy.get('#register-link').click();
    cy.url().should('eq', `${BASE_URL}/register`);

    cy.get('#register-submit').should('be.disabled');
  });

  it('should show an error when typing an email without an @', () => {
      cy.visit('/');

      cy.get('#register-link').click();
      cy.url().should('eq', `${BASE_URL}/register`);

      cy.get('#email').type('Abc.example.com');
      cy.get('#register-submit');

      cy.contains('Please enter a valid email address.');
  });

  it('should show an error when typing an email with multiple @', () => {
      cy.visit('/');

      cy.get('#register-link').click();
      cy.url().should('eq', `${BASE_URL}/register`);

      cy.get('#email').type('A@b@c@example.com ');
      cy.get('#register-submit');

      cy.contains('Please enter a valid email address.');
  });

  it('should show an error when typing an email with invalid characters', () => {
      cy.visit('/');

      cy.get('#register-link').click();
      cy.url().should('eq', `${BASE_URL}/register`);

      cy.get('#email').type("a'b(c)d,e:f;g<h>i[j\\k]l@example.com");
      cy.get('#register-submit');

      cy.contains('Please enter a valid email address.');
  });

  it('should show an error when typing a password without lowercase letters', () => {
      cy.visit('/');

      cy.get('#register-link').click();
      cy.url().should('eq', `${BASE_URL}/register`);

      cy.get('#password').type('TESTPASSWORD123!@#');
      cy.get('#register-submit');

      cy.get('div:has(#password)').contains('Password must have a lower and upper case letter, digit, and special character.');
  });

  it('should show an error when typing a password without uppercase letters', () => {
      cy.visit('/');

      cy.get('#register-link').click();
      cy.url().should('eq', `${BASE_URL}/register`);

      cy.get('#password').type('testpassword123!@#');
      cy.get('#register-submit');

      cy.get('div:has(#password)').contains('Password must have a lower and upper case letter, digit, and special character.');
  });

  it('should show an error when typing a password without numbers', () => {
      cy.visit('/');

      cy.get('#register-link').click();
      cy.url().should('eq', `${BASE_URL}/register`);

      cy.get('#password').type('TestPassword!@#');
      cy.get('#register-submit');

      cy.get('div:has(#password)').contains('Password must have a lower and upper case letter, digit, and special character.');
  });

  it('should show an error when typing a password without special characters', () => {
      cy.visit('/');

      cy.get('#register-link').click();
      cy.url().should('eq', `${BASE_URL}/register`);

      cy.get('#password').type('TestPassword123');
      cy.get('#register-submit');

      cy.get('div:has(#password)').contains('Password must have a lower and upper case letter, digit, and special character.');
  });

  it('should show an error when typing a password that is to short', () => {
      cy.visit('/');

      cy.get('#register-link').click();
      cy.url().should('eq', `${BASE_URL}/register`);

      cy.get('#password').type('T1!e2@s3#');
      cy.get('#register-submit');

      cy.get('div:has(#password)').contains('Password must be at least 10 characters long.');
  });

  it('should show an error when typing a different password', () => {
      cy.visit('/');

      cy.get('#register-link').click();
      cy.url().should('eq', `${BASE_URL}/register`);

      cy.get('#password').type(NEW_PASSWORD);
      cy.get('#confirmPassword').type(DEFAULT_PASSWORD);
      cy.get('#register-submit');

      cy.get('div:has(#confirmPassword)').contains('Please retype the password.');
  });

  afterEach(() => {
      cy.visit('/logout');
      cy.exec('npm run reload')
  });
});
