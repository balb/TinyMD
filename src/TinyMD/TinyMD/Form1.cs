using System;
using System.IO;
using System.Windows.Forms;

namespace TinyMD
{
    // TODO: Open with command line stuff
    // TODO: Spellchecker
    // TODO: Ctrl+S

    public partial class Form1 : Form
    {
        string _tempFile;
        string _currentFile;
        MarkdownSharp.Markdown _markdown = new MarkdownSharp.Markdown();
        public Form1()
        {
            InitializeComponent();
            // We need to give the web browser a document
            _tempFile = Path.GetTempFileName();
            File.WriteAllText(_tempFile, " ");
            webBrowser1.Navigate(_tempFile);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            webBrowser1.DocumentText = "<div style='font-family: Helvetica,arial,freesans,clean,sans-serif;'>" +
                                        _markdown.Transform(textBox1.Text)
                                        + "</div>";
            btnSaveFile.Enabled = true;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            try
            {
                // Clean up
                if (File.Exists(_tempFile))
                    File.Delete(_tempFile);
            }
            catch { }
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            var fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Markdown files (*.md;*.markdown)|*.md;*.markdown";
            if (fileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                var stream = fileDialog.OpenFile();
                if (stream != null)
                {
                    using (var reader = new StreamReader(stream))
                    {
                        textBox1.Text = reader.ReadToEnd();
                    }
                    stream.Close();
                    _currentFile = fileDialog.FileName;
                    this.Text = _currentFile;
                }
            }

            btnSaveFile.Enabled = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_currentFile))
            {
                File.WriteAllText(_currentFile, textBox1.Text);
            }
            else
            {
                var fileDialog = new SaveFileDialog();
                fileDialog.Filter = "Markdown files (*.md;*.markdown)|*.md;*.markdown";
                if (fileDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    var stream = fileDialog.OpenFile();
                    if (stream != null)
                    {
                        using (var writer = new StreamWriter(stream))
                        {
                            writer.Write(textBox1.Text);
                        }
                        stream.Close();
                        _currentFile = fileDialog.FileName;
                        this.Text = _currentFile;
                    }
                }
            }

            btnSaveFile.Enabled = false;
        }
    }
}
