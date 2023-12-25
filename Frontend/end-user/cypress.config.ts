import { defineConfig } from 'cypress'
import { getLastEmail, internalResetDatabase } from 'src/cypress/util/tasks'

export default defineConfig({
  
  e2e: {
    specPattern: 'src/cypress/e2e/**/*.cy.{js,jsx,ts,tsx}',
    supportFile: 'src/cypress/support/e2e.{js,jsx,ts,tsx}',
    fixturesFolder: 'src/cypress/fixture',
    baseUrl: 'http://localhost:5001',
    setupNodeEvents: (on, config) => {
      on('task', {
        getLastEmail
      }),
      on('task', {
        async resetDatabase() {
          await internalResetDatabase(config.env)
          return null;
        }
      })
    }
  },
  
  
  component: {
    devServer: {
      framework: 'angular',
      bundler: 'webpack',
    },
    specPattern: '**/*.cy.ts'
  }
  
})