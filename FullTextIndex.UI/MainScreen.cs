using FullTextIndex.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FullTextIndex.UI
{
    public partial class MainScreen : Form
    {
        private InvertedIndex index;
        public MainScreen()
        {
            InitializeComponent();
        }

        private async Task PopulateIndex()
        {
            await Task.Run(() =>
            {
                var entries = EntryReader.ReadDump(@"C:\Users\matt.burke.POINT\Downloads\enwiki-latest-abstract1.xml\enwiki-latest-abstract1.xml");

                var invertedIndex = new InvertedIndex(700_000);
                foreach (var entry in entries)
                    invertedIndex.Index(entry);

                this.index = invertedIndex;
            });

            lblStatus.Text = $"Index: {index.DocumentCount:n0} docs {index.TermCount:n0} terms";
            btnSearch.Enabled = true;
        }

        private void MainScreen_Load(object sender, EventArgs e)
        {
            PopulateIndex();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            lvResults.Items.Clear();
            lvResults.BeginUpdate();
            var sw = Stopwatch.StartNew();
            foreach(var entry in index.Search(txtSearch.Text))
            {
                lvResults.Items.Add(new ListViewItem(new string[] { entry.Title, entry.Abstract }));
            }
            sw.Stop();
            lvResults.EndUpdate();
            lvResults.Columns[1].Width = -2;
            lblSearchResults.Text = $"Loaded {lvResults.Items.Count:n0} documents in {sw.ElapsedMilliseconds}ms";
        }
    }
}
