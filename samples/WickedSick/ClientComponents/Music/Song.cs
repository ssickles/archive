using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using WickedSick.ClientComponents.Music.SDBService;
using System.Runtime.Serialization;

namespace WickedSick.ClientComponents.Music
{
    public enum SongLocations
    {
        Archive = 0,
        Library = 1
    }

    [Serializable]
    public class Song : ISerializable
    {
        private int _ID;
        private Artist _ParentArtist;
        private Album _ParentAlbum;
        private string _Title;
        private Int16 _TrackNumber;
        private TimeSpan _Duration;
        private SongLocations _SongLocation;
        private DateTime _UploadDate;

        public Song()
        {
        }

        /// <summary>
        /// Deserializable constructor
        /// </summary>
        /// <param name="info"></param>
        /// <param name="ctxt"></param>
        public Song(SerializationInfo info, StreamingContext context)
        {
            this.ID = info.GetInt32("ID");
            this.Title = info.GetString("Title");
            this.TrackNumber = info.GetInt16("TrackNumber");
            this.Duration = (TimeSpan)info.GetValue("Duration", typeof(TimeSpan));
            this.SongLocation = (SongLocations)info.GetValue("SongLocation", typeof(SongLocations));
            this.UploadDate = info.GetDateTime("UploadDate");
        }

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public Artist ParentArtist
        {
            get { return _ParentArtist; }
            set { _ParentArtist = value; }
        }

        public Album ParentAlbum
        {
            get { return _ParentAlbum; }
            set { _ParentAlbum = value; }
        }

        public string Title
        {
            get { return _Title; }
            set { _Title = value; }
        }

        public Int16 TrackNumber
        {
            get { return _TrackNumber; }
            set { _TrackNumber = value; }
        }

        public TimeSpan Duration
        {
            get { return _Duration; }
            set { _Duration = value; }
        }

        public SongLocations SongLocation
        {
            get { return _SongLocation; }
            set { _SongLocation = value; }
        }

        public DateTime UploadDate
        {
            get { return _UploadDate; }
            set { _UploadDate = value; }
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("ID", this.ID);
            info.AddValue("Title", this.Title);
            info.AddValue("TrackNumber", this.TrackNumber);
            info.AddValue("Duration", this.Duration);
            info.AddValue("SongLocation", this.SongLocation);
            info.AddValue("UploadDate", this.UploadDate);
        }

        #endregion
    }
}
