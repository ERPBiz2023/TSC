using System;
using System.Reflection;


namespace eDSC
{
    
    public class AssemblyInfo
    {

        private static Assembly assembly = Assembly.GetExecutingAssembly();

        public static string appName = Assembly.GetExecutingAssembly().GetName().Name;
        public static string appPath = AppDomain.CurrentDomain.BaseDirectory;

        public static string appCompany = ((AssemblyCompanyAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyCompanyAttribute), false)).Company;
        public static string appDescription = ((AssemblyDescriptionAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyDescriptionAttribute), false)).Description;
        public static string appTitle = ((AssemblyTitleAttribute)Attribute.GetCustomAttribute(assembly, typeof(AssemblyTitleAttribute), false)).Title;
        
        public static string appVersion = Assembly.GetExecutingAssembly().GetName().Version.Major.ToString() + "." +
                                      Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString() + "." +
                                      Assembly.GetExecutingAssembly().GetName().Version.Build.ToString() + "." +
                                      Assembly.GetExecutingAssembly().GetName().Version.Revision.ToString();

    }
}