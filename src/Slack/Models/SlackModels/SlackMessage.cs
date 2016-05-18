using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models.SlackModels
{
    public class SlackMessage
    {
        public string Response_Type { get; set; }
        public string Text { get; set; }
        public string Username { get; set; }
        public List<Attachment> Attachments { get; set; }
    }

    public class Attachment
    {
        public string Fallback { get; set; }
        public string Color { get; set; }
        public string Pretext { get; set; }
        public string Author_Name { get; set; }
        public string Author_Link { get; set; }
        public string Author_Icon { get; set; }
        public string Title { get; set; }
        public string Title_Link { get; set; }
        public string Text { get; set; }
        public List<Fields> Fields { get; set; }
        public string Image_Url { get; set; }
        public string Thumb_Url { get; set; }
        public List<string> Mrkdwn_In { get; set; }
    }

    public class Fields
    {
        public string Title { get; set; }
        public string Value { get; set; }
        public string Short { get; set; }
    }
}
