using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Drawing;
using System.Windows.Forms;

namespace WannaPlay
{
    public partial class ShowScore : Form
    {
        private int score1;
        private int score2;
        private string title;
        private int upScore;
        private string gameTime;
        private String connectionString;
        private int limitTime;
        private int gameMode;

        /// <summary>
        /// Get the result of match and show score then save all of Score to database
        /// </summary>
        /// <param name="title">name of match</param>
        /// <param name="score1">Player A score</param>
        /// <param name="score2">Player B score</param>
        /// <param name="upScore">Max of score</param>
        public ShowScore(string title, int score1, int score2, int upScore, int limitTime, int gameMode)
        {
            InitializeComponent();
            connectionString = @"Data source=D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\bin\Debug\save\save.db";
            label3.Text = score1.ToString();
            label7.Text = score2.ToString();
            label4.Text = "Max Score : " + upScore;
            label3.Location = new Point(label5.Location.X + (label5.Width / 2) - (label3.Width / 2), label3.Location.Y);
            label7.Location = new Point(label6.Location.X + (label6.Width / 2) - (label7.Width / 2), label7.Location.Y);

            this.score1 = score1;
            this.score2 = score2;
            this.title = title;
            this.upScore = upScore;
            this.limitTime = limitTime;
            this.gameMode = gameMode;
            save();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.Show();
        }
        private void save()
        {
            DateTimePicker d = new DateTimePicker();
            gameTime = d.Value.ToString();
            var dic = new Dictionary<string, object>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    SQLiteHelper sh = new SQLiteHelper(cmd);
                    dic["gameTitle"] = this.title;
                    dic["playerAscore"] = this.score1;
                    dic["playerBscore"] = this.score2;
                    dic["upScore"] = this.upScore;
                    dic["date"] = gameTime;
                    dic["limitTime"] = limitTime;
                    dic["gameMode"] = gameMode;
                    sh.Insert("save2", dic);
                    conn.Close();
                }
            }
        }
        private void ShowScore_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
