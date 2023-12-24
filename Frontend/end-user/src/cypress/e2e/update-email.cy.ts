import { BASE_URL } from "../util/constants";

describe('update-email.cy.ts', () => {
  it('should show not found if id is not a number', () => {
    cy.visit('/email/abc/123');
    cy.url().should('eq', `${BASE_URL}/not-found`);
  });

  it('should show error', () => {
    cy.visit('/email/123/abc');
    cy.contains('Email update failed.');
  });

  afterEach(() => {
      cy.visit('/logout');
      cy.task('resetDatabase');
  });
});