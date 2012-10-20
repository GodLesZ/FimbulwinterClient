using System.Net;

namespace FimbulwinterClient.Network.Packets.Account {
	public struct CharServerInfo
	{
		public IPAddress IP { get; set; }
		public int Port { get; set; }
		public string Name { get; set; }
		public int Users { get; set; }
		public byte Type { get; set; }
		public byte New { get; set; }

		public override string ToString()
		{
			return Name + " (" + Users + " Player)";
		}
	}
}