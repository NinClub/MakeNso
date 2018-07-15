// Decompiled with JetBrains decompiler
// Type: MakeNso.Program
// Assembly: MakeNso, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50566761-79DF-45F0-8201-DBE7206E943C
// Assembly location: E:\MakeNso\MakeNso.exe

using MakeNso.Elf;
using MakeNso.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;

namespace MakeNso
{
  internal class Program
  {
    private static void Main(string[] args)
    {
      if (Directory.Exists(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "en")))
        Thread.CurrentThread.CurrentUICulture = new CultureInfo("en", false);
      MakeNsoArgs makeNsoArgs = new MakeNsoArgs();
      try
      {
        if (!makeNsoArgs.ParseArgs(args))
          return;
      }
      catch
      {
        Environment.ExitCode = 1;
        return;
      }
      try
      {
        bool flag = true;
        MakeNsoParams makeNsoParams = makeNsoArgs.Params;
        NsoFile nsoFile = new NsoFile();
        if (makeNsoParams.Compress)
          flag = true;
        if (makeNsoParams.NoCompress)
          flag = false;
        nsoFile.CompressMode = flag;
        nsoFile.SetModuleName(makeNsoParams.ModuleName);
        ElfInfo elf = new ElfInfo(makeNsoParams.DsoFileName);
        foreach (ElfSectionInfo sectionInfo in (IEnumerable<ElfSectionInfo>) elf.SectionInfos)
        {
          if (elf.FileType == ElfFileType.SharedObjectFile && sectionInfo.AddressAlign > 4096UL)
            throw new ArgumentException(string.Format(Resources.Message_InvalidSectionAlign, (object) sectionInfo.SectionName, (object) sectionInfo.AddressAlign));
        }
        byte[] buildId = BuildId.GetBuildId(elf);
        if (buildId != null)
          nsoFile.SetModuleId(buildId);
        elf.GetExElfSegmentInfo();
        ElfSegmentInfo roElfSegmentInfo = elf.GetRoElfSegmentInfo();
        elf.GetRwElfSegmentInfo();
        elf.GetZiElfSegmentInfo();
        nsoFile.SetTextSegment(elf.GetExElfSegmentInfo());
        nsoFile.SetRoSegment(elf.GetRoElfSegmentInfo());
        nsoFile.SetDataSegment(elf.GetRwElfSegmentInfo());
        nsoFile.SetBssSegment(elf.GetZiElfSegmentInfo());
        ElfSectionInfo infoElfSectionInfo = elf.GetApiInfoElfSectionInfo();
        if (infoElfSectionInfo != null)
          nsoFile.SetApiInfo(infoElfSectionInfo.VirtualAddress - roElfSegmentInfo.VirtualAddress, infoElfSectionInfo.SectionSize);
        nsoFile.CompressTextSegment();
        nsoFile.CompressRoSegment();
        nsoFile.CompressDataSegment();
        nsoFile.CalcPosition();
        using (FileStream fs = new FileStream(makeNsoParams.NsoFileName, FileMode.Create, FileAccess.Write))
          nsoFile.WriteData(fs);
        if (!makeNsoParams.VerboseMode)
          return;
        nsoFile.PrintNsoHeader();
      }
      catch (Exception ex)
      {
        Console.Error.WriteLine(string.Format("MakeNso INPUT='{0}', OUTPUT='{1}'", (object) makeNsoArgs.Params.DsoFileName, (object) makeNsoArgs.Params.NsoFileName));
        Console.Error.WriteLine(ex.Message);
        Environment.ExitCode = 1;
      }
    }
  }
}
