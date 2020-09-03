namespace OAuth2Example.DataServices
{
    public interface IValidationDictionary
    {
        void AddError(string key, string value);
        void Clear();

        bool IsValid { get; }
    }
}
