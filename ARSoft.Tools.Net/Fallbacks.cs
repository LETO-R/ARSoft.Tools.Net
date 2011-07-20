using System;
using System.Net;
namespace ARSoft.Tools.Net
{
	/// <summary>
	/// Fallback defaults.
	/// </summary>
	internal static class Fallbacks
	{
		/// <summary>
		/// Primary DNS Fallback
		/// <para>
		/// Used when the GetDnsServers method (located in DnsClient/DnsSocksClient) is unable to get the system DNS-servers.
		/// </para>
		/// </summary>
		/// <remarks>8.8.8.8 is the primary public Google DNS server.</remarks>
		public static readonly IPAddress PrimaryDNS = IPAddress.Parse("8.8.8.8");
		
		/// <summary>
		/// Secondary DNS Fallback
		/// <para>
		/// Used when the GetDnsServers method (located in DnsClient/DnsSocksClient) is unable to get the system DNS-servers.
		/// </para>
		/// </summary>
		/// <remarks>8.8.4.4 is the secondary public Google DNS server.</remarks>
		public static readonly IPAddress SecondaryDNS = IPAddress.Parse("8.8.4.4");
	}
}

