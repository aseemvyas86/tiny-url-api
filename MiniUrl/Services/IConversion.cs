namespace MiniUrl.Services
{
    public interface IConversion
    {
        string Encode(ulong value);
        ulong Decode(string value);
    }
}
