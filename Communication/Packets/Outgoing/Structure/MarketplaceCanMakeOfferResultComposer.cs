﻿namespace Butterfly.Communication.Packets.Outgoing.Structure
{
    internal class MarketplaceCanMakeOfferResultComposer : ServerPacket
    {
        public MarketplaceCanMakeOfferResultComposer(int Result)
            : base(ServerPacketHeader.MARKETPLACE_SELL_ITEM)
        {
            this.WriteInteger(Result);
            this.WriteInteger(0);
        }
    }
}
