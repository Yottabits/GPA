using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Slack.Models;
using System.Net.Http;
using Newtonsoft.Json;
using System.Globalization;
using System.Text.RegularExpressions;
using Slack.Models.SlackModels;
using Slack.Models.CanteenModels;

namespace Slack.Controllers
{
    [Route("api/[controller]")]
    public class SlackSlashController : Controller
    {
        [FromServices]
        public ICanteenRepository Canteens { get; set; }

        // POST api/slackslash
        [HttpPost]
        public SlackMessage Post(SlackSlashCommand cmd)
        {
            //// First: Check if token is valid
            //if (cmd.Token != "******************")
            //    return new SlackMessage() { Text = "Invalid Token." };

            //// Second: Check that the right team is calling (Suggested by api reference).
            //if (cmd.Team_Id != "*********")
            //    return new SlackMessage() { Text = "Invalid Team." };

            SlackMessage msg;
            // Currently only the /mensa command is supported
            // TODO: Short option for mensa. Will do the same, but only print out the first syllable of evey word.
            switch (cmd.Command)
            {
                case "/mensa":
                    msg = Mensa(cmd);
                    break;
                default:
                    msg = new SlackMessage() { Text = "Unknown Command." };
                    break;
            }

            // Send back Response. Must be within 3000ms. Gets parsed automagically to JSON.
            return msg;
        }

        private SlackMessage Mensa(SlackSlashCommand cmd)
        {
            int requestedMealGroup = 0; // All MealGroups
            DateTime requestedDay = DateTime.Today;

            // Parse arguments. Seperated by spaces
            if (cmd.Text != null)
            {
                string[] args = cmd.Text.Split(' ');

                // Parse Line
                int.TryParse(args[0], out requestedMealGroup);
                if (requestedMealGroup < 0 || requestedMealGroup > MealGroup.MealGroupMapping.Count)
                    requestedMealGroup = 0; // All MealGroups

                if (args.Length > 1)
                {
                    // Parse Day
                    if (args[1].ToLower() == "tomorrow" || args[1].ToLower() == "morgen")
                        requestedDay = DateTime.Today.AddDays(1);
                    else if (args[1].ToLower() == "tdat" || args[1].ToLower() == "übermorgen")
                        requestedDay = DateTime.Today.AddDays(2);
                    else if (!DateTime.TryParse(args[1], out requestedDay) && !DateTime.TryParseExact(args[1], "dd'.'MM'.'yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out requestedDay))
                        requestedDay = DateTime.Today;
                }
            }

            // Build Message
            SlackMessage msg = new SlackMessage();
            msg.Response_Type = "in_channel";
            msg.Username = "Mensa";

            // Get Information
            Canteen canteen = Canteens.Get(requestedDay);
            if (canteen == null)
            {
                msg.Text = "Fehler beim Laden des Mensaplans";
                return msg;
            }

            switch(canteen.Status)
            {
                case "ok":
                    break;
                case "no mealplan available":
                case "invalid request date":
                    msg.Text = String.Format("Für den {0} ist kein Essensplan verfügbar. Wahrscheinlich gibt's da nix.", requestedDay.ToString("dd.MM.yyyy"));
                    return msg;
                case "invalid canteen id":
                    msg.Text = "Ungültige Mensa-ID";
                    return msg;
                case "malformed request date":
                    msg.Text = "Falsch formatiertes Datum";
                    return msg;
            }

            msg.Text = requestedDay.ToString("dd.MM.yyyy");
            msg.Attachments = new List<Attachment>();

            if(requestedMealGroup != 0)
            {
                MealGroup group = canteen.MealGroups.First(item => item.Title == MealGroup.MealGroupMapping[requestedMealGroup]);
                Attachment attachm = group.FillInAttachment();

                // Add Attachment to message
                msg.Attachments.Add(attachm);
            }
            else
            {
                for(int i = 0; i <= 5; i++)
                {
                    if (i == 4) { continue; } // Skip Schnitzelbar.

                    Attachment attachm = canteen.MealGroups[i].FillInAttachment();
                    msg.Attachments.Add(attachm);
                }
            }

            return msg;
        }
    }
}
