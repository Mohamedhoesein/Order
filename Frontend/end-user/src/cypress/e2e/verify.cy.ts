import { BASE_URL } from "../util/constants";

describe('verify.cy.ts', () => {
  it('should show not found if id is not a number', () => {
    cy.visit('/verify/abc/123');
    cy.url().should('eq', `${BASE_URL}/not-found`);
  });

  it('should show error', () => {
    cy.visit('/verify/123/abc');
    cy.contains('Account verification failed.');
  });
});