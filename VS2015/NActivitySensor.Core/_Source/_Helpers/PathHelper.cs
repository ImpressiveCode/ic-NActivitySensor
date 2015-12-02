namespace NActivitySensor
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    #endregion

    public static class PathHelper
    {
        #region Methods
        public static bool TryGetFileName(string path, out string output)
        {
            if (File.Exists(path))
            {
                output = Path.GetFileName(path);
                return true;
            }

            output = path;
            return false;
        }
        #endregion
    }
}
