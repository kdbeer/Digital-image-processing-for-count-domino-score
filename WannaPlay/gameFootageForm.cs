using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using System.Windows.Forms;
using count;
using System.Collections.Generic;
using getDomino;
using gameActionTest;
using WannaPlay.ImageClass;
using Emgu.CV.CvEnum;
using System.Linq;
using System.Media;

namespace WannaPlay
{
    public partial class gameFootageForm : Form
    {
        int saveNumber = 0;
        bool clickAndnotOverFlow = false;
        int grabFramSum = 0;
        List<String> leftSide = new List<String>();
        List<String> rightSide = new List<String>();
        SoundPlayer warnSound, shutterSound;

        int ticky = 0;

        public static string gameName = "";
        public static int maximumScore = 0;
        public static int timeLimit = 0;
        public static int gameMode = 5;

        private int playerPuttime;

        Image<Bgr, byte> connectImage = new Image<Bgr, byte>(10,10);
        Capture capture;
        Mat globalMat;
        //Rectangle globalRec = new Rectangle(new Point(85, 0), new Size(470, 425));
        Rectangle globalRec = new Rectangle(new Point(85, 90), new Size(470, 425));
        //Rectangle chooseRec = new Rectangle();
        //Image<Bgr, byte> alphaImg = new Image<Bgr, byte>(@"E:\Image Processing Project\Ui design\HUD\alphaImg.png");

        bool isPaused = false;
        bool isEndTurn = false;
        Rectangle buttonRecLeft = new Rectangle(new Point(0, 0), new Size(50, 50));


        Image<Gray, byte> historyImg;
        DominoGame dominoGame = new DominoGame();
        byte alpha = 30;
        private double uiBlacksize = 7.5;
        bool playerClick = false;

        private GameActiviry gameActivity;
        private dominnoCounting domCount;

        List<Point> isPuttedDomino = new List<Point>();
        int[] domTail = new int[2];
        string[] domDirection = new string[2];
        bool isBlock = false;
        bool waitingActivete = false;
        int domduplicate = 0;
        int cFrame = 0;

        int saveImgCount = 0;
        LineSegment2D lsm;
        private Image<Gray, byte> tempGrayImage;
        private Image<Bgr, byte> tempColorImage;

        //timing
        private int secondTick = 0;
        private int minuteTick = 0;
        private int timeRemaining = -1;

        //Scoreing
        GameScore gameScore;
        Domino domino;

        //Domino putting checking here
        private Image<Gray, byte> currentFrame;
        private Image<Gray, byte> previousFrame;

        //Check if score not match

        //Restart
        private bool isNewGame = false;
        private int[] initScore = new int[2];
        private double focusProp = 35;

        //Path
        Image<Bgr, byte> cLeft, cRight;


        //Image drawer
        private Image<Bgr, byte>[] domImg = new Image<Bgr, byte>[7];

        //Boolean of put 2 item
        bool exp1 = false, exp2 = false, isItemMatch = true;
        private Image<Bgr, byte> turnProcess;
        private dominnoCounting turnDc;
        private Rectangle turnRecTemp;
        private Image<Bgr, byte> detectErrorImage;
        private bool mouuseHover;

        private Color progressColor;

        
        public gameFootageForm(string title, int MaxScore, int limitTime, int gm)
        {
            InitializeComponent();
            this.Text = "Wannaplay : " + title;
            gameName = title;
            maximumScore = MaxScore;
            timeLimit = limitTime;
            gameMode = gm;

            gameScore = new GameScore();
            gameScore.Score[0] = 0;
            gameScore.Score[1] = 0;
            initScore[0] = 0;
            initScore[1] = 1;
            playerASumScore.Text = "0";
            playerBSumScore.Text = "0";

            //Image Background Mode
            onGameLoad();
            isNewGame = true;
        }
        public gameFootageForm(string title, int MaxScore, int limitTime, int gm, int scoreA, int scoreB)
        {
            InitializeComponent();
            this.Text = "Wannaplay : " + title;
            gameName = title;
            //maximumScore = MaxScore;
            maximumScore = MaxScore;
            timeLimit = limitTime;
            gameMode = gm;

            //Init
            onGameLoad();
            initScore[0] = scoreA;
            initScore[1] = scoreB;

            gameScore.Score[0] = scoreA;
            gameScore.Score[1] = scoreB;
            playerASumScore.Text = scoreA.ToString();
            playerBSumScore.Text = scoreB.ToString();

            if (scoreB > scoreA)
                gameScore.turn = 1;
                
            //lsm = new LineSegment2D(new Point(imagePanel.Location.X + (imagePanel.Width / 2), imagePanel.Location.Y), new Point(imagePanel.Location.X + (imagePanel.Width / 2), imagePanel.Location.Y + imagePanel.Height));

        }
        private void gameFootageForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void gameFootageForm_Resize(object sender, EventArgs e)
        {
            /*hudScore.Location = new Point(((this.Size.Width / 2) - (hudScore.Width / 2)), hudScore.Location.Y);
            player2_panel.Location = new Point(-25 + this.Size.Width - player2_panel.Width, player2_panel.Location.Y);
            imagePanel.Location = new Point((player1_panel.Location.X + player2_label.Location.X) / 2, player1_panel.Location.Y);
            historyPanel.Width = player1_panel.Location.X + player2_panel.Location.X - player2_panel.Width;
            historyPanel.Height = this.Height - historyPanel.Location.Y - 50;

            historyImg = new Image<Gray, byte>(historyPanel.Width - 5, historyPanel.Height - 5);
            dominoGame.drawNewHistory(historyImg.Width, historyImg.Height);
            historyPanel.Image = dominoGame.getHistoryimage();
            imagePanel.Location = new Point((Size.Width / 2) - (imagePanel.Width / 2), player1_panel.Location.Y);*/

            //Player HUD
            onGameResize();



            //Position endturn control panel
            /*doneButton.Location = new Point(imagePanel.Location.X + (imagePanel.Width / 2) - (doneButton.Width / 2), imagePanel.Location.Y + (imagePanel.Height / 2) - (doneButton.Height / 2));
            player1_label.Location = new Point(doneButton.Location.X - doneButton.Width, imagePanel.Location.Y + 20);
            player2_label.Location = new Point(doneButton.Location.X + 40 + doneButton.Width, imagePanel.Location.Y + 20);

            backButton.Location = new Point(imagePanel.Location.X + imagePanel.Width + 15, backButton.Location.Y);*/
            //setPlayerTurn();
        }
        private void gameFootageForm_Load(object sender, EventArgs e)
        {
            try
            {
                capture = new Capture(2);
                capture.SetCaptureProperty(CapProp.Focus, focusProp);
                Application.Idle += grabNextFrame;
            }
            catch
            {
                MessageBox.Show("ไม่พบกล้อง กรุณาตรวจสอบการเชื่อมต่อของคุณ");
            }
        }


        /// <summary>
        /// Restart game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reset()
        {
            try
            {
                if(isNewGame)
                {
                    gameFootageForm gameForm = new gameFootageForm(gameName, maximumScore, timeLimit, gameMode);
                    gameForm.Show();
                    this.Hide();
                } else
                {
                    gameFootageForm gameForm = new gameFootageForm(gameName, maximumScore, timeLimit, gameMode, initScore[0], initScore[1]);
                    gameForm.Show();
                    this.Hide();
                }
            } catch
            {
                Application.Restart();
            }


        }
       
        /// <summary>
        /// Process image from camera and control the game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void grabNextFrame(object sender, EventArgs e)
        {
            globalMat = capture.QueryFrame();
            connectImage = globalMat.ToImage<Bgr, byte>();
            connectImage = connectImage.Rotate(180, new Bgr(255, 255, 255));

            if (gameActivity == null)
                gameActivity = new GameActiviry(connectImage.Copy());
            else
                gameActivity.setImage(connectImage.Copy());           

            if (!isEndTurn)
            {
                gameActivity.updateFrame(gameScore.turn);

                if (gameActivity != null)
                {
                    //imageBox1.Image = GameActiviry.temp1;
                }

                //Test Average
                this.detectError();
                if(!isItemMatch)
                    return;
                this.scoringAction();
                if (gameActivity.playerClick == true)
                {
                    ticky = secondTick;
                    clickAndnotOverFlow = true;
                    int domIndex = 0;
                    domDirection = gameActivity.DomDiretion;
                    foreach (Image<Bgr, byte> item in gameActivity.getOutputImage())
                    {
                        if (domCount == null)
                            domCount = new dominnoCounting(item);
                        else
                            domCount.update(item);
                        gameActivity.playerClick = false;

                        List<int> a = domCount.getdominoScore();
                        int left = 0;
                        int right = 0;
                        int faceCount = 0;
                        foreach (int item1 in a)
                        {
                            if (faceCount == 0)
                                left = item1;
                            else
                                right = item1;
                            faceCount++;
                        }

                        domIndex++;
                    }
                }
                setPlayerTurn();
            }

            endturnActivity();

            if (!isEndTurn)
                imagePanel.Image = gameActivity.workingFrame;
            else
            {
                Image<Bgr, byte> temp = gameActivity.workingFrame.Copy();
                temp.Draw(lsm, new Bgr(0, 0, 255), 3);
                imagePanel.Image = temp;
            }

            previousFrame = gameActivity.workingFrame.Copy().Convert<Gray, byte>();
            currentFrame = gameActivity.workingFrame.Copy().Convert<Gray, byte>();

            try
            {
                //imageBox1.Image = gameActivity.processIMg;
                pictureBox3.Image = gameActivity.processIMg.ToBitmap();
            }
            catch { }

            label3.Text = gameActivity.CTN.ToString();
        }

        private void detectError()
        {
            if (!isItemMatch)
            {
                int backFormSub = gameActivity.isRemoveWrongDom();
               // imageBox1.Image = GameActiviry.temp1;

                if (backFormSub < 700)
                {
                    isItemMatch = true;
                    gameScore.isMatch = true;
                }

                detectErrorImage = gameActivity.workingFrame.Copy();
                Rectangle drawRec = new Rectangle(new Point(15, 20), new Size(detectErrorImage.Width - 40, detectErrorImage.Height - 40));
                detectErrorImage.Draw(drawRec, new Bgr(0, 150, 255), 2);
                string textPut = "Wrong domino is putted";
                CvInvoke.PutText(
                   detectErrorImage,
                   textPut,
                   new System.Drawing.Point(drawRec.Location.X + (drawRec.Width / 2) - (textPut.Length * 10 / 2), drawRec.Location.Y + 40),
                   FontFace.HersheyPlain,
                   1.2,
                   new Bgr(0, 0, 255).MCvScalar);
                imagePanel.Image = detectErrorImage;                
            }
        }
        private void setPlayerTurn()
        {
            int pgBar = 50 - (int)gameActivity.getBP() > 0 ? 50 - (int)gameActivity.getBP() : 0;
            if (pgBar > 30)
                progressColor = Color.Green;
            else
                progressColor = Color.Tomato;

            if (gameScore.turn == 0)
            {
                if (timeLimit != -1)
                    player1_Statur.Text = playerPuttime + "";
                else
                    player1_Statur.Text = "Your Turn";

                player2_Statur.Text = "Wait";
                player1_Statur.ForeColor = Color.LawnGreen;
                player2_Statur.ForeColor = Color.Tomato;

                //Set click force
                playerAProgress.Visible = true;
                playerBProgress.Visible = false;
                playerAProgress.Value = pgBar;
                playerAProgress.BackColor = progressColor;
            }
            else
            {
                if (timeLimit != -1)
                    player2_Statur.Text = playerPuttime + "";
                else
                    player2_Statur.Text = "Your Turn";
                player1_Statur.Text = "Wait";
                player2_Statur.ForeColor = Color.LawnGreen;
                player1_Statur.ForeColor = Color.Tomato;

                playerAProgress.Visible = false;
                playerBProgress.Visible = true;
                playerBProgress.Value = pgBar;
                playerAProgress.BackColor = progressColor;

            }

            player1_Statur.Location = new Point(player1_panel.Location.X + (player1_panel.Width / 2) - (player1_Statur.Width / 2), player1_panel.Location.Y - 50);
            player2_Statur.Location = new Point(player2_panel.Location.X + (player2_panel.Width / 2) - (player2_Statur.Width / 2), player2_panel.Location.Y - 50);
        }
        private void endturnActivity()
        {
            if (isEndTurn)
            {
                doneButton.Visible = true;
                playerALabel.Visible = true;
                playerBLabel.Visible = true;

                isPaused = false;
                lsm = new LineSegment2D(new Point(gameActivity.workingFrame.Width / 2, 0), new Point(gameActivity.workingFrame.Width / 2, gameActivity.workingFrame.Height));
                connectImage.Draw(lsm, new Bgr(0, 0, 255), 3);
            } else
            {
                doneButton.Visible = false;
                playerALabel.Visible = false;
                playerBLabel.Visible = false;
            }
        }
        private void setDomHistory(int x, int y)
        {
            Point p = new Point(x,y);
            if (p.Y > p.X)
            {
                int t = p.X;
                p.X = p.Y;
                p.Y = t;
            }
            
            // isPuttedDomino ตือ list ของแต้ใโดมิโนที่ถูกบักทึกไว้
            bool foundDom = false;
            foreach (Point item in isPuttedDomino)
            {
                if (item == p)
                {
                    foundDom = true;
                    domduplicate++;
                }
            }
            if (!foundDom)
                isPuttedDomino.Add(p);
            dominoGame.drawHistory(p, historyPanel.Width, historyPanel.Height);
            try
            {
                historyPanel.Image = dominoGame.getHistoryimage().ToBitmap();
            }
            catch { }
        }
        //Buttun click listenner
        /// <summary>
        /// This function separate domino and sum the score
        /// </summary>
        /// <param name="input">Color Image which include upturned left domino</param>
        /// <returns>The sum of all domino left</returns>
        private int upturnedScoreCounting(Image<Bgr, byte> input)
        {
            tempGrayImage = input.Convert<Hsv, byte>().Split()[2].ThresholdBinary(new Gray(60), new Gray(255));
            tempGrayImage = DominoGadget.fillhold(new Point(0, 0), 255, tempGrayImage.Copy());
            tempGrayImage._ThresholdBinaryInv(new Gray(60), new Gray(1));
            ImageSegmentation imgSeg = new ImageSegmentation(tempGrayImage);
            List<Rectangle> recs = imgSeg.bwLabelRec();
            int turnUpScore = 0;
            foreach (Rectangle item in recs)
            {
                if (item.Width > 100 || item.Height > 100 || item.Width < 20 || item.Height < 20)
                    continue;
                turnProcess = tempColorImage.Copy();

                turnRecTemp = new Rectangle(new Point(item.X - 5, item.Y - 5), new Size(item.Width + 10, item.Height + 10));
                
                turnProcess.ROI = turnRecTemp;              
                turnProcess = turnProcess.Copy();
                turnProcess = DominoGadget.reAngleDom(turnProcess.Copy(), turnProcess.Copy().Convert<Gray, byte>());
                turnProcess = DominoGadget.cropImage(turnProcess);                
                turnDc = new dominnoCounting(turnProcess);
                List<int> a = turnDc.getdominoScore();                            
                foreach (int x in a)
                {
                    turnUpScore += x;
                }
            }
            turnUpScore = (turnUpScore / gameMode) * gameMode;
            return turnUpScore;
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (secondTick == 59)
                minuteTick = (minuteTick + 1) % 60;
            secondTick = (secondTick + 1) % 60;
            string m = minuteTick > 9 ? minuteTick.ToString() : "0" + minuteTick;
            string s = secondTick > 9 ? secondTick.ToString() : "0" + secondTick;

            //Set player time remaining
            playerPuttime -= 1;
            if (playerPuttime == 0)
            {
                playerPuttime = timeLimit;
                gameScore.updatePlayerTurn();
            }

            if (mouuseHover == false)
                textInfo.Text = m + " : " + s;
        }
        public bool isRanRegionClear(Image<Gray, byte> input)
        {
            input = input.ThresholdBinaryInv(new Gray(60), new Gray(255));
            if (input.CountNonzero()[0] < 30)
                return true;
            return false;
        }
        private void scoringAction()
        {
            if (clickAndnotOverFlow)
            {
                if (grabFramSum == 10)
                {
                    shutterSound.Play();
                    clickAndnotOverFlow = false;
                    grabFramSum = 0;

                    string rd = rightSide.GroupBy(s => s)
                     .OrderByDescending(s => s.Count())
                     .First().Key;
                    Domino rightDom = new Domino(int.Parse(rd[0].ToString()), int.Parse(rd[1].ToString()));
                    exp1 = gameScore.newPut(rightDom, domDirection[0]);
                    if (exp1)
                        setDomHistory(int.Parse(rd[0].ToString()), int.Parse(rd[1].ToString()));
                    //Test if image is lean
                    if (true)
                    {
                        if (leftSide.Count != 0)
                        {
                            string ld = leftSide.GroupBy(s => s)
                             .OrderByDescending(s => s.Count())
                             .First().Key;
                            Domino leftDom = new Domino(int.Parse(ld[1].ToString()), int.Parse(ld[0].ToString()));
                            exp2 = gameScore.newPut(leftDom, domDirection[1]);
                            if (exp2)
                                setDomHistory(int.Parse(ld[1].ToString()), int.Parse(ld[0].ToString()));
                        }
                    }
                    else
                    {
                        MessageBox.Show("โดมิโนเอียงเกินไป กรุณาตรวจสอบด้วยครับ");
                        //gameScore.isMatch = false;
                        //return;
                    }

                    isItemMatch = exp1 || exp2;
                    EndScore.Text = gameScore.Total.ToString();
                    EndScore.Location = new Point((sumscorePanel.Width / 2) - (EndScore.Width / 2), EndScore.Location.Y);
                    playerASumScore.Text = gameScore.Score[0].ToString();
                    playerBSumScore.Text = gameScore.Score[1].ToString();
                    playerASumScore.Location = new Point(player1_panel.Width / 2 - playerASumScore.Width / 2, playerASumScore.Location.Y);
                    playerBSumScore.Location = new Point(player2_panel.Width / 2 - playerBSumScore.Width / 2, playerBSumScore.Location.Y);

                    //If game is ended
                    if (gameScore.score[0] >= maximumScore || gameScore.score[1] >= maximumScore)
                    {
                        this.Hide();
                        capture.Stop();
                        int winner = gameScore.score[0] > gameScore.score[1] ? 0 : 1;
                        EndGame eg = new EndGame(winner);
                        eg.Show();
                        this.Dispose();
                    }

                    //If domino is match then save current to gameactivity for check error
                    if (isItemMatch)
                    {
                        gameActivity.correct_put_dom = gameActivity.workingFrame.SmoothGaussian(3).Convert<Gray, byte>();
                        gameActivity.correct_put_dom._ThresholdBinary(new Gray(70), new Gray(255));
                        gameActivity.correct_put_dom = DominoGadget.fillhold(new Point(10, 10), 255, gameActivity.correct_put_dom.Copy());
                    }



                    rd = "";
                    leftSide.Clear();
                    rightSide.Clear();

                    cLeft = domImg[gameScore.Left];
                    pictureBox1.Image = cLeft.ToBitmap();
                    cRight = domImg[gameScore.Right];
                    pictureBox2.Image = cRight.ToBitmap();
                }
                grabFramSum++;

                if (grabFramSum % 2 == 0)
                {
                    gameActivity.isClick = true;
                    int countSide = 0;
                    foreach (Image<Bgr, byte> item in gameActivity.getOutputImage())
                    {
                        //item.Save(saveNumber+".jpg");
                        //saveNumber++;
                        if (domCount == null)
                            domCount = new dominnoCounting(item);
                        else
                            domCount.update(item);
                        gameActivity.playerClick = false;

                        List<int> a = domCount.getdominoScore();
                        int left = 0;
                        int right = 0;
                        int faceCount = 0;
                        foreach (int domFace in a)
                        {
                            if (faceCount == 0)
                                left = domFace;
                            else
                                right = domFace;
                            faceCount++;
                        }

                        if (countSide == 0)
                            rightSide.Add(right + "" + left);
                        else
                            leftSide.Add(left + "" + right);


                        //Write domino
                        countSide++;
                    }
                }
            }
        }

        private void leftPanel_Paint(object sender, PaintEventArgs e)
        {

        }

        //Player Activity
        private void doneButton_Click(object sender, EventArgs e)
        {
            Rectangle r = new Rectangle(new Point(0, 0), new Size(gameActivity.workingFrame.Width / 2, gameActivity.workingFrame.Height));
            tempColorImage = gameActivity.workingFrame.Copy();
            tempColorImage.ROI = r;
            int p1 = upturnedScoreCounting(tempColorImage);

            r = new Rectangle(new Point(gameActivity.workingFrame.Width / 2, 0), new Size(gameActivity.workingFrame.Width / 2, gameActivity.workingFrame.Height));
            tempColorImage = gameActivity.workingFrame.Copy();
            tempColorImage.ROI = r;
            int p2 = upturnedScoreCounting(tempColorImage);

            if (p1 > p2)
                gameScore.score[1] += (p1 / gameMode) * gameMode;
            else if (p2 > p1)
                gameScore.score[0] += (p2 / gameMode) * gameMode;

            isEndTurn = false;
            doneButton.Visible = false;
            ShowScore sc = new ShowScore(gameName, gameScore.Score[0], gameScore.Score[1], maximumScore, timeLimit, gameMode);
            this.Hide();
            sc.Show();
        }

        /// <summary>
        /// Initial game element
        /// </summary>
        private void onGameLoad()
        {
            //Initial image panel
            this.MaximizeBox = true;
            this.BackColor = Color.FromArgb(71, 73, 76);
            this.Size = new Size(1370, 810);
            this.BackgroundImage = WannaPlay.Properties.Resources.Blurred_Background2;
            progressColor = Color.Tomato;

            imagePanel.Location = new Point((Size.Width / 2) - (imagePanel.Width / 2), player1_panel.Location.Y);

            historyImg = new Image<Gray, byte>(historyPanel.Width - 5, historyPanel.Height - 5);
            historyImg = historyImg.Or(new Gray(200));
            historyPanel.Image = historyImg.ToBitmap(); ;
            shutterSound = shutterSound = new SoundPlayer(@"D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\Resources\shotterSound.wav");

            //Position endturn control panel
            doneButton.Location = new Point(imagePanel.Location.X + (imagePanel.Width / 2) - (doneButton.Width / 2), imagePanel.Location.Y + (imagePanel.Height / 2) - (doneButton.Height));
            globalMat = new Mat();

            player1_panel.Width = (int)(Width / uiBlacksize);
            player1_panel.Height = (int)(player1_panel.Width * 1.28);
            player1_panel.BackgroundImageLayout = ImageLayout.Stretch;
            player1_panel.Location = new Point(player1_panel.Location.X, 100);

            player2_panel.Width = (int)(Width / uiBlacksize);
            player2_panel.Height = (int)(player2_panel.Width * 1.28);
            player2_panel.BackgroundImageLayout = ImageLayout.Stretch;
            int newLo1Point = (Width - (player2_panel.Width + 25));
            player2_panel.Location = new Point(newLo1Point, player1_panel.Location.Y);

            //Game View
            imagePanel.Location = new Point(player1_panel.Location.X + (int)(player1_panel.Width * 1.1) - 5, 20);
            imagePanel.Width = player2_panel.Location.X - (player1_panel.Location.X + player1_panel.Width) - ((imagePanel.Location.X - (player1_panel.Location.X + player1_panel.Width)) * 2);

            //backButton.Location = new Point(imagePanel.Location.X + imagePanel.Width + 10, imagePanel.Location.Y);

            player1_Statur.Text = "Your Turn";
            player2_Statur.Text = "Wait";
            player1_Statur.ForeColor = Color.LawnGreen;
            player2_Statur.ForeColor = Color.Tomato;

            //timing            
            timer1.Start();
            tempGrayImage = new Image<Gray, byte>(0, 0);
            tempColorImage = new Image<Bgr, byte>(0, 0);

            //Scoring
            gameScore = new GameScore();
            domino = new Domino();
            playerPuttime = timeLimit;
            //endturn
            playerALabel.Location = new Point(imagePanel.Location.X + (doneButton.Location.X - imagePanel.Location.X) / 3, imagePanel.Location.Y + 20);
            playerBLabel.Location = new Point(doneButton.Location.X + doneButton.Width + (doneButton.Location.X - (playerALabel.Location.X + playerALabel.Width)), imagePanel.Location.Y + 20);

            pictureBox1.Location = new Point((player1_panel.Width / 2) - (pictureBox1.Width / 2) - 2, (player1_panel.Height / 2) - (pictureBox1.Height / 4));
            pictureBox2.Location = new Point((player2_panel.Width / 2) - (pictureBox2.Width / 2) - 2, (player2_panel.Height / 2) - (pictureBox2.Height / 4));


            //Load domino graphic to show on screen D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\Resources\Elevator Bell Ring Sound Effect - YouTube_01.wav
            domImg[0] = new Image<Bgr, byte>(@"D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\Resources\dom0.jpg");
            domImg[1] = new Image<Bgr, byte>(@"D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\Resources\dom1.jpg");
            domImg[2] = new Image<Bgr, byte>(@"D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\Resources\dom2.jpg");
            domImg[3] = new Image<Bgr, byte>(@"D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\Resources\dom3.jpg");
            domImg[4] = new Image<Bgr, byte>(@"D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\Resources\dom4.jpg");
            domImg[5] = new Image<Bgr, byte>(@"D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\Resources\dom5.jpg");
            domImg[6] = new Image<Bgr, byte>(@"D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\Resources\dom6.jpg");


            //Menu          

            historyDiv.Location = new Point(imagePanel.Location.X, imagePanel.Location.Y + imagePanel.Height + 10);
            historyDiv.Height = 207;
            historyDiv.Width = imagePanel.Width;

            leftPanel.Location = new Point(historyDiv.Location.X - (leftPanel.Width / 2), historyDiv.Location.Y);
            rightPanel.Location = new Point(historyDiv.Location.X - rightPanel.Width / 2 + historyDiv.Width, leftPanel.Location.Y);
            rightPanel.SendToBack();

            int margin = 7;
            historyPanel.Location = new Point(0, margin);
            historyPanel.Height = historyDiv.Height - margin * 2;
            historyImg = new Image<Gray, byte>(2000, historyPanel.Height, new Gray(205));
            historyPanel.Image = historyImg.ToBitmap();


            cursorPanel.Location = new Point(leftPanel.Location.X + 60, leftPanel.Location.Y + 60);
            textInfo.Text = "00 : 00";
            textInfo.Location = new Point(cursorPanel.Width / 2 - textInfo.Width / 2, cursorPanel.Height / 2 - textInfo.Height / 2);
            sumscorePanel.Location = new Point(rightPanel.Location.X + 10 , historyDiv.Location.Y + (historyDiv.Height / 2) - (sumscorePanel.Height / 2));
            sumscorePanel.BringToFront();

            playerASumScore.Location = new Point(player1_panel.Width / 2 - playerASumScore.Width / 2, playerASumScore.Location.Y);
            playerBSumScore.Location = new Point(player2_panel.Width / 2 - playerBSumScore.Width / 2, playerBSumScore.Location.Y);

            player1_Statur.Location = new Point(player1_panel.Location.X + (player1_panel.Width / 2) - (player1_Statur.Width / 2), 50);
            player2_Statur.Location = new Point(player2_panel.Location.X + (player2_panel.Width / 2) - (player2_Statur.Width / 2), 50);


            //Flag
            leftFlag.Location = new Point(player1_panel.Location.X + player1_panel.Width / 2 - leftFlag.Width / 2, player1_panel.Location.Y + player1_panel.Height - 15 );
            rightFlag.Location = new Point(player2_panel.Location.X + player2_panel.Width / 2 - rightFlag.Width / 2, leftFlag.Location.Y);
            leftFlag.BringToFront();

            //
            playerAProgress.Location = new Point(player1_panel.Width / 2 - playerAProgress.Width / 2, playerAProgress.Location.Y);
            playerBProgress.Location = new Point(player2_panel.Width / 2 - playerAProgress.Width / 2, playerAProgress.Location.Y);

            pictureBox3.Visible = false;

        }
        private void onGameResize()
        {
            player1_panel.Width = (int)(Width / uiBlacksize);
            player1_panel.Height = (int)(player1_panel.Width * 1.28);
            player1_panel.BackgroundImageLayout = ImageLayout.Stretch;
            player1_panel.Location = new Point(player1_panel.Location.X, player1_panel.Location.Y);

            player2_panel.Width = (int)(Width / uiBlacksize);
            player2_panel.Height = (int)(player2_panel.Width * 1.28);
            player2_panel.BackgroundImageLayout = ImageLayout.Stretch;
            int newLo1Point = (Width - (player2_panel.Width + 25));
            player2_panel.Location = new Point(newLo1Point, player1_panel.Location.Y);



            //Game View
            imagePanel.Location = new Point(player1_panel.Location.X + (int)(player1_panel.Width * 1.1), imagePanel.Location.Y);
            imagePanel.Width = player2_panel.Location.X - (player1_panel.Location.X + (int)(player1_panel.Width * 1.8));

            historyPanel.Location = new Point(imagePanel.Location.X, imagePanel.Location.Y + imagePanel.Height + 10);
            historyPanel.Height = Height - historyPanel.Location.Y - 10;

            player1_Statur.Location = new Point(player1_panel.Location.X + (player1_panel.Width / 2) - (player1_Statur.Width / 2), player1_panel.Location.Y + player1_panel.Height + 5);
            player2_Statur.Location = new Point(player2_panel.Location.X + (player2_panel.Width / 2) - (player2_Statur.Width / 2), player2_panel.Location.Y + player2_panel.Height + 5);
        }
        private void homeButton_MouseLeave(object sender, EventArgs e)  {   mouuseHover = false;    }
        private void resetButton_MouseLeave(object sender, EventArgs e) {   mouuseHover = false;     }
        private void restartBtn_MouseLeave(object sender, EventArgs e)  {   mouuseHover = false;     }
        private void button4_MouseLeave(object sender, EventArgs e)     {   mouuseHover = false;     }

        /// <summary>
        /// When click this will header to homepage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void homeButton_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            this.Hide();
            f1.Show();
        }
        /// <summary>
        /// Clear status domino
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void resetButton_Click(object sender, EventArgs e)
        {
            isItemMatch = true;
            isPaused = false;
            imagePanel.Visible = true;
            gameActivity.correct_put_dom = gameActivity.workingFrame.SmoothGaussian(3).Convert<Gray, byte>();
            gameActivity.correct_put_dom._ThresholdBinary(new Gray(70), new Gray(255));
            gameActivity.correct_put_dom = DominoGadget.fillhold(new Point(10, 10), 255, gameActivity.correct_put_dom.Copy());
        }
        /// <summary>
        /// Restart this form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void restartBtn_Click(object sender, EventArgs e)
        {
            reset();
        }
        /// <summary>
        /// End this game
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            if (!isEndTurn)
            {
                if (gameActivity.LeftDom > 0)
                {
                    MessageBox.Show("อุปส์ : ยังไม่สามารถหงายแต้มได้ เนื่องจากยังมีโดมิโนที่สามารถจั่วได้อยู่ค่ะ");
                    return;
                }
                bool isClear = dominoGame.isScreenClear(gameActivity.workingFrame.Convert<Gray, byte>());

                if (isClear)
                {
                    isEndTurn = true;
                }
                else
                    MessageBox.Show("อุปส์ : กรุณาเคลียร์กระดานเพื่อจะสามารถนับแต้มที่เหลือได้");
            } else
            {
                isEndTurn = false;
                endturnActivity();
            }
        }
        private void homeButton_MouseHover(object sender, EventArgs e)
        {            
            textInfo.Text = "Home";
            textInfo.Location = new Point(cursorPanel.Width / 2 - textInfo.Width / 2, cursorPanel.Height / 2 - textInfo.Height / 2);
            mouuseHover = true;
        }
        private void resetButton_MouseHover(object sender, EventArgs e)
        {
            textInfo.Text = "Reset";
            textInfo.Location = new Point(cursorPanel.Width / 2 - textInfo.Width / 2, cursorPanel.Height / 2 - textInfo.Height / 2);
            mouuseHover = true;
        }
        private void restartBtn_MouseHover(object sender, EventArgs e)
        {
            textInfo.Text = "Restart";
            textInfo.Location = new Point(cursorPanel.Width / 2 - textInfo.Width / 2, cursorPanel.Height / 2 - textInfo.Height / 2);
            mouuseHover = true;
        }
        private void button4_MouseHover(object sender, EventArgs e)
        {
            textInfo.Text = "จบเกม";
            textInfo.Location = new Point(cursorPanel.Width / 2 - textInfo.Width / 2, cursorPanel.Height / 2 - textInfo.Height / 2);
            mouuseHover = true;
        }

    }
}