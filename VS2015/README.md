ic-NActivitySensor
==================

NActivitySensor (ActivitySensor for .NET) with functionality derived from ActivitySensor and SmartSensor Eclipse plugins. For more information, see the following papers or books: 1) http://madeyski.e-informatyka.pl/download/Madeyski13ENASE.pdf  2) http://www.springer.com/978-3-642-04287-4  3) http://madeyski.e-informatyka.pl/download/Madeyski10c.pdf (or http://dx.doi.org/10.1016/j.infsof.2009.08.007)  4) http://madeyski.e-informatyka.pl/download/Madeyski07d.pdf (or http://dx.doi.org/10.1007/978-3-540-75381-0_18)



## Starting the experimental instance with an extension (for testing purposes)

1. To start a project as a new instance of Visual Studio with NActivitySensor on it you have to 
  1. set startup project to NActivitySensor
  2. open project's properties, under the `Debug` section set `Start Action` to be `Start external program` and set it to be something like `C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\devenv.exe` - depends on your VisualStudio location
  3. add `Command line arguments` below: `/rootsuffix Exp`
  