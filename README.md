# VST Plugin Metadata Scanner (PDR)

A .NET 9 console application that scans common Windows directories for VST2 (.dll) and VST3 (.vst3) plugins, extracts basic metadata, and writes a JSON report to the user’s Desktop.

## Features
- Recursively scans:
  - C:\\Program Files\\Common Files
  - C:\\Program Files\\Steinberg\\VSTPlugins
  - C:\\Program Files\\VSTPlugins
- Detects plugin types: VST2 (.dll) and VST3 (.vst3)
- Extracts metadata per plugin:
  - Name (file name without extension)
  - Path (full path)
  - Type (VST2 or VST3)
  - Is64Bit (determined by PE Machine type)
  - IsValid (true if metadata extraction     succeeds)
  - ErrorMessage (set when extraction fails)
- Outputs JSON with indentation to Desktop as `VST_Plugin_List.json`

## Requirements
- .NET 9 SDK
- Windows (paths assume standard Windows VST locations)

## Build
- From repository root:
  - dotnet restore
  - dotnet build -c Release

## Run
- From repository root:
  - dotnet run -c Release --project ./VSTAnalysis.csproj
- After completion, the output file is written to:
  - %USERPROFILE%\Desktop\VST_Plugin_List.json

## Output Example
```json
[
  {
    "Name": "GuitarRig",
    "Path": "C:\\Program Files\\Native Instruments\\GuitarRig.vst3",
    "Type": "VST3",
    "Is64Bit": true,
    "IsValid": true,
    "ErrorMessage": null
  }
]
```

## How it works
- Files are enumerated recursively under the configured directories.
- For each candidate plugin file:
  - Type is derived from extension: `.dll` => VST2, `.vst3` => VST3.
  - The PE header is read to determine 64-bit support (AMD64/ARM64 => true).
  - Exceptions during parsing mark the item as `IsValid = false` and populate `ErrorMessage`.
- Results are serialized using `System.Text.Json` (cached `JsonSerializerOptions` to avoid CA1869) and saved to the Desktop.

## Configuration
- To add/remove scan locations, edit `pluginDirs` in `Program.cs`.

## Project Structure
```
VSTAnalysis/
- Program.cs       (Scanning, metadata extraction, JSON output)
- PluginInfo.cs    (POCO for plugin metadata)
- VSTAnalysis.csproj
- README.md
```

## Roadmap
- Vendor and version extraction via file properties/metadata
- Optional CSV export
- GUI wrapper
- Plugin validation via registry or DAW integration

## Troubleshooting
- Some directories may require elevated permissions. The scanner skips inaccessible paths and continues.
- If `VST_Plugin_List.json` is not created, ensure the app has access to your Desktop directory and the target folders exist.

## Contributing
Issues and pull requests are welcome. Please open an issue to discuss major changes.

## License
MIT License. See LICENSE file in repository.