using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WannaPlay.ImageClass;

namespace getDomino
{
    class DominoGadget
    {
        private static Image<Gray, byte> tempBwImage;

        public static Image<Bgr, byte> separateDominoFromGroup(Image<Bgr, byte> inp)
        {
            Image<Gray, byte> tempGrayImage = inp.Copy().Convert<Gray, byte>().ThresholdBinary(new Gray(90), new Gray(255));
            int top = 0;
            int left = 0;
            for (int i = 0; i < tempGrayImage.Height; i++)
            {
                int sum = 0;
                left = 0;
                for (int j = 0; j < tempGrayImage.Width; j++)
                {
                    if (tempGrayImage.Data[i, j, 0] == 0)
                    {
                        if (left == 0)
                            left = j;
                        sum++;
                    }
                }

                if (((double)sum / tempGrayImage.Width) > 0.1)
                {
                    top = i;
                    break;
                }
            }
            Rectangle rec = new Rectangle(new Point(left - 10, top - 5), new Size(42, 60));
            inp.ROI = rec;
            inp = inp.Copy();
            return inp;
        }
        public static Image<Gray, byte> fillhold(Point p, int newVal, Image<Gray, byte> img)
        {
            Image<Gray, byte> maskFill = new Image<Gray, byte>(img.Width, img.Height);
            Stack<Point> stackPoint = new Stack<Point>();
            bool openUP = false, openDown = false;
            stackPoint.Push(p);
            while (stackPoint.Count != 0)
            {
                Point temp = stackPoint.Pop();
                openUP = false;
                openDown = false;

                //Move to the very left point
                while (temp.X - 1 >= 0 && img.Data[temp.Y, temp.X - 1, 0] == 255)
                {
                    temp.X--;
                }

                while (temp.X + 1 < img.Width && img.Data[temp.Y, temp.X + 1, 0] == 255)
                {
                    maskFill.Data[temp.Y, temp.X, 0] = 255;
                    temp.X++;
                    maskFill.Data[temp.Y, temp.X, 0] = 255;
                    img.Data[temp.Y, temp.X, 0] = 150;

                    if (!openUP)
                    {
                        if (temp.Y + 1 < img.Height && img.Data[temp.Y + 1, temp.X, 0] == 255)
                        {
                            stackPoint.Push(new Point(temp.X, temp.Y + 1));
                            openUP = !openUP;
                        }
                    }
                    else if (img.Data[temp.Y + 1, temp.X, 0] != 255)
                    {
                        openUP = false;
                    }

                    if (!openDown)
                    {
                        if (temp.Y - 1 >= 0 && img.Data[temp.Y - 1, temp.X, 0] == 255)
                        {
                            stackPoint.Push(new Point(temp.X, temp.Y - 1));
                            openDown = true;
                        }
                    }
                    else if (img.Data[temp.Y - 1, temp.X, 0] != 255)
                    {
                        openDown = false;
                    }
                }
            }
            return maskFill.Copy();
        }
        public static Image<Gray, byte> cutBorder(Image<Gray, byte> img)
        {
            Image<Gray, byte> tempImage = img.Copy();
            int top = 0, left = 0, right = tempImage.Width, buttom = tempImage.Height, sum = 0;
            for (int i = 5; i < right; i++)
            {
                sum = 0;
                for (int j = 0; j < buttom; j++)
                    if (tempImage.Data[j, i, 0] == 0)
                        sum++;

                if (sum > 10)
                {
                    left = i;
                    break;
                }
            }

            for (int i = right - 1; i > 0; i--)
            {
                sum = 0;
                for (int j = 0; j < buttom; j++)
                    if (tempImage.Data[j, i, 0] == 0)
                        sum++;
                if (sum > 10)
                {
                    right = i;
                    break;
                }
            }

            //Top and buttom
            for (int i = 0; i < tempImage.Height; i++)
            {
                sum = 0;
                for (int j = left; j < right; j++)
                    if (tempImage.Data[i, j, 0] == 0)
                        sum++;
                if (sum > 5)
                {
                    top = i;
                    break;
                }
            }

            for (int i = tempImage.Height - 1; i > 0; i--)
            {
                sum = 0;
                for (int j = left; j < right; j++)
                    if (tempImage.Data[i, j, 0] == 0)
                        sum++;
                if (sum > 10)
                {
                    buttom = i;
                    break;
                }
            }

            Rectangle rec = new Rectangle(new Point(left, top), new Size(right - left, buttom - top));
            tempImage.ROI = rec;
            tempImage = tempImage.Copy();
            return tempImage;
        }
        public static void cutoutGross(Image<Gray, byte> img, ref Image<Bgr, byte> colorImg)
        {
            int left = 0;
            for (int i = 2; i < img.Width; i++)
            {
                int sum = 0;
                for (int j = 0; j < img.Height; j++)
                {
                    if (img.Data[j, i, 0] == 0)
                        sum++;
                }
                if (sum != 0)
                {
                    left = i;
                    break;
                }
            }
            Rectangle rec = new Rectangle(left, 0, img.Width, img.Height);
            colorImg.ROI = rec; colorImg = colorImg.Copy();
        }
        public static int[,] thinning(int[,] A, int l)
        {

            int height = A.GetLength(0);
            int width = A.GetLength(1);
            int loop = 0;
            bool isDeleted;
            do
            {
                isDeleted = false;
                //A ero B1
                int[,] temp = new int[height, width];
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i + 1, j - 1] + temp[i + 1, j + 1] + temp[i + 1, j] - temp[i - 1, j - 1] - temp[i - 1, j] - temp[i - 1, j + 1];
                        if (sum == 4)
                        {
                            A[i, j] = 0;
                            isDeleted = true;
                        }
                    }
                }

                //(A ero B1) ero B2
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i, j - 1] + temp[i + 1, j - 1] + temp[i + 1, j] - temp[i - 1, j] - temp[i - 1, j + 1] - temp[i, j + 1];
                        if (sum == 4)
                        {
                            A[i, j] = 0;
                            isDeleted = true;
                        }
                    }
                }

                //((A ero B1) ero B2) ero B3
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i, j - 1] + temp[i - 1, j - 1] + temp[i + 1, j - 1] - temp[i - 1, j + 1] - temp[i, j + 1] - temp[i + 1, j + 1];
                        if (sum == 4)
                        {
                            A[i, j] = 0;
                            isDeleted = true;
                        }
                    }
                }

                //(((A ero B1) ero B2) ero B3) ero B4
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i - 1, j - 1] + temp[i - 1, j] + temp[i, j - 1] - temp[i + 1, j] - temp[i + 1, j + 1] - temp[i, j + 1];
                        if (sum == 4)
                        {
                            A[i, j] = 0;
                            isDeleted = true;
                        }
                    }
                }

                //((((A ero B1) ero B2) ero B3) ero B4) ero B5
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i - 1, j - 1] + temp[i - 1, j] + temp[i - 1, j + 1] - temp[i + 1, j - 1] - temp[i + 1, j] - temp[i + 1, j + 1];
                        if (sum == 4)
                        {
                            A[i, j] = 0;
                            isDeleted = true;
                        }
                    }
                }

                //(((((A ero B1) ero B2) ero B3) ero B4) ero B5) ero B6
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i - 1, j] + temp[i - 1, j + 1] + temp[i, j + 1] - temp[i, j - 1] - temp[i + 1, j - 1] - temp[i + 1, j];
                        if (sum == 4)
                        {
                            A[i, j] = 0;
                            isDeleted = true;
                        }
                    }
                }

                //((((((A ero B1) ero B2) ero B3) ero B4) ero B5) ero B6) ero B7
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i - 1, j - 1] + temp[i, j + 1] + temp[i + 1, j + 1] - temp[i - 1, j - 1] - temp[i, j - 1] - temp[i + 1, j - 1];
                        if (sum == 4)
                        {
                            A[i, j] = 0;
                            isDeleted = true;
                        }
                    }
                }

                //(((((((A ero B1) ero B2) ero B3) ero B4) ero B5) ero B6) ero B7) ero B8
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i, j + 1] + temp[i + 1, j] + temp[i + 1, j + 1] - temp[i - 1, j - 1] - temp[i, j - 1] - temp[i - 1, j];
                        if (sum == 4)
                        {
                            A[i, j] = 0;
                            isDeleted = true;
                        }
                    }
                }
                loop++;
                if (l == 1)
                    isDeleted = false;
            } while (isDeleted);
            return A;
        }
        public static int[,] pruning(int[,] A,int iterative)
        {
            int height = A.GetLength(0);
            int width = A.GetLength(1);
            int iterativeCount = 0;
            int[,] temp = new int[height, width];
            do
            {
                //A ero B1
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i, j - 1] - temp[i - 1, j] - temp[i - 1, j + 1] - temp[i, j + 1] - temp[i + 1, j + 1] - temp[i + 1, j];
                        if (sum == 2)
                            A[i, j] = 0;
                    }
                }                
                //(A ero B1) ero B2
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i - 1, j] - temp[i, j + 1] - temp[i + 1, j + 1] - temp[i + 1, j] - temp[i + 1, j - 1] - temp[i, j - 1];
                        if (sum == 2)
                            A[i, j] = 0;
                    }
                }                
                //((A ero B1) ero B2) ero B3
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i, j + 1] - temp[i + 1, j] - temp[i + 1, j - 1] - temp[i, j - 1] - temp[i - 1, j - 1] - temp[i - 1, j];
                        if (sum == 2)
                            A[i, j] = 0;
                    }
                }                
                //(((A ero B1) ero B2) ero B3) ero B4
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i + 1, j] - temp[i, j + 1] - temp[i - 1, j + 1] - temp[i - 1, j] - temp[i - 1, j - 1] - temp[i, j - 1];
                        if (sum == 2)
                            A[i, j] = 0;
                    }
                }
                
                //((((A ero B1) ero B2) ero B3) ero B4) ero B5
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i - 1, j - 1] - temp[i - 1, j] - temp[i - 1, j + 1] - temp[i, j + 1] - temp[i + 1, j + 1] - temp[i, j + 1] - temp[i + 1, j - 1] - temp[i, j - 1];
                        if (sum == 2)
                            A[i, j] = 0;
                    }
                }
                
                //(((((A ero B1) ero B2) ero B3) ero B4) ero B5) ero B6
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i - 1, j + 1] - temp[i - 1, j] - temp[i - 1, j - 1] - temp[i, j - 1] - temp[i + 1, j - 1] - temp[i + 1, j] - temp[i + 1, j + 1] - temp[i, j + 1];
                        if (sum == 2)
                            A[i, j] = 0;
                    }
                }
                
                //((((((A ero B1) ero B2) ero B3) ero B4) ero B5) ero B6) ero B7
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i + 1, j + 1] - temp[i + 1, j] - temp[i + 1, j - 1] - temp[i, j - 1] - temp[i - 1, j - 1] - temp[i - 1, j] - temp[i - 1, j + 1] - temp[i, j + 1];
                        if (sum == 2)
                            A[i, j] = 0;
                    }
                }
                
                //(((((((A ero B1) ero B2) ero B3) ero B4) ero B5) ero B6) ero B7) ero B8
                Array.Copy(A, temp, A.Length);
                for (int i = 1; i < height - 1; i++)
                {
                    for (int j = 1; j < width - 1; j++)
                    {
                        int sum = temp[i, j] + temp[i + 1, j - 1] - temp[i, j - 1] - temp[i - 1, j - 1] - temp[i - 1, j] - temp[i - 1, j + 1] - temp[i, j + 1] - temp[i + 1, j + 1] - temp[i + 1, j];
                        if (sum == 2)
                            A[i, j] = 0;
                    }
                }
                iterativeCount++;
            } while (iterativeCount < iterative);

            Array.Copy(A, temp, A.Length);
            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    int sum = temp[i, j] + temp[i, j - 1] - temp[i - 1, j] - temp[i - 1, j + 1] - temp[i, j + 1] - temp[i + 1, j + 1] - temp[i + 1, j];
                    if (sum == 2)
                        A[i, j] = 0;
                }
            }

            Array.Copy(A, temp, A.Length);
            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    int sum = temp[i, j] + temp[i + 1, j] - temp[i, j + 1] - temp[i - 1, j + 1] - temp[i - 1, j] - temp[i - 1, j - 1] - temp[i, j - 1];
                    if (sum == 2)
                        A[i, j] = 0;
                }
            }

            return A;
        }
        public static Image<Gray, byte> erode(Image<Gray, byte> input,ref int iterative)
        {
            byte[,] mask = { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } };
            Image<Gray, byte> padArr = padArray(input.ThresholdBinaryInv(new Gray(100), new Gray(1)), 3);
            Image<Gray, byte> output = new Image<Gray, byte>(input.Width, input.Height);
            int c = 1000;
            iterative = 0;
            do
            {
                c = 0;
                output._ThresholdToZero(new Gray(100));
                for (int i = 0; i < padArr.Height - 2; i++)
                {
                    for (int j = 0; j < padArr.Width - 2; j++)
                    {
                        double sum = 0;
                        sum += Math.Pow(mask[0, 0] - (padArr.Data[i, j, 0] * mask[0, 0]), 2);
                        sum += Math.Pow(mask[0, 1] - (padArr.Data[i, j + 1, 0] * mask[0, 1]), 2);
                        sum += Math.Pow(mask[0, 2] - (padArr.Data[i, j + 2, 0] * mask[0, 2]), 2);
                        sum += Math.Pow(mask[1, 0] - (padArr.Data[i + 1, j, 0] * mask[1, 0]), 2);
                        sum += Math.Pow(mask[1, 1] - (padArr.Data[i + 1, j + 1, 0] * mask[1, 1]), 2);
                        sum += Math.Pow(mask[1, 2] - (padArr.Data[i + 1, j + 2, 0] * mask[1, 2]), 2);
                        sum += Math.Pow(mask[2, 0] - (padArr.Data[i + 2, j, 0] * mask[2, 0]), 2);
                        sum += Math.Pow(mask[2, 1] - (padArr.Data[i + 2, j + 1, 0] * mask[2, 1]), 2);
                        sum += Math.Pow(mask[2, 2] - (padArr.Data[i + 2, j + 2, 0] * mask[2, 2]), 2);
                        if (sum == 0)
                        {
                            output.Data[i, j, 0] = 1;
                            c++;
                        }
                    }
                }
                iterative++;
                padArr = padArray(output, 3);
            } while (c > 389);
            return output.ThresholdBinary(new Gray(0), new Gray(255));
        }
        public static Image<Gray, byte> padArray(Image<Gray, byte> input, int maskSize)
        {
            int padsize = (maskSize - 1) / 2;
            Image<Gray, byte> padedImage = new Image<Gray, byte>(new Size(input.Width + (padsize * 2), input.Height + (padsize) * 2));
            padedImage.SetZero();
            for (int i = 0; i < input.Height; i++)
            {
                for (int j = 0; j < input.Width; j++)
                {
                    padedImage.Data[i + padsize, j + padsize, 0] = input.Data[i, j, 0];
                }
            }
            return padedImage;
        }
        public static Rectangle connectedComponent(Image<Gray, byte> input, Point p, int[,] B)
        {
            int r = input.Height;
            int c = input.Width;
            Image<Gray, byte> output = new Image<Gray, byte>(r, c);
            Image<Gray, byte> prev;
            Image<Gray, byte> diff = new Image<Gray, byte>(r, c);
            output.Data[p.Y, p.X, 0] = 1;
            double cumulativeSub = -1;
            while(cumulativeSub != 0)
            {
                prev = output.Copy();
                output = prev.Dilate(1);
                output = output.And(output, input);
                diff = output.Sub(prev, output);
                cumulativeSub = ImgSum(diff);
            }
            return new Rectangle();
        }
        private static double ImgSum(Image<Gray, byte> img)
        {
            double sum = 0;
            for (int i = 0; i < img.Height; i++)
            {
                for (int j = 0; j < img.Width; j++)
                {
                    sum += Math.Pow(img.Data[i, j, 0], 2);
                }
            }
            return sum;
        }
        public static Image<Bgr, byte>[] imageSegmentation(Image<Bgr, byte> colorimg)
        {
            Image<Gray, byte> BW = colorimg.Convert<Gray, byte>().ThresholdBinary(new Gray(100), new Gray(1));
            Rectangle domRec = new Rectangle();
            for (int i = 0; i < BW.Height; i++)
            {
                for (int j = 0; j < BW.Width; j++)
                {
                    if (BW.Data[i, j, 0] == 1)
                        domRec = connectedComponent(BW, new Point(i, j), new int[3, 3]);
                }
            }
            return null;
        }
        /// <summary>
        /// Function that convert gray image to binary image by using Basic Global Threshold technique
        /// </summary>
        /// <param name="input">Input gray image</param>
        /// <returns></returns>
        public static Image<Gray, byte> BasicGlobalThreshold(Image<Gray, byte> input)
        {
            Image<Gray, byte> grayImg = input.Copy();
            int[] arr = new int[grayImg.Width * grayImg.Height];
            int c = 0;
            for (int i = 0; i < grayImg.Height; i++)
            {
                for (int j = 0; j < grayImg.Width; j++)
                {
                    arr[c] = grayImg.Data[i, j, 0];
                    c++;
                }
            }
            Array.Sort(arr);
            double avg = arr.Average();
            double t2 = 0;
            while (true)
            {
                int[] m1 = Array.FindAll(arr, s => s < avg);
                int[] m2 = Array.FindAll(arr, s => s >= avg);
                t2 = (m1.Average() + m2.Average()) / 2;
                if (Math.Abs(avg - t2) < 0.005)
                    break;
                avg = t2;
            }
            grayImg._ThresholdBinary(new Gray((int)t2), new Gray(255));
            return grayImg.Copy();
        }
        /// <summary>
        /// Get slop from gray scale domino from atan(x, y)
        /// </summary>
        /// <param name="inpImg">gray scale image</param>
        /// <returns>double of radian angle</returns>
        private static double getSlopAngle(Image<Gray, byte> inpImg)
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
        /// <summary>
        /// Cut background of image out this is simple method to seperate domino from image
        /// </summary>
        /// <param name="inp">gray scale image</param>
        /// <returns>cutted gray scale image</returns>
        private static Image<Gray, byte> cropedImage(Image<Gray, byte> inp)
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
        /// <summary>
        /// หมุนภาพ เพื่อให้โดมิโนขนานกับพื้นมาดที่สุด
        /// </summary>
        /// <param name="inpImg">ภาพของโดมิโนที่เอียง</param>
        /// <returns>ภาพของโดมิโนที่ผ่านการหมุนและตัดแล้ว</returns>
        public static Image<Bgr, byte> rotateDom(Image<Bgr, byte> inpImg, ref Image<Gray, byte> img)
        {
            Image<Bgr, byte> tempColor = inpImg.Copy();
            Image<Gray, byte> tempImg = inpImg.Copy().Convert<Hsv, byte>().Split()[2];
            tempImg = tempImg.ThresholdBinary(new Gray(170), new Gray(255));
            tempImg = cropedImage(tempImg);

            double ratio = tempImg.Width > tempImg.Height ? tempImg.Width / (double)tempImg.Height : tempImg.Height / (double)tempImg.Width;

            if (ratio > 1.5)
            {
                int factorDel = tempImg.Width / 7;
                Rectangle rec = new Rectangle(new Point(factorDel, 0), new Size(tempImg.Width - (factorDel * 2), tempImg.Height));
                tempImg.ROI = rec;
                tempImg = tempImg.Copy();
                double zeta = getSlopAngle(tempImg);
                zeta = (180.0 / Math.PI) * Math.Atan(zeta);
                tempColor = tempColor.Rotate(zeta, new Bgr(255, 255, 255));
                zeta = zeta - 3;
                img = img.Rotate(zeta, new Gray(0));
            }
            //tempColor = cropedImageColor(tempColor);
            return tempColor;
        }
        /// <summary>
        /// This functin make domino linierize by rotate up 1 degree.
        /// </summary>
        /// <param name="colorImage">Color input image</param>
        /// <param name="segmentImage">Gray image but is same image with param 1</param>
        /// <returns>Image that rotated</returns>
        public static Image<Bgr, byte> reAngleDom(Image<Bgr, byte> colorImage, Image<Gray, byte> segmentImage)
        {
            segmentImage._ThresholdBinaryInv(new Gray(60), new Gray(1));
            //Try to select maximum group
            ImageSegmentation imgSeg = new ImageSegmentation(segmentImage.Copy());
            imgSeg.bwLabel();
            segmentImage = imgSeg.getMaxArea(); //Now we have maximum area this mean we can cut out unwanted part of other domino

            //Convert for dilation image
            segmentImage._ThresholdBinaryInv(new Gray(0), new Gray(255));
            segmentImage *= 255;
            segmentImage._Erode(5);
            segmentImage._Dilate(6);
            segmentImage._ThresholdBinaryInv(new Gray(0), new Gray(255));

            Image<Gray, byte> origin_turn_right = segmentImage.Copy();
            Image<Gray, byte> origin_turn_left = segmentImage.Copy();
            Image<Gray, byte> cannyRight = origin_turn_right.Copy().Canny(80, 120);
            Image<Gray, byte> cannyLeft = origin_turn_left.Copy().Canny(80, 120);

            

            int rotateDegree = 0;
            int rot = 0;
            int degree = 0;
            //Thus loop will turn image to left and right by 1 degree and detect line
            //Ez?
            
            while (rot < 25 && rotateDegree < 90)
            {
                rot = 0;
                cannyLeft = origin_turn_left.Copy().Canny(80, 120);
                cannyRight = origin_turn_right.Copy().Canny(80, 120);

                //Detect Left Side
                for (int i = 0; i < origin_turn_left.Height; i++)
                {
                    int sum = 0;
                    for (int j = 0; j < origin_turn_left.Width; j++)
                    {
                        if (cannyLeft.Data[i, j, 0] != 0)
                            sum++;
                    }
                    if (sum > 30)
                    {
                        degree = -1 * rotateDegree;
                        rot = 90;
                        //MessageBox.Show(deegree.ToString());
                        break;
                    }
                }

                //Detect left side
                if (rot != 90)
                {
                    for (int i = 0; i < origin_turn_left.Width; i++)
                    {
                        int sum = 0;
                        for (int j = 0; j < origin_turn_left.Height; j++)
                        {
                            if (cannyLeft.Data[j, i, 0] != 0)
                                sum++;
                        }
                        if (sum > 30)
                        {
                            degree = -1 * rotateDegree;
                            rot = 90;
                            //MessageBox.Show(deegree.ToString());
                            break;
                        }
                    }
                }

                //if turn left and still not rotate domino
                if (rot != 90)
                {
                    //Detect when turn right domino
                    for (int i = 0; i < origin_turn_right.Height; i++)
                    {
                        int sum = 0;
                        for (int j = 0; j < origin_turn_right.Width; j++)
                        {
                            if (cannyRight.Data[i, j, 0] != 0)
                                sum++;
                        }
                        if (sum > 30)
                        {
                            degree = rotateDegree;
                            rot = 90;
                            //MessageBox.Show(deegree.ToString());
                            break;
                        }
                    }

                    for (int i = 0; i < origin_turn_right.Width; i++)
                    {
                        int sum = 0;
                        for (int j = 0; j < origin_turn_right.Height; j++)
                        {
                            if (cannyRight.Data[j, i, 0] != 0)
                                sum++;
                        }
                        if (sum > 30)
                        {
                            degree = rotateDegree;
                            rot = 90;
                            //MessageBox.Show(deegree.ToString());
                            break;
                        }
                    }
                }
                //Rotate
                origin_turn_left = origin_turn_left.Rotate(-1, new Gray(0));
                origin_turn_right = origin_turn_right.Rotate(1, new Gray(0));
                rotateDegree++;
            }
            colorImage = colorImage.Rotate(degree, new Bgr(255, 255, 255));
            //colorImage.Save(@"test/a" + ".jpg");
            return colorImage;
        }
        public static Image<Bgr, byte> reAngleDom_vertical(Image<Bgr, byte> colorImage, Image<Gray, byte> segmentImage)
        {
            segmentImage._ThresholdBinaryInv(new Gray(60), new Gray(1));
            //Try to select maximum group
            ImageSegmentation imgSeg = new ImageSegmentation(segmentImage.Copy());
            imgSeg.bwLabel();
            segmentImage = imgSeg.getMaxArea(); //Now we have maximum area this mean we can cut out unwanted part of other domino

            //Convert for dilation image
            segmentImage._ThresholdBinaryInv(new Gray(0), new Gray(255));
            segmentImage *= 255;
            segmentImage._Erode(5);
            segmentImage._Dilate(6);
            segmentImage._ThresholdBinaryInv(new Gray(0), new Gray(255));

            Image<Gray, byte> origin_turn_right = segmentImage.Copy();
            Image<Gray, byte> origin_turn_left = segmentImage.Copy();
            Image<Gray, byte> cannyRight = origin_turn_right.Copy().Canny(80, 120);
            Image<Gray, byte> cannyLeft = origin_turn_left.Copy().Canny(80, 120);
            int rotateDegree = 0;
            int rot = 0;
            int deegree = 0;
            //Thus loop will turn image to left and right by 1 degree and detect line
            //Ez?
            while (rot < 25)
            {
                rot = 0;
                cannyLeft = origin_turn_left.Copy().Canny(80, 120);
                cannyRight = origin_turn_right.Copy().Canny(80, 120);

                //Detect Left Side
                for (int i = 0; i < origin_turn_left.Height; i++)
                {
                    int sum = 0;
                    for (int j = 0; j < origin_turn_left.Width; j++)
                    {
                        if (cannyLeft.Data[i, j, 0] != 0)
                            sum++;
                    }
                    if (sum > 30)
                    {
                        deegree = -1 * rotateDegree;
                        rot = 90;
                        //MessageBox.Show(deegree.ToString());
                        break;
                    }
                }

                //if turn left and still not rotate domino
                if (rot != 90)
                {
                    //Detect when turn right domino
                    for (int i = 0; i < origin_turn_right.Height; i++)
                    {
                        int sum = 0;
                        for (int j = 0; j < origin_turn_right.Width; j++)
                        {
                            if (cannyRight.Data[i, j, 0] != 0)
                                sum++;
                        }
                        if (sum > 30)
                        {
                            deegree = rotateDegree;
                            rot = 90;
                            //MessageBox.Show(deegree.ToString());
                            break;
                        }
                    }
                }
                //Rotate
                origin_turn_left = origin_turn_left.Rotate(-1, new Gray(0));
                origin_turn_right = origin_turn_right.Rotate(1, new Gray(0));
                rotateDegree++;
            }
            colorImage = colorImage.Rotate(deegree, new Bgr(255, 255, 255));
            return colorImage;
        }
        /// <summary>
        /// Part of kmean algorithm to find nearest mean of each intensity
        /// </summary>
        /// <param name="intensity">Intensity if each pixel</param>
        /// <param name="means">Array of means</param>
        /// <returns>integer represent the nearest mean in group</returns>
        public static int findNearest(int intensity, double[] means)
        {
            int index = 0;
            double compare = 255;
            double temp = 0;
            for (int i = 0; i < means.Length; i++)
            {
                temp = Math.Sqrt(Math.Pow(intensity - means[i], 2));
                if (temp < compare)
                {
                    compare = temp;
                    index = i;
                }
            }
            return index;
        }
        /// <summary>
        /// Implement k-mean algorithm to cluster group of gray scale image
        /// </summary>
        /// <param name="input">Gray scale image </param>
        /// <param name="k">Whick k that want to cluster</param>
        /// <param name="preciseK">Image that specifies k</param>
        /// <returns>Gray image that or by k groups</returns>
        public static Image<Gray, byte> kmeans(Image<Gray, byte> input, int k, int[] preciseK)
        {

            int[] vectorDim = toVector(input);
            Array.Sort(vectorDim);

            int splitWall = vectorDim.Length / (k - 1);
            string s = "";
            int c = 1;
            int[,] lst = new int[k, splitWall];
            double[] mean = new double[k];
            List<int>[] group = new List<int>[k];
            List<int>[] previusGroups = new List<int>[k];
            //Assign defualt zero value
            for (int i = 0; i < k; i++)
            {
                group[i] = new List<int>();
                previusGroups[i] = new List<int>();
            }

            //Separate list item to small Equal Group
            List<int> temp = new List<int>();
            int temp_k = 0;
            for (int i = 0; i < vectorDim.Length - 1; i++)
            {
                group[temp_k].Add(vectorDim[i]);

                if (i == c * splitWall)
                {
                    //Find mean of each group
                    mean[temp_k] = group[temp_k].Average();
                    temp_k++;
                    c++;
                }
            }

            Array.Sort(mean);

            try
            {
                mean[temp_k] = group[temp_k].Average();
            }
            catch { }        

            //Assign value by previousgroup equal group
            for (int i = 0; i < k; i++)
                previusGroups[i] = new List<int>(group[i].ToList());

            //Loop distribute value to belong group my mean
            bool isEqual = false;
            while (!isEqual)
            {
                //Regroup Part
                for (int i = 0; i < mean.Length - 1; i++)
                {
                    List<int> tempGroup = new List<int>(group[i]);
                    foreach (int x in group[i])
                    {
                        int near = DominoGadget.findNearest(x, mean);    //near = mean that x nearest distance
                        if (near != i)                      //If x isn't belong current group
                        {
                            group[near].Add(x);             //Add x to belong group
                            tempGroup.Remove(x);            //Delete x from old group
                        }
                    }
                    tempGroup.Sort();                       //Sort all item
                    group[i] = new List<int>(tempGroup);    //assign value to group
                }

                //Compare Part
                //This part will check group before regroup is equal to after regroup?
                //If equal then k-means done! else find mean and regroup again
                isEqual = true;
                for (int i = 0; i < k; i++)
                {
                    bool hasElement = Enumerable.SequenceEqual(group[1].OrderBy(t => t), previusGroups[1].OrderBy(t => t));     //check equal by each element
                    if (!hasElement)
                    {
                        isEqual = false;
                        break;
                    }
                }

                //MessageBox.Show(isEqual.ToString());
                //Calculate new mean
                if (!isEqual)
                {
                    for (int i = 0; i < mean.Length; i++)
                    {
                        try
                        {
                            mean[i] = group[i].Average();
                        }
                        catch { }
                        previusGroups[i] = new List<int>(group[i].ToList());
                    }
                }                
            }

            Image<Gray, byte> returnImg = new Image<Gray, byte>(input.Width, input.Height);
            for (int i = 0; i < input.Rows; i++)
            {
                for (int j = 0; j < input.Cols; j++)
                {
                    foreach (int xc in preciseK)
                    {
                        if (group[xc - 1].Find(T => T == input.Data[i, j, 0]) != 0)
                            returnImg.Data[i, j, 0] = 255;
                    }
                }
            }
            return returnImg;
        }
        /// <summary>
        /// Convert image metrix data to 1 row vector
        /// </summary>
        /// <param name="input">Gray image v channal fromm hsv image</param>
        /// <returns>array of vector with 1 row</returns>
        private static int[] toVector(Image<Gray, byte> input)
        {
            int c = 0;
            int[] output = new int[input.Width * input.Height];
            for (int i = 0; i < input.Height; i++)
            {
                for (int j = 0; j < input.Width; j++)
                {
                    output[c] = input.Data[i, j, 0];
                    c++;
                }
            }
            return output;
        }
        public static Image<Bgr, byte> cropImage(Image<Bgr, byte> input)
        {
            int top = -1;
            int left = -1;
            tempBwImage = input.Copy().Convert<Gray, byte>();            
            tempBwImage._ThresholdBinaryInv(new Gray(60), new Gray(1));

            for (int i = 0; i < tempBwImage.Height; i++)
            {
                double sum = 0;
                for (int j = 0; j < tempBwImage.Width; j++)
                {
                    if (tempBwImage.Data[i, j, 0] != 0)
                        sum++;
                }
                if ((sum / tempBwImage.Width > 0.5))
                {
                    top = i;
                    break;
                }
            }

            for (int i = 2; i < tempBwImage.Width; i++)
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
            Rectangle imageRec = new Rectangle(new Point(left, top + 1), new Size(52, 28));           
            input.ROI = imageRec;
            return input.Copy();
        }
    }
}
