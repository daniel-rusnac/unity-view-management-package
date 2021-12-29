# View Management

A flexible system for managing UI view (screens).

## Installation

This package depends on [Sriptable Object Architecture](https://github.com/danielrusnac/unity-so-architecture-package) that must be mannualy installed first.

When all dependencies are resolved:
- Go to *Window/Package Manager*
- Click on *+ Add package from git URL...*
- Paste link for the wanted version.
Package Link for latest version, [v1.1.0](https://github.com/danielrusnac/unity-view-management-package/releases/tag/1.1.0)
```
https://github.com/danielrusnac/unity-view-management-package.git?path=/View Management/Assets/ViewManagement#v1.1.0
```

To get a specific verion of the package, go to releases and copy the "Package Link".

## Usage

There are 2 main components to this system:
  - View
  - View Manager
  - UI Channel (void channel from SOArchitecure)

The view are shown and hidden when the corresponding channels send an event. The *ViewManager* itslef has a *Back Channel*, which will go back in the view stack as long as the *Depth* of the previous *View* is lower that the current one.

### Animations and Callbacks

- To add animations to you views, inherit from *ViewAnimation* or make use of the simple default animation component *CascadeViewAnimation*. You can also inport the animation samples that use DoTween to animate the elements.
- If you want custom logic for your views, inherit from *ViewCallbacks*.
