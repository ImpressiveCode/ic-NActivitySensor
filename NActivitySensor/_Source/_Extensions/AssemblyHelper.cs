using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NActivitySensor
{
    public static class AssemblyHelper
    {
        public static string GetCurrentAssemblyDirectory(this Assembly assembly)
        {
            string CodeBase = assembly.CodeBase;
            UriBuilder uri = new UriBuilder(CodeBase);
            string CurrentPath = Uri.UnescapeDataString(uri.Path);
            
            return Path.GetDirectoryName(CurrentPath);
        }
    }
}
