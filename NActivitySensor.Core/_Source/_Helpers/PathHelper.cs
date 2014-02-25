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
        /// <summary>
        /// Gets the file name if path is valid or returns path from parameter
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string TryGetFileName(string path)
        {
            if (File.Exists(path))
            {
                return Path.GetFileName(path);
            }

            return path;
        }
        #endregion
    }
}
