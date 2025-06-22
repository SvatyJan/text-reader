namespace Text_Reader
{
    partial class TextReader
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnOpenFile = new Button();
            ss = new StatusStrip();
            dgvLines = new DataGridView();
            tbSearch = new TextBox();
            cbFilteredLines = new CheckBox();
            tbUrl = new TextBox();
            btnLoadFromUrl = new Button();
            btnGenerateRandomText = new Button();
            btnSaveToFile = new Button();
            ((System.ComponentModel.ISupportInitialize)dgvLines).BeginInit();
            SuspendLayout();
            // 
            // btnOpenFile
            // 
            btnOpenFile.Location = new Point(12, 8);
            btnOpenFile.Name = "btnOpenFile";
            btnOpenFile.Size = new Size(118, 49);
            btnOpenFile.TabIndex = 0;
            btnOpenFile.Text = "Open File";
            btnOpenFile.UseVisualStyleBackColor = true;
            btnOpenFile.Click += btnOpenFile_Click;
            // 
            // ss
            // 
            ss.Location = new Point(0, 589);
            ss.Name = "ss";
            ss.Size = new Size(804, 22);
            ss.TabIndex = 2;
            ss.Text = "Status";
            // 
            // dgvLines
            // 
            dgvLines.AllowUserToAddRows = false;
            dgvLines.AllowUserToDeleteRows = false;
            dgvLines.AllowUserToResizeRows = false;
            dgvLines.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvLines.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvLines.ColumnHeadersVisible = false;
            dgvLines.Location = new Point(0, 63);
            dgvLines.Name = "dgvLines";
            dgvLines.RowHeadersVisible = false;
            dgvLines.RowTemplate.Height = 25;
            dgvLines.Size = new Size(804, 548);
            dgvLines.TabIndex = 3;
            dgvLines.VirtualMode = true;
            dgvLines.CellPainting += dgvLines_CellPainting;
            dgvLines.CellValueNeeded += DgvLines_CellValueNeeded;
            // 
            // tbSearch
            // 
            tbSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbSearch.Location = new Point(136, 34);
            tbSearch.Name = "tbSearch";
            tbSearch.PlaceholderText = "search";
            tbSearch.Size = new Size(200, 23);
            tbSearch.TabIndex = 4;
            tbSearch.KeyDown += tbSearch_KeyDown;
            // 
            // cbFilteredLines
            // 
            cbFilteredLines.AutoSize = true;
            cbFilteredLines.Location = new Point(342, 36);
            cbFilteredLines.Name = "cbFilteredLines";
            cbFilteredLines.Size = new Size(185, 19);
            cbFilteredLines.TabIndex = 5;
            cbFilteredLines.Text = "Zobrazit pouze nalezené řádky";
            cbFilteredLines.UseVisualStyleBackColor = true;
            cbFilteredLines.CheckedChanged += cbFilteredLines_CheckedChanged;
            // 
            // tbUrl
            // 
            tbUrl.Location = new Point(136, 12);
            tbUrl.Name = "tbUrl";
            tbUrl.PlaceholderText = "https://...";
            tbUrl.Size = new Size(200, 23);
            tbUrl.TabIndex = 6;
            // 
            // btnLoadFromUrl
            // 
            btnLoadFromUrl.Location = new Point(342, 11);
            btnLoadFromUrl.Name = "btnLoadFromUrl";
            btnLoadFromUrl.Size = new Size(124, 23);
            btnLoadFromUrl.TabIndex = 7;
            btnLoadFromUrl.Text = "Načíst z webu";
            btnLoadFromUrl.UseVisualStyleBackColor = true;
            btnLoadFromUrl.Click += btnLoadFromUrl_Click;
            // 
            // btnGenerateRandomText
            // 
            btnGenerateRandomText.Location = new Point(550, 8);
            btnGenerateRandomText.Name = "btnGenerateRandomText";
            btnGenerateRandomText.Size = new Size(118, 49);
            btnGenerateRandomText.TabIndex = 8;
            btnGenerateRandomText.Text = "Generovat náhodný text";
            btnGenerateRandomText.UseVisualStyleBackColor = true;
            btnGenerateRandomText.Click += btnGenerateRandomText_Click;
            // 
            // btnSaveToFile
            // 
            btnSaveToFile.Location = new Point(674, 8);
            btnSaveToFile.Name = "btnSaveToFile";
            btnSaveToFile.Size = new Size(118, 47);
            btnSaveToFile.TabIndex = 9;
            btnSaveToFile.Text = "Uložit do souboru";
            btnSaveToFile.UseVisualStyleBackColor = true;
            btnSaveToFile.Click += btnSaveToFile_Click;
            // 
            // TextReader
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(804, 611);
            Controls.Add(btnSaveToFile);
            Controls.Add(btnGenerateRandomText);
            Controls.Add(btnLoadFromUrl);
            Controls.Add(tbUrl);
            Controls.Add(cbFilteredLines);
            Controls.Add(tbSearch);
            Controls.Add(dgvLines);
            Controls.Add(ss);
            Controls.Add(btnOpenFile);
            Name = "TextReader";
            Text = "Text Reader";
            ((System.ComponentModel.ISupportInitialize)dgvLines).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button btnOpenFile;
        private StatusStrip ss;
        private DataGridView dgvLines;
        private TextBox tbSearch;
        private CheckBox cbFilteredLines;
        private TextBox tbUrl;
        private Button btnLoadFromUrl;
        private Button btnGenerateRandomText;
        private Button btnSaveToFile;
    }
}