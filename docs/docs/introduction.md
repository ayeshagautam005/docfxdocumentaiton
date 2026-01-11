# Introduction

BOOSEapp is a small drawing and scripting environment where you control a canvas using simple text commands. It is designed to demonstrate variables, control flow, arrays, and methods through visual output on a canvas.

BOOSE scripts are parsed into command objects (like `AppMoveTo`, `AppDrawTo`, `AppCircle`, `AppRectangle`) and executed on the `AppCanvas` surface. Commands implement the `ICommand` interface and are created by the `AppCommandFactory` and `AppParser` components.

## What you can learn

- Basic drawing with `moveto`, `drawto`, `circle`, and `rect`.
- Using integer and real variables (`int`, `real`) to control shape size and colour.
- Creating arrays and reading/writing values with `poke` and `peek`.
- Writing conditional logic with `if` / `else` and loops with `while` and `for`.
- Defining reusable methods and calling them from your scripts.

## Where to start

- Read the [Getting Started](getting-started.md) page for a full set of example programs you can paste directly into BOOSEapp.
- Open the [homepage](../index.md) to navigate the rest of the documentation and API reference.
- 
