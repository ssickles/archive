using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace BaseballModels
{
    [DataContract]
    public class Player
    {
        [DataMember]
        private int _id;
        [DataMember]
        private string _firstName;
        [DataMember]
        private string _lastName;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; }
        }
    }
}
