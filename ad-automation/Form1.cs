using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TagLib;
using System.Text.RegularExpressions;

namespace ad_automation
{
    public partial class adForm : Form
    {

        string advertPath = "";
        string promoPath = "";
        string targetPath = "";
        int adsPerBreak = 2;
        int promosPerBreak = 3;
        DateTime nextMonday;
        public static List<advert> allAdverts = new List<advert>();
        public static List<advert> adsToPlayPerDay = new List<advert>(); // List of adverts by number of plays needed
        public static List<promo> allPromos = new List<promo>();
        List<adBreak> allBreaksList = new List<adBreak>();
        public static List<audioFile> thisBreak = new List<audioFile>();
        public static List<audioFile> lastBreak = new List<audioFile>();
        public static List<string> thisBreakKeywords = new List<string>();

        public static string jingleStudioPathValue { get; private set; }

        public adForm()
        {
            InitializeComponent();
            nextMonday = DateTime.Today.AddDays(1);
            while (nextMonday.DayOfWeek != DayOfWeek.Monday)
            {
                nextMonday = nextMonday.AddDays(1);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            advertPath = getAdvertFolder();
            canGenerateButtonBeDisabled();
        }

        private void promosFolderSelectorButton_Click(object sender, EventArgs e)
        {
            promoPath = getPromosFolder();
            canGenerateButtonBeDisabled();
        }

        private void savePlaylistsInButton_Click(object sender, EventArgs e)
        {
            targetPath = getTargetFolder();
            canGenerateButtonBeDisabled();
        }

        private string getAdvertFolder()
        {
            FolderBrowserDialog advertBrowserDialog = new FolderBrowserDialog();
            if (advertBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string advertPath = advertBrowserDialog.SelectedPath;
                advertFolderSelectedLabel.Text = advertPath;
                getAdvertsInFolder(advertPath);
                return advertPath;
            }
            return "";
        }

        private string getPromosFolder()
        {
            FolderBrowserDialog promoBrowserDialog = new FolderBrowserDialog();
            if (promoBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string promoPath = promoBrowserDialog.SelectedPath;
                promosFolderSelectedLabel.Text = promoPath;
                return promoPath;
            }
            return "";
        }

        private string getTargetFolder()
        {
            FolderBrowserDialog saveBrowserDialog = new FolderBrowserDialog();
            if (saveBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                string savePath = saveBrowserDialog.SelectedPath;
                savePlaylistsInFolderSelectedLabel.Text = savePath;
                return savePath;
            }
            return "";
        }


        private void canGenerateButtonBeDisabled()
        {
            if (advertPath.Length>0 && promoPath.Length>0 && targetPath.Length>0)
            {
                 generateBreaksButton.Enabled = true;
            }
        }

        private void generateBreaksButton_Click(object sender, EventArgs e)
        {
            getPromosInFolder(promoPath);
            createTargetDirectory();
            createBreaks();
            jingleStudioPathValue = jinglesStudioPath.Text;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = allBreaksList.Count;
            progressBar1.Value = 1;
            progressBar1.Step = 1;
            foreach (adBreak thisBreak in allBreaksList)
            {
                thisBreak.generateBreak();
                thisBreak.outputM3UFile();
                progressBar1.PerformStep();
                Application.DoEvents(); // Updates label containing file being created
            }
            outputLog();
            fileBeingGeneratedLabel.Text = "Done";
            Process.Start(targetPath);  // Opens Windows Explorer to output directory
        }

        private void outputLog()
        {
            // Create log files
            string logFileOutput = "";
            foreach (advert tmpAd in allAdverts)
            {
                logFileOutput += tmpAd.createLogFileEntry();
            }
            System.IO.File.WriteAllText(targetPath + "\\adverts.txt", logFileOutput);
            logFileOutput = "";
            foreach (promo tmpPromo in allPromos)
            {
                logFileOutput += tmpPromo.createLogFileEntry();
            }
            System.IO.File.WriteAllText(targetPath + "\\promos.txt", logFileOutput);
        }

        private void createBreaks()
        {
            // TODO: Split this out into a config file
            List<string> allBreaksLst = new List<string> { "Mon_0620", "Mon_0640", "Mon_0720", "Mon_0740", "Mon_0820", "Mon_0840", "Mon_0920", "Mon_0940", "Mon_1020", "Mon_1040", "Mon_1120", "Mon_1140", "Mon_1220", "Mon_1240", "Mon_1320", "Mon_1340", "Mon_1420", "Mon_1440", "Mon_1520", "Mon_1540", "Mon_1620", "Mon_1640", "Mon_1720", "Mon_1740", "Mon_1820", "Mon_1840"};
            string[] allBreaks = allBreaksLst.ToArray();
            foreach (string breakId in allBreaks)
            {
                adBreak tmpBreak = new adBreak();
                DateTime breakTargetTime = new DateTime();
                TimeSpan advertTime = new TimeSpan(int.Parse(breakId.Substring((breakId.Length - 4), 2)), int.Parse(breakId.Substring((breakId.Length - 2), 2)), 0);
                if (breakId.Contains("Mon_")) { breakTargetTime = nextMonday; }
                else if (breakId.Contains("Tue_")) { breakTargetTime = nextMonday.AddDays(1); }
                else if (breakId.Contains("Wed_")) { breakTargetTime = nextMonday.AddDays(2); }
                else if (breakId.Contains("Thur_")) { breakTargetTime = nextMonday.AddDays(3); }
                else if (breakId.Contains("Fri_")) { breakTargetTime = nextMonday.AddDays(4); }
                else if (breakId.Contains("Sat_")) { breakTargetTime = nextMonday.AddDays(5); }
                else if (breakId.Contains("Sun_")) { breakTargetTime = nextMonday.AddDays(6); }
                breakTargetTime = breakTargetTime.Add(advertTime);
                tmpBreak.breakTime = breakTargetTime;
                tmpBreak.outputFilename = breakTargetTime.ToString("yyyyMMddHHmm") + ".m3u";
                tmpBreak.outputFullPath = targetPath + "\\" + tmpBreak.outputFilename;
                double adsToPlayPerBreak = (double)adsToPlayPerDay.Count / allBreaks.Length;
                double adsToPlayPerBreakCeil = Math.Ceiling(adsToPlayPerBreak);
                tmpBreak.targetAds = (int)adsToPlayPerBreakCeil;
                tmpBreak.targetPromos = 5 - tmpBreak.targetAds;
                tmpBreak.setIsOvernight();
                allBreaksList.Add(tmpBreak);
                // TODO: Handle overnight/evenings
            }
        }

        public static advert selectAd(DateTime breakTargetTime)
        {
            advert suggestedAd = chooseAdvertAtRandom();
            if (suggestedAd == null) { return null; }
            if (suggestedAd.canAdBePlayedInBreak(breakTargetTime)) {
                adsToPlayPerDay.Remove(suggestedAd);
                return suggestedAd;
            }
            return null;
        }

        public static promo selectPromo(DateTime breakTargetTime)
        {
            promo suggestedPromo = choosePromoAtRandom();
            if (suggestedPromo.canPromoBePlayedInBreak(breakTargetTime)) { return suggestedPromo; }
            return null;
        }

        private static promo choosePromoAtRandom()
        {
            Random rnd = new Random();
            int numericRnd = rnd.Next(0, allPromos.Count);
            return allPromos[numericRnd];
        }

        private static advert chooseAdvertAtRandom()
        {
            if (adsToPlayPerDay.Count <1) { return null; }
            Random rnd = new Random();
            int numericRnd = rnd.Next(0, adsToPlayPerDay.Count);
            return adsToPlayPerDay[numericRnd];
        }
        /*
        private void old_generateBreaksButton_Click(object sender, EventArgs e)
        {
            availableAds = getMP3sInFolder(advertPath);
            availablePromos = getMP3sInFolder(promoPath);
            setUpAdLog();
            createTargetDirectory();
            List<string> allBreaksLst = new List<string> { "_Overnight1", "_Overnight2", "_Overnight3", "_Overnight4", "_Overnight5", "_Overnight6", "Mon_0620", "Mon_0640", "Mon_0720", "Mon_0740", "Mon_0820", "Mon_0840", "Mon_0920", "Mon_0940", "Mon_1020", "Mon_1040", "Mon_1120", "Mon_1140", "Mon_1220", "Mon_1240", "Mon_1320", "Mon_1340", "Mon_1420", "Mon_1440", "Mon_1520", "Mon_1540", "Mon_1620", "Mon_1640", "Mon_1720", "Mon_1740", "Mon_1820", "Mon_1840", "Tue_0620", "Tue_0640", "Tue_0720", "Tue_0740", "Tue_0820", "Tue_0840", "Tue_0920", "Tue_0940", "Tue_1020", "Tue_1040", "Tue_1120", "Tue_1140", "Tue_1220", "Tue_1240", "Tue_1320", "Tue_1340", "Tue_1420", "Tue_1440", "Tue_1520", "Tue_1540", "Tue_1620", "Tue_1640", "Tue_1720", "Tue_1740", "Tue_1820", "Tue_1840", "Wed_0620", "Wed_0640", "Wed_0720", "Wed_0740", "Wed_0820", "Wed_0840", "Wed_0920", "Wed_0940", "Wed_1020", "Wed_1040", "Wed_1120", "Wed_1140", "Wed_1220", "Wed_1240", "Wed_1320", "Wed_1340", "Wed_1420", "Wed_1440", "Wed_1520", "Wed_1540", "Wed_1620", "Wed_1640", "Wed_1720", "Wed_1740", "Wed_1820", "Wed_1840", "Thur_0620", "Thur_0640", "Thur_0720", "Thur_0740", "Thur_0820", "Thur_0840", "Thur_0920", "Thur_0940", "Thur_1020", "Thur_1040", "Thur_1120", "Thur_1140", "Thur_1220", "Thur_1240", "Thur_1320", "Thur_1340", "Thur_1420", "Thur_1440", "Thur_1520", "Thur_1540", "Thur_1620", "Thur_1640", "Thur_1720", "Thur_1740", "Thur_1820", "Thur_1840", "Fri_0620", "Fri_0640", "Fri_0720", "Fri_0740", "Fri_0820", "Fri_0840", "Fri_0920", "Fri_0940", "Fri_1020", "Fri_1040", "Fri_1120", "Fri_1140", "Fri_1220", "Fri_1240", "Fri_1320", "Fri_1340", "Fri_1420", "Fri_1440", "Fri_1520", "Fri_1540", "Fri_1620", "Fri_1640", "Fri_1720", "Fri_1740", "Sat_0820", "Sat_0840", "Sat_0920", "Sat_0940", "Sat_1020", "Sat_1040", "Sat_1120", "Sat_1140", "Sun_0820", "Sun_0840", "Sun_0920", "Sun_0940", "Sun_1020", "Sun_1040", "Sun_1120", "Sun_1140", "Sun_1220", "Sun_1240"};
            List<string> eveningBreaks = addEveningBreaks();
            allBreaksLst.AddRange(eveningBreaks);
            string[] allBreaks = allBreaksLst.ToArray();
            int totalBreaks = allBreaks.Length;
            int doneSoFar = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = totalBreaks;
            progressBar1.Value = 1;
            progressBar1.Step = 1;
            foreach (string breakId in allBreaks)
            {
                doneSoFar++;
                string m3uOut = "";
                DateTime breakTargetTime = new DateTime();
                string breakIdWithDate = "";
                if (!breakId.Contains("Overnight") && !breakId.Contains("Eve"))
                {
                    TimeSpan advertTime = new TimeSpan(int.Parse(breakId.Substring((breakId.Length - 4),2)), int.Parse(breakId.Substring((breakId.Length -2),2)), 0);
                    if (breakId.Contains("Mon_")) { breakTargetTime = nextMonday; }
                    else if (breakId.Contains("Tue_")) { breakTargetTime = nextMonday.AddDays(1); }
                    else if (breakId.Contains("Wed_")) { breakTargetTime = nextMonday.AddDays(2); }
                    else if (breakId.Contains("Thur_")) { breakTargetTime = nextMonday.AddDays(3); }
                    else if (breakId.Contains("Fri_")) { breakTargetTime = nextMonday.AddDays(4); }
                    else if (breakId.Contains("Sat_")) { breakTargetTime = nextMonday.AddDays(5); }
                    else if (breakId.Contains("Sun_")) { breakTargetTime = nextMonday.AddDays(6); }
                    breakTargetTime = breakTargetTime.Add(advertTime);
                    breakIdWithDate = breakTargetTime.ToString("yyyyMMddHHmm");
                }
                else
                {
                    breakIdWithDate = breakId;
                }
                fileBeingGeneratedLabel.Text = "Saving: " + breakIdWithDate + ".m3u";
                m3uOut += addBreakBumper("Into");
                for (int i=0; i<(adsPerBreak);)
                {
                    string advert = selectAd(breakTargetTime);
                    if (advert.Length > 1)
                    {
                        thisBreak.Add(advert);
                        string exitingLogEntry = advertLog[advert];
                        advertLog[advert] = exitingLogEntry + "\r\n--" + breakId;
                        if (!breakId.Contains("Overnight"))
                        {
                            // Don't count plays overnight
                            int existingPlays = advertPlays[advert];
                            advertPlays[advert] = existingPlays + 1;
                        }
                        m3uOut += studioAdvertsPath.Text  +  advert + "\n";
                        i++;
                    }
                    else
                    {
                        if (numFailures > 99)
                        {
                            // By the time we've tried 99 times to find an advert that can be played, we give up and play a promo instead
                            promosPerBreak = promosPerBreak + 1;
                        }
                        else { numFailures++;  }
                    }
                }
                numFailures = 0;
                for (int j = 0; j < (promosPerBreak);)
                {
                    string promo = selectPromo(breakTargetTime);
                    if (promo.Length > 1)
                    {
                        thisBreak.Add(promo);
                        string exitingLogEntry = promoLog[promo];
                        promoLog[promo] = exitingLogEntry + "\r\n--" + breakId;
                        int existingPlays = promoPlays[promo];
                        promoPlays[promo] = existingPlays + 1;
                        m3uOut += promosStudioPath.Text + promo + "\n";
                        j++;
                    }
                    else
                    {
                        numFailures++;
                        if (numFailures > 99)
                        {
                            throw new NotEnoughFileOptionsException("No files haven't been played this or last break");
                        }
                    }
                }
                m3uOut += addBreakBumper("Out Of");
                lastBreak = thisBreak;
                thisBreak = new List<string>();
                numFailures = 0;
                progressBar1.PerformStep();
                Application.DoEvents();
                Console.WriteLine("== WRITING " + breakIdWithDate + ".m3u");
                System.IO.File.WriteAllText(targetPath + "\\" + breakIdWithDate + ".m3u", m3uOut);
            }
            outputLog();
            fileBeingGeneratedLabel.Text = "Done";
            Process.Start(targetPath);
        }

        */
        private void createTargetDirectory()
        {
            string dateTime = DateTime.Now.ToString();
            dateTime = dateTime.Replace("/", "");
            dateTime = dateTime.Replace(" ", "");
            dateTime = dateTime.Replace(":", "");
            targetPath = targetPath + "\\" + dateTime;
            Directory.CreateDirectory(targetPath);
        }


        private void getAdvertsInFolder(string folderPath)
        {
            DirectoryInfo d = new DirectoryInfo(folderPath);
            advertTablePanel.RowCount = 0;
            advertTablePanel.RowStyles.Clear();
            advertTablePanel.AutoScroll = true;
            FileInfo[] Files = d.GetFiles("*.mp3");
            foreach (FileInfo file in Files)
            {
                advert tmpAd = new advert();
                tmpAd.filename = file.Name;
                tmpAd.originalPath = file.FullName;
                tmpAd.targetPath = studioAdvertsPath.Text + tmpAd.filename;
                tmpAd.getMP3Comment();
                tmpAd.deserialiseSavedInfo();
                tmpAd.addToAdvertTablePanel(advertTablePanel);
                allAdverts.Add(tmpAd);
                for (int i = 0; i<tmpAd.targetPlaysPerDay; i++)
                {
                    adsToPlayPerDay.Add(tmpAd);
                }
            }
        }

        private void getPromosInFolder(string folderPath)
        {
            DirectoryInfo d = new DirectoryInfo(folderPath);
            FileInfo[] Files = d.GetFiles("*.mp3");
            foreach (FileInfo file in Files)
            {
                promo tmpPromo = new promo();
                tmpPromo.filename = file.Name;
                tmpPromo.originalPath = file.FullName;
                tmpPromo.targetPath =  promosStudioPath.Text +  tmpPromo.filename;
                tmpPromo.getMP3Comment();
                allPromos.Add(tmpPromo);
            }
        }

 

        private void adForm_Load(object sender, EventArgs e)
        {
            // From: https://stackoverflow.com/questions/28511858/update-label-through-another-class
            adBreak.StatusTextChanged += (sender1, e1) => fileBeingGeneratedLabel.Text = adBreak.StatusText;
        }
    }

    [Serializable]
    internal class NotEnoughFileOptionsException : Exception
    {
        
        public NotEnoughFileOptionsException(string message) : base(message)
        {
        }
        
    }
}
