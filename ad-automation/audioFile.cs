using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ad_automation
{
    public class audioFile
    {
        public DateTime startDate = new DateTime(2010,1,1);
        public DateTime endDate = new DateTime(2029,12,31);
        public List<DateTime> inBreaks = new List<DateTime>();
        public string filename;
        public string targetPath;
        public string originalPath;

        int playsPerDay(DateTime targetDay, bool includeOvernight)
        {
            throw new NotImplementedException();
        }

        public bool isAdBreakBetweenStartAndEndDates(DateTime breakTime)
        {
            if (breakTime < this.endDate && breakTime > this.startDate)
            {
                return true;
            }
            return false;
        }

        public bool hasFileBeenPlayedThisOrLastBreak()
        {
            System.Threading.Thread.Sleep(20); // Hack to avoid race condition where this method can return true when it's actually false
            if (adForm.thisBreak.Exists(x => x.filename.Equals(this.filename)))
            {
                return true;
            }
            if (adForm.lastBreak.Exists(x => x.filename.Equals(this.filename)))
            {
                return true;
            }
            return false;
        }

        public void getExpiryDate()
        {
            TagLib.File tmpFile = TagLib.File.Create(this.originalPath);
            if (tmpFile.Tag.Comment != null && tmpFile.Tag.Comment.Contains("EXPIRES:"))
            {
                int expiryPos = tmpFile.Tag.Comment.IndexOf("EXPIRES:") + 8;
                string expiryDateStr = tmpFile.Tag.Comment.Substring(expiryPos, 14);
                int expiryDateYears = int.Parse(expiryDateStr.Substring(0, 4));
                int expiryDateMonths = int.Parse(expiryDateStr.Substring(4, 2));
                int expiryDateDays = int.Parse(expiryDateStr.Substring(6, 2));
                int expiryDateHours = int.Parse(expiryDateStr.Substring(8, 2));
                int expiryDateMins = int.Parse(expiryDateStr.Substring(10, 2));
                this.endDate = new DateTime(expiryDateYears, expiryDateMonths, expiryDateDays, expiryDateHours, expiryDateMins, 0);
            }
            else
            {
                this.endDate = new DateTime(2029, 12, 31);
            }
        }

    }
}
