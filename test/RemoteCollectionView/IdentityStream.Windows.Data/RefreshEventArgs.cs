// Copyright (c) Manish Dalal, 2008-2009. All Rights Reserved.
// http://weblogs.asp.net/manishdalal
//
// This product's copyrights are licensed under 
// the CreativeCommons Attribution-ShareAlike (version 3)
// http://creativecommons.org/licenses/by-sa/3.0/

using System;
using System.ComponentModel;

namespace SilverlightApplication.CustomSort {
    /// <summary>
    /// Refresh Event Arguments, provides indication of need for data refresh
    /// </summary>
    public class RefreshEventArgs : EventArgs {
        public SortDescriptionCollection SortDescriptions { get; set; }
    }
}
