using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FimbulwinterClient.Gui;
using FimbulwinterClient.Core.Config;

namespace FimbulwinterClient.Screens
{
    public class ServiceSelectScreen : BaseLoginScreen
    {
        private ServiceSelectWindow window;

        public ServiceSelectScreen()
        {
            window = new ServiceSelectWindow();
            window.ServerSelected += new Action<ServerInfo>(window_ServerSelected);

            RagnarokClient.Singleton.GuiManager.Controls.Add(window);
        }

        void window_ServerSelected(ServerInfo obj)
        {
            RagnarokClient.Singleton.NetworkState.SelectedServer = obj;
            RagnarokClient.Singleton.ChangeScreen(new LoginScreen());
        }

        public override void Dispose()
        {
            base.Dispose();

            window.Close();
        }
    }
}
