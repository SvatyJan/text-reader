namespace Text_Reader
{
    public partial class Form1 : Form
    {
        /** Konstanta pro limit øádkù. */
        private const int LINES_LIMIT = 500_000;

        /** Zobrazené øádky. */
        private List<string> lines = new();

        /** Aktuální index hledání. */
        private int currentSearchIndex = -1;

        /** Poslední hledaný výraz. */
        private string? lastSearchTerm = null;

        /** Vyfiltrované øádky. */
        private List<string> filteredLines = new();

        /** Pøíznak, zda je filtrování aktivní. */
        private bool filteringActive => cbFilteredLines.Checked;

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

                        if (count > LINES_LIMIT)
                        {
                            MessageBox.Show("Pøekroèekn limit: " + LINES_LIMIT);
                            break;
                        }
                    }

                    dgvLines.RowCount = lines.Count;
                    dgvLines.Refresh();
                    MessageBox.Show($"Naèteno {lines.Count:N0} náhodných øádkù.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Chyba: " + ex.Message);
                }
            }
        }

        private async void btnLoadFromUrl_Click(object sender, EventArgs e)
        {
            string url = tbUrl.Text.Trim();

            if (string.IsNullOrWhiteSpace(url))
            {
                MessageBox.Show("Zadejte platnou URL adresu.");
                return;
            }

            try
            {
                btnLoadFromUrl.Enabled = false;
                lines.Clear();
                filteredLines.Clear();
                cbFilteredLines.Checked = false;
                tbSearch.Clear();
                currentSearchIndex = -1;

                using HttpClient client = new HttpClient();
                using Stream stream = await client.GetStreamAsync(url);
                using StreamReader reader = new StreamReader(stream);

                string? line;
                int count = 0;

                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                    count++;

                    if (count > LINES_LIMIT)
                    {
                        MessageBox.Show("Pøekroèekn limit: " + LINES_LIMIT);
                        break;
                    }
                }

                dgvLines.RowCount = lines.Count;
                dgvLines.Refresh();

                MessageBox.Show($"Naèteno {lines.Count:N0} øádkù z webu.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Chyba pøi naèítání z webu:\n" + ex.Message);
            }
            finally
            {
                btnLoadFromUrl.Enabled = true;
            }
        }

        private void btnGenerateRandomText_Click(object sender, EventArgs e)
        {
            const int rowCount = 100_000;
            var rnd = new Random();

            lines.Clear();
            filteredLines.Clear();
            cbFilteredLines.Checked = false;
            tbSearch.Clear();
            currentSearchIndex = -1;

            string[] wordBank = new[]
            {
                "lorem", "ipsum", "dolor", "amet", "želva", "matrix", "search", "fusion", "quantum", "debug",
                "random", "alpha", "kebab", "test", "text", "value", "data", "engine", "core", "byte"
            };

            for (int i = 0; i < rowCount; i++)
            {
                int wordCount = rnd.Next(5, 50);
                var words = new List<string>();

                for (int j = 0; j < wordCount; j++)
                {
                    words.Add(wordBank[rnd.Next(wordBank.Length)]);
                }

                lines.Add(string.Join(' ', words));
            }

            dgvLines.RowCount = lines.Count;
            dgvLines.Refresh();

            MessageBox.Show($"Generováno {lines.Count:N0} náhodných øádkù.");
        }

        private void SearchNext()
        {
            var source = filteringActive ? filteredLines : lines;

            if (string.IsNullOrWhiteSpace(tbSearch.Text) || source.Count == 0)
                return;

            string term = tbSearch.Text;
            int start = currentSearchIndex + 1;

            for (int i = start; i < source.Count; i++)
            {
                if (source[i].Contains(term, StringComparison.OrdinalIgnoreCase))
                {
                    currentSearchIndex = i;
                    dgvLines.ClearSelection();
                    dgvLines.CurrentCell = dgvLines[0, i];
                    dgvLines.FirstDisplayedScrollingRowIndex = i;
                    return;
                }
            }

            MessageBox.Show("Další výskyt nenalezen.");
            currentSearchIndex = -1;
        }

        private void SearchPrevious()
        {
            var source = filteringActive ? filteredLines : lines;

            if (string.IsNullOrWhiteSpace(tbSearch.Text) || source.Count == 0)
                return;

            string term = tbSearch.Text;
            int start = currentSearchIndex - 1;

            for (int i = start; i >= 0; i--)
            {
                if (source[i].Contains(term, StringComparison.OrdinalIgnoreCase))
                {
                    currentSearchIndex = i;
                    dgvLines.ClearSelection();
                    dgvLines.CurrentCell = dgvLines[0, i];
                    dgvLines.FirstDisplayedScrollingRowIndex = i;
                    return;
                }
            }

            MessageBox.Show("Pøedchozí výskyt nenalezen.");
            currentSearchIndex = -1;
        }

        private void DgvLines_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            List<string> source = filteringActive ? filteredLines : lines;

            if (e.RowIndex >= 0 && e.RowIndex < source.Count)
            {
                e.Value = source[e.RowIndex];
            }
        }

        private void tbSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                currentSearchIndex = -1;
                ApplyFilter();
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

                string cellText = (filteringActive ? filteredLines : lines)[e.RowIndex];
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
                        string remaining = cellText.Substring(index);
                        g.DrawString(remaining, e.CellStyle.Font, normalBrush, x, y);
                        break;
                    }

                    string beforeMatch = cellText.Substring(index, matchIndex - index);
                    SizeF sizeBefore = g.MeasureString(beforeMatch, e.CellStyle.Font);
                    g.DrawString(beforeMatch, e.CellStyle.Font, normalBrush, x, y);
                    x += sizeBefore.Width;

                    string match = cellText.Substring(matchIndex, searchText.Length);
                    SizeF sizeMatch = g.MeasureString(match, e.CellStyle.Font);
                    RectangleF highlightRect = new RectangleF(x, y, sizeMatch.Width, sizeMatch.Height);
                    g.FillRectangle(highlightBrush, highlightRect);
                    g.DrawString(match, e.CellStyle.Font, textBrush, x, y);
                    x += sizeMatch.Width;

                    index = matchIndex + searchText.Length;
                }
            }
        }

        private void cbFilteredLines_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ApplyFilter()
        {
            if (!filteringActive || string.IsNullOrWhiteSpace(tbSearch.Text))
            {
                filteredLines.Clear();
                dgvLines.RowCount = lines.Count;
                dgvLines.Refresh();
                return;
            }

            string term = tbSearch.Text;
            filteredLines = lines
                .Where(line => line.Contains(term, StringComparison.OrdinalIgnoreCase))
                .ToList();

            dgvLines.RowCount = filteredLines.Count;
            dgvLines.Refresh();
        }
    }
}