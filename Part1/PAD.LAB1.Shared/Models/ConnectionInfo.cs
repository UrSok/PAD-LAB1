using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PAD.LAB1.Shared
{
//cu el salvam conexiunile care le primim, le atribuim un id random
//salvam socketul clientului
//buffer prin care o sa primim datele
    public class ConnectionInfo
    {
        public  Guid Id {  get; set; }
        public byte[] Buffer { get; set; }
        public const int BufferSize = 1024;
        public Socket Socket { get; set; }

        public ConnectionInfo()
        {
            Id = Guid.NewGuid();
            Buffer = new byte[BufferSize];
        }
    }
}
