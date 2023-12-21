import { BASE_URL, DEFAULT_PASSWORD, NEW_PASSWORD } from "../util/constants";
import { LastEmail } from "../util/tasks";

describe('login.cy.ts', () => {
  it('should login', () => {
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
    });
  });

  it('should be disabled', () => {
    cy.visit('/');

    cy.get('#login-link').click();
    cy.url().should('eq', `${BASE_URL}/login`);

    cy.get('#login-submit').should('be.disabled');
  });

  it('should show an error for an invalid email', () => {
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
  
      cy.get('#email').type('test2@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);

      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/login`);

      cy.contains('Invalid email or password.');
      cy.contains('Register');
      cy.contains('Log in');
    });
  });

  it('should show an error for an invalid passowrd', () => {
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
      cy.get('#password').type(NEW_PASSWORD);

      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/login`);

      cy.contains('Invalid email or password.');
      cy.contains('Register');
      cy.contains('Log in');
    });
  });

  it('should show an error when typing an email without an @', () => {
      cy.visit('/');

      cy.get('#login-link').click();
      cy.url().should('eq', `${BASE_URL}/login`);

      cy.get('#email').type('Abc.example.com');
      cy.get('#login-submit');

      cy.contains('Please enter a valid email address.');
  });

  it('should show an error when typing an email with multiple @', () => {
      cy.visit('/');

      cy.get('#login-link').click();
      cy.url().should('eq', `${BASE_URL}/login`);

      cy.get('#email').type('A@b@c@example.com ');
      cy.get('#login-submit');

      cy.contains('Please enter a valid email address.');
  });

  it('should show an error when typing an email with invalid characters', () => {
      cy.visit('/');

      cy.get('#login-link').click();
      cy.url().should('eq', `${BASE_URL}/login`);

      cy.get('#email').type("a'b(c)d,e:f;g<h>i[j\\k]l@example.com");
      cy.get('#login-submit');

      cy.contains('Please enter a valid email address.');
  });

  it('should show an error when typing a password without lowercase letters', () => {
      cy.visit('/');

      cy.get('#login-link').click();
      cy.url().should('eq', `${BASE_URL}/login`);

      cy.get('#password').type('TESTPASSWORD123!@#');
      cy.get('#login-submit');

      cy.get('div:has(#password)').contains('Password must have a lower and upper case letter, digit, and special character.');
  });

  it('should show an error when typing a password without uppercase letters', () => {
      cy.visit('/');

      cy.get('#login-link').click();
      cy.url().should('eq', `${BASE_URL}/login`);

      cy.get('#password').type('testpassword123!@#');
      cy.get('#login-submit');

      cy.get('div:has(#password)').contains('Password must have a lower and upper case letter, digit, and special character.');
  });

  it('should show an error when typing a password without numbers', () => {
      cy.visit('/');

      cy.get('#login-link').click();
      cy.url().should('eq', `${BASE_URL}/login`);

      cy.get('#password').type('TestPassword!@#');
      cy.get('#login-submit');

      cy.get('div:has(#password)').contains('Password must have a lower and upper case letter, digit, and special character.');
  });

  it('should show an error when typing a password without special characters', () => {
      cy.visit('/');

      cy.get('#login-link').click();
      cy.url().should('eq', `${BASE_URL}/login`);

      cy.get('#password').type('TestPassword123');
      cy.get('#login-submit');

      cy.get('div:has(#password)').contains('Password must have a lower and upper case letter, digit, and special character.');
  });

  it('should show an error when typing a password that is to short', () => {
      cy.visit('/');

      cy.get('#login-link').click();
      cy.url().should('eq', `${BASE_URL}/login`);

      cy.get('#password').type('T1!e2@s3#');
      cy.get('#login-submit');

      cy.get('div:has(#password)').contains('Password must be at least 10 characters long.');
  });

  afterEach(() => {
      cy.visit('/logout');
      cy.exec('npm run linux-reload')
  });
});