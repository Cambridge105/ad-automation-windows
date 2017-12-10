using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ad_automation
{
    public class advert : audioFile
    {
        public string keyword;
        public int targetPlaysPerDay = 2;

        public bool canAdBePlayedInBreak(DateTime breakTime)
        {
            bool retVal = true;
            if (!isAdBreakBetweenStartAndEndDates(breakTime)) { retVal = false; }
            if (categoryHasBeenPlayedThisBreak()) { retVal = false; }
            if (hasFileBeenPlayedThisOrLastBreak()) { retVal = false; }
            return retVal;
        }

        private bool categoryHasBeenPlayedThisBreak()
        {
            if (keyword != null)
            {
                System.Threading.Thread.Sleep(20); // Hack to avoid race condition where this method can return true when it's actually false
                if (adForm.thisBreakKeywords.Contains(keyword.ToLower()))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        internal string createLogFileEntry()
        {
            string output = this.filename + "\r\nPlayed at:\r\n";
            output += String.Join("\r\n", this.inBreaks);
            output += "\r\nTarget plays per day: " + this.targetPlaysPerDay.ToString() + " | Total plays: " + this.inBreaks.Count.ToString();
            output += "\r\n\r\n";
            return output;
        }

        internal void addToAdvertTablePanel(TableLayoutPanel advertTablePanel)
        {
            TextBox tmpKeyWordTextBox = new TextBox { Text = keyword };
            tmpKeyWordTextBox.TextChanged += new System.EventHandler(tmpKeyWordTextBox_TextChanged);
            DateTimePicker tmpStartDatePicker = new DateTimePicker { Value = startDate };
            tmpStartDatePicker.TextChanged += new System.EventHandler(tmpStartDatePicker_TextChanged);
            DateTimePicker tmpEndDatePicker = new DateTimePicker { Value = endDate };
            tmpEndDatePicker.TextChanged += new System.EventHandler(tmpEndDatePicker_TextChanged);
            TextBox tmpPlaysPerDayTextBox = new TextBox { Text = targetPlaysPerDay.ToString() };
            tmpPlaysPerDayTextBox.TextChanged += new System.EventHandler(tmpPlaysPerDayTextBox_TextChanged);
            Button tmpButton = new Button {Text = "Save" };
            tmpButton.Click += new System.EventHandler(tmpButton_Click);


            advertTablePanel.RowCount = advertTablePanel.RowCount + 1;
            advertTablePanel.Controls.Add(new Label() { Text = filename }, 0, advertTablePanel.RowCount);
            advertTablePanel.Controls.Add(tmpKeyWordTextBox, 1,  advertTablePanel.RowCount);
            advertTablePanel.Controls.Add(tmpStartDatePicker, 2,  advertTablePanel.RowCount);
            advertTablePanel.Controls.Add(tmpEndDatePicker, 3,  advertTablePanel.RowCount);
            advertTablePanel.Controls.Add(tmpPlaysPerDayTextBox, 4,  advertTablePanel.RowCount);
           advertTablePanel.Controls.Add(tmpButton, 5,  advertTablePanel.RowCount);
        }

        private void tmpPlaysPerDayTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tmpTextBox = sender as TextBox;
            targetPlaysPerDay = int.Parse(tmpTextBox.Text);
        }

        private void tmpEndDatePicker_TextChanged(object sender, EventArgs e)
        {
            DateTimePicker tmpDatePicker = sender as DateTimePicker;
            endDate = DateTime.Parse(tmpDatePicker.Text);
        }

        private void tmpStartDatePicker_TextChanged(object sender, EventArgs e)
        {
            DateTimePicker tmpDatePicker = sender as DateTimePicker;
            startDate = DateTime.Parse(tmpDatePicker.Text);
        }

        private void tmpKeyWordTextBox_TextChanged(object sender, EventArgs e)
        {
            TextBox tmpTextBox = sender as TextBox;
            keyword = tmpTextBox.Text;
        }

        protected void tmpButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            writeMetadataToMP3();
        }

        private void writeMetadataToMP3()
        {
            TagLib.File tmpFile = TagLib.File.Create(originalPath);
            string output = JsonConvert.SerializeObject(this);
            tmpFile.Tag.Comment = mp3CommentString + " ###ADDATA###" + output;
            tmpFile.Save();
        }

        internal void deserialiseSavedInfo()
        {
            if (serialisedJsonBlock != null && serialisedJsonBlock.Length>1)
            {
                advert tmpAd = JsonConvert.DeserializeObject<advert>(serialisedJsonBlock);
                keyword = tmpAd.keyword;
                targetPlaysPerDay = tmpAd.targetPlaysPerDay;
                startDate = tmpAd.startDate;
                endDate = tmpAd.endDate;
                serialisedJsonBlock = "";
            }
        }
    }
}
