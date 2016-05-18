using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models.CanteenModels
{
    public interface ICanteenRepository
    {
        Canteen Get(DateTime day, CanteensEnum canteen = CanteensEnum.AmAdenauerring);
    }
}
