import { defineConfig } from 'cypress'
import { getLastEmail } from 'src/cypress/util/tasks'

export default defineConfig({
  
  e2e: {
    specPattern: 'src/cypress/e2e/**/*.cy.{js,jsx,ts,tsx}',
    supportFile: 'src/cypress/support/e2e.{js,jsx,ts,tsx}',
    fixturesFolder: 'src/cypress/fixture',
    baseUrl: 'http://localhost:5001',
    setupNodeEvents: (on, _) => {
      on('task', {
        getLastEmail
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