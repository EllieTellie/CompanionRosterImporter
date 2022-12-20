using Companion.Data;
using CompanionFramework.Core.Log;
using CompanionFramework.Core.Threading.Messaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TrackBar;

namespace SimpleRosterImporter
{
    public partial class Importer : Form
    {
        private RosterResult currentResult;
        private RosterReader rosterReader;

        private Main parentForm;

        private int ticks;
        private const int maxTicks = 4;

        private string defaultRosterPath;

        public Importer()
        {
            InitializeComponent();
        }

        public void SetParentForm(Main parentForm)
        {
            this.parentForm = parentForm;
        }

        private void nameText_TextChanged(object sender, EventArgs e)
        {
            nameText.Text = StripInvalidCharacters(nameText.Text);
        }

        private void Importer_Load(object sender, EventArgs e)
        {

        }

        private void Importer_Paint(object sender, PaintEventArgs e)
        {
            
        }

        internal void Import(string battleScribeDirectory, string text)
        {
            // check for game systems in battle scribe directory
            string dataDirectory = Path.Combine(battleScribeDirectory, "data" + Path.DirectorySeparatorChar);
            List<string> gameSystemPaths = SystemManager.Instance.DetectGameSystems(dataDirectory);

            if (gameSystemPaths.Count == 0)
            {
                MessageBox.Show("Unable to detect any Game Systems in " + dataDirectory + ".", "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            defaultRosterPath = Path.Combine(battleScribeDirectory, "rosters" + Path.DirectorySeparatorChar);

            saveButton.Enabled = false;
            //browseButton.Enabled = false;
            nameText.Enabled = false;
            //saveDirectory.Enabled = false;

            rosterReader = new RosterReader(text);
            rosterReader.OnGameSystemDetected += OnGameSystemDetected;
            rosterReader.DetectGameSystemAsync(gameSystemPaths);
        }

        private void ResetRosterReader()
        {
            if (rosterReader != null)
            {
                rosterReader.OnGameSystemDetected -= OnGameSystemDetected;
                rosterReader.OnRosterParsed -= OnRosterParsed;
            }

            rosterReader = null;

            // run on UI thread
            Invoke(() =>
            {
                if (!saveButton.Enabled)
                    saveButton.Enabled = true;

                //if (!browseButton.Enabled)
                //    browseButton.Enabled = true;

                if (!nameText.Enabled)
                    nameText.Enabled = true;

                //if (!saveDirectory.Enabled)
                //    saveDirectory.Enabled = true;
            });
        }

        private void OnGameSystemDetected(object sender, EventArgs e)
        {
            if (rosterReader == null)
            {
                ResetRosterReader();
                return;
            }

            // unhook
            rosterReader.OnGameSystemDetected -= OnGameSystemDetected;

            GameSystem gameSystem = (GameSystem)sender;
            if (gameSystem == null)
            {
                // reset roster reader
                ResetRosterReader();

                Invoke(() =>
                {
                    MessageBox.Show("Unable to detect Game System for this roster.", "Import Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                });

                return;
            }

            Invoke(() =>
            {
                rosterOutput.Text = "Detected Game System: " + gameSystem.GetName();
            });

            // mark as active 
            SystemManager.Instance.SetActiveGameSystem(gameSystem);

            // continue parsing
            rosterReader.OnRosterParsed += OnRosterParsed;
            rosterReader.ParseAsync(gameSystem);
        }

        private void OnRosterParsed(object sender, EventArgs e)
        {
            if (rosterReader == null)
            {
                ResetRosterReader();
                return;
            }

            // unhook
            rosterReader.OnRosterParsed -= OnRosterParsed;

            RosterResult rosterResult = (RosterResult)sender;
            if (rosterResult.GetRoster() != null)
            {
                List<RosterResultEntry> entries = rosterResult.GetLogs();

                StringBuilder stringBuilder = new StringBuilder();

                rosterOutput.Text = "Detected Game System: " + SystemManager.Instance.GetActiveGameSystem()?.GetName();

                foreach (RosterResultEntry entry in entries)
                {
                    if (entry.logLevel == LogLevel.Debug) // skip debug
                        continue;

                    if (entry.logLevel > LogLevel.Message)
                    {
                        stringBuilder.AppendLine(entry.logLevel + " - " + entry.message);
                    }
                    else
                    {
                        stringBuilder.AppendLine(entry.message);
                    }
                }

                if (rosterResult.HasErrors() || rosterResult.HasWarnings())
                    stringBuilder.AppendLine("Roster parsed with (" + rosterResult.Count(LogLevel.Error) + ") errors and " + "(" + rosterResult.Count(LogLevel.Warning) + ") warnings.");
                else
                    stringBuilder.AppendLine("Roster parsed successfully.");

                // store result
                currentResult = rosterResult;

                // run on UI thread
                Invoke(() =>
                {
                    rosterOutput.Text = stringBuilder.ToString();

                    // back in the dark ages of win forms
                    rosterOutput.SelectionStart = rosterOutput.Text.Length;
                    rosterOutput.SelectionLength = 0;
                    rosterOutput.ScrollToCaret();

                    nameText.Text = rosterResult.GetRoster().GetName();
                });

                //MessageBox.Show("Roster detection completed", "Import Success");
            }
            else
            {
                // run on UI thread
                Invoke(() =>
                {
                    MessageBox.Show("Roster detection failed", "Import Failed");
                });
            }

            ResetRosterReader();
        }

        private void Importer_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (parentForm != null)
                parentForm.Show();
        }

        private void updateStatusTick_Tick(object sender, EventArgs e)
        {
            ticks++;

            if (ticks >= maxTicks)
            {
                ticks = 0;
            }

            if (rosterReader != null)
            {
                status.Text = "Parsing" + new string('.', ticks);
            }
            else if (status.Text != "")
            {
                status.Text = "";
            }
        }

        private void messageQueue_Tick(object sender, EventArgs e)
        {
            MessageHandler.Instance().ProcessQueue();
        }

        //private void browseButton_Click(object sender, EventArgs e)
        //{
        //    FolderBrowserDialog dialog = new FolderBrowserDialog();

        //    // set the initial directory
        //    dialog.SelectedPath = saveDirectory.Text;

        //    if (dialog.ShowDialog() == DialogResult.OK)
        //    {
        //        string path = dialog.SelectedPath;

        //        if (Directory.Exists(path))
        //            saveDirectory.Text = path;
        //    }
        //}

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                string name = nameText.Text;
                string folder = defaultRosterPath;

                if (currentResult == null || currentResult.GetRoster() == null)
                {
                    MessageBox.Show("Roster failed to import", "Failed to Import");
                    return;
                }
                else if (!Directory.Exists(folder))
                {
                    MessageBox.Show("Save directory does not exist: " + folder, "Failed to Save");
                    return;
                }
                else if (string.IsNullOrEmpty(name)) // probably should have more validation for invalid characters
                {
                    MessageBox.Show("Please provide a valid name.", "Failed to Save");
                    return;
                }

                // remove this
                name = StripInvalidCharacters(name);

                // set new name
                currentResult.GetRoster().name = name;

                if (currentResult.HasWarnings() || currentResult.HasErrors())
                {
                    if (MessageBox.Show("This roster has (" + currentResult.Count(LogLevel.Error) + ") errors and " + "(" + currentResult.Count(LogLevel.Warning) + ") warnings. Are you sure you want to save this roster?", "Save?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        return;
                    }
                }

                //string targetFile = Path.Combine(folder, name + ".rosz");
                //if (File.Exists(targetFile))
                //{
                //    MessageBox.Show("An existing roster already exists at: " + targetFile, "Failed to Save");
                //    return;
                //}

                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.OverwritePrompt = true;
                saveFileDialog.DefaultExt = ".rosz";
                saveFileDialog.InitialDirectory = defaultRosterPath;
                saveFileDialog.FileName = name + ".rosz";
                saveFileDialog.Filter = "Roster Files (.rosz)|*.rosz|Uncompressed Roster Files (*.ros)|*.ros";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog.FileName;

                    // write roster
                    if (currentResult.GetRoster().SaveRosterXml(fileName, true, true))
                    {
                        MessageBox.Show("Roster has been saved: " + name, "Saved Roster");

                        //parentForm.Reset();

                        // close it
                        Close();
                    }
                }
            }
            catch (Exception ex)
            {
                // please no crashy
                MessageBox.Show("Exception occurred: " + ex.Message, "Failed to Save");
            }
        }

        private string StripInvalidCharacters(string text)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                text = text.Replace(c.ToString(), "");
            }

            return text;
        }
    }
}
