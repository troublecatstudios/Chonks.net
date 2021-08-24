const { promisify } = require('util')
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

          },
          issueResolution: {
            template: '{baseUrl}/{owner}/{repo}/issues/{ref}',
            baseUrl: 'https://github.com',
            source: 'github.com'
          }
        }
      }
    ],
    [
      '@semantic-release/git',
      {
          assets: [
              'package.json',
              'CHANGELOG.md'
          ],
          message: '🧹: ${nextRelease.version} [skip ci]\n\n${nextRelease.notes}'
      }
    ],
    '@semantic-release/github',
    '@semantic-release/changelog'
  ]
}