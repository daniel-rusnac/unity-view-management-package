# View Management

A simple system for managing UI views.

## Installation

This package depends on [Sriptable Object Architecture](https://github.com/danielrusnac/unity-so-architecture-package) that must be mannualy installed first.

When all dependencies are resolved:
- Go to *Window/Package Manager*
- Click on *+ Add package from git URL...*
- Paste link for the wanted version.

To get a specific verion of the package, go to releases and copy the "Package Link".

## Usage

There are 4 main components to this system:
  - *View Manager* - a class that controls a list of views.
  - *View* - a gameobject that represents a menu or a screen in the UI (Main Menu, Inventory, Shop etc.)
  - *View Component* - animations and spesific behaviours that work for one view.
  - *Channel* - a scriptable object that is tied to a view. Used to call Show() an any view from anywhere in the game.

The view are shown and hidden when the corresponding channels send an event. The *ViewManager* itslef has a *Back Channel*, which will go back in the view stack as long as the *Depth* of the previous *View* is lower that the current one.

### Animations and Callbacks

- To add animations to you views, inherit from *ViewAnimation* or make use of the simple default animation component *CascadeViewAnimation*. You can also inport the animation samples that use DoTween to animate the elements.
- If you want custom logic for your views, inherit from *ViewCallbacks*.
