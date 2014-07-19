using System;
using System.Collections.Generic;
using System.Text;

namespace Multithreading
{
    public class FindPrimesInput
    {
        private int to;
        public int To
        {
            get
            {
                return to;
            }
            set
            {
                to = value;
            }
        }

        private int from;
        public int From
        {
            get
            {
                return from;
            }
            set
            {
                from = value;
            }
        }

        public FindPrimesInput(int from, int to)
        {
            To = to;
            From = from;
        }

    }
}
