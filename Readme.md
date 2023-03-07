# SourceTree Clipboard Plugin
## Installation Requirements
- .NET 7
- Windows 7,10, 11/Linux distro that supports .NET 7+
- Visual Studio 2022
- Latest version of SourceTree

## Deployment
Checkout out `master` branch locally
Publish using CLI or an IDE such as Visual Studio 
or JetBrains Rider recommended to a directory of your 
choice.
- Open SourceTree,
- Navigate to Tools > Options
- Custom Actions > Add/Edit
- Create action specify published exe path in 
'Script to run' field
Set 'parameters' field to --text $variable
- Change $variable to a variable SourceTree variable
- Save and close option dialog and the new action should
be available and ready to run.
## Usage

`ClipboardPlugin --text "Text to be copied to the clipboard`