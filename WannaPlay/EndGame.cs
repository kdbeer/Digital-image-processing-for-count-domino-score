using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace WannaPlay
{
    public partial class EndGame : Form
    {   
        private int player = 0;
        SoundPlayer introSound;
        private string playerSide;

        public EndGame(int player)
        {
            InitializeComponent();
            this.player = player;
            this.Size = new Size(1200, 800);
            introSound = new SoundPlayer(@"D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\Resources\Elevator Bell Ring Sound Effect - YouTube_01.wav");
            playerSide = this.player == 0 ? "A" : "B";
            label1.Text = "PLAYER " + playerSide + " VICTORY";
            label1.Location = new Point(this.panel1.Width / 2 - label1.Width / 2, panel1.Height - (int)(label1.Height * 1.5));
            panel1.Location = new Point(this.Width / 2 - panel1.Width / 2, panel1.Location.Y);
            this.BackgroundImage = WannaPlay.Properties.Resources.bg;
            this.button1.Location = new Point(Width / 2 - button1.Width / 2, Height - 150);
            introSound.Play();
        }

        private void EndGame_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.Show();
        }
    }
}
