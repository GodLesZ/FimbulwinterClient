using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FimbulwinterClient.Core.Exports;
using FimbulwinterClient.Gui;
using FimbulwinterClient.Gui.System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using FimbulwinterClient.Network.Packets;
using FimbulwinterClient.Network.Packets.Character;
using FimbulwinterClient.Network.Packets.Zone;

namespace FimbulwinterClient.Screens
{
    public class CharSelectScreen : BaseLoginScreen
    {
        private CharSelectWindow window;
        private string _mapname;
        public CharSelectScreen()
        {
            window = new CharSelectWindow();
            window.OnCreateChar += new Action<int>(window_OnCreateChar);
            window.OnSelectChar += new Action<int>(window_OnSelectChar);

            RagnarokClient.Singleton.GuiManager.Controls.Add(window);
        }

        void window_OnSelectChar(int obj)
        {
            RagnarokClient.Singleton.NetworkState.SelectedChar = RagnarokClient.Singleton.NetworkState.CharAccept.Chars[obj];
            RagnarokClient.Singleton.CurrentConnection.PacketSerializer.PacketHooks[(int)PacketHeader.HEADER_HC_NOTIFY_ZONESVR] = new Action<ushort, int, HC_Notify_Zonesvr>(packet_notify_zonesrv);
            
            new CH_Select_Char(obj).Write(RagnarokClient.Singleton.CurrentConnection.BinaryWriter);
        }

        private void packet_notify_zonesrv(ushort cmd, int size, HC_Notify_Zonesvr pkt)
        {
            _mapname = pkt.Mapname.Replace(".gat", ".gnd");
            if (RagnarokClient.Singleton.CurrentConnection != null && RagnarokClient.Singleton.CurrentConnection.Client.Connected)
            {
                RagnarokClient.Singleton.CurrentConnection.Disconnect();
            }
            
            RagnarokClient.Singleton.CurrentConnection = new Network.Connection();
            RagnarokClient.Singleton.CurrentConnection.PacketSerializer.PacketHooks[(int)PacketHeader.HEADER_ZC_ACCEPT_ENTER2] = new Action<ushort, int, ZC_Accept_Enter2>(packetLoginAccepted);

            try
            {
                RagnarokClient.Singleton.CurrentConnection.Connect(pkt.IP.ToString(), pkt.Port);
            }
            catch
            {
                CloseWait();
                MessageBox.ShowOk("Could not connect to server.", GotoLoginScreen);
            }

            RagnarokClient.Singleton.CurrentConnection.Start();
            
            new CZ_Enter(
                RagnarokClient.Singleton.NetworkState.LoginAccept.AccountID,
                RagnarokClient.Singleton.NetworkState.LoginAccept.LoginID1,
                RagnarokClient.Singleton.NetworkState.LoginAccept.LoginID2,
                RagnarokClient.Singleton.NetworkState.LoginAccept.Sex).Write(RagnarokClient.Singleton.CurrentConnection.BinaryWriter);
        }

        void GotoLoginScreen(int dummy)
        {
            RagnarokClient.Singleton.ChangeScreen(new LoginScreen());
        }

        void packetLoginAccepted(ushort cmd, int size, ZC_Accept_Enter2 pkt)
        {
            CloseWait();
            RagnarokClient.Singleton.StartMapChange(_mapname.Replace(".gat", ""));
        }

        NewCharWindow newCharWindow;
        void window_OnCreateChar(int obj)
        {
            if (RagnarokClient.Singleton.GuiManager.Controls.Contains(newCharWindow))
            {
                // bring to front
                return;
            }
            newCharWindow = new NewCharWindow();
            RagnarokClient.Singleton.GuiManager.Controls.Add(newCharWindow);
        }

        public override void Update(SpriteBatch sb, GameTime gameTime)
        {
            base.Update(sb, gameTime);

            if (gameTime.TotalGameTime.TotalSeconds % 12 < 1.0F)
            {
                new Ping((int)gameTime.TotalGameTime.TotalMilliseconds).Write(RagnarokClient.Singleton.CurrentConnection.BinaryWriter);
            }
        }

        public override void Dispose()
        {
            base.Dispose();

            window.Close();
        }
    }
}
