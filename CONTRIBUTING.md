# Contributing to the Indigenous Art Gallery Project

You can help to improve the Thoth Tech Indigenous Art Gallery Project by sending pull requests to
this repository.

Feel free to propose any new ideas for the development of this project, or put forward any ideas on
how to improve the current state of our project.

## File testing

All files that are committed to the repository will need to pass a series of checks to ensure they
are in line with our standard file formatting. To help with this, we recommend installing Prettier,
Vale and MarkdownLint packages in VS code. Our tests lint the words and structure of documentation
and code files.

The tests used and created are fully credited to the documentation team in Thoth Tech. Please refer
to the documentation repository to further clarify our testing process.

### Prettier

[Prettier](https://prettier.io/) checks that Markdown syntax follows the
[CommonMark](https://commonmark.org/) specifications.

### Vale

[Vale](https://docs.errata.ai/vale/about/) is a grammar, style, and word usage linter for the
English language. Vale's configuration is stored in the
[`.vale.ini`](https://github.com/thoth-tech/art-gallery/blob/development/.vale.ini) file located in
the root directory.

Vale supports creating [custom tests](https://docs.errata.ai/vale/styles) that extend any of several
types of checks, which we store in the `.vale/thothtech/` directory in the vale directory.

### Linelint

[Linelint](https://github.com/fernandrone/linelint) is a linter that validates simple newline and
whitespace rules in all sorts of files.

## Repository Contribution Guidelines

Please ensure all code and documentation files are committed using the following process.

### Local Repository Setup (this only needs to be completed once before your first commit)

1. Fork this repository in the GitHub GUI
2. Clone your forked repository into a local directory using
   `git clone <https://github.com/{username}/art-gallery.git>`

### Creating a new branch

1. To create and switch to a new branch use `git checkout -b <topic>/<branch description>` (please
   refer to our branch naming conventions below)

### Comitting and pushing changes

1. To add files to your commit use `git add .`
2. To commit these files use `git commit`, you will be directed to make a comment. To make your
   comment press the `i` key, then format your comment as: `<topic>:<message>` (please refer to our
   commit message conventions below)
3. To push your changes use `git push -u origin <your new branch name>`

### Creating a draft pull request

1. Go into the GitHub GUI and locate the `art-gallery` repository. Compare and merge your latest
   push, and create a **draft pull request**. Your draft pull request will be between The
   `thoth-tech:development` branch and your own branch (eg.
   `chloeehulme:fix/fixing-prettier-errors`)
2. If your changes do not pass our workflow tests, please review your errors and re-commit your
   updates files.
3. Once your files pass all checks in our workflow, mark your commit as `ready for review`

Please note: once a pull request is closed, it will not be reopened, please create a new branch and
proceed with the above instructions on your new branch.

### Branch naming conventions

Your branch name should follow this format: `<topic>/<branch description>`

Your branch topic should briefly describe the topic your changes/additions fall under.

Accepted topics include:

- docs -> for adding documentation
- feature -> for adding a new feature
- fix -> for fixing a bug
- update -> for updating an existing feature/file

Your branch description should briefly describe your inteded changes/additions.

ie. `fix/middleware-functions`

### Commit message conventions

Your commit message should follow this format: `<topic>:<message>`

Your message topic should briefly describe the topic your changes/additions fall under.

Accepted topics include:

- build -> for changes that affect the build system or external dependencies
- ci -> for changes to our CI configuration files and scripts
- docs -> for changes exclusively made to documentaton files
- feature -> for new features
- fix -> a bug fix
- performance -> for a code change that improves performance
- refactor -> for a code change that neither fixes a bug nor adds a feature
- test -> for adding missing tests or correcting existing tests

Your commit message description should briefly describe your inteded changes/additions.

ie. `docs: adding missing info to readme`
