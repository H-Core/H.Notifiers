# H.Notifiers Agent Notes

## Project Role

- This repo contains event/notifier modules for H-Core/HomeCenter.
- Notifiers produce events that can trigger commands or runners, including timer, TCP ping, RSS, screenshot, and mask/screenshot detection scenarios.
- For career/portfolio analysis, this repo supports the automation-trigger side of the Raycast-style Windows system: actions are not limited to manual launch.

## Layout

- `H.Notifiers.sln` is the solution entry point.
- `src/libs/H.Notifiers.TimerNotifier` contains periodic trigger support.
- `src/libs/H.Notifiers.TcpPingNotifier` contains TCP ping trigger support.
- `src/libs/H.Notifiers.RssNotifier` contains RSS trigger support.
- `src/libs/H.Notifiers.ScreenshotNotifier` and `src/libs/H.Notifiers.MaskScreenshotNotifier` contain legacy screenshot/computer-vision trigger work.
- Tests live under `src/tests`.

## Build And Test

- Historical CI used Windows with .NET 5.
- From the repo root, the intended validation path is:

```bash
dotnet test H.Notifiers.sln --configuration Release --nologo
```

- Full validation is Windows-oriented because parts of the repo target `net472`, use screenshot APIs, and depend on EMGU CV.
- RSS and ping tests can be network-sensitive. Screenshot/mask tests can be desktop-session-sensitive.

## Maintenance Notes

- `src/Directory.Build.props` enables `LangVersion=preview` and nullable reference types.
- Library package metadata and versions live under `src/libs/Directory.Build.props`.
- Some legacy screenshot project references appear to use older folder/project names. Verify paths before assuming a clean solution build.
- Keep notifier semantics compatible with `H.Core.Notifiers` and `H.Services` consumers.
- Do not commit private screenshots, network captures, local masks, generated packages, or machine-specific image assets unless intentionally updating release artifacts.
