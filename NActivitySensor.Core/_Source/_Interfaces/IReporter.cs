using NActivitySensor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NActivitySensor
{
    public interface IReporter
    {
        void Report(Report reportModel);
    }
}
