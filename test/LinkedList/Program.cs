using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LinkedList
{
    class Program
    {
        static void Main(string[] args)
        {
            LinkedListItem listRoot = generateData();
            printList(listRoot);
            listRoot = reverseList(listRoot);
            //printList(listRoot);
            Console.ReadLine();
        }

        static LinkedListItem generateData()
        {
            LinkedListItem root = new LinkedListItem(1, "Item 1");
            LinkedListItem previous = root;
            for (int i = 2; i < 10; i++)
            {
                LinkedListItem next = new LinkedListItem(i, string.Format("Item {0}", i));
                previous.Next = next;
                previous = next;
            }
            return root;
        }

        static void printList(LinkedListItem root)
        {
            LinkedListItem x = root;
            do
            {
                if (x!=null) Console.WriteLine(x.Id);
                //x = root.Next;
            } while ((x = x.Next)!=null);
            

        }

        static LinkedListItem reverseList(LinkedListItem root)
        {
            return null;
        }
    }
}
