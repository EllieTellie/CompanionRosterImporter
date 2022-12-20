using Companion.Data;
using Companion.Data.Update;
using CompanionFramework.Core.Log;
using CompanionFramework.Core.Threading.Messaging;
using CompanionFramework.IO.Utils;
using System.Security.Policy;
using System.Text;

namespace SimpleRosterImporter
{
    public partial class Main : Form
    {
        private string lastDirectory = null;
        private string lastClipboard = null;

        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            //// check if battle scribe is installed
            //string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            //string battleScribePath = Path.Combine(userPath, "BattleScribe" + Path.DirectorySeparatorChar);

            //if (Directory.Exists(battleScribePath))
            //{
            //    Console.WriteLine("Hello World");
            //}

            string text = Clipboard.GetText(TextDataFormat.Text);

            if (text != null && text.Contains("++"))
            {
                lastClipboard = text;
                inputText.Text = text;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            fileDialog.FilterIndex = 1;
            fileDialog.Multiselect = false;

            if (!string.IsNullOrEmpty(lastDirectory) && Directory.Exists(lastDirectory))
            {
                fileDialog.InitialDirectory = lastDirectory;
            }

            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string path = fileDialog.FileName;

                if (File.Exists(path))
                {
                    lastDirectory = FileUtils.GetDirectoryFromPath(path);

                    string text = FileUtils.ReadTextFile(path);
                    inputText.Text = text;
                }
            }
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void importButton_Click(object sender, EventArgs e)
        {
            string text = inputText.Text;

            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("Please provide some roster text to import.", "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // detect game systems present in battle scribe directory
            string battleScribeDirectory = GetValidBattleScribeDirectory();

            if (battleScribeDirectory == null)
            {
                MessageBox.Show("Unable to detect Battle Scribe directory in user path. Checked: " + battleScribeDirectory, "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            // hide ourselves
            Hide();

            Importer importer = new Importer();
            importer.SetParentForm(this);
            importer.Show();

            // start importing
            importer.Import(battleScribeDirectory, text);
        }

        private string GetValidBattleScribeDirectory()
        {
            // check if battle scribe is installed
            string userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);

            string battleScribePath = Path.Combine(userPath, "BattleScribe" + Path.DirectorySeparatorChar);

            return Directory.Exists(battleScribePath) ? battleScribePath : null;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Simple Roster Importer v1.0.", "About", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void Main_Paint(object sender, PaintEventArgs e)
        {
        }

        private void inputText_TextChanged(object sender, EventArgs e)
        {
            resetButton.Visible = inputText.Text != null && inputText.Text.Length > 0;
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            Reset();
        }

        internal void Reset()
        {
            inputText.Text = "";
        }

        private void messageQueue_Tick(object sender, EventArgs e)
        {
            MessageHandler.Instance().ProcessQueue();
        }

        private void clipboardRefresh_Tick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(inputText.Text))
            {
                // check if we have anything in clipboard and copy it in
                string text = Clipboard.GetText(TextDataFormat.Text);

                if (text != null && text != lastClipboard && text.Contains("++"))
                {
                    lastClipboard = text;
                    inputText.Text = text;
                }
            }
        }
    }
}