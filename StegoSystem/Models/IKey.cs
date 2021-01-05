namespace StegoSystem.Models
{
    public interface IKey<T>
    {
        string GetKeyName { get; }
        T Payload { get; }
        bool IsValid();
        string ValidationDescription { get; }
    }
}
