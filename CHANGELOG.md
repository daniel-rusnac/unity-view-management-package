# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.2] - 2022-02-05

### Added

- More tooltips

### Removed

- Removed deprecated `View.IsShown` property
- Removed unused view events

## [2.0.1] - 2022-01-31

### Changed

- `ViewManager` initialization is moved from `Start()` to `Awake()`
- Updated dependencies

## [2.0.0] - 2022-01-12

### Added

- Components add menu

### Changed

- Raycast blocker is now a ViewComponent

### Fixed

- Views not being shown/hidden properly in a specific scenario

## [1.2.0] - 2022-01-12

### Changed
- The raycast blocker is now active only if at least one active view has this setting
- Now only one view per object is allowed

### Fixed
- Fixed views not being set dirty when a callback was toggled

## [1.1.1] - 2022-01-10

### Added
- More view registration logs
- Added a raycast blocker
- Views now have all callbacks as unity events

### Changed
- Updated dependency versions

### Fixed
- Fixed cascade view animations not stopping the coroutines when starting the animations before the last on finished

### Removed
- Removed the old OnExit unity event on the view

## [1.1.0] - 2021-12-29
### Added
- Cascade view animation component
- Added a changelog

### Changed
- Updated the samples that use DoTween for animations

### Fixed
- The view manager was sometimes getting stuck when multiple Show event were called with an increasing number of depth

[Unreleased]: https://github.com/danielrusnac/unity-view-management-package
[1.1.0]: https://github.com/danielrusnac/unity-view-management-package/releases/tag/v1.1.0
[1.1.1]: https://github.com/danielrusnac/unity-view-management-package/releases/tag/v1.1.1
[1.2.0]: https://github.com/danielrusnac/unity-view-management-package/releases/tag/v1.2.0
[2.0.0]: https://github.com/danielrusnac/unity-view-management-package/releases/tag/v2.0.0
[2.0.1]: https://github.com/danielrusnac/unity-view-management-package/releases/tag/v2.0.1
[2.0.2]: https://github.com/danielrusnac/unity-view-management-package/releases/tag/v2.0.2
