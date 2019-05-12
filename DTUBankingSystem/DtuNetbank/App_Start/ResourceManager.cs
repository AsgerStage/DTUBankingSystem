using System.Globalization;

namespace DtuNetbank
{
    public static class ResourceManager
    {
        private static readonly System.Resources.ResourceManager sysResources = new System.Resources.ResourceManager(typeof(DtuNetbank.Properties.Resources));
        public static string GetResourceString(string resourceName, CultureInfo culture)
        {
            return sysResources.GetString(resourceName, culture);
        }

        public static string GetResourceString(string resourceName)
        {
            return sysResources.GetString(resourceName);
        }

    }
}