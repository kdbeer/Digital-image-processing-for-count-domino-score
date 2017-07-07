namespace WannaPlay
{
    partial class gameFootageForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(gameFootageForm));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.playerALabel = new System.Windows.Forms.Label();
            this.playerBLabel = new System.Windows.Forms.Label();
            this.player1_Statur = new System.Windows.Forms.Label();
            this.player2_Statur = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.historyDiv = new System.Windows.Forms.Panel();
            this.historyPanel = new System.Windows.Forms.PictureBox();
            this.rightFlag = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.leftFlag = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.sumscorePanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.EndScore = new System.Windows.Forms.Label();
            this.rightPanel = new System.Windows.Forms.Panel();
            this.cursorPanel = new System.Windows.Forms.Panel();
            this.textInfo = new System.Windows.Forms.Label();
            this.leftPanel = new System.Windows.Forms.Panel();
            this.endturnBtn = new System.Windows.Forms.Button();
            this.restartBtn = new System.Windows.Forms.Button();
            this.resetButton = new System.Windows.Forms.Button();
            this.homeButton = new System.Windows.Forms.Button();
            this.doneButton = new System.Windows.Forms.Button();
            this.imagePanel = new Emgu.CV.UI.ImageBox();
            this.player2_panel = new System.Windows.Forms.Panel();
            this.playerBProgress = new System.Windows.Forms.ProgressBar();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.p2DomPrice = new Emgu.CV.UI.ImageBox();
            this.playerBSumScore = new System.Windows.Forms.Label();
            this.player1_panel = new System.Windows.Forms.Panel();
            this.playerAProgress = new System.Windows.Forms.ProgressBar();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.playerASumScore = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.historyDiv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.historyPanel)).BeginInit();
            this.rightFlag.SuspendLayout();
            this.leftFlag.SuspendLayout();
            this.sumscorePanel.SuspendLayout();
            this.cursorPanel.SuspendLayout();
            this.leftPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.imagePanel)).BeginInit();
            this.player2_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.p2DomPrice)).BeginInit();
            this.player1_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // playerALabel
            // 
            this.playerALabel.AutoSize = true;
            this.playerALabel.BackColor = System.Drawing.Color.Turquoise;
            this.playerALabel.Font = new System.Drawing.Font("Palatino Linotype", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerALabel.ForeColor = System.Drawing.Color.White;
            this.playerALabel.Location = new System.Drawing.Point(498, 264);
            this.playerALabel.Name = "playerALabel";
            this.playerALabel.Padding = new System.Windows.Forms.Padding(5);
            this.playerALabel.Size = new System.Drawing.Size(83, 33);
            this.playerALabel.TabIndex = 9;
            this.playerALabel.Text = "Player A";
            this.playerALabel.Visible = false;
            // 
            // playerBLabel
            // 
            this.playerBLabel.AutoSize = true;
            this.playerBLabel.BackColor = System.Drawing.Color.MediumPurple;
            this.playerBLabel.Font = new System.Drawing.Font("Palatino Linotype", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerBLabel.ForeColor = System.Drawing.Color.White;
            this.playerBLabel.Location = new System.Drawing.Point(938, 264);
            this.playerBLabel.Name = "playerBLabel";
            this.playerBLabel.Padding = new System.Windows.Forms.Padding(5);
            this.playerBLabel.Size = new System.Drawing.Size(82, 33);
            this.playerBLabel.TabIndex = 10;
            this.playerBLabel.Text = "Player B";
            this.playerBLabel.Visible = false;
            // 
            // player1_Statur
            // 
            this.player1_Statur.AutoSize = true;
            this.player1_Statur.BackColor = System.Drawing.Color.Transparent;
            this.player1_Statur.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.player1_Statur.Location = new System.Drawing.Point(354, 375);
            this.player1_Statur.Name = "player1_Statur";
            this.player1_Statur.Size = new System.Drawing.Size(95, 24);
            this.player1_Statur.TabIndex = 14;
            this.player1_Statur.Text = "Your Turn";
            // 
            // player2_Statur
            // 
            this.player2_Statur.AutoSize = true;
            this.player2_Statur.BackColor = System.Drawing.Color.Transparent;
            this.player2_Statur.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.player2_Statur.Location = new System.Drawing.Point(354, 405);
            this.player2_Statur.Name = "player2_Statur";
            this.player2_Statur.Size = new System.Drawing.Size(46, 24);
            this.player2_Statur.TabIndex = 15;
            this.player2_Statur.Text = "Wait";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // historyDiv
            // 
            this.historyDiv.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(46)))), ((int)(((byte)(47)))));
            this.historyDiv.Controls.Add(this.historyPanel);
            this.historyDiv.Location = new System.Drawing.Point(242, 489);
            this.historyDiv.Name = "historyDiv";
            this.historyDiv.Size = new System.Drawing.Size(1037, 207);
            this.historyDiv.TabIndex = 20;
            // 
            // historyPanel
            // 
            this.historyPanel.BackColor = System.Drawing.Color.Transparent;
            this.historyPanel.Location = new System.Drawing.Point(20, 5);
            this.historyPanel.Name = "historyPanel";
            this.historyPanel.Size = new System.Drawing.Size(1031, 191);
            this.historyPanel.TabIndex = 0;
            this.historyPanel.TabStop = false;
            // 
            // rightFlag
            // 
            this.rightFlag.BackColor = System.Drawing.Color.Transparent;
            this.rightFlag.BackgroundImage = global::WannaPlay.Properties.Resources.banner_width_120_x_47;
            this.rightFlag.Controls.Add(this.label2);
            this.rightFlag.Location = new System.Drawing.Point(834, 38);
            this.rightFlag.Name = "rightFlag";
            this.rightFlag.Size = new System.Drawing.Size(120, 47);
            this.rightFlag.TabIndex = 103;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(33, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 13);
            this.label2.TabIndex = 104;
            this.label2.Text = "Player B";
            // 
            // leftFlag
            // 
            this.leftFlag.BackColor = System.Drawing.Color.Transparent;
            this.leftFlag.BackgroundImage = global::WannaPlay.Properties.Resources.banner_width_120_x_47;
            this.leftFlag.Controls.Add(this.label1);
            this.leftFlag.Location = new System.Drawing.Point(12, 375);
            this.leftFlag.Name = "leftFlag";
            this.leftFlag.Size = new System.Drawing.Size(120, 47);
            this.leftFlag.TabIndex = 102;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(34, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Player A";
            // 
            // sumscorePanel
            // 
            this.sumscorePanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.sumscorePanel.BackgroundImage = global::WannaPlay.Properties.Resources._78398_OFK2T5_910_140_x_120;
            this.sumscorePanel.Controls.Add(this.label5);
            this.sumscorePanel.Controls.Add(this.EndScore);
            this.sumscorePanel.Location = new System.Drawing.Point(242, 151);
            this.sumscorePanel.Name = "sumscorePanel";
            this.sumscorePanel.Size = new System.Drawing.Size(140, 120);
            this.sumscorePanel.TabIndex = 101;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.label5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(46)))), ((int)(((byte)(47)))));
            this.label5.Location = new System.Drawing.Point(30, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 20);
            this.label5.TabIndex = 1;
            this.label5.Text = "แต้มปลาย";
            // 
            // EndScore
            // 
            this.EndScore.AutoSize = true;
            this.EndScore.BackColor = System.Drawing.Color.Transparent;
            this.EndScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.EndScore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(46)))), ((int)(((byte)(47)))));
            this.EndScore.Location = new System.Drawing.Point(43, 24);
            this.EndScore.Name = "EndScore";
            this.EndScore.Size = new System.Drawing.Size(52, 55);
            this.EndScore.TabIndex = 0;
            this.EndScore.Text = "0";
            // 
            // rightPanel
            // 
            this.rightPanel.BackColor = System.Drawing.Color.Transparent;
            this.rightPanel.BackgroundImage = global::WannaPlay.Properties.Resources.Mobile_Dashboard_Template_right;
            this.rightPanel.Location = new System.Drawing.Point(1072, 276);
            this.rightPanel.Name = "rightPanel";
            this.rightPanel.Size = new System.Drawing.Size(207, 207);
            this.rightPanel.TabIndex = 6;
            // 
            // cursorPanel
            // 
            this.cursorPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(205)))), ((int)(((byte)(205)))), ((int)(((byte)(205)))));
            this.cursorPanel.BackgroundImage = global::WannaPlay.Properties.Resources.texthome_85_x_85;
            this.cursorPanel.Controls.Add(this.textInfo);
            this.cursorPanel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(46)))), ((int)(((byte)(47)))));
            this.cursorPanel.Location = new System.Drawing.Point(242, 284);
            this.cursorPanel.Name = "cursorPanel";
            this.cursorPanel.Size = new System.Drawing.Size(85, 85);
            this.cursorPanel.TabIndex = 100;
            // 
            // textInfo
            // 
            this.textInfo.AutoSize = true;
            this.textInfo.BackColor = System.Drawing.Color.Transparent;
            this.textInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.textInfo.ForeColor = System.Drawing.Color.White;
            this.textInfo.Location = new System.Drawing.Point(17, 29);
            this.textInfo.Name = "textInfo";
            this.textInfo.Size = new System.Drawing.Size(52, 18);
            this.textInfo.TabIndex = 0;
            this.textInfo.Text = "label4";
            // 
            // leftPanel
            // 
            this.leftPanel.BackColor = System.Drawing.Color.Transparent;
            this.leftPanel.BackgroundImage = global::WannaPlay.Properties.Resources.Mobile_Dashboard_Template;
            this.leftPanel.Controls.Add(this.endturnBtn);
            this.leftPanel.Controls.Add(this.restartBtn);
            this.leftPanel.Controls.Add(this.resetButton);
            this.leftPanel.Controls.Add(this.homeButton);
            this.leftPanel.Location = new System.Drawing.Point(14, 539);
            this.leftPanel.Name = "leftPanel";
            this.leftPanel.Size = new System.Drawing.Size(207, 207);
            this.leftPanel.TabIndex = 5;
            this.leftPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.leftPanel_Paint);
            // 
            // endturnBtn
            // 
            this.endturnBtn.BackgroundImage = global::WannaPlay.Properties.Resources.goal;
            this.endturnBtn.FlatAppearance.BorderSize = 0;
            this.endturnBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.endturnBtn.ForeColor = System.Drawing.Color.Transparent;
            this.endturnBtn.Location = new System.Drawing.Point(64, 152);
            this.endturnBtn.Name = "endturnBtn";
            this.endturnBtn.Size = new System.Drawing.Size(33, 33);
            this.endturnBtn.TabIndex = 36;
            this.endturnBtn.UseVisualStyleBackColor = true;
            this.endturnBtn.Click += new System.EventHandler(this.button4_Click);
            this.endturnBtn.MouseLeave += new System.EventHandler(this.button4_MouseLeave);
            this.endturnBtn.MouseHover += new System.EventHandler(this.button4_MouseHover);
            // 
            // restartBtn
            // 
            this.restartBtn.BackgroundImage = global::WannaPlay.Properties.Resources.refresh;
            this.restartBtn.FlatAppearance.BorderSize = 0;
            this.restartBtn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.restartBtn.ForeColor = System.Drawing.Color.Transparent;
            this.restartBtn.Location = new System.Drawing.Point(21, 113);
            this.restartBtn.Name = "restartBtn";
            this.restartBtn.Size = new System.Drawing.Size(33, 33);
            this.restartBtn.TabIndex = 35;
            this.restartBtn.UseVisualStyleBackColor = true;
            this.restartBtn.Click += new System.EventHandler(this.restartBtn_Click);
            this.restartBtn.MouseLeave += new System.EventHandler(this.restartBtn_MouseLeave);
            this.restartBtn.MouseHover += new System.EventHandler(this.restartBtn_MouseHover);
            // 
            // resetButton
            // 
            this.resetButton.BackgroundImage = global::WannaPlay.Properties.Resources.undo_button;
            this.resetButton.FlatAppearance.BorderSize = 0;
            this.resetButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.resetButton.ForeColor = System.Drawing.Color.Transparent;
            this.resetButton.Location = new System.Drawing.Point(21, 61);
            this.resetButton.Name = "resetButton";
            this.resetButton.Size = new System.Drawing.Size(33, 33);
            this.resetButton.TabIndex = 34;
            this.resetButton.UseVisualStyleBackColor = true;
            this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
            this.resetButton.MouseLeave += new System.EventHandler(this.resetButton_MouseLeave);
            this.resetButton.MouseHover += new System.EventHandler(this.resetButton_MouseHover);
            // 
            // homeButton
            // 
            this.homeButton.BackgroundImage = global::WannaPlay.Properties.Resources.home_icon_silhouette;
            this.homeButton.FlatAppearance.BorderSize = 0;
            this.homeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.homeButton.ForeColor = System.Drawing.Color.Transparent;
            this.homeButton.Location = new System.Drawing.Point(64, 19);
            this.homeButton.Name = "homeButton";
            this.homeButton.Size = new System.Drawing.Size(33, 33);
            this.homeButton.TabIndex = 33;
            this.homeButton.UseVisualStyleBackColor = true;
            this.homeButton.Click += new System.EventHandler(this.homeButton_Click);
            this.homeButton.MouseLeave += new System.EventHandler(this.homeButton_MouseLeave);
            this.homeButton.MouseHover += new System.EventHandler(this.homeButton_MouseHover);
            // 
            // doneButton
            // 
            this.doneButton.BackColor = System.Drawing.Color.Transparent;
            this.doneButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("doneButton.BackgroundImage")));
            this.doneButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.doneButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doneButton.ForeColor = System.Drawing.Color.White;
            this.doneButton.Location = new System.Drawing.Point(683, 244);
            this.doneButton.Name = "doneButton";
            this.doneButton.Size = new System.Drawing.Size(130, 52);
            this.doneButton.TabIndex = 25;
            this.doneButton.Text = "Done";
            this.doneButton.UseVisualStyleBackColor = false;
            this.doneButton.Visible = false;
            this.doneButton.Click += new System.EventHandler(this.doneButton_Click);
            // 
            // imagePanel
            // 
            this.imagePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imagePanel.Location = new System.Drawing.Point(455, 108);
            this.imagePanel.Name = "imagePanel";
            this.imagePanel.Size = new System.Drawing.Size(600, 525);
            this.imagePanel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imagePanel.TabIndex = 2;
            this.imagePanel.TabStop = false;
            // 
            // player2_panel
            // 
            this.player2_panel.BackColor = System.Drawing.Color.Transparent;
            this.player2_panel.BackgroundImage = global::WannaPlay.Properties.Resources.lastPut2;
            this.player2_panel.Controls.Add(this.playerBProgress);
            this.player2_panel.Controls.Add(this.pictureBox2);
            this.player2_panel.Controls.Add(this.p2DomPrice);
            this.player2_panel.Controls.Add(this.playerBSumScore);
            this.player2_panel.Location = new System.Drawing.Point(1142, 12);
            this.player2_panel.Name = "player2_panel";
            this.player2_panel.Size = new System.Drawing.Size(200, 261);
            this.player2_panel.TabIndex = 2;
            // 
            // playerBProgress
            // 
            this.playerBProgress.Location = new System.Drawing.Point(3, 200);
            this.playerBProgress.Maximum = 50;
            this.playerBProgress.Name = "playerBProgress";
            this.playerBProgress.Size = new System.Drawing.Size(130, 5);
            this.playerBProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.playerBProgress.TabIndex = 106;
            this.playerBProgress.Visible = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Location = new System.Drawing.Point(68, 119);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(60, 60);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 9;
            this.pictureBox2.TabStop = false;
            // 
            // p2DomPrice
            // 
            this.p2DomPrice.BackColor = System.Drawing.Color.Transparent;
            this.p2DomPrice.Location = new System.Drawing.Point(68, 136);
            this.p2DomPrice.Name = "p2DomPrice";
            this.p2DomPrice.Size = new System.Drawing.Size(59, 38);
            this.p2DomPrice.TabIndex = 6;
            this.p2DomPrice.TabStop = false;
            // 
            // playerBSumScore
            // 
            this.playerBSumScore.AutoSize = true;
            this.playerBSumScore.BackColor = System.Drawing.Color.Transparent;
            this.playerBSumScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerBSumScore.ForeColor = System.Drawing.Color.White;
            this.playerBSumScore.Location = new System.Drawing.Point(61, 34);
            this.playerBSumScore.Name = "playerBSumScore";
            this.playerBSumScore.Size = new System.Drawing.Size(74, 37);
            this.playerBSumScore.TabIndex = 5;
            this.playerBSumScore.Text = "000";
            // 
            // player1_panel
            // 
            this.player1_panel.BackColor = System.Drawing.Color.Transparent;
            this.player1_panel.BackgroundImage = global::WannaPlay.Properties.Resources.lastPut2;
            this.player1_panel.Controls.Add(this.playerAProgress);
            this.player1_panel.Controls.Add(this.pictureBox1);
            this.player1_panel.Controls.Add(this.playerASumScore);
            this.player1_panel.Location = new System.Drawing.Point(12, 108);
            this.player1_panel.Name = "player1_panel";
            this.player1_panel.Size = new System.Drawing.Size(200, 261);
            this.player1_panel.TabIndex = 5;
            // 
            // playerAProgress
            // 
            this.playerAProgress.Location = new System.Drawing.Point(3, 200);
            this.playerAProgress.Maximum = 50;
            this.playerAProgress.Name = "playerAProgress";
            this.playerAProgress.Size = new System.Drawing.Size(130, 5);
            this.playerAProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.playerAProgress.TabIndex = 105;
            this.playerAProgress.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Location = new System.Drawing.Point(67, 119);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(60, 60);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 8;
            this.pictureBox1.TabStop = false;
            // 
            // playerASumScore
            // 
            this.playerASumScore.AutoSize = true;
            this.playerASumScore.BackColor = System.Drawing.Color.Transparent;
            this.playerASumScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.playerASumScore.ForeColor = System.Drawing.Color.White;
            this.playerASumScore.Location = new System.Drawing.Point(62, 34);
            this.playerASumScore.Name = "playerASumScore";
            this.playerASumScore.Size = new System.Drawing.Size(74, 37);
            this.playerASumScore.TabIndex = 5;
            this.playerASumScore.Text = "000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 104;
            this.label3.Text = "label3";
            // 
            // pictureBox3
            // 
            this.pictureBox3.BackColor = System.Drawing.Color.DarkRed;
            this.pictureBox3.Location = new System.Drawing.Point(15, 441);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(197, 92);
            this.pictureBox3.TabIndex = 105;
            this.pictureBox3.TabStop = false;
            // 
            // gameFootageForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1354, 757);
            this.Controls.Add(this.pictureBox3);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rightFlag);
            this.Controls.Add(this.leftFlag);
            this.Controls.Add(this.sumscorePanel);
            this.Controls.Add(this.rightPanel);
            this.Controls.Add(this.cursorPanel);
            this.Controls.Add(this.historyDiv);
            this.Controls.Add(this.leftPanel);
            this.Controls.Add(this.player2_Statur);
            this.Controls.Add(this.player1_Statur);
            this.Controls.Add(this.playerBLabel);
            this.Controls.Add(this.playerALabel);
            this.Controls.Add(this.doneButton);
            this.Controls.Add(this.imagePanel);
            this.Controls.Add(this.player2_panel);
            this.Controls.Add(this.player1_panel);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(222)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1370, 810);
            this.Name = "gameFootageForm";
            this.Text = "WannaPlay Build V 1.00";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.gameFootageForm_FormClosed);
            this.Load += new System.EventHandler(this.gameFootageForm_Load);
            this.Resize += new System.EventHandler(this.gameFootageForm_Resize);
            this.historyDiv.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.historyPanel)).EndInit();
            this.rightFlag.ResumeLayout(false);
            this.rightFlag.PerformLayout();
            this.leftFlag.ResumeLayout(false);
            this.leftFlag.PerformLayout();
            this.sumscorePanel.ResumeLayout(false);
            this.sumscorePanel.PerformLayout();
            this.cursorPanel.ResumeLayout(false);
            this.cursorPanel.PerformLayout();
            this.leftPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.imagePanel)).EndInit();
            this.player2_panel.ResumeLayout(false);
            this.player2_panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.p2DomPrice)).EndInit();
            this.player1_panel.ResumeLayout(false);
            this.player1_panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label playerASumScore;
        private System.Windows.Forms.Panel player1_panel;
        private System.Windows.Forms.Label playerBSumScore;
        private Emgu.CV.UI.ImageBox p2DomPrice;
        private System.Windows.Forms.Panel player2_panel;
        private Emgu.CV.UI.ImageBox imagePanel;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button doneButton;
        private System.Windows.Forms.Label playerALabel;
        private System.Windows.Forms.Label playerBLabel;
        private System.Windows.Forms.Label player1_Statur;
        private System.Windows.Forms.Label player2_Statur;
        private System.Windows.Forms.Timer timer1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Panel leftPanel;
        private System.Windows.Forms.Panel historyDiv;
        private System.Windows.Forms.PictureBox historyPanel;
        private System.Windows.Forms.Button homeButton;
        private System.Windows.Forms.Button resetButton;
        private System.Windows.Forms.Button restartBtn;
        private System.Windows.Forms.Button endturnBtn;
        private System.Windows.Forms.Panel cursorPanel;
        private System.Windows.Forms.Label textInfo;
        private System.Windows.Forms.Panel rightPanel;
        private System.Windows.Forms.Panel sumscorePanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label EndScore;
        private System.Windows.Forms.Panel leftFlag;
        private System.Windows.Forms.Panel rightFlag;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ProgressBar playerAProgress;
        private System.Windows.Forms.ProgressBar playerBProgress;
        private System.Windows.Forms.PictureBox pictureBox3;
    }
}