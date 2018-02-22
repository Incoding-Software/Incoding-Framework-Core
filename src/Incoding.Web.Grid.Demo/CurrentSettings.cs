using System;
using System.Reflection;

namespace Incoding.Web.Grid.Demo
{
    public static class CurrentSettings
    {
        public static string CurrentVersion
        {
            get
            {
#if DEBUG
                return Guid.NewGuid().ToString();

#else
                return Assembly.GetExecutingAssembly()
                        .GetName()
                        .Version.ToString();
#endif
            }
        } 
    }
}