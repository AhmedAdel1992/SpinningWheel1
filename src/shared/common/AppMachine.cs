using System.Net;
using System.Net.Sockets;

namespace Common;

public class AppMachine
{
    public static string MachineName => Dns.GetHostName();
    
    public static string IpAddress => GetIpAddress();

    private static string GetIpAddress()
    {
        try
        {
            return Dns.GetHostByName(Dns.GetHostName())
                .AddressList
                .First(e => e.AddressFamily == AddressFamily.InterNetwork)
                .ToString();
        }
        catch (Exception e)
        {
            return string.Empty;
        }

    }

    public static int MachineSerialNumber => Math.Abs( Dns.GetHostName().GetHashCode());

    public string Name { get; set; }
    public string Ip { get; set; }
    public int Serial { get; set; }
    public static AppMachine GetMachine()
    {
        return new AppMachine()
        {
            Ip = IpAddress,
            Name = MachineName,
            Serial = MachineSerialNumber
        };
    }
}