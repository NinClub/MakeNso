// Decompiled with JetBrains decompiler
// Type: MakeNso.MakeNsoParams
// Assembly: MakeNso, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50566761-79DF-45F0-8201-DBE7206E943C
// Assembly location: E:\MakeNso\MakeNso.exe

using Nintendo.Foundation.IO;

namespace MakeNso
{
  internal class MakeNsoParams
  {
    [CommandLineOption("module-name", DefaultValue = "", Description = "Set module name for the NSO file.")]
    public string ModuleName { get; set; }

    [CommandLineOption("verbose", DefaultValue = false, Description = "Show log")]
    public bool VerboseMode { get; set; }

    [CommandLineOption("compress", DefaultValue = false, Description = "Compress the contents")]
    public bool Compress { get; set; }

    [CommandLineOption("nocompress", DefaultValue = false, Description = "Not compress the contents")]
    public bool NoCompress { get; set; }

    [CommandLineValue(0, Description = "Input filename.", ValueName = "INPUT_FILE")]
    public string DsoFileName { get; set; }

    [CommandLineValue(1, Description = "Output filename.", ValueName = "OUTPUT_FILE")]
    public string NsoFileName { get; set; }
  }
}
