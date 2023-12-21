import { BASE_URL, DEFAULT_PASSWORD, NEW_PASSWORD } from "../util/constants";
import { LastEmail } from "../util/tasks";

describe('delete.cy.ts', () => {
  it('should delete', () => {
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

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#show-delete-account').click();

      cy.get('#deletePassword').type(DEFAULT_PASSWORD);
      cy.get('#delete-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);
      cy.contains('Register');
      cy.contains('Log in');

      cy.get('#login-link').click();

      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.contains('Invalid email or password.');
      cy.contains('Register');
      cy.contains('Log in');
    });
  });

  it('should be disabled', () => {
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
      cy.get('#account-link').click();

      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#show-delete-account').click();

      cy.get('#delete-submit').should('be.disabled');
    });
  });

  it('should show an error when typing a password without lowercase letters', () => {
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

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#show-delete-account').click();

      cy.get('#deletePassword').type('TESTPASSWORD123!@#');
      cy.get('#delete-submit');

      cy.get('div:has(#deletePassword)').contains('Password must have a lower and upper case letter, digit, and special character.');
    });
  });

  it('should show an error when typing a password without uppercase letters', () => {
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

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#show-delete-account').click();

      cy.get('#deletePassword').type('testpassword123!@#');
      cy.get('#delete-submit');

      cy.get('div:has(#deletePassword)').contains('Password must have a lower and upper case letter, digit, and special character.');
    });
  });

  it('should show an error when typing a password without numbers', () => {
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

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#show-delete-account').click();

      cy.get('#deletePassword').type('TestPassword!@#');
      cy.get('#delete-submit');

      cy.get('div:has(#deletePassword)').contains('Password must have a lower and upper case letter, digit, and special character.');
    });
  });

  it('should show an error when typing a password without special characters', () => {
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

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#show-delete-account').click();

      cy.get('#deletePassword').type('TestPassword123');
      cy.get('#delete-submit');

      cy.get('div:has(#deletePassword)').contains('Password must have a lower and upper case letter, digit, and special character.');
    });
  });

  it('should show an error when typing a password that is to short', () => {
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

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#show-delete-account').click();

      cy.get('#deletePassword').type('T1!e2@s3#');
      cy.get('#delete-submit');

      cy.get('div:has(#deletePassword)').contains('Password must be at least 10 characters long.');
    });
  });

  afterEach(() => {
      cy.visit('/logout');
      cy.exec('npm run linux-reload')
  });
});