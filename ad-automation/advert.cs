using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ad_automation
{
    public class advert : audioFile
    {
        private enum adCategory {BID,BUILDINGSUPPLIES,CAFE,COUNCIL,GARAGE,GARDENCENTRE,NURSERY,OFFLICENCE,PHARMACY};
        private int targetPlaysPerDay;

        public bool canAdBePlayedInBreak(DateTime breakTime)
        {
            bool retVal = true;
            if (!isAdBreakBetweenStartAndEndDates(breakTime)) { retVal = false; }
            // Check category
            if (hasFileBeenPlayedThisOrLastBreak()) { retVal = false; }
            return retVal;
        }

        internal string createLogFileEntry()
        {
            string output = this.filename + "\r\nPlayed at:\r\n";
            output += String.Join("\r\n", this.inBreaks);
            output += "\r\nTarget plays per day: " + this.targetPlaysPerDay.ToString() + " | Total plays: " + this.inBreaks.Count.ToString();
            output += "\r\n\r\n";
            return output;
        }
    }
}
