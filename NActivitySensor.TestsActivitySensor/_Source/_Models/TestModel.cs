﻿namespace NActivitySensor
{
    #region Methods
    using Microsoft.VisualStudio.TestWindow.Extensibility;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    #endregion

    public class TestModel
    {
        #region Properties
        public string DisplayName
        {
            get;
            set;
        }

        public string FilePath
        {
            get;
            set;
        }

        public double DurationInMilliseconds
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }

        public int LineNumber
        {
            get;
            set;
        }

        public Guid ProjectId
        {
            get;
            set;
        }

        public string Source
        {
            get;
            set;
        }

        public string TestState
        {
            get;
            set;
        }

        public IEnumerable<IResult> Results
        {
            get;
            set;
        }
        #endregion

        #region Constructors
        public TestModel()
        {

        }

        public TestModel(ITest test)
        {
            if (test == null)
            {
                throw new ArgumentNullException("test");
            }

            FilePath = test.FilePath;
            DurationInMilliseconds = test.Duration.TotalMilliseconds;
            DisplayName = test.DisplayName;
            Id = test.Id;
            LineNumber = test.LineNumber;
            ProjectId = test.ProjectId;
            Source = test.Source;
            TestState = test.State.ToString();
            // Results = test.Results;
        }
        #endregion
    }
}
