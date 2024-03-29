import { defineConfig } from 'cypress';
import { getLastEmail, internalResetDatabase } from './src/cypress/util/tasks';

export default defineConfig({
  defaultCommandTimeout: 10000,
  e2e: {
    specPattern: 'src/cypress/e2e/**/*.cy.{js,jsx,ts,tsx}',
    supportFile: 'src/cypress/support/e2e.{js,jsx,ts,tsx}',
    fixturesFolder: 'src/cypress/fixture',
    baseUrl: 'http://localhost:5000',
    setupNodeEvents: (on, config) => {
      on('task', {
        getLastEmail
      });
      on('task', {
        async resetDatabase() {
          await internalResetDatabase(config.env)
          return null;
        }
      });
    },
  }
});
