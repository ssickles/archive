using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace WickedSick.ClientComponents.Music
{
    [Serializable]
    public class MediaBank : ISerializable
    {
        private DateTime _RecentUpdateTime;
        private Dictionary<int, Artist> _AllArtists;

        public MediaBank()
        {
            _AllArtists = new Dictionary<int, Artist>();
        }

        public MediaBank(SerializationInfo info, StreamingContext context)
        {
            this._RecentUpdateTime = info.GetDateTime("RecentUpdateTime");
            this._AllArtists = (Dictionary<int, Artist>)info.GetValue("Artists", typeof(Dictionary<int, Artist>));
        }

        public Dictionary<int, Artist> Artists
        {
            get { return _AllArtists; }
        }

        public void LoadFullBank(ref List<Artist> artists, ref Dictionary<int, Album> albums, ref Dictionary<int, Song> songs, ref List<int> library)
        {
            foreach (int j in library)
            {
                songs[j].SongLocation = SongLocations.Library;
            }
            
            for (int i = 0; i < artists.Count; i++)
            {
                _AllArtists.Add(artists[i].ID, artists[i]);
            }
            artists.Clear();
            
            foreach (int k in _AllArtists.Keys)
            {
                foreach (int k2 in _AllArtists[k].Albums.Keys)
                {
                    _AllArtists[k].Albums[k2].Name = albums[k2].Name;
                    _AllArtists[k].Albums[k2].ReleaseDate = albums[k2].ReleaseDate;
                    _AllArtists[k].Albums[k2].Parent = _AllArtists[k];
                    foreach (int k3 in albums[k2].Songs.Keys)
                    {
                        Song s = new Song();
                        s.ID = k3;
                        s.Duration = songs[k3].Duration;
                        s.ParentAlbum = _AllArtists[k].Albums[k2];
                        s.ParentArtist = _AllArtists[k];
                        s.SongLocation = songs[k3].SongLocation;
                        s.Title = songs[k3].Title;
                        s.TrackNumber = songs[k3].TrackNumber;
                        s.UploadDate = songs[k3].UploadDate;
                        _AllArtists[k].Albums[k2].Songs.Add(k3, s);
                    }
                }
            }
            albums.Clear();
            songs.Clear();
            _RecentUpdateTime = DateTime.Now;
        }

        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("RecentUpdateTime", this._RecentUpdateTime);
            info.AddValue("Artists", this.Artists);
        }

        #endregion
    }
}
