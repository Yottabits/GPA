using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Slack.Models.SlackModels;

namespace Slack.Models.CanteenModels
{
    public class MealGroup
    {
        // Mapping from int index to string literal. 0=All Lines, 1=Line 1, ...
        public static ReadOnlyCollection<string> MealGroupMapping = new ReadOnlyCollection<string>(new List<string>()
        { "All Lines", "Linie 1", "Linie 2", "Linie 3" , "Linie 4/5" , "Linie 4/5", "L6 Update",
            "Schnitzelbar", "Curry Queen", "Cafeteria Heiße Theke", "Cafeteria ab 14:30" });

        public string Title { get; set; }
        public List<Meal> Meals { get; set; }
        public string Message { get; set; }

        public Attachment FillInAttachment()
        {
            Attachment item = new Attachment();

            // Fill in title of the line
            item.Pretext = string.Format("*{0}*", Title);

            // Fill in most important meals
            if (Meals == null)
                item.Text = Message == string.Empty ? "Kein Essen an dieser Linie" : Message;
            else
            {
                if (Title == "Linie 2")
                    item.Text = string.Format("_{0}_", Meals[0].Name);
                else if (Title == "Linie 3" && Meals.Last().Name == "Tagesangebot")
                    item.Text = string.Format("_{0}_\n_{1}_", Meals[0].Name, Meals.Last().Name);
                else
                    item.Text = string.Format("_{0}_\n_{1}_", Meals[0].Name, Meals[1].Name);
            }

            // Enable Markdown Syntax Highlighting
            item.Mrkdwn_In = new List<string> { "text", "pretext" };

            // TODO: Add Colours
            return item;
        }
    }
}
