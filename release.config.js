const { promisify } = require('util')
const dateFormat = require('dateformat')
const readFileAsync = promisify(require('fs').readFile)
const path = require('path');

const TEMPLATE_DIR = path.resolve(__dirname, './.github/templates');

// the *.hbs template and partials should be passed as strings of contents
const template = readFileAsync(path.join(TEMPLATE_DIR, 'default.hbs'))
const commitTemplate = readFileAsync(path.join(TEMPLATE_DIR, 'commit.hbs'))

module.exports = {
  plugins: [
    [
      'semantic-release-gitmoji', {
        releaseRules: {
          major: [ ':boom:', '💥' ],
          minor: [ ':sparkles:', '✨' ],
          patch: [
            ':bug:', '🐛',
            ':ambulance:', '🚑',
            ':lock:', '🔒'
          ]
        },
        releaseNotes: {
          template,
          partials: { commitTemplate },
          helpers: {
            datetime: function (format = 'UTC:yyyy-mm-dd') {
              return dateFormat(new Date(), format)
            }
          },
          issueResolution: {
            template: '{baseUrl}/{owner}/{repo}/issues/{ref}',
            baseUrl: 'https://github.com',
            source: 'github.com'
          }
        }
      }
    ],
    '@semantic-release/git',
    '@semantic-release/changelog'
  ]
}