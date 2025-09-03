using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSTAnalysis
{
    internal class PluginInfo
    {
        public string? Name { get; set; }
        public string? Version { get; set; }
        public string? Vendor { get; set; }
        public string? Path { get; set; }
        public string? Type { get; set; } // e.g., VST2, VST3, AU
        public bool Is64Bit { get; set; }
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }
        public override string ToString()
        {
            return $"{Name} ({Version}) by {Vendor} - {Type} - {(Is64Bit ? "64-bit" : "32-bit")} - {(IsValid ? "Valid" : "Invalid")}" + (string.IsNullOrEmpty(ErrorMessage) ? "" : $" - Error: {ErrorMessage}");
        }
    }
}
