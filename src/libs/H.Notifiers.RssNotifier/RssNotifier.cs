using System;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Xml;
using H.Core.Settings;

namespace H.Notifiers
{
    /// <summary>
    /// 
    /// </summary>
    public class RssNotifier : TimerNotifier
    {
        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string LastTitle { get; private set; } = string.Empty;

        #endregion

        #region Contructors

        /// <summary>
        /// 
        /// </summary>
        public RssNotifier()
        {
            AddSetting(nameof(Url), o => Url = o, Always, Url, SettingType.Path);

            AddVariable("$rss_last_title$", () => LastTitle);
        }

        #endregion

        #region Protected methods
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override bool OnResult()
        {
            if (string.IsNullOrWhiteSpace(Url))
            {
                return false;
            }

            SyndicationFeed feed;
            try
            {
                using var reader = XmlReader.Create(Url);

                feed = SyndicationFeed.Load(reader);
            }
            catch (WebException exception)
            {
                Print($"Rss Web Exception: {exception.Message}");

                return false;
            }

            var firstItem = feed.Items.FirstOrDefault();
            var title = firstItem?.Title.Text;

            if (title == null ||
                string.Equals(title, LastTitle, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var isFirstFeed = string.IsNullOrWhiteSpace(LastTitle);

            LastTitle = title;

            return !isFirstFeed;
        }

        #endregion

    }
}
