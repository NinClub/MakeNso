// Decompiled with JetBrains decompiler
// Type: MakeNso.MakeNsoArgs
// Assembly: MakeNso, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50566761-79DF-45F0-8201-DBE7206E943C
// Assembly location: E:\MakeNso\MakeNso.exe

using Nintendo.Foundation.IO;
using System;
using System.Collections.Generic;
using System.IO;

namespace MakeNso
{
  internal class MakeNsoArgs
  {
    private MakeNsoParams parameters;

    public MakeNsoParams Params
    {
      get
      {
        return this.parameters;
      }
    }

    public bool ParseArgs(string[] args)
    {
      if (!new CommandLineParser(new CommandLineParserSettings()
      {
        ApplicationDescription = "MakeNso converts DSO file to NSO file.",
        HelpWriter = (Action<string>) (text => Console.WriteLine(text))
      }).ParseArgs<MakeNsoParams>((IEnumerable<string>) args, out this.parameters))
        return false;
      if (this.parameters.ModuleName == null)
      {
        this.parameters.ModuleName = Path.GetFileNameWithoutExtension(this.parameters.NsoFileName);
        Console.WriteLine(this.parameters.NsoFileName);
      }
      return !this.parameters.Compress || !this.parameters.NoCompress;
    }
  }
}
