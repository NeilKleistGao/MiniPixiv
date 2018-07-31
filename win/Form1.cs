using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace MiniPixiv
{
    public partial class Form1 : Form
    {
        class Data
        {
            public string[] datas = new string[4];
        }

        public Form1()
        {
            InitializeComponent();
        }

        private int index = 1;
        private bool flag = false;
        private ArrayList list = new ArrayList();

        private void Form1_Load(object sender, EventArgs e)
        {
            Thread thread = new Thread(load);
            thread.IsBackground = true;
            thread.Start();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            index++;

            if (index > 10)
            {
                index -= 10;
            }

            pictureBox.ImageLocation = index.ToString() + ".jpg";
            setText();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            if (!flag)
            {
                return;
            }

            Data d = (Data)list[index - 1];
            Process.Start("https://www.pixiv.net/member_illust.php?mode=medium&illust_id=" + d.datas[0]);
        }

        private void load()
        {
            Process process = new Process();
            process.StartInfo.FileName = "main.exe";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            process.WaitForExit();

            StreamReader reader = new StreamReader("output.tmp");
            while (!reader.EndOfStream)
            {
                string str = reader.ReadLine();

                if (str == "Network Error")
                {
                    label.Text = "网络异常，请稍后再试";
                    return;
                }

                int pos = str.IndexOf("#"), count = 0;
                Data d = new Data();

                while (pos != -1)
                {
                    d.datas[count] = str.Substring(0, pos);
                    str = str.Substring(pos + 1); pos = str.IndexOf("#");
                    count++;
                }
                d.datas[count] = str;

                list.Add(d);
            }

            flag = true;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (flag)
            {
                pictureBox.ImageLocation = "1.jpg";
                setText();
                timer.Enabled = false;
                button2.Enabled = true;
            }
        }

        private void setText()
        {
            Data d = (Data)list[index - 1];
            label.Text = d.datas[1] + "\n作者：" + d.datas[2] + "\ntags:\n";
            string temp = d.datas[3];

            int pos = temp.IndexOf(" ");
            while (pos != -1)
            {
                label.Text += temp.Substring(0, pos) + "\n";
                temp = temp.Substring(pos + 1);
                pos = temp.IndexOf(" ");
            }

            label.Text += temp;
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            for (int i = 1; i <= 10; i++)
            {
                string name = i.ToString() + ".jpg";
                if (File.Exists(name))
                {
                    File.Delete(name);
                }
            }

            if (File.Exists("output.tmp"))
            {
                File.Delete("output.tmp");
            }

            this.Close();
        }
    }
}
