namespace Butterfly.Communication.Packets.Outgoing.Structure
{
    internal class CampaignComposer : ServerPacket
    {
        public CampaignComposer(string campaignString, string campaignName)
            : base(ServerPacketHeader.DESKTOP_CAMPAIGN)
        {
            this.WriteString(campaignString);
            this.WriteString(campaignName);
        }
    }
}
