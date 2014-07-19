using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace WickedSick.ClientComponents.Music
{
    [Serializable]
    public class Artist : ISerializable
    {
        private int _ID;
        private string _Name;
        private Dictionary<int, Album> _Albums;

        public Artist()
        {
            _Albums = new Dictionary<int, Album>();
        }

        public Artist(SerializationInfo info, StreamingContext context)
        {
            this.ID = info.GetInt32("ID");
            this.Name = info.GetString("Name");
            this.Albums = (Dictionary<int, Album>)info.GetValue("Albums", typeof(Dictionary<int, Album>));

            foreach (int k in this.Albums.Keys)
            {
                this.Albums[k].Parent = this;
                foreach (int l in this.Albums[k].Songs.Keys)
                {
                    this.Albums[k].Songs[l].ParentArtist = this;
                }
            }
        }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public Dictionary<int, Album> Albums
        {
            get { return _Albums; }
            set { _Albums = value; }
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", this.ID);
            info.AddValue("Name", this.Name);
            info.AddValue("Albums", this.Albums);
        }

        #endregion
    }
}
