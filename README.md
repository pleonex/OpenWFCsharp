# ![logo](docs/images/logo_32.png) OpenWFC\#

<!-- markdownlint-disable MD033 -->
<p align="center">
  <a href="https://github.com/pleonex/OpenWFCsharp/releases">
    <img alt="GitHub release" src="https://img.shields.io/github/v/release/pleonex/OpenWFCsharp">
  </a>
  &nbsp;
  <a href="https://github.com/pleonex/OpenWFCsharp/actions/workflows/build-and-release.yml">
    <img alt="Build and release" src="https://github.com/pleonex/OpenWFCsharp/actions/workflows/build-and-release.yml/badge.svg?branch=main" />
  </a>
  &nbsp;
  <a href="https://choosealicense.com/licenses/mit/">
    <img alt="MIT License" src="https://img.shields.io/badge/license-MIT-blue.svg?style=flat" />
  </a>
  &nbsp;
</p>

**Open-source** alternative to the NDS DWC / WFC servers with .NET (C#)
technologies.

## Server status implementation

- 🌍 **Connectivity test** (`conntest`): ✅ Implemented
- 👤 **Authorization** (`nas`): 🌱 Basic implementation
  - Supported actions: `login`, `SVCLOC`
  - It doesn't perform any user auth or registration. Emulators don't randomize
    the MAC address of the DS, which means that for the server everyone using an
    emulator would be the same user.
- 🔽 **Download** (`dls1`): ✅ Implemented
  - Supported actions: `count`, `list`, `contents`
  - It doesn't support return of random files (mystery gift)

> [!IMPORTANT]  
> This project only aims to provide DLCs / download content to DS games. It will
> implement only the required servers (nas and dls1). I don't have time at this
> moment to design, support, review or maintain features for online multiplayer
> gamespy servers. Feel free to fork if you would like to take this project as a
> base.

## Get started

Self-hosting instructions will be available in the
[project website](https://www.pleonex.dev/OpenWFCsharp).

Feel free to ask any question in the project
[Discussion site!](https://github.com/pleonex/OpenWFCsharp/discussions)

**I don't provide any public server to connect.**

## Build

The project requires to build .NET 8.0 SDK.

To build, test and generate artifacts run:

```sh
# Build and run tests (with code coverage!)
dotnet run --project build/orchestrator

# (Optional) Create bundles (nuget, zips, docs)
dotnet run --project build/orchestrator -- --target=Bundle
```

## Release

Create a new GitHub release with a tag `v{Version}` (e.g. `v2.4`) and that's it!
This triggers a pipeline that builds and deploy the project.

## License

Under the MIT license.

[Icon by Daniel ceha](https://www.freepik.com/icon/hot-tea_8122151#fromView=search&term=sake+cup&page=8&position=13&track=ais&uuid=4216053d-58f6-447c-ade8-1332310378ba").
