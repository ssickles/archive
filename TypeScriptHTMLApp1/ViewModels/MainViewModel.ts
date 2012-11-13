///<reference path='../Models/Song.ts'/>

module TypeScriptHTMLApp1.ViewModels {

    export class MainViewModel {

        public Name: string;
    }
}

var mvm = new TypeScriptHTMLApp1.ViewModels.MainViewModel();
mvm.Name = "Scott Sickles";

var song = new TypeScriptHTMLApp1.Models.Song();