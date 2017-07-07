using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WannaPlay
{
    public partial class LoadGameForm : Form
    {
        private BindingSource bindingSource1 = new BindingSource();
        private DataTable dt = new DataTable();
        private String connectionString;
        private int score1 = 0;
        private int score2 = 0;
        private string title = "";
        private int upScore = 150;
        private int limitTime;
        private int gameMode;

        public LoadGameForm()
        {
            InitializeComponent();
            dataGridView1.DataSource = bindingSource1;
            connectionString = @"Data source=D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\bin\Debug\save\save.db";
            dt = new DataTable();
            Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 game = new Form1();
            game.Show();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void LoadGameForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        public void Init()
        {
            loadBtn.Enabled = false;
            string statement = "SELECT * FROM save2 ORDER BY id DESC";
            DataTable temp = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    SQLiteHelper sh = new SQLiteHelper(cmd);
                    temp = sh.Select(statement);
                    conn.Close();
                }
            }

            bindingSource1.DataSource = temp;

            dataGridView1.AutoResizeColumns(
                DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
            this.dataGridView1.ForeColor = Color.Black;
            this.dataGridView1.Columns[0].Visible = false;
            this.dataGridView1.Columns[2].Visible = false;
            this.dataGridView1.Columns[3].Visible = false;
            this.dataGridView1.Columns[4].Visible = false;
            this.dataGridView1.Columns[6].Visible = false;
            this.dataGridView1.Columns[7].Visible = false;

            this.dataGridView1.Columns[1].Width = groupBox2.Width / 2;
            this.dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();            
            form1.Show();
            this.Hide();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                label3.Text = this.dataGridView1[2, e.RowIndex].Value.ToString();
                label7.Text = this.dataGridView1[3, e.RowIndex].Value.ToString();
                label4.Text = "Max Score : " + this.dataGridView1[4, e.RowIndex].Value.ToString();

                label3.Location = new Point(label5.Location.X + (label5.Width / 2) - (label3.Width / 2), label3.Location.Y);
                label7.Location = new Point(label6.Location.X + (label6.Width / 2) - (label7.Width / 2), label7.Location.Y);

                score1 = int.Parse(this.dataGridView1[2, e.RowIndex].Value.ToString());
                score2 = int.Parse(this.dataGridView1[3, e.RowIndex].Value.ToString());
                upScore = int.Parse(this.dataGridView1[4, e.RowIndex].Value.ToString());
                title = this.dataGridView1[1, e.RowIndex].Value.ToString();
                limitTime = int.Parse(this.dataGridView1[6, e.RowIndex].Value.ToString());
                gameMode = int.Parse(this.dataGridView1[7, e.RowIndex].Value.ToString());
                loadBtn.Enabled = true;
            }
            else
            {
                loadBtn.Enabled = false;
            }
        }

        private void loadBtn_Click_1(object sender, EventArgs e)
        {
            gameFootageForm gft = new gameFootageForm(title, upScore, limitTime, gameMode, score1, score2);
            gft.Show();
            this.Hide();
        }
    }
}
