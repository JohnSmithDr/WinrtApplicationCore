namespace JohnSmithDr.ApplicationCore
{
    public interface ILocalizedStrings
    {
        string GetString(string key);

        string GetFormatString(string key, params object[] args);

        string GetState<TEnum>(TEnum value);
    }
}
