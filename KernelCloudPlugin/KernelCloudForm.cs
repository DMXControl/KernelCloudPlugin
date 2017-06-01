using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Lumos.GUI;
using Lumos.GUI.BaseWindow;
using LumosLIB.GUI.Windows;
using WeifenLuo.WinFormsUI.Docking;

namespace KernelCloudPlugin
{
    public partial class KernelCloudForm : ToolWindow //A toolwindow is a window that can be docked
    {
        public KernelCloudForm()
        {
            InitializeComponent();
            this.checkedListBox1.Items.Clear();

            //Define in which Menu the Window is displayed
            this.MainFormMenu = MenuType.Connection;
            //Define the Tab Text
            this.TabText = "Kernel Cloud";

            this.Shown += KernelCloudForm_Shown;
        }

        private void KernelCloudForm_Shown(object sender, EventArgs e)
        {
            this.DockState = DockState.Float;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.checkedListBox1.Items.Clear();
            SetStatus("Trying to connect...");
            SleepLoop(1000);

            bool success = false;
            //1st check Internet Connection
            using (WebClient wc = new WebClient())
            {
                try
                {
                    var s = wc.DownloadString("http://www.dmxc.org/kernel");
                    success = s.Contains("DMXControl");
                }
                catch (Exception)
                {
                    //Ignore
                }   
            }
            if (!success)
            {
                DisableStatus();
                MessageBox.Show("Unable to connect to Kernel Cloud. Please check Internet connection.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SetStatus("Connection to Kernel Cloud established!");
            Random r = new Random();
            SleepLoop(r.Next(1000, 3000));

            SetStatus("Verification of DMXControl Version.");
            SleepLoop(r.Next(1000, 3000));

            SetStatus("Searching Kernels.");
            SleepLoop(r.Next(1000, 3000));

            SetStatus("Loading List...");
            SleepLoop(r.Next(1000, 3000));

            int last = 0;
            for(int i = r.Next(5, 15); i >= 0; i--)
            {
                last = r.Next(last, last + 5);
                this.checkedListBox1.Items.Add("Kernel-" + last);
                last++;
                SleepLoop(100);
            }
            SetStatus("Finishing up!");
            SleepLoop(r.Next(1000, 3000));

            SetStatus("Please pick Kernel...");
            this.progressBar1.Style = ProgressBarStyle.Continuous;
        }

        private void DisableStatus()
        {
            this.labelStatus.Text = "Status:";
            this.progressBar1.Style = ProgressBarStyle.Continuous;
        }

        private void SetStatus(string status)
        {
            this.labelStatus.Text = "Status: " + status;
            this.progressBar1.Style = ProgressBarStyle.Marquee;
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                this.checkedListBox1.ItemCheck -= checkedListBox1_ItemCheck;
                for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
                {
                    if (i != e.Index)
                        this.checkedListBox1.SetItemChecked(i, false);
                }
                this.button2.Enabled = true;
                this.checkedListBox1.ItemCheck += checkedListBox1_ItemCheck;
            }
            else
            {
                bool check = false;
                for (int i = 0; i < this.checkedListBox1.Items.Count; i++)
                {
                    check |= this.checkedListBox1.GetItemChecked(i);
                }
                this.button2.Enabled = check;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult r = DialogResult.Retry;
            while (r == DialogResult.Retry)
                r = MessageBox.Show("Unable to connect to Kernel, Error: DMXControl April Fools' Day 2017", "Error",
                    MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
        }

        private void SleepLoop(int sleep)
        {
            while (sleep > 50)
            {
                Thread.Sleep(50);
                Application.DoEvents();
                sleep -= 50;
            }
            Thread.Sleep(sleep);
        }
    }
}
