# SOLIDWORKS Prompt Assistant

Starter project for a prompt-driven SOLIDWORKS add-in built with [CAD Booster SolidDNA](https://github.com/CAD-Booster/SolidDNA).

## What works in this starter

The add-in opens a small command window when SOLIDWORKS starts. Type `help`, `active document`, or `set property <name> = <value>`. A property change is previewed and requires confirmation before it is sent to SOLIDWORKS. The parser is deliberately narrow and local: it does not call an AI service yet.

## Why start with narrow commands?

The long-term flow is `prompt → proposed structured action → user review → SOLIDWORKS API call`. The UI and action executor in this repo establish the last two steps. An AI interpreter can later propose actions using the same action types; it should never execute arbitrary generated code directly.

## Build and run

1. On Windows, install SOLIDWORKS, Visual Studio with .NET desktop development, and the .NET Framework 4.8 developer pack.
2. Open `src/SolidWorksPromptAssistant/SolidWorksPromptAssistant.csproj` in Visual Studio and restore NuGet packages.
3. Build the DLL. Register it using the [SolidDNA add-in installer](https://github.com/CAD-Booster/SolidDNA) or `RegAsm` as described in SolidDNA's README, then enable it in SOLIDWORKS Add-Ins.
4. Open a part, assembly, or drawing and use the command window. For example: `set property Project = Prototype`.

The add-in cannot be built or tested on macOS because it targets Windows .NET Framework and needs SOLIDWORKS. The package version and integration code should be validated in a Windows/SOLIDWORKS environment before relying on it.

## Next milestones

- Add a small set of typed, reversible modeling actions with explicit units and document-type checks.
- Add an AI interpreter that returns only validated action JSON; keep API credentials outside the repository.
- Add a task pane and command-manager button so the prompt UI can be opened on demand.
- Add Windows integration tests against a disposable SOLIDWORKS document.

## Safety boundaries

The current command set cannot save, delete, or run arbitrary code. Confirm each mutation and use test documents while developing. No prompts, document content, or API keys are sent to a remote service by this starter.
