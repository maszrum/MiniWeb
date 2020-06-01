namespace MiniWeb.Core.Abstractions
{
    public interface IWebEndpoint
    {
        IWebResponse Process(IWebRequest request);
    }
}
