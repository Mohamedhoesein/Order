import { BASE_URL, DEFAULT_PASSWORD } from "../util/constants";

describe('category.cy.ts', () => {
    it('should add category', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#categories-link').click();
        cy.url().should('eq', `${BASE_URL}/categories`);
        cy.contains('InitialCategory').should('exist');

        cy.get('#add-category').click();
        cy.get('#add-category-submit').should('exist');

        cy.get('#name').type('TempCategory');
        cy.get('#add-category-submit').click();

        cy.get('#add-category-submit').should('not.exist');
        cy.contains('TempCategory').should('exist');
    });

    it('should delete and restore category', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#categories-link').click();
        cy.url().should('eq', `${BASE_URL}/categories`);
        cy.contains('InitialCategory').should('exist');

        cy.get('#InitialCategory .delete-category').click();
        cy.get('#InitialCategory .restore-category').should('exist');

        cy.get('#home-link').click();
        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#categories-link').click();
        cy.url().should('eq', `${BASE_URL}/categories`);

        cy.get('#InitialCategory .restore-category').click();
        cy.get('#InitialCategory .delete-category').should('exist');

        cy.get('#home-link').click();
        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#categories-link').click();
        cy.url().should('eq', `${BASE_URL}/categories`);

        cy.get('#InitialCategory .delete-category').should('exist');
    });

    it('should rename open specification', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#categories-link').click();
        cy.url().should('eq', `${BASE_URL}/categories`);
        cy.contains('InitialCategory').should('exist');

        cy.get('#open_InitialCategory_OpenSpecification .rename-open-specification').click();
        cy.get('#name').type('OpenSpecification1');
        cy.get('#rename-open-specification-submit').click();

        cy.contains('OpenSpecification1').should('exist');
    });

    it('should delete and restore open specification', () => {
        cy.visit('/login');

        cy.get('#email').type('test@test.com');
        cy.get('#password').type(DEFAULT_PASSWORD);
        cy.get('#login-submit').click();

        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#categories-link').click();
        cy.url().should('eq', `${BASE_URL}/categories`);
        cy.contains('InitialCategory').should('exist');

        cy.get('#open_InitialCategory_OpenSpecification .delete-open-specification').click();
        cy.get('#open_InitialCategory_OpenSpecification .restore-open-specification').should('exist');

        cy.get('#home-link').click();
        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#categories-link').click();
        cy.url().should('eq', `${BASE_URL}/categories`);

        cy.get('#open_InitialCategory_OpenSpecification .restore-open-specification').click();
        cy.get('#open_InitialCategory_OpenSpecification .delete-open-specification').should('exist');

        cy.get('#home-link').click();
        cy.url().should('eq', `${BASE_URL}/`);

        cy.get('#categories-link').click();
        cy.url().should('eq', `${BASE_URL}/categories`);

        cy.get('#open_InitialCategory_OpenSpecification .delete-open-specification').should('exist');
    });

    before(() => {
        cy.task('resetDatabase');
    });

    afterEach(() => {
        cy.get('#logout-link').click();
        cy.task('resetDatabase');
    });
});