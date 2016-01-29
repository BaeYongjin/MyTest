using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DZ.MediaPlayer.Vlc.Io;

namespace AdManagerClient.AdFile
{
    public sealed class PlaylistItem
    {
        private readonly MediaInput mediaInput;

        /// <summary>
        /// <see cref="MediaInput"/> associated with this item.
        /// </summary>
        public MediaInput MediaInput 
        {
            get 
            {
                return (mediaInput);
            }
        }

        private readonly string title;

        /// <summary>
        /// Specified title of media.
        /// </summary>
        private string Title 
        {
            get 
            {
                return (title);
            }
        }

        private readonly TimeSpan duration;

        /// <summary>
        /// Duration of media.
        /// </summary>
        public TimeSpan Duration 
        {
            get 
            {
                return (duration);
            }
        }

        private bool isError;

        /// <summary>
        /// Have error during last attempt to start playing.
        /// </summary>
        public bool IsError {
            get {
                return (isError);
            }
            set {
                isError = value;
            }
        }

        /// <summary>
        /// <see cref="ToString"/> overriding method.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            TimeSpan durationCopy = Duration;
            return String.Format("{0} - {1:D2}:{2:D2}:{3:D2}", Title, durationCopy.Hours, durationCopy.Minutes, durationCopy.Seconds);
        }

        /// <summary>
        /// Get display string for item.
        /// </summary>
        public string DisplayTitle {
            get {
                return (ToString());
            }
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public PlaylistItem(MediaInput mediaInput, string title, TimeSpan duration) {
            if (mediaInput == null) {
                throw new ArgumentNullException("mediaInput");
            }
            if (title == null) {
                throw new ArgumentNullException("title");
            }
            if (title.Length == 0) {
                throw new ArgumentException("String is empty.", "title");
            }
            //
            this.mediaInput = mediaInput;
            this.title = title;
            this.duration = duration;
            //
            isError = false;
        }
    }
    
}
