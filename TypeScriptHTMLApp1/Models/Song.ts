///<reference path='Album.ts'/>

module TypeScriptHTMLApp1.Models {

    export class Song {
        public Id: number;
        public Title: string;
        public Duration: number;
        public TrackNumber: number;
        public UploadDate: Date;
        public Album: Album;
    }
}