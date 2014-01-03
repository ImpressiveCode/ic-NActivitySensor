using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NActivitySensor.Models
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Proj")]
    public class BuildProjConfigContent
    {
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
    }
}
