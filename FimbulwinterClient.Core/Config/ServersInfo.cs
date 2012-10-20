using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;

namespace FimbulwinterClient.Core.Config {

	[Serializable]
	public class ServersInfo {

		[XmlArray("Servers")]
		public List<ServerInfo> Servers {
			get;
			set;
		}

		public string ServiceType {
			get;
			set;
		}

		public string ServerType {
			get;
			set;
		}


		public ServersInfo() {
			Servers = new List<ServerInfo>();
			ServiceType = "";
			ServerType = "";
		}

		public static ServersInfo FromStream(Stream s) {
			if (s == null || s.CanRead == false) {
				throw new ArgumentException("Stream cant be null and must be readable", "s");
			}

			var xs = new XmlSerializer(typeof(ServersInfo));
			return (ServersInfo)xs.Deserialize(s);
		}

	}

}
