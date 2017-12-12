using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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


        internal void deserialiseSavedInfo()
        {
            if (serialisedJsonBlock != null && serialisedJsonBlock.Length > 1)
            {
                promo tmpPromo = JsonConvert.DeserializeObject<promo>(serialisedJsonBlock);
                priority = tmpPromo.priority;
                startDate = tmpPromo.startDate;
                endDate = tmpPromo.endDate;
                serialisedJsonBlock = "";
            }
        }

        internal void setPriority(DateTime breakTime)
        {
            if (endDate.Subtract(breakTime).TotalHours < 72) { priority = 1; }
            else if (filename.Contains("Cam105")) { priority = 3; }
            else { priority = 2; }
        }


        internal void addToPromoTablePanel(TableLayoutPanel promoTablePanel)
        {
            DateTimePicker tmpStartDatePicker = new DateTimePicker { Value = startDate };
            tmpStartDatePicker.TextChanged += new System.EventHandler(tmpStartDatePicker_TextChanged);
            DateTimePicker tmpEndDatePicker = new DateTimePicker { Value = endDate, Format = DateTimePickerFormat.Custom, CustomFormat = "dd MMM yyyy HH:mm:ss" };
            tmpEndDatePicker.TextChanged += new System.EventHandler(tmpEndDatePicker_TextChanged);
            Button tmpButton = new Button { Text = "Save" };
            tmpButton.Click += new System.EventHandler(tmpButton_Click);

            promoTablePanel.RowCount = promoTablePanel.RowCount + 1;
            promoTablePanel.Controls.Add(new Label() { Text = filename, AutoSize=true }, 0, promoTablePanel.RowCount);
            promoTablePanel.Controls.Add(tmpStartDatePicker, 1, promoTablePanel.RowCount);
            promoTablePanel.Controls.Add(tmpEndDatePicker, 2, promoTablePanel.RowCount);
            promoTablePanel.Controls.Add(tmpButton, 4, promoTablePanel.RowCount);
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

    }
}
