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
        public bool advertDateHasBeenSet = false;

        public static string jingleStudioPathValue { get; private set; }

        public adForm()
        {
            InitializeComponent();
            DateTime tomorrow = DateTime.Today.AddDays(1);
            createBreaksForPicker.Value = tomorrow;
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
                getPromosInFolder(promoPath);
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
            if (advertPath.Length > 0 && promoPath.Length > 0 && targetPath.Length > 0 && advertDateHasBeenSet)
            {
                generateBreaksButton.Enabled = true;
            }
        }

        private void generateBreaksButton_Click(object sender, EventArgs e)
        {
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
            string[] allBreaks = breakTimesOnDayTextBox.Lines;
            foreach (string breakId in allBreaks)
            {
                adBreak tmpBreak = new adBreak();
                DateTime breakTargetTime = createBreaksForPicker.Value.Date;
                TimeSpan advertTime = new TimeSpan(int.Parse(breakId.Substring(0, 2)), int.Parse(breakId.Substring(3, 2)), 0);
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
            if (suggestedAd.canAdBePlayedInBreak(breakTargetTime))
            {
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
            if (adsToPlayPerDay.Count < 1) { return null; }
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
                for (int i = 0; i < tmpAd.targetPlaysPerDay; i++)
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
                tmpPromo.targetPath = promosStudioPath.Text + tmpPromo.filename;
                tmpPromo.getMP3Comment();
                tmpPromo.deserialiseSavedInfo();
                tmpPromo.setPriority(createBreaksForPicker.Value);
                tmpPromo.addToPromoTablePanel(promoTableLayoutPanel);
                // Add promo mulitple times depending on priorities to weight the probabilityu of it being chosen
                allPromos.Add(tmpPromo);
                if (tmpPromo.priority < 3)
                {
                    allPromos.Add(tmpPromo);
                    allPromos.Add(tmpPromo);
                }
                if (tmpPromo.priority < 2)
                {
                    allPromos.Add(tmpPromo);
                    allPromos.Add(tmpPromo);
                    allPromos.Add(tmpPromo);
                }
            }
        }



        private void adForm_Load(object sender, EventArgs e)
        {
            // From: https://stackoverflow.com/questions/28511858/update-label-through-another-class
            adBreak.StatusTextChanged += (sender1, e1) => fileBeingGeneratedLabel.Text = adBreak.StatusText;
        }

        private void createBreaksForPicker_ValueChanged(object sender, EventArgs e)
        {
            DateTime breakDate = createBreaksForPicker.Value.Date;
            string[] breaksOnDay = new string[] { "Select the day to set advert times" };
            switch (breakDate.DayOfWeek)
            {
                case DayOfWeek.Monday: string[] breaksOnMonday = { "06:20", "06:40", "07:20", "07:40", "08:20", "08:40", "09:20", "09:40", "10:20", "10:40", "11:20", "11:40", "12:20", "12:40", "13:20", "13:40", "14:20", "14:40", "15:20", "15:40", "16:20", "16:40", "17:20", "17:40", "18:20", "18:40", "20:59", "22:59" }; breaksOnDay = breaksOnMonday; break;
                case DayOfWeek.Tuesday: string[] breaksOnTuesday = { "06:20", "06:40", "07:20", "07:40", "08:20", "08:40", "09:20", "09:40", "10:20", "10:40", "11:20", "11:40", "12:20", "12:40", "13:20", "13:40", "14:20", "14:40", "15:20", "15:40", "16:20", "16:40", "17:20", "17:40", "18:20", "18:40", "20:59", "22:59" }; breaksOnDay = breaksOnTuesday; break;
                case DayOfWeek.Wednesday: string[] breaksOnWednesday = { "06:20", "06:40", "07:20", "07:40", "08:20", "08:40", "09:20", "09:40", "10:20", "10:40", "11:20", "11:40", "12:20", "12:40", "13:20", "13:40", "14:20", "14:40", "15:20", "15:40", "16:20", "16:40", "17:20", "17:40", "18:20", "18:40", "21:59" }; breaksOnDay = breaksOnWednesday; break;
                case DayOfWeek.Thursday: string[] breaksOnThursday = { "06:20", "06:40", "07:20", "07:40", "08:20", "08:40", "09:20", "09:40", "10:20", "10:40", "11:20", "11:40", "12:20", "12:40", "13:20", "13:40", "14:20", "14:40", "15:20", "15:40", "16:20", "16:40", "17:20", "17:40", "18:20", "18:40", "21:59" }; breaksOnDay = breaksOnThursday; break;
                case DayOfWeek.Friday: string[] breaksOnFriday = { "06:20", "06:40", "07:20", "07:40", "08:20", "08:40", "09:20", "09:40", "10:20", "10:40", "11:20", "11:40", "12:20", "12:40", "13:20", "13:40", "14:20", "14:40", "15:20", "15:40", "16:20", "16:40", "17:20", "17:40", "21:59" }; breaksOnDay = breaksOnFriday; break;
                case DayOfWeek.Saturday: string[] breaksOnSaturday = { "08:20", "08:40", "09:20", "09:40", "10:20", "10:40", "11:20", "11:40" }; breaksOnDay = breaksOnSaturday; break;
                case DayOfWeek.Sunday: string[] breaksOnSunday = { "08:20", "08:40", "09:20", "09:40", "10:20", "10:40", "11:20", "11:40", "12:20", "12:40", "12:59" }; breaksOnDay = breaksOnSunday; break;
            }
            breakTimesOnDayTextBox.Lines = breaksOnDay;
            advertDateHasBeenSet = true;
            canGenerateButtonBeDisabled();
        }
    }
}




        [Serializable]
        internal class NotEnoughFileOptionsException : Exception
        {

            public NotEnoughFileOptionsException(string message) : base(message)
            {
            }

        }
 