// Decompiled with JetBrains decompiler
// Type: MakeNso.NsoFile
// Assembly: MakeNso, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50566761-79DF-45F0-8201-DBE7206E943C
// Assembly location: E:\MakeNso\MakeNso.exe

using MakeNso.Elf;
using MakeNso.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace MakeNso
{
  internal class NsoFile
  {
    private NsoHeader header;
    private uint headerSize;
    private string moduleName;
    private byte[] textBinary;
    private byte[] roBinary;
    private byte[] dataBinary;
    private uint bssMemoryOffset;

    public NsoFile()
    {
      this.header = new NsoHeader();
      this.header.Signature = new byte[4];
      this.header.Signature[0] = (byte) 78;
      this.header.Signature[1] = (byte) 83;
      this.header.Signature[2] = (byte) 79;
      this.header.Signature[3] = (byte) 48;
      this.header.ModuleId = new byte[32];
      this.header.Reserved2 = new byte[52];
      this.header.TextHash = new byte[32];
      this.header.RoHash = new byte[32];
      this.header.DataHash = new byte[32];
      this.headerSize = (uint) Marshal.SizeOf(typeof (NsoHeader));
      this.header.Flags = 0U;
    }

    public void SetModuleName(string moduleName)
    {
      this.moduleName = moduleName;
      this.header.ModuleNameSize = (uint) (moduleName.Length + 1);
    }

    public void SetModuleId(byte[] moduleId)
    {
      moduleId.CopyTo((Array) this.header.ModuleId, 0);
    }

    public void SetApiInfo(ulong offset, ulong size)
    {
      this.header.EmbededOffset = (uint) offset;
      this.header.EmbededSize = (uint) size;
    }

    public bool CompressMode { get; set; }

    private byte[] Compress(byte[] srcData, int bufferSize)
    {
      if (!this.CompressMode)
        throw new Exception();
      int length = srcData.Length;
      int maxDestSize = Lz4.LZ4_compressBound(length);
      if (maxDestSize <= 0)
        throw new Exception();
      byte[] array1 = new byte[maxDestSize];
      int num = Lz4.LZ4_compress_default(srcData, array1, length, maxDestSize);
      if (0 >= num || num >= length)
        throw new Exception();
      Array.Resize<byte>(ref array1, num);
      byte[] array2 = new byte[bufferSize];
      Array.Copy((Array) array1, 0, (Array) array2, bufferSize - num, num);
      if (Lz4.LZ4_decompress_safe(array2, bufferSize - num, array2, 0, num, length) != length)
        throw new Exception();
      Array.Resize<byte>(ref array2, length);
      if (!((IEnumerable<byte>) array2).SequenceEqual<byte>((IEnumerable<byte>) srcData))
        throw new Exception();
      return array1;
    }

    public void SetTextSegment(ElfSegmentInfo info)
    {
      if (info == null)
        return;
      if (info.VirtualAddress != 0UL)
        throw new ArgumentException(string.Format(Resources.Message_InvalidTextAddress, (object) info.VirtualAddress));
      if (info.MemorySize > (ulong) int.MaxValue)
        throw new ArgumentException(string.Format(Resources.Message_InvalidSegmentSize, (object) "Text", (object) info.MemorySize));
      this.header.TextMemoryOffset = (uint) info.VirtualAddress;
      this.header.TextSize = (uint) info.MemorySize;
      this.textBinary = info.GetContents();
      this.header.TextFileSize = (uint) this.textBinary.Length;
    }

    public void SetRoSegment(ElfSegmentInfo info)
    {
      if (info == null)
        return;
      if ((info.VirtualAddress & 4095UL) > 0UL)
        throw new ArgumentException(string.Format(Resources.Message_InvalidSegmentAlign, (object) "RO", (object) info.VirtualAddress));
      if (info.MemorySize > (ulong) int.MaxValue)
        throw new ArgumentException(string.Format(Resources.Message_InvalidSegmentSize, (object) "RO", (object) info.MemorySize));
      this.header.RoMemoryOffset = (uint) info.VirtualAddress;
      this.header.RoSize = (uint) info.MemorySize;
      this.roBinary = info.GetContents();
      this.header.RoFileSize = (uint) this.roBinary.Length;
    }

    public void SetDataSegment(ElfSegmentInfo info)
    {
      if (info == null)
        return;
      if ((info.VirtualAddress & 4095UL) > 0UL)
        throw new ArgumentException(string.Format(Resources.Message_InvalidSegmentAlign, (object) "Data", (object) info.VirtualAddress));
      if (info.MemorySize > (ulong) int.MaxValue)
        throw new ArgumentException(string.Format(Resources.Message_InvalidSegmentSize, (object) "Data", (object) info.MemorySize));
      this.header.DataMemoryOffset = (uint) info.VirtualAddress;
      this.header.DataSize = (uint) info.MemorySize;
      this.dataBinary = info.GetContents();
      this.header.DataFileSize = (uint) this.dataBinary.Length;
    }

    public void SetBssSegment(ElfSegmentInfo info)
    {
      if (info == null)
        return;
      if ((info.VirtualAddress & 15UL) > 0UL)
        throw new ArgumentException(string.Format(Resources.Message_InvalidBssSegmentAlign, (object) info.VirtualAddress));
      this.header.BssSize = (uint) info.MemorySize;
      this.bssMemoryOffset = (uint) info.VirtualAddress;
      if (this.header.BssSize <= 0U)
        return;
      if (this.header.DataSize == 0U)
        this.header.RoMemoryOffset = this.bssMemoryOffset;
      if (this.bssMemoryOffset < this.header.DataMemoryOffset + this.header.DataSize)
        throw new ArgumentException(Resources.Message_InvalidBssSegments);
      uint num = this.header.DataMemoryOffset + this.header.DataSize;
      this.header.BssSize += num > 0U ? this.bssMemoryOffset - num : 0U;
    }

    public void CompressTextSegment()
    {
      try
      {
        byte[] textBinary = this.textBinary;
        long num = (long) (uint) ((int) this.header.DataMemoryOffset + (int) this.header.DataSize + (int) this.header.BssSize + 4095) & -4096L;
        byte[] numArray = this.Compress(textBinary, (int) (num - (long) this.header.TextMemoryOffset));
        new SHA256Managed().ComputeHash(textBinary).CopyTo((Array) this.header.TextHash, 0);
        this.textBinary = numArray;
        this.header.Flags |= 1U;
        this.header.Flags |= 8U;
        this.header.TextFileSize = (uint) this.textBinary.Length;
      }
      catch (Exception ex)
      {
      }
    }

    public void CompressRoSegment()
    {
      try
      {
        byte[] roBinary = this.roBinary;
        long num = (long) (uint) ((int) this.header.DataMemoryOffset + (int) this.header.DataSize + (int) this.header.BssSize + 4095) & -4096L;
        byte[] numArray = this.Compress(roBinary, (int) (num - (long) this.header.RoMemoryOffset));
        new SHA256Managed().ComputeHash(roBinary).CopyTo((Array) this.header.RoHash, 0);
        this.roBinary = numArray;
        this.header.Flags |= 2U;
        this.header.Flags |= 16U;
        this.header.RoFileSize = (uint) this.roBinary.Length;
      }
      catch (Exception ex)
      {
      }
    }

    public void CompressDataSegment()
    {
      try
      {
        byte[] dataBinary = this.dataBinary;
        long num = (long) (uint) ((int) this.header.DataMemoryOffset + (int) this.header.DataSize + (int) this.header.BssSize + 4095) & -4096L;
        byte[] numArray = this.Compress(dataBinary, (int) (num - (long) this.header.DataMemoryOffset));
        new SHA256Managed().ComputeHash(dataBinary).CopyTo((Array) this.header.DataHash, 0);
        this.dataBinary = numArray;
        this.header.Flags |= 4U;
        this.header.Flags |= 32U;
        this.header.DataFileSize = (uint) this.dataBinary.Length;
      }
      catch (Exception ex)
      {
      }
    }

    public void CalcPosition()
    {
      this.header.ModuleNameOffset = this.headerSize;
      this.header.TextFileOffset = this.header.ModuleNameOffset + this.header.ModuleNameSize;
      this.header.RoFileOffset = this.header.TextFileOffset + this.header.TextFileSize;
      this.header.DataFileOffset = this.header.RoFileOffset + this.header.RoFileSize;
    }

    public void WriteData(FileStream fs)
    {
      BinaryWriter binaryWriter = new BinaryWriter((Stream) fs);
      byte[] buffer = new byte[(int) this.headerSize];
      GCHandle gcHandle = GCHandle.Alloc((object) buffer, GCHandleType.Pinned);
      try
      {
        Marshal.StructureToPtr((object) this.header, gcHandle.AddrOfPinnedObject(), false);
      }
      finally
      {
        gcHandle.Free();
      }
      binaryWriter.Write(buffer);
      binaryWriter.Write(this.moduleName);
      if (this.header.TextSize > 0U)
        binaryWriter.Write(this.textBinary);
      if (this.header.RoSize > 0U)
        binaryWriter.Write(this.roBinary);
      if (this.header.DataSize <= 0U)
        return;
      binaryWriter.Write(this.dataBinary);
    }

    public void PrintNsoHeader()
    {
      Console.Write("signature: ");
      for (int index = 0; index < this.header.Signature.GetLength(0); ++index)
        Console.Write("{0}", (object) (char) this.header.Signature[index]);
      Console.WriteLine();
      Console.WriteLine("flags: 0x{0:X}", (object) this.header.Flags);
      Console.WriteLine("text file offset: 0x{0:X}", (object) this.header.TextFileOffset);
      Console.WriteLine("text mem offset: 0x{0:X}", (object) this.header.TextMemoryOffset);
      Console.WriteLine("text file size: 0x{0:X}", (object) this.header.TextSize);
      Console.WriteLine("module offset: 0x{0:X}", (object) this.header.ModuleNameOffset);
      Console.WriteLine("ro file offset: 0x{0:X}", (object) this.header.RoFileOffset);
      Console.WriteLine("ro mem offset: 0x{0:X}", (object) this.header.RoMemoryOffset);
      Console.WriteLine("ro file size: 0x{0:X}", (object) this.header.RoSize);
      Console.WriteLine("module file size: 0x{0:X}", (object) this.header.ModuleNameSize);
      Console.WriteLine("data file offset: 0x{0:X}", (object) this.header.DataFileOffset);
      Console.WriteLine("data mem offset: 0x{0:X}", (object) this.header.DataMemoryOffset);
      Console.WriteLine("data file size: 0x{0:X}", (object) this.header.DataSize);
      Console.WriteLine("bss file size: 0x{0:X}", (object) this.header.BssSize);
      Console.WriteLine("embeded offset: 0x{0:X}", (object) this.header.EmbededOffset);
      Console.WriteLine("embeded size: 0x{0:X}", (object) this.header.EmbededSize);
    }
  }
}
