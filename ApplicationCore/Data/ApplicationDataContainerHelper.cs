using Windows.Storage;

namespace JohnSmithDr.ApplicationCore.Data
{
    public static class ApplicationDataContainerHelper
    {
        public static void Load<TRef>(
            this ApplicationDataContainer container,
            ref TRef localValue,
            string settingKey)
        {
            if (container.Values.ContainsKey(settingKey))
            {
                localValue = (TRef)container.Values[settingKey];
            }
        }

        public static void Save<TRef>(
            this ApplicationDataContainer container,
            TRef localValue,
            string settingKey)
        {
            container.Values[settingKey] = localValue;
        }
    }
}
