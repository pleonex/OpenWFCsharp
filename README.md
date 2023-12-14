# OpenWFCsharp

<!-- markdownlint-disable MD033 -->
<p align="center">
  <a href="https://github.com/pleonex/OpenWFCsharp/workflows/Build%20and%20release">
    <img alt="Build and release" src="https://github.com/pleonex/OpenWFCsharp/workflows/Build%20and%20release/badge.svg?branch=main&event=push" />
  </a>
  &nbsp;
  <a href="https://choosealicense.com/licenses/mit/">
    <img alt="MIT License" src="https://img.shields.io/badge/license-MIT-blue.svg?style=flat" />
  </a>
  &nbsp;
</p>

Open-source alternative to the NDS DWC servers.

> [!IMPORTANT]  
> At this moment this project only aims to reproduce the download content server
> (DLS1). I don't have time to design, support or maintain the online player
> servers. Feel free to fork if you would like to take this project as a base.

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
