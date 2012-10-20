using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FimbulwinterClient.Gui;
using FimbulwinterClient.Network.Packets.Account;
using FimbulwinterClient.Gui.System;
using FimbulwinterClient.Network.Packets.Character;

namespace FimbulwinterClient.Screens
{
    public class CharServerSelectScreen : BaseLoginScreen
    {
        private CharServerSelectWindow window;

        public CharServerSelectScreen()
        {
            window = new CharServerSelectWindow();
            window.ServerSelected += new Action<CharServerInfo>(window_ServerSelected);

            RagnarokClient.Singleton.GuiManager.Controls.Add(window);
        }

        void window_ServerSelected(CharServerInfo obj)
        {
            RagnarokClient.Singleton.NetworkState.SelectedCharServer = obj;

            if (RagnarokClient.Singleton.CurrentConnection != null && RagnarokClient.Singleton.CurrentConnection.Client.Connected)
            {
                RagnarokClient.Singleton.CurrentConnection.Disconnect();
            }

            RagnarokClient.Singleton.CurrentConnection = new Network.Connection();
            RagnarokClient.Singleton.CurrentConnection.PacketSerializer.PacketHooks[0x6B] = new Action<ushort, int, HC_Accept_Enter>(packetLoginAccepted);
            RagnarokClient.Singleton.CurrentConnection.PacketSerializer.PacketHooks[0x6C] = new Action<ushort, int, HC_Refuse_Enter>(packetLoginRejected);

            try
            {
                RagnarokClient.Singleton.CurrentConnection.Connect(obj.IP.ToString(), obj.Port);
            }
            catch
            {
                CloseWait();
                MessageBox.ShowOk("Could not connect to server.", ReenterScreen);
            }

            RagnarokClient.Singleton.CurrentConnection.PacketSerializer.BytesToSkip = 4; // Skip AID
            RagnarokClient.Singleton.CurrentConnection.Start();

            new CH_Enter(
                RagnarokClient.Singleton.NetworkState.LoginAccept.AccountID,
                RagnarokClient.Singleton.NetworkState.LoginAccept.LoginID1,
                RagnarokClient.Singleton.NetworkState.LoginAccept.LoginID2,
                RagnarokClient.Singleton.NetworkState.LoginAccept.Sex).Write(RagnarokClient.Singleton.CurrentConnection.BinaryWriter);
        }

        void packetLoginAccepted(ushort cmd, int size, HC_Accept_Enter pkt)
        {
            CloseWait();
            RagnarokClient.Singleton.NetworkState.CharAccept = pkt;
            RagnarokClient.Singleton.ChangeScreen(new CharSelectScreen());
        }

        void packetLoginRejected(ushort cmd, int size, HC_Refuse_Enter pkt)
        {
            CloseWait();
            MessageBox.ShowOk("Connection rejected.", ReenterScreen);
        }

        void ReenterScreen(int dummy)
        {
            RagnarokClient.Singleton.ChangeScreen(new LoginScreen());
        }

        public override void Dispose()
        {
            base.Dispose();

            window.Close();
        }
    }
}
