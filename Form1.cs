namespace Text_Reader
{
    public partial class Form1 : Form
    {
        /** Zobrazené øádky. */
        private List<string> lines = new();

        /** Aktuální index hledání. */
        private int currentSearchIndex = -1;

        /** Poslední hledaný výraz. */
        private string? lastSearchTerm = null;

        public Form1()
        {
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message message, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.F))
            {
                tbSearch.Visible = true;
                tbSearch.Focus();
                return true;
            }

            if (keyData == Keys.F3)
            {
                SearchNext();
                return true;
            }

            if (keyData == (Keys.Shift | Keys.F3))
            {
                SearchPrevious();
                return true;
            }

            return base.ProcessCmdKey(ref message, keyData);
        }

        private async void btnOpenFile_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Textové soubory (*.txt)|*.txt|Všechny soubory (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                lines.Clear();

                try
                {
                    using StreamReader reader = new StreamReader(openFileDialog.FileName);
                    string? line;
                    int count = 0;

                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        lines.Add(line);
                        count++;

                        if (count > 500_000)
                        {
                            MessageBox.Show("Naèteno 500 000 øádkù pro test. Funguje to!");
                            break;
                        }
                    }

                    dgvLines.RowCount = lines.Count;
                    dgvLines.Refresh();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba: " + ex.Message);
                }
            }
        }

        private void SearchNext()
        {
            if (string.IsNullOrWhiteSpace(tbSearch.Text) || lines.Count == 0)
                return;

            string term = tbSearch.Text;
            int start = currentSearchIndex + 1;

            for (int i = start; i < lines.Count; i++)
            {
                if (lines[i].Contains(term, StringComparison.OrdinalIgnoreCase))
                {
                    currentSearchIndex = i;
                    dgvLines.ClearSelection();
                    dgvLines.CurrentCell = dgvLines[0, i];
                    dgvLines.FirstDisplayedScrollingRowIndex = i;
                    return;
                }
            }

            MessageBox.Show("Další výskyt nenalezen.");
        }

        private void SearchPrevious()
        {
            if (string.IsNullOrWhiteSpace(tbSearch.Text) || lines.Count == 0)
                return;

            string term = tbSearch.Text;
            int start = currentSearchIndex - 1;

            for (int i = start; i >= 0; i--)
            {
                if (lines[i].Contains(term, StringComparison.OrdinalIgnoreCase))
                {
                    currentSearchIndex = i;
                    dgvLines.ClearSelection();
                    dgvLines.CurrentCell = dgvLines[0, i];
                    dgvLines.FirstDisplayedScrollingRowIndex = i;
                    return;
                }
            }

            MessageBox.Show("Pøedchozí výskyt nenalezen.");
        }

        private void DgvLines_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < lines.Count)
            {
                e.Value = lines[e.RowIndex];
            }
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                currentSearchIndex = -1;
                SearchNext();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void dgvLines_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0 && !string.IsNullOrEmpty(tbSearch.Text))
            {
                e.Handled = true;
                e.PaintBackground(e.CellBounds, true);

                string cellText = lines[e.RowIndex];
                string searchText = tbSearch.Text;
                StringComparison comparison = StringComparison.OrdinalIgnoreCase;

                using var normalBrush = new SolidBrush(dgvLines.DefaultCellStyle.ForeColor);
                using var highlightBrush = new SolidBrush(Color.Yellow);
                using var textBrush = new SolidBrush(Color.Black);
                using var sf = new StringFormat { LineAlignment = StringAlignment.Center };

                Rectangle cellRect = e.CellBounds;
                var g = e.Graphics;
                float x = cellRect.Left + 5;
                float y = cellRect.Top + (cellRect.Height - e.CellStyle.Font.Height) / 2;

                int index = 0;

                while (index < cellText.Length)
                {
                    int matchIndex = cellText.IndexOf(searchText, index, comparison);
                    if (matchIndex == -1)
                    {
                        // Zbytek textu bez zvýraznìní
                        string remaining = cellText.Substring(index);
                        g.DrawString(remaining, e.CellStyle.Font, normalBrush, x, y);
                        break;
                    }

                    // Text pøed výskytem
                    string beforeMatch = cellText.Substring(index, matchIndex - index);
                    SizeF sizeBefore = g.MeasureString(beforeMatch, e.CellStyle.Font);
                    g.DrawString(beforeMatch, e.CellStyle.Font, normalBrush, x, y);
                    x += sizeBefore.Width;

                    // Zvýraznìný text
                    string match = cellText.Substring(matchIndex, searchText.Length);
                    SizeF sizeMatch = g.MeasureString(match, e.CellStyle.Font);
                    RectangleF highlightRect = new RectangleF(x, y, sizeMatch.Width, sizeMatch.Height);
                    g.FillRectangle(highlightBrush, highlightRect);
                    g.DrawString(match, e.CellStyle.Font, textBrush, x, y);
                    x += sizeMatch.Width;

                    // Posuò index za výskyt
                    index = matchIndex + searchText.Length;
                }
            }
        }
    }
}