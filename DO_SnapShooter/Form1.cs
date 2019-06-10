using System;

using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DO_SnapShooter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            string jsonreturn = string.Empty;
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = "crl.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                Arguments =
                    "-X GET -H \"Content-Type: application/json\" -H \"Authorization: Bearer " + textBoxKEY.Text + "\" \"https://api.digitalocean.com/v2/droplets\""
            };

            using (var exeProcess = Process.Start(startInfo))
            {

                if (exeProcess != null) jsonreturn = exeProcess.StandardOutput.ReadToEnd();
            }



            dynamic dynObj = JsonConvert.DeserializeObject(jsonreturn);
            foreach (var data in dynObj.droplets)
            {

                ListViewItem li = new ListViewItem();
                li.Text = data.id.ToString();
                li.SubItems.Add(data.name.ToString());
                listView1.Items.Add(li);


            }



        }

        private void button2_Click(object sender, EventArgs e)
        {

            foreach (ListViewItem item in listView1.CheckedItems)
            {

                var random = new Random();
                int num = random.Next(1000);
                string snapname = "h-snapshoot" + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
                                  DateTime.Now.Second.ToString() + DateTime.Now.Millisecond.ToString() + num.ToString();
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    FileName = "crl.exe",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    Arguments =
                        "-X POST -H \"Content-Type: application/json\" -H \"Authorization: Bearer " + textBoxKEY.Text + "\" -d \"{\\\"type\\\":\\\"snapshot\\\",\\\"name\\\":\\\"" + snapname + "\\\"}\" \"https://api.digitalocean.com/v2/droplets/" + item.Text + "/actions\""
                };

                 Process.Start(startInfo);

                


            }

            MessageBox.Show("Done, Snapshots in Progress...");


        }

        private void button3_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();

            if (listView1.SelectedItems.Count < 1)
            {
                MessageBox.Show("Please select a Droplet");
                return;
            }

            string jsonreturn = string.Empty;
            var dropletId = listView1.SelectedItems[0].Text;

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = "crl.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                Arguments =
                    "curl -X GET -H \"Content-Type: application/json\" -H \"Authorization: Bearer " + textBoxKEY.Text + "\" \"https://api.digitalocean.com/v2/droplets/" + dropletId + "/snapshots?page=1&per_page=1000"
            };

            using (var exeProcess = Process.Start(startInfo))
            {

                if (exeProcess != null) jsonreturn = exeProcess.StandardOutput.ReadToEnd();
            }



            dynamic dynObj = JsonConvert.DeserializeObject(jsonreturn);
            foreach (var data in dynObj.snapshots)
            {

                ListViewItem li = new ListViewItem {Text = data.id.ToString()};
                li.SubItems.Add(data.name.ToString());
                li.SubItems.Add(data.created_at.ToString());
                li.SubItems.Add(data.size_gigabytes.ToString() + " GB");
                listView2.Items.Add(li);


            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            listView2.Items.Clear();

            int totalSize = 0;


            string jsonreturn = string.Empty;
   

            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                FileName = "crl.exe",
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardOutput = true,
                Arguments =
                    "curl -X GET -H \"Content-Type: application/json\" -H \"Authorization: Bearer " + textBoxKEY.Text + "\" \"https://api.digitalocean.com/v2/snapshots?page=1&per_page=1000000"
            };

            using (var exeProcess = Process.Start(startInfo))
            {

                if (exeProcess != null) jsonreturn = exeProcess.StandardOutput.ReadToEnd();
            }



            dynamic dynObj = JsonConvert.DeserializeObject(jsonreturn);
            foreach (var data in dynObj.snapshots)
            {

                ListViewItem li = new ListViewItem { Text = data.id.ToString() };
                li.SubItems.Add(data.name.ToString());
                li.SubItems.Add(data.created_at.ToString());

                int snapSize = (int) data.size_gigabytes;
                totalSize = totalSize + snapSize;
                li.SubItems.Add(snapSize.ToString() + " GB");
                listView2.Items.Add(li);


            }


            //calculate estimate for 0.05$ per GB

            double estimate = totalSize * Convert.ToDouble(textBoxPrice.Text);
            MessageBox.Show("Estimated Monthly Costs= " + estimate.ToString(CultureInfo.InvariantCulture) + " $");


        }

        private void button4_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listView2.CheckedItems)
            {

                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    FileName = "crl.exe",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    RedirectStandardOutput = true,
                    Arguments =
                        "-X DELETE -H \"Content-Type: application/json\" -H \"Authorization: Bearer " + textBoxKEY.Text + "\" \"https://api.digitalocean.com/v2/snapshots/" + item .Text + "\""
                };

                Process.Start(startInfo);

                

            }

            MessageBox.Show("Done, Snapshots Deletion in Progress...");
        }

        private void toolStripStatusLabel3_Click(object sender, EventArgs e)
        {
            Process.Start("https://h-educate.com/");
        }
    }
}
