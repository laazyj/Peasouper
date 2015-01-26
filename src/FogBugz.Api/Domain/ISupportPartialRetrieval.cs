namespace FogBugz.Api.Domain
{
    /// <summary>
    /// Objects should implement this interface if it is possible for them to not have all of their data loaded from the server.
    /// </summary>
    public interface ISupportPartialRetrieval
    {
        //bool IsFullyFetched { get; }
        //void RetrieveAll();
    }
}
