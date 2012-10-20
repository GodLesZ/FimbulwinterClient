using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FimbulwinterClient.Core.Content.GRF;

namespace FimbulwinterClient.Core.Content
{
    public class GrfFileSystem : IFileSystem
    {
        private static List<GRF.GRF> _grfFiles;
        public static List<GRF.GRF> GrfFiles
        {
            get { return GrfFileSystem._grfFiles; }
        }

        static GrfFileSystem()
        {
            _grfFiles = new List<GRF.GRF>();
        }

        public static void AddGrf(string file)
        {
            GRF.GRF grf = new GRF.GRF();

            grf.Open(file);

            _grfFiles.Add(grf);
        }

        public Stream Load(string filename)
        {
            for (int i = 0; i < _grfFiles.Count; i++)
            {
                GRFFile f = _grfFiles[i].GetFile(filename);

                if (f != null)
                {
                    byte[] data = _grfFiles[i].GetDataFromFile(f);

                    return new MemoryStream(data);
                }
            }

            return null;
        }
    }
}
