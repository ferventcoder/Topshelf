namespace Shelving.Configuration
{
    using System;

    public class DefaultConfiguration :
        Config
    {
        public string ServicesDirectory
        {
            get { return ".\\services"; }
        }

        public RunAs HowToRun
        {
            get { return RunAs.Interactive; }
        }
    }
}