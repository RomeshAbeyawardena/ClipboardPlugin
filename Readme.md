# Sourcetree Clipboard Plugin
## Installation Requirements
- .NET 7+
- Windows 7,10, 11/Linux distro that supports .NET 7+
- `dotnet` CLI/ IDE capable of compiling C# .NET 7 code - Visual Studio 2022 (Recommended)/
JetBrains Rider IDE
- Latest version of Sourcetree

## Deployment (recommended for non-MS Windows environment)
- Checkout out `master` branch
- Publish using the `dotnet` CLI or an IDE such as Visual Studio 
or JetBrains Rider (recommended) to an easily navigable 
directory

## MS Windows enviroments only (Windows 7, 10, 11)
Download latest release from releases section

## Sourcetree Installation
- Open Sourcetree
- Navigate to Tools > Options
- Custom Actions > Add/Edit
- Create/update action > specify published exe path in 
'Script to run' field
Set 'parameters' field to `--text $variable`
- Change `$variable` to a Sourcetree variable
- Save and close option dialog and the new action should
be available and ready to run.
## Usage

`Clipboard Plugin: Copies provided text to the clipboard:`
`        Usage: ClipboardPlugin.exe [-s|--split-string] [splitCharacterOrString] [i] [0 based index (-1-99+) -1 get last index] [-t|--text] "[Text to copy to clipboard]"`
 `Displays this help text:`
`        Help Usage: ClipboardPlugin.exe [-?|--help] True`