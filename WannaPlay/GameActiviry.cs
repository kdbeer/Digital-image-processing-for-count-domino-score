using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System;
using System.Windows.Forms;
using getDomino;

namespace WannaPlay
{
    class GameActiviry
    {
        //Free static variable
        public static Image<Gray, byte> temp1;
        int ticky = 0;


        private Image<Bgr, byte> image, connectImage, tempImageDrawrec;
        private Image<Gray, byte> globalImage, tempBW, buttonTempImage, grayObserve, edgeObserve;
        //Global Rectangle
        private Rectangle globalRec = new Rectangle(new Point(55, 10), new Size(570, 385));

        private Rectangle[] observRec = new Rectangle[4];
        private Rectangle drawRec, btnObserve;
        private bool isOverFlow = false;
        private int playerTurn = 0;
        private DominoGroup dominoGroup;
        private List<Image<Bgr, byte>> outputImage = new List<Image<Bgr, byte>>();
        private List<Point> dominoFaceScore = new List<Point>();

        //Playsound
        private bool toggleSoundPlay = false;
        private bool prevToggleSoundPlay = false;

        //Click activity
        public bool isClick = false;
        private bool firstFrameClick = false;
        private int countFrame = 0;
        public bool playerClick = false;
        private double nonzeroPixel;
        private int countSave = 0;
        public Image<Gray, byte> clickRegion;

        //Test
        private int ctn = 0;
        private Image<Bgr, byte> imageClick;

        //Choose region
        //This region to place domino to wait player picking up.
        private Rectangle chooseRegion = new Rectangle(new Point(125, 385), new Size(425, 90));
        private Image<Gray, byte> chooseRegionImg;
        private double singleDom = 1715;
        private int leftDom = 0;

        //Assign score
        private int leftFace = 0;
        private int rightFace = 0;
        public bool isSingleDom = false;
        private int dominoCount;
        private string[] domDirection = { "LEFT", "RIGHT" };

        //Getting Player Count
        private int prevDomInScore = 0;
        Image<Gray, byte> ttttyyy = new Image<Gray, byte>(10,10);

        //Detect if put wrong domino
        public Image<Gray, byte> correct_put_dom = new Image<Gray, byte>(0, 0);



        public GameActiviry(Image<Bgr, byte> input)
        {
            image = input;
            grayObserve = input.Copy().Convert<Gray, byte>();
            edgeObserve = input.Copy().Convert<Gray, byte>();
            globalImage = input.Copy().Convert<Gray, byte>().ThresholdBinary(new Gray(60), new Gray(255));
            drawRec = new Rectangle(new Point(15, 20), new Size(globalRec.Width - 40, globalRec.Height - 40));

            connectImage = input.Copy();
            connectImage.ROI = globalRec;
            connectImage = connectImage.Copy();
            observRec[0] = new Rectangle(new Point(0, 0), new Size(drawRec.Location.X, globalRec.Height));
            observRec[1] = new Rectangle(new Point(globalRec.Width - (globalRec.Width - (drawRec.Width + drawRec.Location.X)), 0), new Size(globalRec.Width - (drawRec.Width), globalRec.Height));
            observRec[2] = new Rectangle(new Point(0, 0), new Size(globalRec.Width, drawRec.Location.Y));
            observRec[3] = new Rectangle(new Point(0, globalRec.Height - (globalRec.Height - (drawRec.Location.Y + drawRec.Height))), new Size(globalRec.Width, globalRec.Height - (drawRec.Height)));
            btnObserve = new Rectangle(new Point(0, 0), new Size(79, 90));

            //Choose region
            chooseRegionImg = new Image<Gray, byte>(chooseRegion.Width, chooseRegion.Height);

            //clicking
            clickRegion = new Image<Gray, byte>(0, 0);
            dominoCount = 0;

            correct_put_dom = input.Copy().Convert<Gray, byte>();
            correct_put_dom._ThresholdBinary(new Gray(70), new Gray(255));
            correct_put_dom.ROI = globalRec;
            correct_put_dom = correct_put_dom.Copy();
            correct_put_dom.ROI = drawRec;
            correct_put_dom = correct_put_dom.Copy();
        }
        //Game method
        public void updateFrame(int playerTurn)
        {
            this.playerTurn = playerTurn;        
            this.chooseRegionImg = this.image.Copy().Convert<Gray, byte>();
            setRegion();
            overFlowListenner();
            if (!isOverFlow)
            {
                buttonClickListenner();
                if (isClick)
                {
                    gettingPlayerCount();
                }
            }
        }
        private void gettingPlayerCount()
        {
            temp1 = this.connectImage.Copy().SmoothMedian(9).Convert<Hsv, byte>().Split()[2];
            temp1._ThresholdBinaryInv(new Gray(100), new Gray(255));
            dominoCount = (int)Math.Ceiling(temp1.CountNonzero()[0] / singleDom);
            if (prevDomInScore <= dominoCount)
            {
                getScore();
                isClick = false;
            }
            else
            {
                //MessageBox.Show("The number of dominoes in game doesn't change!!");
            }
            prevDomInScore = dominoCount;
        }
        private void overFlowListenner()
        {
            isOverFlow = false;
            int overCount = 0;
            //tempBW = this.connectImage.Copy().Convert<Hsv, byte>().Split()[0];
            tempBW = this.connectImage.Copy().Convert<Gray, byte>();
            tempBW._SmoothGaussian(5);
            edgeObserve = tempBW.Copy().Convert<Gray, byte>();
            tempBW._ThresholdBinaryInv(new Gray(70), new Gray(255));

            foreach (Rectangle rec in observRec)
            {
                tempBW.ROI = rec;
                overCount = tempBW.CountNonzero()[0];
                ctn = overCount;
                
                if (overCount > 500)
                {
                    isOverFlow = true;
                    break;
                }
                
            }
            if (isOverFlow)
            {
                connectImage.Draw(drawRec, new Bgr(0, 0, 255), 2);
                CvInvoke.PutText(
                   connectImage,
                   "Over Region",
                   new System.Drawing.Point(drawRec.Location.X + (drawRec.Width / 2) - 20, drawRec.Location.Y + 40),
                   FontFace.HersheyPlain,
                   1.0,
                   new Bgr(0, 0, 255).MCvScalar);
                toggleSoundPlay = true;
                if (toggleSoundPlay && !prevToggleSoundPlay)
                {
                    //warnSound.PlayLooping();
                    prevToggleSoundPlay = true;
                }
            }
            else
            {
                toggleSoundPlay = false;
                if (!toggleSoundPlay && prevToggleSoundPlay)
                {
                    //warnSound.Stop();
                    prevToggleSoundPlay = false;
                }
            }
        }
        private void buttonClickListenner()
        {
            if (playerTurn == 0)
                btnObserve.Location = new Point(50, globalRec.Height);
            else
                btnObserve.Location = new Point(globalRec.Width - 23, globalRec.Height);
            buttonTempImage = globalImage.Copy();
            buttonTempImage.ROI = btnObserve;
            clickRegion = grayObserve.Copy();
            clickRegion.ROI = btnObserve;
            buttonTempImage = buttonTempImage.Copy();

            clickRegion = DominoGadget.BasicGlobalThreshold(clickRegion);

            nonzeroPixel = ((double)clickRegion.CountNonzero()[0] / (buttonTempImage.Width * buttonTempImage.Height)) * 100;
            if (nonzeroPixel < 55)
            {
                if (firstFrameClick == false)
                    firstFrameClick = true;
                if (firstFrameClick)

                {
                    countFrame++;
                    if (countFrame >= 10)
                    {
                        countFrame = 0;
                        firstFrameClick = false;
                        isClick = true;
                        playerClick = true;
                    }
                }
            }
            else
            {
                countFrame = 0;
                firstFrameClick = false;
            }
        }
        /// <summary>
        /// Return score from single domino
        /// </summary>
        private void getScore()
        {
            if (!isClick)
                return;
            dominoFaceScore.Clear();
            tempImageDrawrec = this.connectImage.Copy();
            tempImageDrawrec.ROI = drawRec;
            tempImageDrawrec = tempImageDrawrec.Copy();
            if (dominoGroup == null)
                dominoGroup = new DominoGroup(tempImageDrawrec);
            else
                dominoGroup.setImage(tempImageDrawrec);
            dominoGroup.update();
            imageClick = dominoGroup.BeforeProcess;
            outputImage = dominoGroup.getOutputImage();
            domDirection = dominoGroup.DomDirection;

            ttttyyy = dominoGroup.getBWImage().Copy();

            this.isClick = false;
            if (dominoGroup.isSingleDom())
                this.isSingleDom = true;
            else
                this.isSingleDom = false;
            countSave++;
        }

        //Set
        /// <summary>
        /// This function is load image to class. If object is null then it will create new object else
        /// it will load new image to replace current image
        /// </summary>
        /// <param name="input">Image current frame to  calculate realtime</param>
        public void setImage(Image<Bgr, byte> input)
        {
            this.image = input;
            this.grayObserve = input.Convert<Gray, byte>();
            this.globalImage = input.Convert<Gray, byte>();
            //this.globalImage = DominoGadget.BasicGlobalThreshold(this.globalImage);
            this.connectImage = input.Copy();
            connectImage.ROI = globalRec;
            connectImage = connectImage.Copy();
        }
        public void setGlobalRec(Rectangle rec) { globalRec = rec; }
        public void setDrawRec(Rectangle rec) { drawRec = rec; }
        public void setPlayerTurn(int input)
        {
            int x = Math.Abs(input);
            this.playerTurn = x;
        }
        public void setFace(int leftScore, int rightFace)
        {
            this.leftFace = leftScore;
            this.rightFace = rightFace;
        }
        //Get
        public Image<Bgr, byte> getImage() { return this.image; }
        public Image<Bgr, byte> getConnectedImage() { return this.connectImage; }
        public Image<Gray, byte> getGlobalImage() { return this.globalImage; }
        public Image<Gray, byte> getButtonTempImage() { return buttonTempImage; }
        public Image<Gray, byte> getBWImg() { return this.dominoGroup.getBWImage(); }
        public bool isOverflow() { return this.isOverFlow; }
        public int getPlayerTurn() { return this.playerTurn; }
        public List<Image<Bgr, byte>> getOutputImage() { return this.outputImage; }
        public List<Point> getCurrentScore() { return this.dominoFaceScore; }
        public int setRegion()
        {
            chooseRegionImg.ROI = chooseRegion;
            chooseRegionImg = chooseRegionImg.Copy();
            chooseRegionImg = chooseRegionImg.ThresholdBinaryInv(new Gray(60), new Gray(255));           

            double ManyDom = chooseRegionImg.CountNonzero()[0];
            leftDom = (int)Math.Ceiling(ManyDom / singleDom);
            return leftDom;
        }
        public double getBP()
        {
            return this.nonzeroPixel;
        }
        public Image<Bgr, byte> imgClick
        {
            get
            {
                return this.imageClick;
            }
        }
        public Image<Gray, byte> EdgeObserveImage
        {
            get
            {
                return this.edgeObserve;
            }
        }
        public int CTN
        {
            get { return this.ctn; }
        }
        public int DominoCount
        {
            get { return this.dominoCount; }
            set { this.dominoCount = DominoCount; }
        }
        public Image<Bgr, byte> workingFrame
        {
            get { return this.connectImage; }
        }
        public int LeftDom
        {
            get { return leftDom; }
        }
        public Image<Gray, byte> tbw
        {
            get { return tempBW; }
        }
        public Image<Gray, byte> processIMg
        {
            get { return ttttyyy; }
        }
        internal int isRemoveWrongDom()
        {
            connectImage._SmoothGaussian(3);
            temp1 = connectImage.Copy().Convert<Gray, byte>();
            temp1._ThresholdBinary(new Gray(70), new Gray(255));
            temp1 = DominoGadget.fillhold(new Point(0, 0), 255, temp1.Copy());
            try
            {
                temp1 = temp1.Xor(correct_put_dom.Copy());
            }
            catch { return 0; }
            return temp1.CountNonzero()[0];
        }
        public string[] DomDiretion
        {
            get { return this.domDirection; }
        }
    }
}
