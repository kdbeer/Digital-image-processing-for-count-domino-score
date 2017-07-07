using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace WannaPlay
{
    public partial class Form1 : Form
    {
        Point p;
        string[] headerTipsText = { "ความใจเย็น", "สภาพแวดล้อม", "โดมิโน"};
        string[] contentTipsText = {    "การวางโดมิโนให้ถูกต้อง ไม่เอียง ไม่เบี้ยว จะทำให้การตรวจจับผิดพลาดน้อยลง",
                                        "แสงมีผลกระทบมากต่อการนับแต้ม ดังนั้น การควบคุมแสงจะทำให้การเล่นลื่นไหลขึ้น",
                                        "โดมิโนที่พื้นหลังสีขาว จะทำให้การตรวจจับแม่นยำมาก" };
        Random r = new Random();
        int timeremaining = 0;
        SoundPlayer ops = new SoundPlayer(@"D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\Resources\sound\openWp - pitch.wav");
        public Form1()
        {
            InitializeComponent();

            this.Size = new Size(1200, 800);
            this.BackgroundImage = WannaPlay.Properties.Resources.Blurred_Background2;
            p = new Point((this.Size.Width / 2) - (startPanel.Width / 2), 60);
            startPanel.Location = p;
            tipsPanel.Height = this.Size.Height - tipsPanel.Location.Y;
            domMode.SelectedIndex = 0;
            endScore.SelectedIndex = 2;
            comboBox1.SelectedIndex = 0;
            textBox1.Text = "Untitled";

            int tipRand = r.Next(0, 2);
            headerTips.Text = headerTipsText[tipRand];
            contentTips.Text = contentTipsText[tipRand];

            initailPanel.Location = new Point((this.Width / 2) - (initailPanel.Width / 2), 60);

            //initial visible
            startPanel.Visible = false;
            tipsPanel.Visible = false;
            initialtimer.Start();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            startPanel.Visible = false;
            p = startPanel.Location;
            optionPanel.Location = p;
            optionPanel.Visible = true;
        }
        private void button13_Click(object sender, EventArgs e)
        {
            optionPanel.Visible = false;
            startPanel.Location = p;
            startPanel.Visible = true;            
        }
        private void Form1_Resize(object sender, EventArgs e)
        {
            p = new Point((this.Size.Width / 2) - (startPanel.Width / 2), startPanel.Location.Y);
            startPanel.Location = p;
            optionPanel.Location = p;
            tipsPanel.Width = this.Size.Width - (tipsPanel.Location.X * 3);
            Point pp = new Point((this.Size.Width / 2) - (howToPlayPanel.Width / 2), startPanel.Location.Y);
            howToPlayPanel.Location = pp;
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            string gameName = textBox1.Text;
            int maximum = int.Parse(endScore.Text.ToString());
            int gameMode = domMode.Text.ToString() == "All Three" ? 3 : 5;
            int limitTime = comboBox1.Text.ToString() == "No Limit" ? -1 : int.Parse(comboBox1.Text.ToString());
            this.Hide();
            gameFootageForm g1 = new gameFootageForm(gameName, maximum, limitTime, gameMode);
            g1.Show();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            startPanel.Visible = false;
            creditPanel.Visible = true;
            creditPanel.Location = p;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            creditPanel.Visible = false;
            startPanel.Visible = true;
        }
        private void button7_Click(object sender, EventArgs e)
        {
            Point pp = new Point((this.Size.Width / 2) - (howToPlayPanel.Width / 2), startPanel.Location.Y);
            howToPlayPanel.Location = pp;
            startPanel.Visible = false;
            howToPlayPanel.Visible = true;
        }
        private void button3_Click(object sender, EventArgs e)
        {
            startPanel.Visible = true;
            howToPlayPanel.Visible = false;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
        private void button5_Click(object sender, EventArgs e)
        {
            LoadGameForm lg = new LoadGameForm();
            lg.Show();
            this.Hide();
        }
        private void button9_Click(object sender, EventArgs e)
        {
            DateTimePicker d = new DateTimePicker();
            MessageBox.Show(d.Value.ToString());
        }

        private void initialtimer_Tick(object sender, EventArgs e)
        {
            timeremaining++;
            if (timeremaining > 3)
            {
                startPanel.Visible = true;
                tipsPanel.Visible = true;
                initialtimer.Stop();
                initailPanel.Visible = false;                
                timeremaining = 0;
                startPanel.BringToFront();

            }

            if (timeremaining == 1)
                ops.Play();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
