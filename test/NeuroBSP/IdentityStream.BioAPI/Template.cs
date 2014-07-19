using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IdentityStream.BioAPI
{
    public class Template
    {
        public Template(byte[] Data, int MinutiaCount)
        {
            this.Data = Data;
            this.MinutiaCount = MinutiaCount;
        }

        public byte[] Data { get; private set; }
        public int MinutiaCount { get; private set; }
    }
}
