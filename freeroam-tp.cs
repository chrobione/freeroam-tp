using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using GTANetworkServer;
using GTANetworkShared;

namespace FreeroamTeleport
{
	[Serializable]
	public class TeleportLocation
	{
		public string Name { get; set; }
		public Vector3 Position { get; set; }
	}

	public class FreeroamTeleportScript : Script
	{
		public List<TeleportLocation> m_teleportLocations = new List<TeleportLocation>();

		public FreeroamTeleportScript()
		{
			API.onResourceStart += API_onResourceStart;
			API.onResourceStop += API_onResourceStop;
		}

		private void API_onResourceStart()
		{
			LoadLocations();
		}

		private void API_onResourceStop()
		{
			SaveLocations();
		}

		private void LoadLocations()
		{
			if (!File.Exists("freeroam-tp.xml")) {
				return;
			}

			var serializer = new XmlSerializer(m_teleportLocations.GetType());
			using (var reader = File.OpenRead("freeroam-tp.xml")) {
				m_teleportLocations = (List<TeleportLocation>)serializer.Deserialize(reader);
			}
		}

		private void SaveLocations()
		{
			var serializer = new XmlSerializer(m_teleportLocations.GetType());
			using (var writer = File.OpenWrite("freeroam-tp.xml")) {
				serializer.Serialize(writer, m_teleportLocations);
			}
		}

		private TeleportLocation GetLocation(string name)
		{
			return m_teleportLocations.Find((tl) => tl.Name == name);
		}

		[Command("tp")]
		public void TeleportCommand(Client sender, string name)
		{
			var tl = GetLocation(name);
			if (tl == null) {
				API.sendChatMessageToPlayer(sender, "~r~Location does not exist!");
				return;
			}
			sender.position = tl.Position;
			sender.sendChatMessage("~g~Teleported!");
		}

		[Command("tplist")]
		public void TeleportListCommand(Client sender)
		{
			if (m_teleportLocations.Count == 0) {
				API.sendChatMessageToPlayer(sender, "~r~No available locations.");
				return;
			}

			string ret = "";

			foreach (var tl in m_teleportLocations) {
				ret += tl.Name + ", ";
			}

			sender.sendChatMessage("~g~Locations: ~s~" + ret.Trim(',', ' '));
		}

		[Command("tpset", ACLRequired = true, Group = "Admin")]
		public void TeleportSetCommand(Client sender, string name)
		{
			var tl = GetLocation(name);
			if (tl == null) {
				tl = new TeleportLocation();
				tl.Name = name;
				m_teleportLocations.Add(tl);
			}
			tl.Position = sender.position;
			sender.sendChatMessage("~g~Location set!");
		}

		[Command("tprem", ACLRequired = true, Group = "Admin")]
		public void TeleportRemoveCommand(Client sender, string name)
		{
			int index = m_teleportLocations.FindIndex((tl) => tl.Name == name);
			if (index == -1) {
				API.sendChatMessageToPlayer(sender, "~r~Location does not exist!");
				return;
			}
			m_teleportLocations.RemoveAt(index);
			sender.sendChatMessage("~g~Location removed!");
		}

		[Command("tpload", ACLRequired = true, Group = "Admin")]
		public void TeleportLoadCommand(Client sender)
		{
			LoadLocations();
			sender.sendChatMessage("~g~Locations loaded!");
		}

		[Command("tpsave", ACLRequired = true, Group = "Admin")]
		public void TeleportSaveCommand(Client sender)
		{
			SaveLocations();
			sender.sendChatMessage("~g~Locations saved!");
		}
	}
}
