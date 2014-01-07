namespace NActivitySensor.Models
{
    #region Usings
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    #endregion

    public class ProjectItemRenamedContent
    {
        #region Properties
        public ProjectItemInfoContent ProjectItem
        {
            get;
            set;
        }

        public string OldName
        {
            get;
            set;
        }
        #endregion
    }
}
