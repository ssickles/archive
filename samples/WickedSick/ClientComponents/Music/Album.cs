using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedSick.ClientComponents.Music.SDBService;
using System.Runtime.Serialization;

namespace WickedSick.ClientComponents.Music
{
    [Serializable]
    public class Album : ISerializable
    {
        private int _ID;
        private Artist _Parent;
        private string _Name;
        private DateTime _ReleaseDate;
        private Dictionary<int, Song> _Songs;

        public Album()
        {
            _Songs = new Dictionary<int, Song>();
        }

        public Album(SerializationInfo info, StreamingContext context)
        {
            this.ID = info.GetInt32("ID");
            this.Name = info.GetString("Name");
            this.ReleaseDate = info.GetDateTime("ReleaseDate");
            this.Songs = (Dictionary<int, Song>)info.GetValue("Songs", typeof(Dictionary<int, Song>));

            foreach (int k in this.Songs.Keys)
            {
                this.Songs[k].ParentAlbum = this;
            }
        }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public Artist Parent
        {
            get { return _Parent; }
            set { _Parent = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        public DateTime ReleaseDate
        {
            get { return _ReleaseDate; }
            set { _ReleaseDate = value; }
        }

        public Dictionary<int, Song> Songs
        {
            get { return _Songs; }
            set { _Songs = value; }
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", this.ID);
            info.AddValue("Name", this.Name);
            info.AddValue("ReleaseDate", this.ReleaseDate);
            info.AddValue("Songs", this.Songs);
        }

        #endregion
    }
}
