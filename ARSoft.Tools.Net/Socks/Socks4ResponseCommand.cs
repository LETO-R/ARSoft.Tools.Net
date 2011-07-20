namespace ARSoft.Tools.Net.Socks
{
    public enum Socks4ResponseCommand : byte
    {
        None = 0,
        Granted = 90,
        Rejected = 91,
        IdentdOffline = 92,
        IdentRejected = 93,
        MinValue = Granted,
        MaxValue = IdentRejected
    }
}
