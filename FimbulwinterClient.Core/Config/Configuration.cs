using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Xml.Serialization;
using FimbulwinterClient.Core.Content;

namespace FimbulwinterClient.Core.Config {

	[Serializable]
	public class Configuration {
		public static string DefaultPath = @"data\config\client.xml";

		private float mBgmVolume;
		private float mEffectVolume;

		public float BgmVolume {
			get { return mBgmVolume; }
			set {
				if (BgmVolume.Equals(value) == false && BgmVolumeChanged != null) {
					BgmVolumeChanged(value);
				}
				mBgmVolume = value;
			}
		}

		public float EffectVolume {
			get { return mEffectVolume; }
			set {
				if (EffectVolume.Equals(value) == false && EffectVolumeChanged != null) {
					EffectVolumeChanged(value);
				}
				mEffectVolume = value;
			}
		}

		public int ScreenWidth {
			get;
			set;
		}

		public int ScreenHeight {
			get;
			set;
		}

		public string LastLogin {
			get;
			set;
		}

		public bool SaveLast {
			get;
			set;
		}

		[XmlArray(ElementName = "GrfFiles")]
		[XmlArrayItem(ElementName = "Grf")]
		public ObservableCollection<string> GrfFiles {
			get;
			set;
		}

		public ServersInfo ServersInfo {
			get;
			set;
		}


		public event Action<float> BgmVolumeChanged;
		public event Action<float> EffectVolumeChanged;


		public Configuration() {
			mBgmVolume = 1.0f;
			mEffectVolume = 1.0f;

			ScreenWidth = 1280;
			ScreenHeight = 768;

			SaveLast = false;
			LastLogin = "";

			ServersInfo = new ServersInfo();

			GrfFiles = new ObservableCollection<string>();
			GrfFiles.CollectionChanged += GrfFilesOnCollectionChanged;
		}


		private void GrfFilesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e) {
			if (e.Action != NotifyCollectionChangedAction.Add) {
				return;
			}

			foreach (var newItem in e.NewItems) {
				GrfFileSystem.AddGrf((string)newItem);
			}
		}


		public static Configuration FromStream(Stream s) {
			var xs = new XmlSerializer(typeof(Configuration));
			return (Configuration)xs.Deserialize(s);
		}


		public void Save() {
			var xs = new XmlSerializer(typeof(Configuration));
			xs.Serialize(new FileStream(DefaultPath, FileMode.Create), this);
		}

	}

}
