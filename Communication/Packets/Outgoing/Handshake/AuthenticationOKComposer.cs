namespace Butterfly.Communication.Packets.Outgoing.Handshake
{
    internal class AuthenticationOKComposer : ServerPacket
    {
        public AuthenticationOKComposer()
            : base(ServerPacketHeader.AUTHENTICATED)
        {

        }
    }
}