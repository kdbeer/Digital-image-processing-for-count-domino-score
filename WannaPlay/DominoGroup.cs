using Emgu.CV;
using Emgu.CV.Structure;
using getDomino;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using WannaPlay.ImageClass;

namespace WannaPlay
{
    class DominoGroup
    {
        private Image<Bgr, byte> image, temp;
        private Image<Gray, byte> grayImage, maskImage, fillHoleImage;
        private Rectangle localRec;
        private int[,] mapImage;
        private Point startDom = new Point(0, 0);
        private DomOperation domino;
        private List<MappingPoint> domEdge;
        private List<Image<Bgr, byte>> outputImage = new List<Image<Bgr, byte>>();
        private ImageSegmentation imgSeg;
        private Image<Gray, byte> bwImage;
        private Rectangle maxRec;
        private Image<Gray, byte> tempBwImage, yyyyyyyy;
        private Rectangle imageRec = new Rectangle();
        private Image<Gray, byte> leftSide, rightSide;
        public bool singleDom = true;
        MappingPoint currentLeft, currentRight;

        KNN knn;
        Rectangle[] countingRec;
        databaseConnection dc;
        Image<Gray, byte> selectedGroupImage;
        Image<Gray, byte> tempObserveImg, grayImg;

        private bool isSingleDomino = false;
        private Image<Bgr, byte> beforeProcess;
        private string[] domDirection = new string[2];

        private int saveNumber = 0;

        Point distance, newPoint;
        private Image<Bgr, byte> tempColor;
        private Image<Gray, byte> reAngleTemp;
        private Rectangle[] flipObserve = new Rectangle[6];
        private Image<Gray, byte> flipObserveImage;
        private Image<Gray, byte> innerFlipImage;
        private int[] flipC;

        public DominoGroup(Image<Bgr, byte> input)
        {
            image = input;
            dc = new databaseConnection();
            beforeProcess = new Image<Bgr, byte>(10, 10);
            leftSide = new Image<Gray, byte>(10, 10);
            rightSide = new Image<Gray, byte>(10, 10);

            //Observe Rectangle init value
            flipObserve[0] = new Rectangle(new Point(0, 30), new Size(30, 30));
            flipObserve[1] = new Rectangle(new Point(0, 60), new Size(30, 30));

            flipObserve[2] = new Rectangle(new Point(30, 30), new Size(30, 30));
            flipObserve[3] = new Rectangle(new Point(30, 60), new Size(30, 30));


            flipObserve[4] = new Rectangle(new Point(60, 30), new Size(30, 30));            
            flipObserve[5] = new Rectangle(new Point(60, 60), new Size(30, 30));
        }
        public void update()
        {
            grayImage = image.Copy().Convert<Gray, byte>().ThresholdBinary(new Gray(70), new Gray(255));
            maskImage = new Image<Gray, byte>(grayImage.Width, grayImage.Height);
            fillHoleImage = DominoGadget.fillhold(new Point(0, 0), 0, grayImage);

            domino = new DomOperation(fillHoleImage.Convert<Bgr, byte>(), ref startDom);
            mapImage = domino.dominoMappingImage();
            mapImage = DominoGadget.thinning(mapImage, 8);
            mapImage = DominoGadget.pruning(mapImage, 5);
            domEdge = domino.getDominoEdge(mapImage);
            dominoDetection();
        }
        private void dominoDetection()
        {
            //Do this method first for optimise image
            outputImage.Clear();
            int domIndex = 0;
            foreach (MappingPoint p in domEdge)
            {
                localRec = new Rectangle(new Point(p.x - 30 + startDom.X, p.y - 50 + startDom.Y), new Size(90, 90));
                temp = this.image.Copy();
                tempColor = temp.Copy();
                temp.ROI = localRec;
                temp = temp.Copy();
                temp = DominoGadget.reAngleDom(temp, temp.Copy().Convert<Gray, byte>());
                distance = this.getNewRectangle(temp);
                newPoint = new Point(localRec.Location.X + distance.X, localRec.Location.Y + distance.Y);
                //localRec = new Rectangle()
                localRec = new Rectangle(newPoint, new Size(90, 90));
                temp = tempColor.Copy();
                temp.ROI = localRec;
                temp = temp.Copy();

                yyyyyyyy = temp.Copy().Convert<Gray, byte>();

                if (!isSingleDom())
                {
                    if (p.headFace == "Buttom")
                        temp = temp.Rotate(-90, new Bgr(255, 255, 255));
                    else if (p.headFace == "Top")
                        temp = temp.Rotate(90, new Bgr(255, 255, 255));
                    else if (p.headFace == "Left")
                        temp = temp.Rotate(-180, new Bgr(255, 255, 255));
                    beforeProcess = temp.Copy();

                    //Compare the point
                    double leftDistance = p.getDistance(currentLeft);
                    double rightDistance = p.getDistance(currentRight);
                    if (leftDistance < 100 && rightDistance < 100)
                    {
                        if (p.direction == "LEFT" || p.direction == "TOP")
                            currentLeft = p;
                        if (p.direction == "RIGHT" || p.direction == "BUTTOM")
                            currentRight = p;
                    } else if(leftDistance < rightDistance )
                    {
                        p.direction = "LEFT";
                        currentLeft = p;
                    } else if(rightDistance < leftDistance)
                    {
                        p.direction = "RIGHT";
                        currentRight = p;
                    }

                    //Detect domino by K-nearest neighborhood
                    if (knn == null)
                        knn = new KNN(temp.Copy().Convert<Gray, byte>());
                    else
                        knn.Update(temp.Copy().Convert<Gray, byte>());

                }
                else
                {
                    int sum = 0;
                    bool topBounding = false;
                    Image<Gray, byte> t = temp.Copy().Convert<Gray, byte>().ThresholdBinaryInv(new Gray(100), new Gray(255));
                    for (int i = 0; i < t.Height; i++)
                    {
                        sum = 0;
                        for (int j = 0; j < t.Width; j++)
                        {
                            if (t.Data[i, j, 0] != 0)
                                sum++;
                        }

                        if (sum > 20 && topBounding == false)
                        {
                            topBounding = true;
                            i += 5;
                        }
                        else
                        {
                            if (sum < 35 && sum > 20)
                            {
                                temp = temp.Rotate(90, new Bgr(255, 255, 255));
                                break;
                            }
                            else if (sum > 35)
                            {
                                break;
                            }
                        }
                    }
                    currentRight = p;
                    currentLeft = p;
                }

                selectedGroupImage = temp.Convert<Gray, byte>().ThresholdBinaryInv(new Gray(100), new Gray(255));
                selectedGroupImage._Dilate(3);
                selectedGroupImage._Erode(3);
                selectedGroupImage = selectedGroupImage.Convert<Gray, byte>().ThresholdBinary(new Gray(100), new Gray(1));
                //Bw label
                imgSeg = new ImageSegmentation(selectedGroupImage.Copy());
                imgSeg.bwLabel();
                maxRec = imgSeg.getMaxAreaRec();
                selectedGroupImage = imgSeg.getMaxArea();
                tempBwImage = selectedGroupImage.Copy();
                selectedGroupImage.ROI = maxRec;
                selectedGroupImage *= 255;

                double r1 = Math.Min(selectedGroupImage.Width, selectedGroupImage.Height);
                double r2 = Math.Max(selectedGroupImage.Width, selectedGroupImage.Height);
                double ratio = r2 / r1;
                if (ratio < 1.5)
                {
                    countingRec = new Rectangle[9];
                    int maskSize = 3;
                    int countRecWidth = selectedGroupImage.Width / maskSize;
                    int countRecHeight = selectedGroupImage.Height / maskSize;
                    int c = 0;
                    for (int i = 0; i < maskSize; i++)
                    {
                        for (int j = 0; j < maskSize; j++)
                        {
                            Rectangle t = new Rectangle(new Point(j * countRecWidth, i * countRecHeight), new Size(countRecWidth, countRecHeight));
                            countingRec[c] = t;
                            c++;
                        }
                    }
                    string inp = getStrCode(selectedGroupImage, (countRecHeight * countRecWidth));
                    int rotateTime = dc.knnProcess(inp);
                    if (rotateTime != 0)
                    {
                        int degree = 90 * rotateTime;
                        temp = temp.Rotate(degree, new Bgr(255, 255, 255));
                        tempBwImage = tempBwImage.Rotate(degree, new Gray(0));
                    }
                }

                string s = "";
                flipObserveImage = temp.Copy().Convert<Gray, byte>().ThresholdBinaryInv(new Gray(60), new Gray(255));
                flipC = new int[6];
                int k = 0;
                foreach (Rectangle r in this.flipObserve)
                {
                    innerFlipImage = flipObserveImage.Copy();
                    innerFlipImage.ROI = r;
                    innerFlipImage = innerFlipImage.Copy();
                    flipC[k++] = innerFlipImage.CountNonzero()[0];
                    flipObserveImage.Draw(r, new Gray(0), 2);
                }
                if (flipC[0] + flipC[1] > 800 || flipC[2] + flipC[3] > 800)
                {
                    if (flipC[1] < flipC[4])
                    {
                        flipObserveImage._Flip(Emgu.CV.CvEnum.FlipType.Horizontal);
                        temp._Flip(Emgu.CV.CvEnum.FlipType.Horizontal);
                    }
                    else if (flipC[1] + flipC[2] > 800 && flipC[1] > flipC[4])
                    {
                        flipObserveImage._Flip(Emgu.CV.CvEnum.FlipType.Horizontal);
                        temp._Flip(Emgu.CV.CvEnum.FlipType.Horizontal);
                    }
                }

                cropForRotate(temp.Copy());
                temp = temp.Copy();
                temp = DominoGadget.reAngleDom(temp, temp.Copy().Convert<Gray, byte>());

                tempBwImage = temp.Copy().Convert<Gray, byte>();
                tempBwImage._ThresholdBinary(new Gray(100), new Gray(255));
                tempBwImage = DominoGadget.fillhold(new Point(0, 0), 255, tempBwImage.Copy());
                tempBwImage._ThresholdBinaryInv(new Gray(100), new Gray(1));

                //Here
                beforeProcess = temp.Copy();
                temp[0] = temp.Split()[0].Mul(tempBwImage);
                temp[1] = temp.Split()[1].Mul(tempBwImage);
                temp[2] = temp.Split()[2].Mul(tempBwImage);

                tempBwImage = temp.Copy().Convert<Gray, byte>().ThresholdBinary(new Gray(1), new Gray(255));
                temp = temp.Copy();

                //Here will rotate image to normal direction
                cropImage();
                outputImage.Add(temp.Copy());

                //temp.Save(@"test/" + saveNumber + ".jpg");
                //saveNumber++;

                domDirection[domIndex++] = p.direction;
            }
        }

        private Point getNewRectangle(Image<Bgr, byte> input)
        {
            int left = 0, top = 0, right = 0, btm = 0, x = 0, y = 0;
            reAngleTemp = input.Copy().Convert<Gray, byte>().ThresholdBinary(new Gray(100), new Gray(255));
            //Check Left
            bool flagClear = false;
            for (int i = 0; i < reAngleTemp.Width; i++)
            {
                for (int j = 0; j < reAngleTemp.Height; j++)
                {
                    if(reAngleTemp.Data[j,i,0]==0)
                    {
                        flagClear = true;
                        left = i;
                    }
                }
                if (flagClear)
                    break;
            }

            flagClear = false;
            for (int i = 0; i < reAngleTemp.Height; i++)
            {
                for (int j = 0; j < reAngleTemp.Width; j++)
                {
                    if (reAngleTemp.Data[i, j, 0] == 0)
                    {
                        flagClear = true;
                        top = i;
                    }
                }
                if (flagClear)
                    break;
            }


            //Right and buttom
            flagClear = false;
            for (int i = reAngleTemp.Height - 1; i > 0; i--)
            {
                for (int j = 0; j < reAngleTemp.Width; j++)
                {
                    if (reAngleTemp.Data[i, j, 0] == 0)
                    {
                        flagClear = true;
                        btm = i;
                    }
                }
                if (flagClear)
                    break;
            }

            flagClear = false;
            for (int i = reAngleTemp.Width - 1; i > 0; i--)
            {
                for (int j = 0; j < reAngleTemp.Height; j++)
                {
                    if (reAngleTemp.Data[j, i, 0] == 0)
                    {
                        flagClear = true;
                        right = i;
                    }
                }
                if (flagClear)
                    break;
            }


            //โค๊ดถึกนิดนึงน่ะครับ ไม่มีเวลาแล้ว
            if (left > 30)
                x = left - 30;
            else if (left < 30 && left > 10)
                x = left - 30;
            else if (right < 60)
                x = right - 60;
            else if (right > 60 && right < 80)
                x = right - 60;


            if (top > 30)
                y = top - 30;
            else if (top < 30 && top > 10)
                y = top - 30;
            else if (btm < 60)
                y = btm - 60;
            else if (btm > 60 && btm < 80)
                y = btm - 60;

            return new Point(x, y);
        }

        public bool isSingleDom()
        {
            double a = fillHoleImage.ThresholdBinaryInv(new Gray(100), new Gray(255)).CountNonzero()[0];
            a = Math.Ceiling(a / 1715);
            if (a < 2)
                return true;
            return false;
        }
        string getStrCode(Image<Gray, byte> input, int ths)
        {
            double thsPercent = 0.6;
            grayImg = input.Copy();
            string code = "";
            int ii = 1;
            foreach (Rectangle item in countingRec)
            {
                tempObserveImg = grayImg.Copy();
                tempObserveImg.ROI = item;
                tempObserveImg = tempObserveImg.Copy();
                if (tempObserveImg.CountNonzero()[0] > ths * thsPercent)
                    code += 1;
                else
                    code += 0;
                ii++;
            }
            return code;
        }
        //Set
        public void setImage(Image<Bgr, byte> input) { this.image = input; }
        //Get
        public List<Image<Bgr, byte>> getOutputImage() { return this.outputImage; }
        public Image<Gray, byte> getBWImage()
        {
            return yyyyyyyy;
        }
        public void cropImage()
        {
            int top = -1;
            int left = -1;

            if (!isSingleDomino)
            {
                grayImage = temp.Copy().Convert<Gray, byte>().ThresholdBinaryInv(new Gray(50), new Gray(255));
                tempBwImage._And(grayImage);
            }
            else
            {
                tempBwImage = image.Copy().Convert<Gray, byte>().ThresholdBinaryInv(new Gray(100), new Gray(255));
            }

            tempBwImage._ThresholdBinaryInv(new Gray(60), new Gray(255));
            tempBwImage = DominoGadget.fillhold(new Point(0, 0), 255, tempBwImage.Copy());
            tempBwImage._ThresholdBinaryInv(new Gray(60), new Gray(255));
            for (int i = 0; i < tempBwImage.Height; i++)
            {
                double sum = 0;
                for (int j = 0; j < tempBwImage.Width; j++)
                {
                    if (tempBwImage.Data[i, j, 0] != 0)
                        sum++;
                }
                if ((sum/tempBwImage.Width) > 0.4)
                {
                    top = i;
                    break;
                }
            }

            for (int i = 3; i < tempBwImage.Width; i++)
            {
                double sum = 0;
                for (int j = 0; j < tempBwImage.Height; j++)
                {
                    if (tempBwImage.Data[j, i, 0] != 0)
                        sum++;
                }
                if ((sum / tempBwImage.Height) > 0.2)
                {
                    left = i;
                    break;
                }
            }
            imageRec = new Rectangle(new Point(left - 2, top - 2), new Size(56, 32));
            this.temp.ROI = imageRec;
            this.temp = this.temp.Copy();

            yyyyyyyy = temp.Copy().Convert<Gray, byte>();
            tempBwImage = temp.Convert<Gray, byte>().Copy();
        }

        public void cropForRotate(Image<Bgr, byte> input)
        {
            int top = -1;
            int left = -1;

            tempBwImage = input.Copy().Convert<Gray, byte>();
            tempBwImage._ThresholdBinaryInv(new Gray(100), new Gray(255));

            for (int i = 0; i < tempBwImage.Height; i++)
            {
                double sum = 0;
                for (int j = 0; j < tempBwImage.Width; j++)
                {
                    if (tempBwImage.Data[i, j, 0] != 0)
                        sum++;
                }
                if ((sum / tempBwImage.Width) > 0.5)
                {
                    top = i;
                    break;
                }
            }

            for (int i = 5; i < tempBwImage.Width; i++)
            {
                double sum = 0;
                for (int j = 0; j < tempBwImage.Height; j++)
                {
                    if (tempBwImage.Data[j, i, 0] != 0)
                        sum++;
                }
                if ((sum / tempBwImage.Height) > 0.3)
                {
                    left = i;
                    break;
                }
            }
            imageRec = new Rectangle(new Point(left - 10, top - 20), new Size(74, 60));
            Rectangle processRec = new Rectangle(new Point(imageRec.X - 10, imageRec.Y - 2), new Size(imageRec.Width + 20, imageRec.Height - 2));
            this.temp.ROI = imageRec;
            this.temp = this.temp.Copy();
            tempBwImage = temp.Convert<Gray, byte>().Copy();
        }
        public Image<Bgr, byte> BeforeProcess { get { return this.beforeProcess; } }
        public string[] DomDirection
        {
            get { return this.domDirection; }
        }
    }
}