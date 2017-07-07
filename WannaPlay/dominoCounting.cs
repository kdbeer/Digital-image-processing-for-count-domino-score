using Emgu.CV;
using Emgu.CV.Structure;
using getDomino;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WannaPlay.ImageClass;

namespace count
{
    class dominnoCounting
    {
        Image<Bgr, byte> domColor;
        Image<Gray, byte> tempImg;
        Image<Gray, byte> bwImg;
        Image<Gray, byte> left, right, rightk;
        Image<Gray, byte> prev;
        Image<Gray, byte> diff;
        Image<Gray, byte> img2;
        Image<Bgr, byte> tempColor;
        Image<Gray, byte> t0, t1, t3, t4;
        int[,] mask = { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } };
        List<Image<Gray, byte>> listImg = new List<Image<Gray, byte>>();
        List<Rectangle> countRec = new List<Rectangle>();
        List<int> domScore = new List<int>();
        public List<Image<Gray, byte>> img3 = new List<Image<Gray, byte>>();
        string processName;
        ImageSegmentation bwLabelLow;
        private Image<Gray, byte> temp;
        private Image<Gray, byte> processImg;
        private List<Image<Gray, byte>> imageList;
        private ImageSegmentation bwLabelLow2;

        public dominnoCounting (Image<Bgr,byte> imgBgr)
        {
            this.update(imgBgr);
        }
        public void update(Image<Bgr, byte> input)
        {
            domScore.Clear();
            countRec.Clear();
            listImg.Clear();
            processName = @"D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\bin\Debug\imageSaveForProcess.jpg";
            this.domColor = new Image<Bgr, byte>(processName);
            //this.domColor = rotateDom(input);
            this.domColor = input.Copy();
            this.domColor._SmoothGaussian(3);
            //this.cutImg();
            this.sepThreshold();
            this.bwLabel();
        }
        //get
        public Image<Bgr, byte> getColorimage()
        {
            return this.domColor;
        }
        public Image<Gray, byte> getbwImg()
        {
            return this.bwImg;
        }
        public List<Image<Gray, byte>> getsepList()
        {
            return listImg;
        }
        private double getSlopAngle(Image<Gray, byte> inpImg)
        {
            int x = inpImg.Width;
            int y = 0;
            double zeta = 0;

            int temp1 = 0, temp2 = 0;
            for (int i = inpImg.Height - 1; i > 0; i--)
            {
                if (inpImg.Data[i, 0, 0] == 255)
                    temp1++;
                else
                    break;
            }

            for (int i = inpImg.Height - 1; i > 0; i--)
            {
                if (inpImg.Data[i, inpImg.Width - 1, 0] == 255)
                    temp2++;
                else
                    break;
            }

            y = Math.Max(temp1, temp2);
            if (temp2 < temp1)
                y *= -1;

            zeta = Math.Atan2(y, x);
            return zeta;
        }
        public List<int> getdominoScore()
        {
            return this.domScore;
        }
        //set
        private Image<Gray, byte> cropedImage(Image<Gray, byte> inp)
        {
            bool leftFound = false;
            int left = -1, Right = inp.Width, top = 0, btm = inp.Height;
            //Get left & right
            for (int i = 0; i < inp.Width; i++)
            {
                int sum = 0;
                for (int j = 0; j < inp.Height; j++)
                {
                    if (inp.Data[j, i, 0] == 0)
                        sum++;
                }
                if (sum == 0)
                {
                    if (!leftFound)
                        left++;
                    else
                    {
                        Right = i;
                        break;
                    }
                }
                else
                {
                    if (!leftFound)
                        leftFound = !leftFound;
                }
            }

            //Find top and buttom
            leftFound = false;
            for (int i = 0; i < inp.Height; i++)
            {
                int sum = 0;
                for (int j = 0; j < inp.Width; j++)
                {
                    if (inp.Data[i, j, 0] == 0)
                        sum++;
                }

                if (sum == 0)
                {
                    if (!leftFound)
                        top++;
                    else
                    {
                        btm = i;
                        break;
                    }
                }
                else
                {
                    if (!leftFound)
                        leftFound = !leftFound;
                }

            }
            Rectangle rec = new Rectangle(new Point(left + 1, top), new Size(Right - left, btm - top));
            Image<Gray, byte> innerImg = inp.Copy();
            innerImg.ROI = rec;
            innerImg = innerImg.Copy();
            return innerImg;
        }     
        private Image<Bgr, byte> cropedImageColor(Image<Bgr, byte> inpImg)
        {
            Image<Gray, byte> inp = inpImg.Copy().Convert<Hsv, byte>().Split()[2];
            inp = inp.ThresholdBinary(new Gray(150), new Gray(255));

            Image<Bgr, byte> colorImg = inpImg.Copy();
            bool leftFound = false;
            int left = -1, Right = inp.Width, top = 0, btm = inp.Height;
            //Get left & right
            for (int i = 0; i < inp.Width; i++)
            {
                int sum = 0;
                for (int j = 0; j < inp.Height; j++)
                {
                    if (inp.Data[j, i, 0] == 0)
                        sum++;
                }
                if (sum != 0)
                {
                    left = i;
                    break;
                }
            }

            for (int i = inp.Width - 1; i > 0; i--)
            {
                int sum = 0;
                for (int j = 0; j < inp.Height; j++)
                {
                    if (inp.Data[j, i, 0] == 0)
                        sum++;
                }
                if (sum != 0)
                {
                    Right = i;
                    break;
                }
            }

            //Find top and buttom
            leftFound = false;
            for (int i = 0; i < inp.Height; i++)
            {
                int sum = 0;
                for (int j = 0; j < inp.Width; j++)
                {
                    if (inp.Data[i, j, 0] == 0)
                        sum++;
                }

                if (sum == 0)
                {
                    if (!leftFound)
                        top++;
                    else
                    {
                        btm = i;
                        break;
                    }
                }
                else
                {
                    if (!leftFound)
                        leftFound = !leftFound;
                }

            }
            Rectangle rec = new Rectangle(new Point(left, top), new Size(Right - left, btm - top));
            Image<Gray, byte> innerImg = inp.Copy();
            colorImg.ROI = rec;
            colorImg = colorImg.Copy();
            return colorImg;
        }
        private void cutImg()
        {
            bwImg = domColor.Copy().Convert<Gray, byte>().ThresholdBinary(new Gray(100), new Gray(255));
            int h = bwImg.Height;
            int w = bwImg.Width;
            int bound = 0;   
            for (int i = 0; i < h; i++)
            {
                double sum = 0;
                for (int j = 0; j < w; j++)
                {
                    if (bwImg.Data[i, j, 0] == 0)
                    {
                        sum++;
                    }
                }

                if (sum / w > 0.8)
                {
                    bound = i;
                    break;

                }
            }
            Rectangle rec = new Rectangle(new Point(0,bound),new Size (w,h));
            bwImg.ROI =  rec;
            domColor.ROI = rec;
            domColor = domColor.Copy();
            bwImg = bwImg.Copy();
        }
        private void sepThreshold()
        {
            //adaptive threshold here!!
            //split domino
            int w = this.domColor.Width;
            int bound = w / 2;
            int h = this.domColor.Height;
            if (w < 20 || h < 20)
                return;
            Rectangle rec1 = new Rectangle(new Point(2, 0), new Size(bound + 1, h - 1));
            Rectangle rec2 = new Rectangle(new Point(bound - 1, 0), new Size(w + 2, h - 1));

            tempImg = this.domColor.Copy().Convert<Hsv, byte>().Split()[2];
            // tempImg = temp;
            left = tempImg.Copy();
            left.ROI = rec1;
            left = left.Copy();
            int[] pk = { 29, 30 };
            int[] pk2 = { 7, 8 };

            double lef = left.GetAverage().Intensity;
            //MessageBox.Show(left.GetAverage().ToString());
            if (lef > 50)
            {
                left._SmoothGaussian(3);
                left = DominoGadget.kmeans(left, 30, pk);
            }
            else
            {
                Image<Gray, byte> leftk = DominoGadget.kmeans(left, 8, pk2);
                left = selectLow(left);
                left._Dilate(1);
                left._And(leftk);
            }

            right = tempImg.Copy();
            right.ROI = rec2;
            right = right.Copy();
            rightk = right.Copy();

            double rig = right.GetAverage().Intensity;
            if (rig > 50)
            {
                right._SmoothGaussian(3);
                right = DominoGadget.kmeans(right, 30, pk);
            }
            else
            {
                rightk = DominoGadget.kmeans(right, 8, pk2);
                right = selectLow(right);
                right._Dilate(1);
                right._And(rightk);
            }
            listImg.Add(left);
            listImg.Add(right);
        }
        private double calculateBrightness(Image<Gray, byte> inp)
        {
            int sum = 0;
            for (int i = 0; i < inp.Height; i++)
            {
                for (int j = 0; j < inp.Width; j++)
                {
                    sum += inp.Data[i, j, 0];
                }
            }
            return sum /= (inp.Width * inp.Height);
        }
        private double calculateContrast(Image<Gray, byte> inp)
        {
            double b = calculateBrightness(inp);
            double sum = 0;
            for (int i = 0; i < inp.Height; i++)
            {
                for (int j = 0; j < inp.Width; j++)
                {
                    sum += Math.Pow((inp.Data[i, j, 0] - b), 2);
                }
            }
            sum = sum /= (inp.Width * inp.Height);
            sum = Math.Sqrt(sum);
            return sum;
        }
        //count
        public Rectangle connectedComponent(Image<Gray, byte> inp, Point p, int[,] mask,ref Image<Gray,byte> img)
        {
            int diffCount = 99;
            while (diffCount != 0)
            {
                prev = inp.Copy();
                inp = inp.Dilate(1);
                inp = inp.And(img);
                diff = inp.Sub(prev);
                diffCount = diff.CountNonzero()[0];
            }
            Rectangle r = labelRegion(prev);
            img = img.Sub(inp);
            
            return r;
        }
        public Rectangle labelRegion(Image<Gray, byte> inp)
        {
            int top = -1, left = -1, right = inp.Width + 1, btm = inp.Height + 1;
            for (int i = 0; i < inp.Height; i++)
            {
                for (int j = 0; j < inp.Width; j++)
                {
                    if (inp.Data[i, j, 0] != 0)
                    {
                        top = i;
                        break;
                    }
                }
                if (top != -1) break;
            }

            for (int i = inp.Height - 1; i > 0; i--)
            {
                for (int j = 0; j < inp.Width; j++)
                {
                    if (inp.Data[i, j, 0] != 0)
                    {
                        btm = i;
                        break;
                    }
                }
                if (btm != inp.Height + 1) break;
            }

            for (int i = 0; i < inp.Width; i++)
            {
                for (int j = 0; j < inp.Height; j++)
                {
                    if (inp.Data[j, i, 0] != 0)
                    {
                        left = i;
                        break;
                    }
                }
                if (left != -1)
                    break;
            }

            for (int i = inp.Width - 1; i > 0; i--)
            {
                for (int j = 0; j < inp.Height; j++)
                {
                    if (inp.Data[j, i, 0] != 0)
                    {
                        right = i;
                        break;
                    }
                }
                if (right != inp.Width + 1)
                    break;
            }

            return new Rectangle(new Point(left, top), new Size(right - left + 1, btm - top + 1));
        }
        public void bwLabel()
        {
            foreach (Image<Gray, byte> item in this.listImg)
            {
                Image<Gray, byte> img4 = item.Copy();
                countRec = new List<Rectangle>();
                tempImg = item.ThresholdBinary(new Gray(100), new Gray(1));
                img2 = new Image<Gray, byte>(tempImg.Width, tempImg.Height);
                for (int i = 0; i < tempImg.Height; i++)
                {
                    for (int j = 0; j < tempImg.Width; j++)
                    {
                        if (tempImg.Data[i, j, 0] != 0)
                        {
                            img2.Data[i, j, 0] = 1;

                            Rectangle r = connectedComponent(img2, new Point(i, j), this.mask, ref tempImg);
                            if (r.Y > 2 && r.X != 0 && (r.X + r.Width) < tempImg.Width - 1 && (r.Y + r.Height) < tempImg.Height && r.Width < 10)
                            {
                                countRec.Add(r);
                            }
                            img2 *= 0;
                        }
                    }
                }
                domScore.Add(countRec.Count);
            }
        }
        //Counting score advance algorithm
        /// <summary>
        /// Try to use SmoothGaussian with template size 5,7 then use And operation to denoise
        /// </summary>
        /// <param name="Input">Gray scale image</param>
        /// <returns>image with denoise</returns>
        Image<Gray, byte> selectLow(Image<Gray, byte> Input)
        {
            processImg = Input;
            processImg += 20;                       //Add more brightness
            t0 = processImg.Copy();                 //Init copy

            processImg = processImg.SmoothGaussian(5);
            processImg = processImg.ThresholdAdaptive(new Gray(255), Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary, 3, new Gray(0));
            t1 = processImg.Copy(); //Adaptive with SmoothGaussian template size 5

            processImg = t0.Copy();
            processImg = processImg.SmoothGaussian(7);
            processImg = processImg.ThresholdAdaptive(new Gray(255), Emgu.CV.CvEnum.AdaptiveThresholdType.MeanC, Emgu.CV.CvEnum.ThresholdType.Binary, 3, new Gray(0));
            processImg._And(t1);
            processImg.Dilate(1);
            processImg.Erode(1);
            processImg._ThresholdBinary(new Gray(100), new Gray(1));

            bwLabelLow = new ImageSegmentation(processImg);
            bwLabelLow.bwLabel();
            imageList = bwLabelLow.getListImage();
            temp = new Image<Gray, byte>(processImg.Width, processImg.Height);
            foreach (Image<Gray, byte> item in imageList)
            {
                if (item.CountNonzero()[0] < 50 && item.CountNonzero()[0] >= 10)
                {
                    bwLabelLow2 = new ImageSegmentation(item);
                    bwLabelLow2.bwLabel();
                    Rectangle rr = bwLabelLow2.getMaxAreaRec();
                    if (rr.Width <= 10 && rr.Height < 10)
                        temp._Or(item);
                }
            }
            processImg = bwLabelLow.getMaxArea();
            temp *= 255;
            temp._Erode(1);
            return temp;
        }        
    }
}
