namespace MiniWeb.Core.Abstractions
{
    public interface IRequestData
    {
        T GetData<T>(string key);
        void SetData<T>(string key, T data);
        bool TryGetData<T>(string key, out T data);
    }
}
