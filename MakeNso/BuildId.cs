// Decompiled with JetBrains decompiler
// Type: MakeNso.BuildId
// Assembly: MakeNso, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50566761-79DF-45F0-8201-DBE7206E943C
// Assembly location: E:\MakeNso\MakeNso.exe

using MakeNso.Elf;
using System.Collections.Generic;

namespace MakeNso
{
  internal static class BuildId
  {
    internal static byte[] GetBuildId(ElfInfo elf)
    {
      foreach (ElfSectionInfo sectionInfo in (IEnumerable<ElfSectionInfo>) elf.SectionInfos)
      {
        if (sectionInfo.SectionType == ElfSectionType.Note)
        {
          ElfNoteSectionInfo info = new ElfNoteSectionInfo(sectionInfo);
          if (ElfGnuNoteSection.IsElfGnuNoteSection(info) && ElfGnuNoteSection.GetGnuNoteSectionType(info) == ElfGnuNoteSectionType.BuildId)
            return info.Desc;
        }
      }
      return (byte[]) null;
    }
  }
}
