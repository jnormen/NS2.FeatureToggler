using System;
using Microsoft.Extensions.Configuration;

namespace NS2.FeatureToggler
{
    public static class Feature
    {
        internal static IConfigurationRoot Config { set; get; }

        public static void SetConfiguration(IConfigurationRoot configurationRoot)
        {
            Config = configurationRoot;
        }
    }

    public static class Feature<T> where T : IFeature
    {
        internal static IConfigurationRoot Configuration => Feature.Config ?? GetConfiguration();

        public static State<T> Is()
        {
            var isOn = Configuration[typeof (T).Name];
            if(!string.IsNullOrEmpty(isOn))
            {
                bool isEnabled;
                if (bool.TryParse(isOn, out isEnabled))
                    return new State<T>(isEnabled);
            }
            else
            {
                return ReturnStateFromDynamicCalltoIsEnabled<T>();
                
            }
            return new State<T>(false);
        }

        private static State<T> ReturnStateFromDynamicCalltoIsEnabled<T>() where T : IFeature
        {
            try
            {
                var isEnabled = RunIsEnabled(Activator.CreateInstance<T>());
                return new State<T>(isEnabled);
            }
            catch (Exception) //Has no IsEnabled or cannot create feature class
            {
                return new State<T>(false);
            }
        }

        private static bool RunIsEnabled(dynamic featureObject)
        {
            return featureObject.IsEnabled();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            try
            {
                var builder = new ConfigurationBuilder()
                    .AddJsonFile("featuresettings.json");

                return builder.Build().ReloadOnChanged("featuresettings.json");
            }
            catch (Exception)
            {
                return new ConfigurationBuilder().Build();
            }
        }
    }
}
