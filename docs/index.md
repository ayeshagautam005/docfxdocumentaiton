---
_layout: landing
---

# This is the **HOMEPAGE**.

Refer to [Markdown](http://daringfireball.net/projects/markdown/) for how to write markdown files.

# BOOSEapp Documentation

BOOSEapp is a drawing and scripting application that lets you control a canvas with simple commands and loops.

## Get started

- [Getting Started](docs/getting-started.md)
- [Introduction](docs/introduction.md)

## What you can do

- Move the pen around the canvas with commands like `moveto` and `drawto` (`AppMoveTo`, `AppDrawTo`).
- Draw shapes such as circles and rectangles (`AppCircle`, `AppRectangle`).
- Change pen colour, clear or reset the canvas, and write text (`SetPenColorCommand`, `ClearCommand`, `ResetCommand`, `WriteTextCommand`).
- Use variables, loops, and conditions to create reusable drawing scripts (`AppInt`, `AppReal`, `AppFor`, `AppWhile`, `AppIf`, `AppMethod`).

## Explore the API

- [BOOSEapp namespace](api/api/BOOSEapp.html)
- Key types:
  - Drawing surface: `AppCanvas`, `CanvasException`.
  - Commands: `ICommand`, `AppCommandFactory`, `ICommandParser`, `AppParser`.
  - User interface: `MainForm`.

