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
                tmpPromo.setPriority();
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
