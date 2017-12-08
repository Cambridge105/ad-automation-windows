using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ad_automation
{
    class adBreak
    {
        public DateTime breakTime;
        public bool isOvernight;
        public int targetAds;
        public int targetPromos;
        public int actualAds = 0;
        public int actualPromos = 0;
        public List<audioFile> breakContents = new List<audioFile>();
        public string outputFilename;
        public string outputFullPath;
        public int numFailures = 0;

        public static string StatusText { get; private set; }
        public static event EventHandler StatusTextChanged;


        public void generateBreak()
        {
            ChangeStatusText("Saving: " + outputFilename);
            addRandomBumper("Into");
            addAdverts();
            addPromos();
            addRandomBumper("Out of");
            adForm.lastBreak.Clear();
            adForm.lastBreak = new List<audioFile>(adForm.thisBreak);
            adForm.thisBreak.Clear();
        }

        public void outputM3UFile()
        {
            string m3uOut = "";
            foreach (audioFile content in breakContents)
            {
                m3uOut += content.targetPath + "\r\n";
            }
            System.IO.File.WriteAllText(outputFullPath, m3uOut);
        }

        private void addPromos()
        {
            for (int i = 0; i < targetPromos;)
            {
                promo newPromo = adForm.selectPromo(breakTime);
                if (newPromo != null)
                {
                    breakContents.Add(newPromo);
                    actualPromos++;
                    var index = adForm.allPromos.FindIndex(x => x.filename == newPromo.filename);
                    adForm.allPromos[index].inBreaks.Add(breakTime);
                    adForm.thisBreak.Add(newPromo);
                    i++;
                    numFailures = 0;
                }
                else
                {
                    if (numFailures > 99)
                    {
                        throw new NotEnoughFileOptionsException("No files haven't been played this or last break");
                    }
                    else { numFailures++; }
                }
            }
        }

        private void addRandomBumper(string bumperType)
        {
            bumper tmpBumper = new bumper();
            tmpBumper.filename = bumperType + " Break ID " + tmpBumper.randomLetterAToG() + ".mp3";
            tmpBumper.targetPath = adForm.jingleStudioPathValue + tmpBumper.filename;
            tmpBumper.inBreaks.Add(breakTime);
            breakContents.Add(tmpBumper);
        }

        private void addAdverts()
        {
            int numFailures = 0;
            for (int i = 0; i < targetAds;)
            {
                advert newAdvert = adForm.selectAd(breakTime);
                if (newAdvert != null)
                {
                    breakContents.Add(newAdvert);
                    actualAds++;
                    var index = adForm.allAdverts.FindIndex(x => x.filename == newAdvert.filename);
                    adForm.allAdverts[index].inBreaks.Add(breakTime);
                    adForm.thisBreak.Add(newAdvert);
                    i++;
                    numFailures = 0;
                }
                else
                {
                    if (numFailures > 99)
                    {
                        // By the time we've tried 99 times to find an advert that can be played, we give up and play a promo instead
                        //targetPromos++;
                    }
                    else { numFailures++; }
                }
            }
        }

        static void ChangeStatusText(string text)
        {
            StatusText = text;
            EventHandler handler = StatusTextChanged;
            if (handler != null)
            {
                handler(null, EventArgs.Empty);
            }
        }

        internal void setIsOvernight()
        {
            if (breakTime.Hour < 6 || breakTime.Hour >= 23)
            {
                isOvernight = true;
            }
            else
            {
                isOvernight = false;
            }
        }
    }
}
