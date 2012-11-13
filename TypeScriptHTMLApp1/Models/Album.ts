///<reference path='Artist.ts'/>
///<reference path='Song.ts'/>
///<reference path='../INotifyPropertyChanged.ts'/>

module TypeScriptHTMLApp1.Models {

    export class Album implements WickedSick.Fayde.Core.INotifyPropertyChanged {
        public Id: number;
        public Name: string;
        public Artist: Artist;
        public ReleaseDate: Date;
        public ArtFilename: string;
        public Songs: Song[];

        public PropertyChanged: Event;
    }
}