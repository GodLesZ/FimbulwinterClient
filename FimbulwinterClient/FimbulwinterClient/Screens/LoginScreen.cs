using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FimbulwinterClient.Gui;
using FimbulwinterClient.Network.Packets.Account;
using FimbulwinterClient.Gui.System;

namespace FimbulwinterClient.Screens
{
    public class LoginScreen : BaseLoginScreen
    {
        private LoginWindow window;

        public LoginScreen()
        {
            window = new LoginWindow();
            window.GoBack += new Action(window_GoBack);
            window.DoLogin += new Action<string, string>(window_DoLogin);

            RagnarokClient.Singleton.GuiManager.Controls.Add(window);
        }

        void window_DoLogin(string user, string pass)
        {
            ShowWait();

            if (RagnarokClient.Singleton.CurrentConnection != null && RagnarokClient.Singleton.CurrentConnection.Client.Connected)
            {
                RagnarokClient.Singleton.CurrentConnection.Disconnect();
            }

            RagnarokClient.Singleton.CurrentConnection = new Network.Connection();
            RagnarokClient.Singleton.CurrentConnection.PacketSerializer.PacketHooks[0x69] = new Action<ushort, int, AC_Accept_Login>(packetLoginAccepted);
            RagnarokClient.Singleton.CurrentConnection.PacketSerializer.PacketHooks[0x6A] = new Action<ushort, int, AC_Refuse_Login>(packetLoginRejected);

            try
            {
                RagnarokClient.Singleton.CurrentConnection.Connect(RagnarokClient.Singleton.NetworkState.SelectedServer.Address, RagnarokClient.Singleton.NetworkState.SelectedServer.Port);
            }
            catch
            {
                CloseWait();
                MessageBox.ShowOk("Could not connect to server.", ReenterScreen);
            }

            RagnarokClient.Singleton.CurrentConnection.Start();

            new CA_Login(user, pass, RagnarokClient.Singleton.NetworkState.SelectedServer.Version, 1).Write(RagnarokClient.Singleton.CurrentConnection.BinaryWriter);
        }

        void packetLoginAccepted(ushort cmd, int size, AC_Accept_Login pkt)
        {
            CloseWait();
            RagnarokClient.Singleton.NetworkState.LoginAccept = pkt;
            RagnarokClient.Singleton.ChangeScreen(new CharServerSelectScreen());
        }

        void packetLoginRejected(ushort cmd, int size, AC_Refuse_Login pkt)
        {
            CloseWait();
            MessageBox.ShowOk(pkt.Text, ReenterScreen);
        }

        void ReenterScreen(int dummy)
        {
            RagnarokClient.Singleton.ChangeScreen(new LoginScreen());
        }

        void window_GoBack()
        {
            RagnarokClient.Singleton.ChangeScreen(new ServiceSelectScreen());
        }

        public override void Dispose()
        {
            base.Dispose();

            window.Close();
        }
    }
}
