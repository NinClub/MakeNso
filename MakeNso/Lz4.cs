// Decompiled with JetBrains decompiler
// Type: MakeNso.Lz4
// Assembly: MakeNso, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50566761-79DF-45F0-8201-DBE7206E943C
// Assembly location: E:\MakeNso\MakeNso.exe

using System;
using System.Runtime.InteropServices;

namespace MakeNso
{
  internal static class Lz4
  {
    public static int LZ4_compress_default(byte[] source, byte[] dest, int sourceSize, int maxDestSize)
    {
      using (new Lz4.ScopedGCHandle((object) source, GCHandleType.Pinned))
      {
        using (new Lz4.ScopedGCHandle((object) dest, GCHandleType.Pinned))
          return Lz4.LZ4_compress_default(Marshal.UnsafeAddrOfPinnedArrayElement((Array) source, 0), Marshal.UnsafeAddrOfPinnedArrayElement((Array) dest, 0), sourceSize, maxDestSize);
      }
    }

    public static int LZ4_decompress_safe(byte[] source, int sourceOffset, byte[] dest, int destOffset, int compressedSize, int maxDecompressedSize)
    {
      using (new Lz4.ScopedGCHandle((object) source, GCHandleType.Pinned))
      {
        using (new Lz4.ScopedGCHandle((object) dest, GCHandleType.Pinned))
          return Lz4.LZ4_decompress_safe(Marshal.UnsafeAddrOfPinnedArrayElement((Array) source, sourceOffset), Marshal.UnsafeAddrOfPinnedArrayElement((Array) dest, destOffset), compressedSize, maxDecompressedSize);
      }
    }

    [DllImport("MakeNso.lz4.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern int LZ4_compress_default(IntPtr source, IntPtr dest, int sourceSize, int maxDestSize);

    [DllImport("MakeNso.lz4.dll", CallingConvention = CallingConvention.Cdecl)]
    private static extern int LZ4_decompress_safe(IntPtr source, IntPtr dest, int compressedSize, int maxDecompressedSize);

    [DllImport("MakeNso.lz4.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern int LZ4_compressBound(int inputSize);

    private class ScopedGCHandle : IDisposable
    {
      private bool disposed;
      private GCHandle gchandle;

      public ScopedGCHandle(object value)
      {
        this.gchandle = GCHandle.Alloc(value);
      }

      public ScopedGCHandle(object value, GCHandleType type)
      {
        this.gchandle = GCHandle.Alloc(value, type);
      }

      ~ScopedGCHandle()
      {
        this.Dispose(false);
      }

      public static implicit operator IntPtr(Lz4.ScopedGCHandle scopedGCHandle)
      {
        return (IntPtr) scopedGCHandle;
      }

      public void Dispose()
      {
        this.Dispose(true);
        GC.SuppressFinalize((object) this);
      }

      protected virtual void Dispose(bool disposing)
      {
        if (this.disposed)
          return;
        int num = disposing ? 1 : 0;
        this.gchandle.Free();
        this.disposed = true;
      }
    }
  }
}
