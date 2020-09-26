using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CraneControl.Server
{
    public class client_connected : EventArgs
    {
        public bool Client { get; }

        public client_connected(bool client)
        {
            Client = client;
        }
    }
}
