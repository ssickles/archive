using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WickedSick.ClientComponents.Music.SDBService;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace WickedSick.ClientComponents.Music
{
    public delegate void CommunicationCompleteDelegate(object sender, int CommunicationType);

    public class MediaBI
    {
        private MediaBank mb;

        private string mb_filepath;
        
        private List<Artist> _TempArtists;
        private Dictionary<int, Song> _TempSongs;
        private Dictionary<int, Album> _TempAlbums;
        private List<int> _TempLibrary;
        private int done = 0;
        public event CommunicationCompleteDelegate CommunicationComplete;
        SDBServiceClient ssc;

        public MediaBI(string media_bank_filepath)
        {
            mb_filepath = media_bank_filepath;
            if (File.Exists(media_bank_filepath))
            {
                Stream st = File.Open(media_bank_filepath, FileMode.Open);
                BinaryFormatter bf = new BinaryFormatter();
                try
                {
                    mb = (MediaBank)bf.Deserialize(st);
                }
                catch (SerializationException)
                {
                    mb = new MediaBank();
                }
                st.Close();
            }
            else
            {
                mb = new MediaBank();
            }

            ssc = new SDBServiceClient("BasicHttpBinding_ISDBService");
            ssc.RequestAllAlbumsCompleted += new EventHandler<RequestAllAlbumsCompletedEventArgs>(ssc_RequestAllAlbumsCompleted);
            ssc.RequestNextAlbumChunkCompleted += new EventHandler<RequestNextAlbumChunkCompletedEventArgs>(ssc_RequestNextAlbumChunkCompleted);
            ssc.RequestAllArtistsCompleted += new EventHandler<RequestAllArtistsCompletedEventArgs>(ssc_RequestAllArtistsCompleted);
            ssc.RequestNextArtistChunkCompleted += new EventHandler<RequestNextArtistChunkCompletedEventArgs>(ssc_RequestNextArtistChunkCompleted);
            ssc.RequestAllSongsCompleted += new EventHandler<RequestAllSongsCompletedEventArgs>(ssc_RequestAllSongsCompleted);
            ssc.RequestNextSongChunkCompleted += new EventHandler<RequestNextSongChunkCompletedEventArgs>(ssc_RequestNextSongChunkCompleted);
            ssc.RequestLibraryIDsCompleted += new EventHandler<RequestLibraryIDsCompletedEventArgs>(ssc_RequestLibraryIDsCompleted);
        }

        public MediaBank Bank
        {
            get { return mb; }
        }

        public void InitializeMedia(int MemberID)
        {
            if (mb.Artists == null || mb.Artists.Count < 1)
            {
                done = 0;
                _TempArtists = new List<Artist>();
                ssc.RequestAllArtistsAsync();
                _TempAlbums = new Dictionary<int, Album>();
                ssc.RequestAllAlbumsAsync();
                _TempSongs = new Dictionary<int, Song>();
                ssc.RequestAllSongsAsync();
                _TempLibrary = new List<int>();
                ssc.RequestLibraryIDsAsync(MemberID);
            }
        }
        
        public void SendMediaToFile()
        {
            Stream st = File.Open(mb_filepath, FileMode.Create);
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(st, mb);
            st.Close();
        }

        private void FinishMediaLoad()
        {
            mb.LoadFullBank(ref _TempArtists, ref _TempAlbums, ref _TempSongs, ref _TempLibrary);
            CallCommunicationComplete(1);
        }

        #region WCF EVENTS

        private void ssc_RequestAllArtistsCompleted(object sender, RequestAllArtistsCompletedEventArgs e)
        {
            HandleArtistChunks((ChunkDataOfArtistDataTepVWBla)e.Result);
        }

        private void ssc_RequestNextArtistChunkCompleted(object sender, RequestNextArtistChunkCompletedEventArgs e)
        {
            HandleArtistChunks((ChunkDataOfArtistDataTepVWBla)e.Result);
        }

        private void HandleArtistChunks(ChunkDataOfArtistDataTepVWBla curChunk)
        {
            foreach (int k in curChunk.Chunk.Keys)
            {
                ArtistData ad = curChunk.Chunk[k];
                Artist a = new Artist();
                a.ID = k;
                a.Name = ad.Name;
                foreach (int aid in ad.AlbumIDs)
                {
                    Album al = new Album();
                    al.ID = aid;
                    a.Albums.Add(aid, al);
                }
                _TempArtists.Add(a);
            }

            int nextChunkNumber = curChunk.ChunkNumber + 1;
            if (nextChunkNumber < curChunk.TotalChunks)
            {
                ssc.RequestNextArtistChunkAsync(nextChunkNumber);
            }
            else
            {
                done += 2;
                if (done == 15)
                    FinishMediaLoad();
            }
        }

        private void ssc_RequestAllAlbumsCompleted(object sender, RequestAllAlbumsCompletedEventArgs e)
        {
            HandleAlbumChunks((ChunkDataOfAlbumDataTepVWBla)e.Result);
        }

        private void ssc_RequestNextAlbumChunkCompleted(object sender, RequestNextAlbumChunkCompletedEventArgs e)
        {
            HandleAlbumChunks((ChunkDataOfAlbumDataTepVWBla)e.Result);
        }

        private void HandleAlbumChunks(ChunkDataOfAlbumDataTepVWBla curChunk)
        {
            foreach (int k in curChunk.Chunk.Keys)
            {
                AlbumData ad = curChunk.Chunk[k];
                Album a = new Album();
                a.ID = k;
                a.Name = ad.Name;
                a.ReleaseDate = new DateTime(ad.ReleaseDate);
                foreach (int sid in ad.SongIDs)
                {
                    Song s = new Song();
                    s.ID = sid;
                    a.Songs.Add(sid, s);
                }
                _TempAlbums.Add(k, a);
            }

            int nextChunkNumber = curChunk.ChunkNumber + 1;
            if (nextChunkNumber < curChunk.TotalChunks)
            {
                ssc.RequestNextAlbumChunkAsync(nextChunkNumber);
            }
            else
            {
                done += 4;
                if (done == 15)
                    FinishMediaLoad();
            }
        }

        private void ssc_RequestAllSongsCompleted(object sender, RequestAllSongsCompletedEventArgs e)
        {
            HandleSongChunks((ChunkDataOfSongDataTepVWBla)e.Result);
        }

        private void ssc_RequestNextSongChunkCompleted(object sender, RequestNextSongChunkCompletedEventArgs e)
        {
            HandleSongChunks((ChunkDataOfSongDataTepVWBla)e.Result);
        }

        private void HandleSongChunks(ChunkDataOfSongDataTepVWBla curChunk)
        {
            foreach (int k in curChunk.Chunk.Keys)
            {
                SongData sd = curChunk.Chunk[k];
                Song s = new Song();
                s.Duration = TimeSpan.FromTicks(sd.Duration);
                s.ID = k;
                s.SongLocation = (SongLocations)sd.Location;
                s.Title = sd.Title;
                s.TrackNumber = sd.TrackNumber;
                s.UploadDate = new DateTime(sd.UploadDate);
                _TempSongs.Add(k, s);
            }

            int nextChunkNumber = curChunk.ChunkNumber + 1;
            if (nextChunkNumber < curChunk.TotalChunks)
            {
                ssc.RequestNextSongChunkAsync(nextChunkNumber);
            }
            else
            {
                done += 1;
                if (done == 15)
                    FinishMediaLoad();
            }
        }
        
        private void ssc_RequestLibraryIDsCompleted(object sender, RequestLibraryIDsCompletedEventArgs e)
        {
            _TempLibrary = (List<int>)e.Result;
            done += 8;
            if (done == 15)
                FinishMediaLoad();
        }

        #endregion

        private void CallCommunicationComplete(int CommunicationType)
        {
            CommunicationCompleteDelegate obj = this.CommunicationComplete;
            if (obj != null)
            {
                obj(this, CommunicationType);
            }
        }

    }
}
