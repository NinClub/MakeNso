// Decompiled with JetBrains decompiler
// Type: MakeNso.NsoHeaderFlags
// Assembly: MakeNso, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50566761-79DF-45F0-8201-DBE7206E943C
// Assembly location: E:\MakeNso\MakeNso.exe

namespace MakeNso
{
  internal enum NsoHeaderFlags
  {
    TextCompress = 1,
    RoCompress = 2,
    DataCompress = 4,
    TextHash = 8,
    RoHash = 16, // 0x00000010
    DataHash = 32, // 0x00000020
  }
}
