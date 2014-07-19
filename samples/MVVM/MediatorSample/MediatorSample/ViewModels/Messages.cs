using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MediatorSample.ViewModels
{
    /// <summary>
    /// Defines a list of messages that the controllers use to communicate
    /// </summary>
    public static class Messages
    {
        /// <summary>
        /// Message to notify all Colleagues that the zoom level selected is changed
        /// </summary>
        public const string ZoomLevelSelectedChanged = "zoomChanged";

        /// <summary>
        /// Message to signal that the directory selected is changed
        /// </summary>
        public const string DirectorySelectedChanged = "directoryChanged";
    }
}
