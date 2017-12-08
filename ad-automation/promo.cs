using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ad_automation
{
    public class promo : audioFile
    {
        int priority = 0; // 0=Station generic; 1=Has expiry more than 3 days away; 2=Has expiry in next 3 days. Used for weighted probability of being chosen

        public bool canPromoBePlayedInBreak(DateTime breakTime)
        {
            bool retVal = true;
            if (!isAdBreakBetweenStartAndEndDates(breakTime)) { retVal = false; }
            if (hasFileBeenPlayedThisOrLastBreak()) { retVal = false; }
            return retVal;
        }

        internal string createLogFileEntry()
        {
            string output = this.filename + "\r\nExpires: " + this.endDate.ToString("dd-MM-yyyy HH:mm") + "\r\nTotal plays: " + this.inBreaks.Count.ToString() + "\r\n\r\n";
            return output;
        }

    }
}
