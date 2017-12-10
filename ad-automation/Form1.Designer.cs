namespace ad_automation
{
    partial class adForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(adForm));
            this.inputGroupBox = new System.Windows.Forms.GroupBox();
            this.categoryHelpTextLabel = new System.Windows.Forms.Label();
            this.advertTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.categoryLabel = new System.Windows.Forms.Label();
            this.filenameLabel = new System.Windows.Forms.Label();
            this.saveTrackDataLabel = new System.Windows.Forms.Label();
            this.playsPerDayLabel = new System.Windows.Forms.Label();
            this.playUntilLabel = new System.Windows.Forms.Label();
            this.startDateLabel = new System.Windows.Forms.Label();
            this.studioAdvertsPath = new System.Windows.Forms.TextBox();
            this.advertsStudioPathLabel = new System.Windows.Forms.Label();
            this.advertFolderSelectedLabel = new System.Windows.Forms.Label();
            this.advertsSelectFolderButton = new System.Windows.Forms.Button();
            this.jinglesStudioPath = new System.Windows.Forms.TextBox();
            this.jinglesStudioPathLabel = new System.Windows.Forms.Label();
            this.jinglesLabel = new System.Windows.Forms.Label();
            this.promosStudioPath = new System.Windows.Forms.TextBox();
            this.promosStudioPathLabel = new System.Windows.Forms.Label();
            this.promosFolderSelectedLabel = new System.Windows.Forms.Label();
            this.promosFolderSelectorButton = new System.Windows.Forms.Button();
            this.createBreaksForLabel = new System.Windows.Forms.Label();
            this.generateBreaksButton = new System.Windows.Forms.Button();
            this.savePlaylistsInButton = new System.Windows.Forms.Button();
            this.savePlaylistsInFolderSelectedLabel = new System.Windows.Forms.Label();
            this.promosGroupBox = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.fileBeingGeneratedLabel = new System.Windows.Forms.Label();
            this.createBreaksForPicker = new System.Windows.Forms.DateTimePicker();
            this.breaksToCreateGroupBox = new System.Windows.Forms.GroupBox();
            this.inputGroupBox.SuspendLayout();
            this.advertTablePanel.SuspendLayout();
            this.promosGroupBox.SuspendLayout();
            this.breaksToCreateGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputGroupBox
            // 
            this.inputGroupBox.Controls.Add(this.categoryHelpTextLabel);
            this.inputGroupBox.Controls.Add(this.advertTablePanel);
            this.inputGroupBox.Controls.Add(this.studioAdvertsPath);
            this.inputGroupBox.Controls.Add(this.advertsStudioPathLabel);
            this.inputGroupBox.Controls.Add(this.advertFolderSelectedLabel);
            this.inputGroupBox.Controls.Add(this.advertsSelectFolderButton);
            this.inputGroupBox.Location = new System.Drawing.Point(12, 12);
            this.inputGroupBox.Name = "inputGroupBox";
            this.inputGroupBox.Size = new System.Drawing.Size(910, 235);
            this.inputGroupBox.TabIndex = 0;
            this.inputGroupBox.TabStop = false;
            this.inputGroupBox.Text = "Adverts:";
            // 
            // categoryHelpTextLabel
            // 
            this.categoryHelpTextLabel.AutoSize = true;
            this.categoryHelpTextLabel.Location = new System.Drawing.Point(10, 205);
            this.categoryHelpTextLabel.Name = "categoryHelpTextLabel";
            this.categoryHelpTextLabel.Size = new System.Drawing.Size(499, 13);
            this.categoryHelpTextLabel.TabIndex = 10;
            this.categoryHelpTextLabel.Text = "Category is optional. If provided, no two adverts with the same category will be " +
    "played in the same break.";
            // 
            // advertTablePanel
            // 
            this.advertTablePanel.AutoScroll = true;
            this.advertTablePanel.AutoScrollMargin = new System.Drawing.Size(3, 3);
            this.advertTablePanel.AutoScrollMinSize = new System.Drawing.Size(500, 75);
            this.advertTablePanel.AutoSize = true;
            this.advertTablePanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.advertTablePanel.ColumnCount = 6;
            this.advertTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 320F));
            this.advertTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.advertTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.advertTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 125F));
            this.advertTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.advertTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.advertTablePanel.Controls.Add(this.categoryLabel, 1, 0);
            this.advertTablePanel.Controls.Add(this.filenameLabel, 0, 0);
            this.advertTablePanel.Controls.Add(this.saveTrackDataLabel, 5, 0);
            this.advertTablePanel.Controls.Add(this.playsPerDayLabel, 4, 0);
            this.advertTablePanel.Controls.Add(this.playUntilLabel, 3, 0);
            this.advertTablePanel.Controls.Add(this.startDateLabel, 2, 0);
            this.advertTablePanel.Location = new System.Drawing.Point(3, 68);
            this.advertTablePanel.MaximumSize = new System.Drawing.Size(880, 130);
            this.advertTablePanel.MinimumSize = new System.Drawing.Size(880, 130);
            this.advertTablePanel.Name = "advertTablePanel";
            this.advertTablePanel.RowCount = 1;
            this.advertTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.advertTablePanel.Size = new System.Drawing.Size(880, 130);
            this.advertTablePanel.TabIndex = 9;
            // 
            // categoryLabel
            // 
            this.categoryLabel.AutoSize = true;
            this.categoryLabel.Location = new System.Drawing.Point(323, 3);
            this.categoryLabel.Margin = new System.Windows.Forms.Padding(3);
            this.categoryLabel.Name = "categoryLabel";
            this.categoryLabel.Size = new System.Drawing.Size(52, 13);
            this.categoryLabel.TabIndex = 5;
            this.categoryLabel.Text = "Category:";
            // 
            // filenameLabel
            // 
            this.filenameLabel.AutoSize = true;
            this.filenameLabel.Location = new System.Drawing.Point(3, 3);
            this.filenameLabel.Margin = new System.Windows.Forms.Padding(3);
            this.filenameLabel.Name = "filenameLabel";
            this.filenameLabel.Size = new System.Drawing.Size(49, 13);
            this.filenameLabel.TabIndex = 0;
            this.filenameLabel.Text = "Filename";
            // 
            // saveTrackDataLabel
            // 
            this.saveTrackDataLabel.AutoSize = true;
            this.saveTrackDataLabel.Location = new System.Drawing.Point(768, 3);
            this.saveTrackDataLabel.Margin = new System.Windows.Forms.Padding(3);
            this.saveTrackDataLabel.Name = "saveTrackDataLabel";
            this.saveTrackDataLabel.Size = new System.Drawing.Size(35, 13);
            this.saveTrackDataLabel.TabIndex = 4;
            this.saveTrackDataLabel.Text = "Save:";
            // 
            // playsPerDayLabel
            // 
            this.playsPerDayLabel.AutoSize = true;
            this.playsPerDayLabel.Location = new System.Drawing.Point(698, 3);
            this.playsPerDayLabel.Margin = new System.Windows.Forms.Padding(3);
            this.playsPerDayLabel.Name = "playsPerDayLabel";
            this.playsPerDayLabel.Size = new System.Drawing.Size(57, 13);
            this.playsPerDayLabel.TabIndex = 3;
            this.playsPerDayLabel.Text = "Plays/day:";
            // 
            // playUntilLabel
            // 
            this.playUntilLabel.AutoSize = true;
            this.playUntilLabel.Location = new System.Drawing.Point(573, 3);
            this.playUntilLabel.Margin = new System.Windows.Forms.Padding(3);
            this.playUntilLabel.Name = "playUntilLabel";
            this.playUntilLabel.Size = new System.Drawing.Size(52, 13);
            this.playUntilLabel.TabIndex = 2;
            this.playUntilLabel.Text = "Play until:";
            // 
            // startDateLabel
            // 
            this.startDateLabel.AutoSize = true;
            this.startDateLabel.Location = new System.Drawing.Point(448, 3);
            this.startDateLabel.Margin = new System.Windows.Forms.Padding(3);
            this.startDateLabel.Name = "startDateLabel";
            this.startDateLabel.Size = new System.Drawing.Size(53, 13);
            this.startDateLabel.TabIndex = 1;
            this.startDateLabel.Text = "Play from:";
            // 
            // studioAdvertsPath
            // 
            this.studioAdvertsPath.Location = new System.Drawing.Point(155, 42);
            this.studioAdvertsPath.Name = "studioAdvertsPath";
            this.studioAdvertsPath.Size = new System.Drawing.Size(253, 20);
            this.studioAdvertsPath.TabIndex = 8;
            this.studioAdvertsPath.Text = "D:\\Adverts\\";
            // 
            // advertsStudioPathLabel
            // 
            this.advertsStudioPathLabel.AutoSize = true;
            this.advertsStudioPathLabel.Location = new System.Drawing.Point(85, 45);
            this.advertsStudioPathLabel.Name = "advertsStudioPathLabel";
            this.advertsStudioPathLabel.Size = new System.Drawing.Size(64, 13);
            this.advertsStudioPathLabel.TabIndex = 6;
            this.advertsStudioPathLabel.Text = "Studio path:";
            // 
            // advertFolderSelectedLabel
            // 
            this.advertFolderSelectedLabel.AutoSize = true;
            this.advertFolderSelectedLabel.Location = new System.Drawing.Point(155, 24);
            this.advertFolderSelectedLabel.Name = "advertFolderSelectedLabel";
            this.advertFolderSelectedLabel.Size = new System.Drawing.Size(93, 13);
            this.advertFolderSelectedLabel.TabIndex = 2;
            this.advertFolderSelectedLabel.Text = "No folder selected";
            // 
            // advertsSelectFolderButton
            // 
            this.advertsSelectFolderButton.Location = new System.Drawing.Point(10, 19);
            this.advertsSelectFolderButton.Name = "advertsSelectFolderButton";
            this.advertsSelectFolderButton.Size = new System.Drawing.Size(139, 23);
            this.advertsSelectFolderButton.TabIndex = 0;
            this.advertsSelectFolderButton.Text = "Choose adverts folder...";
            this.advertsSelectFolderButton.UseVisualStyleBackColor = true;
            this.advertsSelectFolderButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // jinglesStudioPath
            // 
            this.jinglesStudioPath.Location = new System.Drawing.Point(170, 454);
            this.jinglesStudioPath.Name = "jinglesStudioPath";
            this.jinglesStudioPath.Size = new System.Drawing.Size(253, 20);
            this.jinglesStudioPath.TabIndex = 12;
            this.jinglesStudioPath.Text = "D:\\Jingles\\";
            // 
            // jinglesStudioPathLabel
            // 
            this.jinglesStudioPathLabel.AutoSize = true;
            this.jinglesStudioPathLabel.Location = new System.Drawing.Point(99, 457);
            this.jinglesStudioPathLabel.Name = "jinglesStudioPathLabel";
            this.jinglesStudioPathLabel.Size = new System.Drawing.Size(64, 13);
            this.jinglesStudioPathLabel.TabIndex = 11;
            this.jinglesStudioPathLabel.Text = "Studio path:";
            // 
            // jinglesLabel
            // 
            this.jinglesLabel.AutoSize = true;
            this.jinglesLabel.Location = new System.Drawing.Point(34, 457);
            this.jinglesLabel.Name = "jinglesLabel";
            this.jinglesLabel.Size = new System.Drawing.Size(42, 13);
            this.jinglesLabel.TabIndex = 10;
            this.jinglesLabel.Text = "Jingles:";
            // 
            // promosStudioPath
            // 
            this.promosStudioPath.Location = new System.Drawing.Point(157, 51);
            this.promosStudioPath.Name = "promosStudioPath";
            this.promosStudioPath.Size = new System.Drawing.Size(253, 20);
            this.promosStudioPath.TabIndex = 9;
            this.promosStudioPath.Text = "D:\\Promos\\";
            // 
            // promosStudioPathLabel
            // 
            this.promosStudioPathLabel.AutoSize = true;
            this.promosStudioPathLabel.Location = new System.Drawing.Point(87, 54);
            this.promosStudioPathLabel.Name = "promosStudioPathLabel";
            this.promosStudioPathLabel.Size = new System.Drawing.Size(64, 13);
            this.promosStudioPathLabel.TabIndex = 7;
            this.promosStudioPathLabel.Text = "Studio path:";
            // 
            // promosFolderSelectedLabel
            // 
            this.promosFolderSelectedLabel.AutoSize = true;
            this.promosFolderSelectedLabel.Location = new System.Drawing.Point(157, 28);
            this.promosFolderSelectedLabel.Name = "promosFolderSelectedLabel";
            this.promosFolderSelectedLabel.Size = new System.Drawing.Size(93, 13);
            this.promosFolderSelectedLabel.TabIndex = 5;
            this.promosFolderSelectedLabel.Text = "No folder selected";
            // 
            // promosFolderSelectorButton
            // 
            this.promosFolderSelectorButton.Location = new System.Drawing.Point(12, 23);
            this.promosFolderSelectorButton.Name = "promosFolderSelectorButton";
            this.promosFolderSelectorButton.Size = new System.Drawing.Size(139, 23);
            this.promosFolderSelectorButton.TabIndex = 4;
            this.promosFolderSelectorButton.Text = "Choose promos folder...";
            this.promosFolderSelectorButton.UseVisualStyleBackColor = true;
            this.promosFolderSelectorButton.Click += new System.EventHandler(this.promosFolderSelectorButton_Click);
            // 
            // createBreaksForLabel
            // 
            this.createBreaksForLabel.AutoSize = true;
            this.createBreaksForLabel.Location = new System.Drawing.Point(10, 29);
            this.createBreaksForLabel.Name = "createBreaksForLabel";
            this.createBreaksForLabel.Size = new System.Drawing.Size(128, 13);
            this.createBreaksForLabel.TabIndex = 1;
            this.createBreaksForLabel.Text = "Date to create breaks for:";
            // 
            // generateBreaksButton
            // 
            this.generateBreaksButton.Enabled = false;
            this.generateBreaksButton.Location = new System.Drawing.Point(847, 717);
            this.generateBreaksButton.Name = "generateBreaksButton";
            this.generateBreaksButton.Size = new System.Drawing.Size(75, 23);
            this.generateBreaksButton.TabIndex = 5;
            this.generateBreaksButton.Text = "OK";
            this.generateBreaksButton.UseVisualStyleBackColor = true;
            this.generateBreaksButton.Click += new System.EventHandler(this.generateBreaksButton_Click);
            // 
            // savePlaylistsInButton
            // 
            this.savePlaylistsInButton.Location = new System.Drawing.Point(28, 664);
            this.savePlaylistsInButton.Name = "savePlaylistsInButton";
            this.savePlaylistsInButton.Size = new System.Drawing.Size(139, 23);
            this.savePlaylistsInButton.TabIndex = 7;
            this.savePlaylistsInButton.Text = "Choose Save folder...";
            this.savePlaylistsInButton.UseVisualStyleBackColor = true;
            this.savePlaylistsInButton.Click += new System.EventHandler(this.savePlaylistsInButton_Click);
            // 
            // savePlaylistsInFolderSelectedLabel
            // 
            this.savePlaylistsInFolderSelectedLabel.AutoSize = true;
            this.savePlaylistsInFolderSelectedLabel.Location = new System.Drawing.Point(173, 669);
            this.savePlaylistsInFolderSelectedLabel.Name = "savePlaylistsInFolderSelectedLabel";
            this.savePlaylistsInFolderSelectedLabel.Size = new System.Drawing.Size(93, 13);
            this.savePlaylistsInFolderSelectedLabel.TabIndex = 8;
            this.savePlaylistsInFolderSelectedLabel.Text = "No folder selected";
            // 
            // promosGroupBox
            // 
            this.promosGroupBox.Controls.Add(this.promosFolderSelectorButton);
            this.promosGroupBox.Controls.Add(this.promosStudioPath);
            this.promosGroupBox.Controls.Add(this.promosFolderSelectedLabel);
            this.promosGroupBox.Controls.Add(this.promosStudioPathLabel);
            this.promosGroupBox.Location = new System.Drawing.Point(12, 253);
            this.promosGroupBox.Name = "promosGroupBox";
            this.promosGroupBox.Size = new System.Drawing.Size(909, 189);
            this.promosGroupBox.TabIndex = 9;
            this.promosGroupBox.TabStop = false;
            this.promosGroupBox.Text = "Promos:";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(480, 717);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(361, 23);
            this.progressBar1.TabIndex = 10;
            // 
            // fileBeingGeneratedLabel
            // 
            this.fileBeingGeneratedLabel.AutoSize = true;
            this.fileBeingGeneratedLabel.Location = new System.Drawing.Point(25, 722);
            this.fileBeingGeneratedLabel.Name = "fileBeingGeneratedLabel";
            this.fileBeingGeneratedLabel.Size = new System.Drawing.Size(38, 13);
            this.fileBeingGeneratedLabel.TabIndex = 11;
            this.fileBeingGeneratedLabel.Text = "Ready";
            // 
            // createBreaksForPicker
            // 
            this.createBreaksForPicker.Location = new System.Drawing.Point(144, 23);
            this.createBreaksForPicker.Name = "createBreaksForPicker";
            this.createBreaksForPicker.Size = new System.Drawing.Size(200, 20);
            this.createBreaksForPicker.TabIndex = 13;
            this.createBreaksForPicker.Value = new System.DateTime(2017, 12, 10, 17, 54, 31, 0);
            // 
            // breaksToCreateGroupBox
            // 
            this.breaksToCreateGroupBox.Controls.Add(this.createBreaksForLabel);
            this.breaksToCreateGroupBox.Controls.Add(this.createBreaksForPicker);
            this.breaksToCreateGroupBox.Location = new System.Drawing.Point(12, 480);
            this.breaksToCreateGroupBox.Name = "breaksToCreateGroupBox";
            this.breaksToCreateGroupBox.Size = new System.Drawing.Size(913, 173);
            this.breaksToCreateGroupBox.TabIndex = 14;
            this.breaksToCreateGroupBox.TabStop = false;
            this.breaksToCreateGroupBox.Text = "Breaks to create:";
            // 
            // adForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 752);
            this.Controls.Add(this.breaksToCreateGroupBox);
            this.Controls.Add(this.jinglesStudioPath);
            this.Controls.Add(this.jinglesLabel);
            this.Controls.Add(this.fileBeingGeneratedLabel);
            this.Controls.Add(this.jinglesStudioPathLabel);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.promosGroupBox);
            this.Controls.Add(this.savePlaylistsInFolderSelectedLabel);
            this.Controls.Add(this.savePlaylistsInButton);
            this.Controls.Add(this.generateBreaksButton);
            this.Controls.Add(this.inputGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "adForm";
            this.Text = "Ad Automation";
            this.Load += new System.EventHandler(this.adForm_Load);
            this.inputGroupBox.ResumeLayout(false);
            this.inputGroupBox.PerformLayout();
            this.advertTablePanel.ResumeLayout(false);
            this.advertTablePanel.PerformLayout();
            this.promosGroupBox.ResumeLayout(false);
            this.promosGroupBox.PerformLayout();
            this.breaksToCreateGroupBox.ResumeLayout(false);
            this.breaksToCreateGroupBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox inputGroupBox;
        private System.Windows.Forms.Button advertsSelectFolderButton;
        private System.Windows.Forms.Label advertFolderSelectedLabel;
        private System.Windows.Forms.Label promosFolderSelectedLabel;
        private System.Windows.Forms.Button promosFolderSelectorButton;
        private System.Windows.Forms.Label createBreaksForLabel;
        private System.Windows.Forms.Button generateBreaksButton;
        private System.Windows.Forms.Button savePlaylistsInButton;
        private System.Windows.Forms.Label savePlaylistsInFolderSelectedLabel;
        private System.Windows.Forms.GroupBox promosGroupBox;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label fileBeingGeneratedLabel;
        private System.Windows.Forms.TextBox promosStudioPath;
        private System.Windows.Forms.TextBox studioAdvertsPath;
        private System.Windows.Forms.Label promosStudioPathLabel;
        private System.Windows.Forms.Label advertsStudioPathLabel;
        public System.Windows.Forms.TextBox jinglesStudioPath;
        private System.Windows.Forms.Label jinglesStudioPathLabel;
        private System.Windows.Forms.Label jinglesLabel;
        public System.Windows.Forms.TableLayoutPanel advertTablePanel;
        private System.Windows.Forms.Label filenameLabel;
        private System.Windows.Forms.Label startDateLabel;
        private System.Windows.Forms.Label playUntilLabel;
        private System.Windows.Forms.Label playsPerDayLabel;
        private System.Windows.Forms.Label saveTrackDataLabel;
        private System.Windows.Forms.Label categoryLabel;
        private System.Windows.Forms.Label categoryHelpTextLabel;
        private System.Windows.Forms.DateTimePicker createBreaksForPicker;
        private System.Windows.Forms.GroupBox breaksToCreateGroupBox;
    }
}

