﻿using System.IO;
using System.Net;

using Sandbox;
using Sandbox.Engine.Multiplayer;
using Sandbox.Game;
using Sandbox.Game.World;
using SteamSDK;

using Oxide.Core;
using Oxide.Core.Libraries.Covalence;
using Oxide.Core.Plugins;

namespace Oxide.Game.SpaceEngineers.Libraries.Covalence
{
    /// <summary>
    /// Represents the server hosting the game instance
    /// </summary>
    public class SpaceEngineersServer : IServer
    {
        #region Information

        /// <summary>
        /// Gets/sets the public-facing name of the server
        /// </summary>
        public string Name
        {
            get { return MySandboxGame.ConfigDedicated.ServerName; }
            set { MySandboxGame.ConfigDedicated.ServerName = value; }
        }

        /// <summary>
        /// Gets the public-facing IP address of the server, if known
        /// </summary>
        public IPAddress Address
        {
            get
            {
                var ip = SteamServerAPI.Instance.GameServer.GetPublicIP();
                return ip == 0 ? null : new IPAddress(ip >> 24 | ((ip & 0xff0000) >> 8) | ((ip & 0xff00) << 8) | ((ip & 0xff) << 24));
            }
        }

        /// <summary>
        /// Gets the public-facing network port of the server, if known
        /// </summary>
        public ushort Port => (ushort)MySandboxGame.ConfigDedicated.ServerPort;

        /// <summary>
        /// Gets the version or build number of the server
        /// </summary>
        public string Version => MyPerGameSettings.BasicGameInfo.GameVersion.ToString();

        /// <summary>
        /// Gets the network protocol version of the server
        /// </summary>
        public string Protocol => Version;

        /// <summary>
        /// Gets the total of players currently on the server
        /// </summary>
        public int Players => MyMultiplayer.Static.MemberCount;

        /// <summary>
        /// Gets/sets the maximum players allowed on the server
        /// </summary>
        public int MaxPlayers
        {
            get { return MyMultiplayer.Static.MemberLimit; }
            set { MyMultiplayer.Static.MemberLimit = value; }
        }

        #endregion

        #region Administration

        /// <summary>
        /// Saves the server and any related information
        /// </summary>
        public void Save() => MySession.Static.Save();

        #endregion

        #region Chat and Commands

        /// <summary>
        /// Broadcasts a chat message to all player clients
        /// </summary>
        /// <param name="message"></param>
        public void Broadcast(string message) => MyMultiplayer.Static.SendChatMessage(message);

        /// <summary>
        /// Runs the specified server command
        /// </summary>
        /// <param name="command"></param>
        /// <param name="args"></param>
        public void Command(string command, params object[] args)
        {
            // TODO
        }

        #endregion

        #region Logging

        /// <summary>
        /// Logs a string of text to a file
        /// </summary>
        /// <param name="text"></param>
        /// <param name="owner"></param>
        public void Log(string text, Plugin owner)
        {
            using (var writer = new StreamWriter(Path.Combine(Interface.Oxide.LogDirectory, Utility.CleanPath(owner.Filename + ".txt")), true))
                writer.WriteLine(text);
        }

        #endregion
    }
}
