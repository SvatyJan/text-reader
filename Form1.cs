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
    }
}