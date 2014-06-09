using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutomationLibrary.Controllers;
using TwinCAT.Ads;

namespace MassBayEngineering.Interop.Beckhoff
{
    public sealed class ControllerConnectionFactory
        : IControllerConnectionFactory<string>
    {
        private readonly AmsAddress _address;

        public ControllerConnectionFactory(AmsAddress address)
        {
            if (address == null) throw new ArgumentNullException();
            _address = address;
        }

        public ControllerConnectionFactory(string amsNetId, int port)
            : this(new AmsAddress(amsNetId, port))
        {
        }

        // connect to a port on the local host
        public ControllerConnectionFactory(int port)
            : this(new AmsAddress(port))
        {
        }

        public IControllerConnection<string> Connect(TimeSpan timeout)
        {
            try
            {
                TcAdsClient client = new TcAdsClient();
                client.Connect(_address.NetId, _address.Port);

                var result = new ControllerConnection(client);

                return result;
            }
            catch (AdsException)
            {
                // TODO: wrap exception
                throw;
            }
        }
    }
}
