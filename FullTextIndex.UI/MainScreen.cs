using FullTextIndex.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FullTextIndex.UI
{
    public partial class MainScreen : Form
    {
        private InvertedIndex index;
        private List<WikipediaEntry> documents = new List<WikipediaEntry>();
        public MainScreen()
        {
            InitializeComponent();
        }

        private async Task PopulateIndex()
        {
            try
            {

                if (File.Exists("index.json"))
                {
                    await Task.Run(() =>
                    {
                        documents = EntryReader.ReadDump(@"C:\Users\matt.burke.POINT\Downloads\enwiki-latest-abstract1.xml\enwiki-latest-abstract1.xml")
                            .ToList();
                    });

                    var serializer = new JsonIndexPersister();
                    index = await serializer.RestpreAsync("index.json");
                }
                else
                {
                    await Task.Run(async () =>
                    {
                        var entries = EntryReader.ReadDump(@"C:\Users\matt.burke.POINT\Downloads\enwiki-latest-abstract1.xml\enwiki-latest-abstract1.xml");

                        var invertedIndex = new InvertedIndex();
                        foreach (var entry in entries)
                        {
                            invertedIndex.Index(entry.DocumentId, entry.Abstract);
                            documents.Add(entry);
                        }

                        invertedIndex.Commit();
                        index = invertedIndex;

                    });


                    var serializer = new JsonIndexPersister();
                    await serializer.PersistAsync("index.json", index);
                }


                lblStatus.Text = $"Index: {index.DocumentCount:n0} docs {index.TermCount:n0} terms";
                btnSearch.Enabled = true;
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void MainScreen_Load(object sender, EventArgs e)
        {
            _ = PopulateIndex();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lvResults.Items.Clear();
            lvResults.BeginUpdate();

            var sw = Stopwatch.StartNew();
            foreach (var result in index.Search(txtSearch.Text))
            {
                var entry = documents[int.Parse(result.DocumentId)];
                lvResults.Items.Add(new ListViewItem(new string[] {result.Score.ToString("n2"), entry.Title, entry.Abstract }));
            }
            sw.Stop();

            lvResults.EndUpdate();
            colScore.Width = -2;
            colAbstract.Width = -2;
            lblSearchResults.Text = $"Loaded {lvResults.Items.Count:n0} documents in {sw.ElapsedMilliseconds}ms";
        }
    }
}
