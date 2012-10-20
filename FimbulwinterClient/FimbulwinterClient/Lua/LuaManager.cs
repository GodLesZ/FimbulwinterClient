﻿using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LuaInterface;
using System.IO;
using FimbulwinterClient.Core.Assets;
using FimbulwinterClient.Core;

namespace FimbulwinterClient.Lua
{
    public class LuaManager : GameComponent
    {
        private const string lua_folder = @"data\luafiles514\lua files\";

        private LuaInterface.Lua _luaparser;

        public LuaManager()
            : base(RagnarokClient.Singleton)
        {
            _luaparser = new LuaInterface.Lua();

            LoadAccessory();
            LoadRobe();
            LoadNpcIdentity();
        }

        private void RunScript(string filename)
        {
            Stream file = SharedInformation.ContentManager.Load<Stream>(lua_folder + filename);
            string temp = Path.GetTempFileName();

            using (FileStream fs = new FileStream(temp, FileMode.Create))
            {
                using (BinaryWriter bw = new BinaryWriter(fs))
                {
                    BinaryReader br = new BinaryReader(file);
                    byte[] bytes = br.ReadBytes((int)file.Length);

                    bw.Write(bytes);
                }
            }

            _luaparser.DoFile(temp);
            System.IO.File.Delete(temp);
        }

        private void LoadAccessory()
        {
            RunScript(@"datainfo\accessoryid.lub");
            RunScript(@"datainfo\accname.lub");

            var tbl = _luaparser.GetTable("ACCESSORY_IDs");
            foreach (DictionaryEntry de in tbl)
                if (!Statics.Accessories.ContainsKey(Convert.ToInt32(de.Value)))
                    Statics.Accessories.Add(Convert.ToInt32(de.Value), new Tuple<string, string>(de.Key.ToString(), ""));

            tbl = _luaparser.GetTable("AccNameTable");
            foreach (DictionaryEntry de in tbl)
                if (Statics.Accessories.ContainsKey(Convert.ToInt32(de.Key)))
                    Statics.Accessories[Convert.ToInt32(de.Key)] = new Tuple<string, string>(Statics.Accessories[Convert.ToInt32(de.Key)].Item1, de.Value.ToString());
        }

        private void LoadRobe()
        {
            RunScript(@"datainfo\spriterobeid.lub");
            RunScript(@"datainfo\spriterobename.lub");

            var tbl = _luaparser.GetTable("SPRITE_ROBE_IDs");
            foreach (DictionaryEntry de in tbl)
                if (!Statics.Robes.ContainsKey(Convert.ToInt32(de.Value)))
                    Statics.Robes.Add(Convert.ToInt32(de.Value), new Tuple<string, string>(de.Key.ToString(), ""));

            tbl = _luaparser.GetTable("RobeNameTable");
            foreach (DictionaryEntry de in tbl)
                if (Statics.Robes.ContainsKey(Convert.ToInt32(de.Key)))
                    Statics.Robes[Convert.ToInt32(de.Key)] = new Tuple<string, string>(Statics.Robes[Convert.ToInt32(de.Key)].Item1, de.Value.ToString());
        }

        private void LoadNpcIdentity()
        {
            RunScript(@"datainfo\npcidentity.lub");
            RunScript(@"datainfo\jobname.lub");

            var tbl = _luaparser.GetTable("jobtbl");
            foreach (DictionaryEntry de in tbl)
                if (!Statics.NpcIdentity.ContainsKey(Convert.ToInt16(de.Value)))
                    Statics.NpcIdentity.Add(Convert.ToInt16(de.Value), new Tuple<string, string>(de.Key.ToString(), ""));

            tbl = _luaparser.GetTable("JobNameTable");
            foreach (DictionaryEntry de in tbl)
                if (Statics.NpcIdentity.ContainsKey(Convert.ToInt16(de.Key)))
                    Statics.NpcIdentity[Convert.ToInt16(de.Key)] = new Tuple<string, string>(Statics.NpcIdentity[Convert.ToInt16(de.Key)].Item1, de.Value.ToString());
        }

    }
}
