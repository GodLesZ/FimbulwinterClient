using System;

namespace FimbulwinterClient.Core.Content.GRF
{
    public class GRFEventArg : EventArgs
    {
        private GRFFile _file;

        public GRFFile File { get { return _file; } }

        public GRFEventArg(GRFFile file)
        {
            _file = file;
        }
    }
}