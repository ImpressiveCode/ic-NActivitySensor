﻿namespace NActivitySensor.Models
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Proj")]
    public class BuildProjConfigContent
    {
        #region Properties
        public string Project
        {
            get;
            set;
        }

        public string ProjectConfig
        {
            get;
            set;
        }

        public string Platform
        {
            get;
            set;
        }

        public string SolutionConfig
        {
            get;
            set;
        }

        public bool Success
        {
            get;
            set;
        }
        #endregion
    }
}
