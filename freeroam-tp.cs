using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using GTANetworkServer;
using GTANetworkShared;

namespace FreeroamTeleport
{
	public class FreeroamTeleportScript : Script
	{
		public Dictionary<string, Vector3> m_teleportLocations = new Dictionary<string, Vector3>();

		[Command("tp")]
		public void TeleportCommand(Client sender, string name)
		{
			if (!m_teleportLocations.ContainsKey(name)) {
				API.sendChatMessageToPlayer(sender, "~r~Location does not exist!");
				return;
			}
			sender.position = m_teleportLocations[name];
			API.sendChatMessageToPlayer(sender, "~g~Teleported!");
		}

		[Command("tplist")]
		public void TeleportListCommand(Client sender)
		{
			if (m_teleportLocations.Count == 0) {
				API.sendChatMessageToPlayer(sender, "~r~No available locations.");
				return;
			}

			string ret = "";

			foreach(var key in m_teleportLocations.Keys) {
				ret += key + ", ";
			}

			API.sendChatMessageToPlayer(sender, "~g~Locations: ~s~" + ret.Trim(',', ' '));
		}

		[Command("tpset", ACLRequired = true, Group = "Admin")]
		public void TeleportSetCommand(Client sender, string name)
		{
			m_teleportLocations[name] = sender.position;
			API.sendChatMessageToPlayer(sender, "~g~Location set!");
		}

		[Command("tprem", ACLRequired = true, Group = "Admin")]
		public void TeleportRemoveCommand(Client sender, string name)
		{
			if (!m_teleportLocations.ContainsKey(name)) {
				API.sendChatMessageToPlayer(sender, "~r~Location does not exist!");
				return;
			}
			m_teleportLocations.Remove(name);
			API.sendChatMessageToPlayer(sender, "~g~Location removed!");
		}
	}
}
