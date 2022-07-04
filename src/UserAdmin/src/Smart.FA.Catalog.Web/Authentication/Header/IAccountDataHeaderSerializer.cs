namespace Smart.FA.Catalog.Web.Authentication.Header;

public interface IAccountDataHeaderSerializer
{
    /// <summary>
    /// Creates a fake AccountData serialized in a json string.
    /// </summary>
    /// <returns>A json string with serialized AccountData</returns>
    string CreateFakeAccountDataHeader();

    /// <summary>
    /// Deserializes a json string into an AccountData object.
    /// </summary>
    /// <param name="serializedCustomData">The json string of an AccountData object (from the customData header)</param>
    /// <returns>An AccountData object</returns>
    AccountData? Deserialize(string serializedCustomData);
}
