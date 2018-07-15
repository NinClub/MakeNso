// Decompiled with JetBrains decompiler
// Type: MakeNso.Properties.Resources
// Assembly: MakeNso, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 50566761-79DF-45F0-8201-DBE7206E943C
// Assembly location: E:\MakeNso\MakeNso.exe

using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace MakeNso.Properties
{
  [GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
  [DebuggerNonUserCode]
  [CompilerGenerated]
  internal class Resources
  {
    private static ResourceManager resourceMan;
    private static CultureInfo resourceCulture;

    internal Resources()
    {
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static ResourceManager ResourceManager
    {
      get
      {
        if (MakeNso.Properties.Resources.resourceMan == null)
          MakeNso.Properties.Resources.resourceMan = new ResourceManager("MakeNso.Properties.Resources", typeof (MakeNso.Properties.Resources).Assembly);
        return MakeNso.Properties.Resources.resourceMan;
      }
    }

    [EditorBrowsable(EditorBrowsableState.Advanced)]
    internal static CultureInfo Culture
    {
      get
      {
        return MakeNso.Properties.Resources.resourceCulture;
      }
      set
      {
        MakeNso.Properties.Resources.resourceCulture = value;
      }
    }

    internal static string Message_InvalidBssSegmentAlign
    {
      get
      {
        return MakeNso.Properties.Resources.ResourceManager.GetString(nameof (Message_InvalidBssSegmentAlign), MakeNso.Properties.Resources.resourceCulture);
      }
    }

    internal static string Message_InvalidBssSegments
    {
      get
      {
        return MakeNso.Properties.Resources.ResourceManager.GetString(nameof (Message_InvalidBssSegments), MakeNso.Properties.Resources.resourceCulture);
      }
    }

    internal static string Message_InvalidSectionAlign
    {
      get
      {
        return MakeNso.Properties.Resources.ResourceManager.GetString(nameof (Message_InvalidSectionAlign), MakeNso.Properties.Resources.resourceCulture);
      }
    }

    internal static string Message_InvalidSegmentAlign
    {
      get
      {
        return MakeNso.Properties.Resources.ResourceManager.GetString(nameof (Message_InvalidSegmentAlign), MakeNso.Properties.Resources.resourceCulture);
      }
    }

    internal static string Message_InvalidSegmentSize
    {
      get
      {
        return MakeNso.Properties.Resources.ResourceManager.GetString(nameof (Message_InvalidSegmentSize), MakeNso.Properties.Resources.resourceCulture);
      }
    }

    internal static string Message_InvalidTextAddress
    {
      get
      {
        return MakeNso.Properties.Resources.ResourceManager.GetString(nameof (Message_InvalidTextAddress), MakeNso.Properties.Resources.resourceCulture);
      }
    }
  }
}
