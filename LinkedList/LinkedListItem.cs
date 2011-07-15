using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedList
{
    public class LinkedListItem
    {
        public LinkedListItem(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public LinkedListItem Next { get; set; }
    }
}
