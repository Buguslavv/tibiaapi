﻿using System;
using Tibia.Objects;

namespace Tibia.Packets.Pipes
{
    public class RemoveAllContextMenusPacket : PipePacket
    {

        public RemoveAllContextMenusPacket(Client client)
            : base(client)
        {
            Type = PipePacketType.RemoveAllContextMenus;
        }

        public override bool ParseMessage(NetworkMessage msg, PacketDestination destination)
        {
            if (msg.GetByte() != (byte)PipePacketType.RemoveAllContextMenus)
                return false;

            Type = PipePacketType.RemoveAllContextMenus;

            return true;
        }

        public override byte[] ToByteArray()
        {
            NetworkMessage msg = new NetworkMessage(Client, 0);
            msg.AddByte((byte)Type);
            return msg.Packet;
        }

        public static bool Send(Objects.Client client)
        {
            RemoveAllContextMenusPacket p = new RemoveAllContextMenusPacket(client);
            return p.Send();
        }

    }
}



