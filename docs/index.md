# OpenWFC\# ![[MIT license](https://choosealicense.com/licenses/mit)](https://img.shields.io/badge/license-MIT-blue.svg?style=flat)

**Open-source** alternative to the NDS DWC / WFC servers with .NET (C#)
technologies.

## Server status implementation

- 🌍 **Connectivity test** (`conntest`): ✅ Implemented
- 👤 **Authorization** (`nas`): 🌱 Basic implementation
  - Supported actions: `acctcreate`, `login`, `SVCLOC`
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

## Installation

Soon available...

> [!NOTE]  
> I don't provide any public server to connect.
