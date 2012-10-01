using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Prysm.WordCounter
{
    public partial class Main : Form
    {
        private IEnumerable<IWordCounter> _wordCounters;

        public Main()
        {
            InitializeComponent();

            _wordCounters = AppDomain.CurrentDomain.GetAssemblies().ToList()
                .SelectMany(s => s.GetTypes())
                .Where(x => !x.IsInterface)
                .Where(p => typeof(IWordCounter).IsAssignableFrom(p))
                .Select(x => (IWordCounter)Activator.CreateInstance(x));

            foreach (var counter in _wordCounters)
                cmbWordCounters.Items.Add(counter.GetType().Name);
        }

        private void MainLoad(object sender, EventArgs e)
        {
            //load the text file into the text box
            var filename = Application.StartupPath + "\\input.txt";
            txtInput.Text = File.ReadAllText(filename);
        }

        private void btnCount_Click(object sender, EventArgs e)
        {
            Application.DoEvents();

            IWordCounter wordCounter = null;

            if (cmbWordCounters.SelectedIndex != -1)
                wordCounter =
                   _wordCounters.SingleOrDefault(x => x.GetType().Name == cmbWordCounters.SelectedItem.ToString());

            if (wordCounter != null)
            {
                var results = wordCounter.CountWords(txtInput.Text)
                    .OrderByDescending(x => x.Count)
                    .ToList();

                resultsView.Items.Clear();

                foreach (var result in results)
                {
                    var item = new ListViewItem(result.Word);
                    item.SubItems.Add(result.Count.ToString(CultureInfo.InvariantCulture));

                    resultsView.Items.Add(item);
                }

                lblCount.Text = results.Count().ToString();
            }

            else
                MessageBox.Show("Please choose a word counter");
        }
    }
}
