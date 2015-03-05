using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutomationLibrary.Controllers.Modbus
{
    public abstract class ModbusTransport
        : IDisposable
    {
        private object _syncLock = new object();


        internal void ValidateResponse(Message request, Message response)
        {
            // always check the function code and slave address, regardless of transport protocol
            if (request.FunctionCode != response.FunctionCode)
                throw new ControllerCommunicationException(string.Format("Received response with unexpected Function Code. Expected {0}, received {1}.", request.FunctionCode, response.FunctionCode));

            if (request.SlaveAddress != response.SlaveAddress)
                throw new ControllerCommunicationException(string.Format("Response slave address does not match request. Expected {0}, received {1}.", response.SlaveAddress, request.SlaveAddress));

            // TODO: message specific validation?
        }


        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
