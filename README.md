# Wwise Tool  
This tool is intended for automating the browsing, playback and exporting of compiled Wwise `.pck`/`.bnk` files, including linkage between Wwise events and referenced audio files.

## 🚧 Work in progress
The project is in **early** development. Do not expect _functional_ functionality at this time.

## Supported projects
The following projects using Wwise are confirmed to work with this project:
- Dishonored 2 `.pck` `target`

<details>
  <summary>Legend</summary>
  
  - `target`: This project is targeted for tool development: it is being used for implementation and testing.

  - `pck`: [Wwise file packages](https://www.audiokinetic.com/fr/library/edge/?source=Help&id=managing_file_packages)\
            **_NOTE:_** do not use `.pck` extraction audio results for audio files, as they tend to be inaccurate.
            Extract the `.bnk` from the `.pck` first and extract the audio files from that.

  - `bnk`: A Wwise bank file containing Wwise metadata and `.wem` audio files.
            These also need extracting, for both the event metadata and audio files.

  - `wem`: Wwise audio file\
            These can be played by **vgmstream**, but can also be converted to a more usable format.

  - `txtp`: **vgmstream** TXTP command file
</details>

## Documentation
_To be added._

## Building
To build this project, you need Visual Studio 2022 or newer, with the **Windows application development** workload installed. Use **Windows 11 SDK (10.0.26100.0)**.
