using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using getDomino;
using Emgu.CV.CvEnum;

namespace WannaPlay
{
    class DominoGame
    {
        Image<Gray, byte> tempGrayImage;

        //this filelds use for drawTheDomino history
        List<Point> historyDom;
        Point currentLocation;
        Image<Gray, byte> historyimage;
        string _Path = @"D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\bin\Debug\img\";
        /*---------------------------------------------------------------------------------*/

        //This fields use to counting score when end of the turn
        Image<Bgr, byte> tempColorImage;
        private bool isOver = false;

        public DominoGame()
        {
            tempGrayImage = null;
            historyDom = new List<Point>();
            currentLocation = new Point(10, 10);
            historyimage = new Image<Gray, byte>(0,0);
            //ต้องรับค่าเข้ามา
            //globalImage = input;
        }

        public void reset()
        {

        }
        public int[] endTurn(Image<Bgr, byte> input)
        {
            int[] returnInt = new int[0];
            Rectangle rec = new Rectangle(new Point(0, 0), new Size(input.Width / 2, input.Height));
            tempColorImage = input.Copy();
            tempColorImage.ROI = rec; tempColorImage = tempColorImage.Copy();
            returnInt[0] = countingHalf(tempColorImage);
            rec = new Rectangle(new Point((input.Width / 2) - 1, 0), new Size(input.Width / 2, input.Height));
            tempColorImage = input.Copy();
            tempColorImage.ROI = rec; tempColorImage = tempColorImage.Copy();
            returnInt[1] = countingHalf(tempColorImage);
            return returnInt;
        }
        public int countingHalf(Image<Bgr, byte> input)
        {
            tempGrayImage = input.Convert<Gray, byte>().ThresholdBinary(new Gray(100), new Gray(255));
            return 0;
        }

        public void drawHistory(Point newPoint, int width, int height)
        {
            if (newPoint.X <= 6 && newPoint.Y <= 6 && newPoint.X >= 0 && newPoint.Y >= 0)
            {
                foreach (Point p in historyDom)
                    if (newPoint == p)
                        return;                 //if domino point is exit then return and not going on
                historyDom.Add(newPoint);
                if(width != historyimage.Width)
                {
                    //60
                    currentLocation = new Point(60, 20);
                    historyimage = new Image<Gray, byte>(width, height).Or(new Gray(205));
                }
                drawCurrentLocation(newPoint);
            }
        }
        public void drawNewHistory(int width, int height)
        {
            historyimage = new Image<Gray, byte>(width * 2, height * 2).Or(new Gray(205));
            if (historyDom.Count == 0)
                return;
            foreach (Point p in historyDom)
                drawCurrentLocation(p);
        }
        public bool  isScreenClear(Image<Gray, byte> input)
        {
            bool clear = true;
            int sumOFBlackPixel = 0;
            tempGrayImage = input.ThresholdBinaryInv(new Gray(60), new Gray(1));
            sumOFBlackPixel = tempGrayImage.CountNonzero()[0];

            if (sumOFBlackPixel > 100)
                return false;
            return clear;
        }
        public void drawCurrentLocation(Point currentDom)
        {
            string tempPath = _Path + "" + currentDom.X + "" + currentDom.Y + ".png";
            if(currentDom.Y < currentDom.X)
                tempPath = _Path + "" + currentDom.Y + "" + currentDom.X + ".png";
            tempGrayImage = new Image<Gray, byte>(tempPath);
            tempGrayImage = tempGrayImage.Resize(0.6, Inter.Linear);

            if (currentLocation.X > historyimage.Width - (tempGrayImage.Width * 7))
                currentLocation = new Point(60, currentLocation.Y + tempGrayImage.Height + 10);
            Rectangle rec = new Rectangle(currentLocation, tempGrayImage.Size);
            for (int j = currentLocation.X; j < currentLocation.X + tempGrayImage.Width; j++)
            {
                for (int i = currentLocation.Y; i < currentLocation.Y + tempGrayImage.Height; i++)
                {
                    historyimage.Data[i, j, 0] = tempGrayImage.Data[i - currentLocation.Y, j - currentLocation.X, 0];
                }
            }
            currentLocation.X = currentLocation.X + 5 + tempGrayImage.Width;
        }
        public Image<Gray, byte> getHistoryimage()
        {
            return historyimage;
        }
        public bool gameOver
        {
            get { return this.isOver; }
        }
    }
}
