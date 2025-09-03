---

# 🎛️ VST Plugin Metadata Scanner – PDR

## 📌 Overview
This .NET 9 console application scans specified directories for VST2 (`.dll`) and VST3 (`.vst3`) plugin files, extracts metadata, and outputs the results as a structured JSON file to the user's Desktop.

---

## 🧩 Purpose
To provide a lightweight, fast, and extensible tool for analyzing installed VST plugins on a Windows system, with support for metadata extraction and future integration into plugin management workflows.

---

## 📁 File Structure
```
G:\repos\VSTAnalysis\
│
├── Program.cs         // Main application logic
├── PluginInfo.cs      // Class definition for plugin metadata
```

---

## 🧠 Functionality

### ✅ Scanning
- Recursively scans the following directories:
  - `C:\Program Files\Common Files`
  - `C:\Program Files\Steinberg\VSTPlugins`
  - `C:\Program Files\VSTPlugins`

### ✅ File Types
- Detects:
  - `.vst3` (VST3 plugins)
  - `.dll` (VST2 plugins)

### ✅ Metadata Extraction
For each plugin file, the app extracts:
- `Name`: File name without extension
- `Path`: Full file path
- `Type`: VST2 or VST3
- `Is64Bit`: Determined via PE header for `.dll` files
- `IsValid`: True if metadata extraction succeeds
- `ErrorMessage`: Populated if extraction fails

### ✅ Output
- Saves metadata to:
  - `VST_Plugin_List.json` on the user's Desktop
- Uses `System.Text.Json` for serialization with indentation

---

## 🧱 Dependencies
- .NET 9 SDK
- No external libraries required

---

## 🧪 Future Enhancements
- Add support for:
  - Vendor and version extraction via file properties
  - CSV export
  - GUI wrapper
  - Plugin validation via registry or DAW integration

---

## 🧾 Example Output (JSON)
```json
[
  {
    "Name": "GuitarRig",
    "Path": "C:\\Program Files\\Native Instruments\\GuitarRig.vst3",
    "Type": "VST3",
    "Is64Bit": true,
    "IsValid": true,
    "ErrorMessage": null
  },
  ...
]
```

---

Would you like a README.md file generated for GitHub or a CSV export added to the app?