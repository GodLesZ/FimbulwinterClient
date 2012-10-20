using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using FimbulwinterClient.Core.Exports;
using FimbulwinterClient.Extensions;

namespace FimbulwinterClient.Network.Packets.Character
{
	[PackerHandler(PacketHeader.HEADER_HC_ACCEPT_ENTER,
        "HC_ACCEPT_ENTER",
        PackerHandlerAttribute.VariableSize,
        PackerHandlerAttribute.PacketDirection.In)]
    public class HC_Accept_Enter : InPacket
    {
        public int MaxSlots { get; set; }
        public int AvailableSlots { get; set; }
        public int PremiumSlots { get; set; }
        public CSCharData[] Chars { get; set; }

        public bool Read(byte[] data)
        {
            using (BinaryReader br = new BinaryReader(new MemoryStream(data)))
            {
                int numChars = (data.Length - 23) / 144;

                MaxSlots = br.ReadByte();
                AvailableSlots = br.ReadByte();
                PremiumSlots = br.ReadByte();

                br.ReadBytes(20);

                Chars = new CSCharData[numChars];
                for (int i = 0; i < numChars; i++)
                {
                    CSCharData cd = new CSCharData();

                    cd.GID = br.ReadInt32();
                    cd.Exp = br.ReadInt32();
                    cd.Zeny = br.ReadInt32();
                    cd.JobExp = br.ReadInt32();
                    cd.JobLevel = br.ReadInt32();
                    cd.BodyState = br.ReadInt32();
                    cd.HealthState = br.ReadInt32();
                    cd.EffectState = br.ReadInt32();
                    cd.Virtue = br.ReadInt32();
                    cd.Honor = br.ReadInt32();
                    cd.StatusPoint = br.ReadInt16();
                    cd.HP = br.ReadInt32();
                    cd.MaxHP = br.ReadInt32();
                    cd.SP = br.ReadInt16();
                    cd.MaxSP = br.ReadInt16();
                    cd.Speed = br.ReadInt16();
                    cd.Job = br.ReadInt16();
                    cd.Hair = br.ReadInt16();
                    cd.Weapon = br.ReadInt16();
                    cd.BaseLevel = br.ReadInt16();
                    cd.SkillPoint = br.ReadInt16();
                    cd.Accessory = br.ReadInt16();
                    cd.Shield = br.ReadInt16();
                    cd.Accessory2 = br.ReadInt16();
                    cd.Accessory3 = br.ReadInt16();
                    cd.HairColor = br.ReadInt16();
                    cd.ClothesColor = br.ReadInt16();
                    cd.Name = br.ReadCString(24);
                    cd.Str = br.ReadByte();
                    cd.Agi = br.ReadByte();
                    cd.Vit = br.ReadByte();
                    cd.Int = br.ReadByte();
                    cd.Dex = br.ReadByte();
                    cd.Luk = br.ReadByte();
                    cd.Slot = br.ReadInt16();
                    cd.Rename = br.ReadInt16();
                    cd.MapName = br.ReadCString(16);
                    cd.DeleteDate = br.ReadInt32();
                    cd.Robe = br.ReadInt32();

                    Chars[i] = cd;
                }
            }

            return true;
        }
    }
}
