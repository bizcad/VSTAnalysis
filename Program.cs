using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using VSTAnalysis;

class Program
{
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    static void Main()
    {
        var pluginDirs = new[]
        {
            @"C:\Program Files\Common Files",
            @"C:\Program Files\Steinberg\VSTPlugins",
            @"C:\Program Files\VSTPlugins"
        };

        var pluginInfoList = new List<PluginInfo>();

        foreach (var dir in pluginDirs)
        {
            if (!Directory.Exists(dir)) continue;

            try
            {
                foreach (var file in Directory.EnumerateFiles(dir, "*.*", SearchOption.AllDirectories))
                {
                    var ext = Path.GetExtension(file);
                    if (!ext.Equals(".vst3", StringComparison.OrdinalIgnoreCase) &&
                        !ext.Equals(".dll", StringComparison.OrdinalIgnoreCase))
                    {
                        continue;
                    }

                    var type = ext.Equals(".vst3", StringComparison.OrdinalIgnoreCase) ? "VST3" : "VST2";

                    if (TryGetIs64Bit(file, out var is64Bit, out var error))
                    {
                        pluginInfoList.Add(new PluginInfo
                        {
                            Name = Path.GetFileNameWithoutExtension(file),
                            Path = file,
                            Type = type,
                            Is64Bit = is64Bit,
                            IsValid = true,
                            ErrorMessage = null
                        });
                    }
                    else
                    {
                        pluginInfoList.Add(new PluginInfo
                        {
                            Name = Path.GetFileNameWithoutExtension(file),
                            Path = file,
                            Type = type,
                            Is64Bit = false,
                            IsValid = false,
                            ErrorMessage = error
                        });
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Skip directories without access and continue scanning others
            }
            catch (IOException)
            {
                // Skip directories with IO issues and continue scanning others
            }
        }

        var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        var outputPath = Path.Combine(desktopPath, "VST_Plugin_List.json");

        var json = JsonSerializer.Serialize(pluginInfoList, JsonOptions);
        File.WriteAllText(outputPath, json);

        Console.WriteLine($"Plugin metadata saved to: {outputPath}");
    }

    static bool TryGetIs64Bit(string filePath, out bool is64Bit, out string? error)
    {
        is64Bit = false;
        error = null;

        try
        {
            using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using var reader = new BinaryReader(stream);

            // Validate DOS header ('MZ')
            if (reader.ReadUInt16() != 0x5A4D)
            {
                error = "Invalid MZ header.";
                return false;
            }

            stream.Seek(0x3C, SeekOrigin.Begin);
            int peOffset = reader.ReadInt32();

            // Validate PE signature ('PE\0\0')
            stream.Seek(peOffset, SeekOrigin.Begin);
            if (reader.ReadUInt32() != 0x00004550)
            {
                error = "Invalid PE signature.";
                return false;
            }

            // Machine field follows PE signature
            ushort machine = reader.ReadUInt16();

            // 64-bit architectures
            is64Bit = machine == 0x8664 /* AMD64 */ || machine == 0xAA64 /* ARM64 */;
            return true;
        }
        catch (Exception ex)
        {
            error = ex.Message;
            return false;
        }
    }
}