import { BASE_URL, DEFAULT_PASSWORD, NEW_PASSWORD } from "../util/constants";
import { LastEmail } from "../util/tasks";

describe('update-account.cy.ts', () => {
  it('should update account', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear().type('TestUser2');
      cy.get('#address').clear().type('address2');
      cy.get('#email').clear().type('test2@test.com');
      cy.get('#password').clear().type(DEFAULT_PASSWORD);
      cy.get('#update-account-submit').click();

      cy.contains('Account updated successfully.');

      cy.get('#home-link').click();
      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser2');
      cy.get('#address').invoke('val').should('equal', 'address2');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.task<LastEmail>('getLastEmail', {name: 'TestUser2',email: 'test2@test.com'}).then(data => {
        cy.wrap(data).its('type').should('eq', 'email');
        cy.wrap(data).its('id').should('not.eq', '1');
        cy.wrap(data).its('code').should('not.eq', '');
  
        cy.visit(`/email/${data.id}/${data.code}`);

        cy.contains('Email updated successfully.');

        cy.url().should('eq', `${BASE_URL}/login`);

        cy.contains('Register');
        cy.contains('Log in');

        cy.get('#email').type('test2@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.get('#account-link').click();
        cy.url().should('eq', `${BASE_URL}/account`);
  
        cy.get('#name').invoke('val').should('equal', 'TestUser2');
        cy.get('#address').invoke('val').should('equal', 'address2');
        cy.get('#email').invoke('val').should('equal', 'test2@test.com');
      });
    });
  });

  it('should show an error on in use email', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear().type('TestUser2');
      cy.get('#address').clear().type('address2');
      cy.get('#email').clear().type('test@test.com');
      cy.get('#password').clear().type(DEFAULT_PASSWORD);
      cy.get('#update-account-submit').click();

      cy.contains('An error occurred while updating. Please try again later.');

      cy.get('#home-link').click();
      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');
    });
  });

  it('should show an error on incorrect password', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear().type('TestUser2');
      cy.get('#address').clear().type('address2');
      cy.get('#email').clear().type('test2@test.com');
      cy.get('#password').clear().type(NEW_PASSWORD);
      cy.get('#update-account-submit').click();

      cy.contains('An error occurred while updating. Please try again later.');

      cy.get('#home-link').click();
      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');
    });
  });

  it('should show an error on empty username', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear();
      cy.get('#address').clear().type('address2');
      cy.get('#email').clear().type('test2@test.com');
      cy.get('#password').clear().type(DEFAULT_PASSWORD);
      cy.get('#update-account-submit').should('be.disabled');

      cy.contains('Name is required.');
    });
  });

  it('should show an error on empty address', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear().type('TestUser2');
      cy.get('#address').clear();
      cy.get('#email').clear().type('test2@test.com');
      cy.get('#password').clear().type(DEFAULT_PASSWORD);
      cy.get('#update-account-submit').should('be.disabled');

      cy.contains('Address is required.');
    });
  });

  it('should show an error on empty email', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear().type('TestUser2');
      cy.get('#address').clear().type('address2');
      cy.get('#email').clear();
      cy.get('#password').clear().type(DEFAULT_PASSWORD);
      cy.get('#update-account-submit').should('be.disabled');

      cy.contains('Email is required.');
    });
  });

  it('should show an error on empty password', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear().type('TestUser2');
      cy.get('#address').clear().type('address2');
      cy.get('#password').clear();
      cy.get('#email').clear().type('test2@test.com');
      cy.get('#update-account-submit').should('be.disabled');

      cy.contains('Password is required.');
    });
  });

  it('should show an error on password without lowercase letters', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear().type('TestUser2');
      cy.get('#address').clear().type('address2');
      cy.get('#email').clear().type('test2@test.com');
      cy.get('#password').clear().type('TESTPASSWORD123!@#');
      cy.get('#update-account-submit').should('be.disabled');

      cy.contains('Password must have a lower and upper case letter, digit, and special character.');
    });
  });

  it('should show an error on password without uppercase letters', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear().type('TestUser2');
      cy.get('#address').clear().type('address2');
      cy.get('#email').clear().type('test2@test.com');
      cy.get('#password').clear().type('testpassword123!@#');
      cy.get('#update-account-submit').should('be.disabled');

      cy.contains('Password must have a lower and upper case letter, digit, and special character.');
    });
  });

  it('should show an error on password without digits', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear().type('TestUser2');
      cy.get('#address').clear().type('address2');
      cy.get('#email').clear().type('test2@test.com');
      cy.get('#password').clear().type('TestPassword!@#');
      cy.get('#update-account-submit').should('be.disabled');

      cy.contains('Password must have a lower and upper case letter, digit, and special character.');
    });
  });

  it('should show an error on password without special characters', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear().type('TestUser2');
      cy.get('#address').clear().type('address2');
      cy.get('#email').clear().type('test2@test.com');
      cy.get('#password').clear().type('TestPasswor123');
      cy.get('#update-account-submit').should('be.disabled');

      cy.contains('Password must have a lower and upper case letter, digit, and special character.');
    });
  });

  it('should show an error on short password', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear().type('TestUser2');
      cy.get('#address').clear().type('address2');
      cy.get('#email').clear().type('test2@test.com');
      cy.get('#password').clear().type('TP!@#123');
      cy.get('#update-account-submit').should('be.disabled');

      cy.contains('Password must be at least 10 characters long.');
    });
  });

  it('should show an error on email with no @', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear().type('TestUser2');
      cy.get('#address').clear().type('address2');
      cy.get('#email').clear().type('test2test.com');
      cy.get('#password').clear().type('TestPassword!@#123');
      cy.get('#update-account-submit').should('be.disabled');

      cy.contains('Please enter a valid email address.');
    });
  });

  it('should show an error on email with multiple @', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear().type('TestUser2');
      cy.get('#address').clear().type('address2');
      cy.get('#email').clear().type('test2@@test.com');
      cy.get('#password').clear().type('TestPassword!@#123');
      cy.get('#update-account-submit').should('be.disabled');

      cy.contains('Please enter a valid email address.');
    });
  });

  it('should show an error on email with invalid characters', () => {
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

      cy.contains('Account verified successfully.');

      cy.url().should('eq', `${BASE_URL}/login`);
  
      cy.get('#email').type('test1@test.com');
      cy.get('#password').type(DEFAULT_PASSWORD);
      cy.get('#login-submit').click();

      cy.url().should('eq', `${BASE_URL}/not-found`);

      cy.contains('Account');
      cy.contains('Log out');

      cy.get('#account-link').click();
      cy.url().should('eq', `${BASE_URL}/account`);

      cy.get('#name').invoke('val').should('equal', 'TestUser');
      cy.get('#address').invoke('val').should('equal', 'address');
      cy.get('#email').invoke('val').should('equal', 'test1@test.com');

      cy.get('#name').clear().type('TestUser2');
      cy.get('#address').clear().type('address2');
      cy.get('#email').clear().type("a'b(c)d,e:f;g<h>i[j\\k]l@example.com");
      cy.get('#password').clear().type('TestPassword!@#123');
      cy.get('#update-account-submit').should('be.disabled');

      cy.contains('Please enter a valid email address.');
    });
  });

  afterEach(() => {
      cy.visit('/logout');
      cy.task('resetDatabase');
  });
});