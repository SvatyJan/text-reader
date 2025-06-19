namespace Text_Reader
{
    public partial class Form1 : Form
    {
        /** Zobrazené øádky. */
        private List<string> lines = new();

        public Form1()
        {
            InitializeComponent();
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

        private void DgvLines_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < lines.Count)
            {
                e.Value = lines[e.RowIndex];
            }
        }
    }
}