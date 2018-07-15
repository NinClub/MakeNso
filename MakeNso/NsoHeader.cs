// Decompiled with JetBrains decompiler
// Type: MakeNso.NsoHeader
// Assembly: MakeNso, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50566761-79DF-45F0-8201-DBE7206E943C
// Assembly location: E:\MakeNso\MakeNso.exe

using System.Runtime.InteropServices;

namespace MakeNso
{
  internal struct NsoHeader
  {
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] Signature;
    public uint Version;
    public uint Reserved1;
    public uint Flags;
    public uint TextFileOffset;
    public uint TextMemoryOffset;
    public uint TextSize;
    public uint ModuleNameOffset;
    public uint RoFileOffset;
    public uint RoMemoryOffset;
    public uint RoSize;
    public uint ModuleNameSize;
    public uint DataFileOffset;
    public uint DataMemoryOffset;
    public uint DataSize;
    public uint BssSize;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] ModuleId;
    public uint TextFileSize;
    public uint RoFileSize;
    public uint DataFileSize;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] Reserved2;
    public uint EmbededOffset;
    public uint EmbededSize;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
    public byte[] Reserved3;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] TextHash;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] RoHash;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public byte[] DataHash;
  }
}
