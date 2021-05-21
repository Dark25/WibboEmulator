﻿using Butterfly;
using Butterfly.Core;
using Butterfly.Net;
using System;

namespace ConnectionManager
{
    public class ConnectionHandeling
    {
        public GameSocketManager Manager;

        public ConnectionHandeling(int port, int maxConnections, int connectionsPerIP)
        {
            this.Manager = new GameSocketManager();
            this.Manager.Init(port, maxConnections, new InitialPacketParser());

            this.Manager.connectionEvent += new GameSocketManager.ConnectionEvent(this.ConnectionEvent);
        }

        private void ConnectionEvent(ConnectionInformation connection)
        {
            connection.ConnectionClose += new ConnectionInformation.ConnectionChange(this.ConnectionChanged);

            ButterflyEnvironment.GetGame().GetClientManager().CreateAndStartClient(connection.GetConnectionID(), connection);
        }

        private void ConnectionChanged(ConnectionInformation information)
        {
            this.CloseConnection(information);

            information.ConnectionClose -= new ConnectionInformation.ConnectionChange(this.ConnectionChanged);
        }

        public void CloseConnection(ConnectionInformation connection)
        {
            try
            {
                ButterflyEnvironment.GetGame().GetClientManager().DisposeConnection(connection.GetConnectionID());

                connection.Dispose();
            }
            catch (Exception ex)
            {
                Logging.LogException((ex).ToString());
            }
        }


        public void Destroy()
        {
            this.Manager.Destroy();
        }
    }
}
