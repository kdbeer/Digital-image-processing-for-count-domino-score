using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace getDomino
{
    class KNN
    {
        private Image<Gray, byte> image;
        private Image<Gray, byte> tempImage;
        private Image<Gray, byte> outputImage, tempImg;
        Image<Gray, byte> img2;
        Image<Gray, byte> prev;
        Image<Gray, byte> diff;
        int[,] mask = { { 0, 1, 0 }, { 1, 1, 1 }, { 0, 1, 0 } };
        public KNN(Image<Gray, byte> input)
        {
            this.image = input.ThresholdBinaryInv(new Gray(60), new Gray(255));
            this.image._Dilate(3);
            this.image._Erode(4);
            this.outputImage = new Image<Gray, byte>(this.image.Width, this.image.Height);
        }
        public void Update(Image<Gray, byte> input)
        {
            this.image = input.ThresholdBinaryInv(new Gray(60), new Gray(255));
            this.image._Dilate(3);
            this.image._Erode(4);
            this.outputImage = new Image<Gray, byte>(this.image.Width, this.image.Height);
        }
        public Image<Gray, byte> filterLeargeGroup()
        {
            bool p = false;
            tempImg = image.Copy();
            img2 = new Image<Gray, byte>(tempImg.Width, tempImg.Height);
            int maxGroupLabel = 0;
            for (int i = 0; i < this.image.Height; i++)
            {
                for (int j = 0; j < this.image.Width; j++)
                {
                    if (this.tempImg.Data[i, j, 0] != 0)
                    {
                        img2.Data[i, j, 0] = 1;
                        Rectangle r = connectedComponent(img2, new Point(i, j), this.mask, ref tempImg);
                        if (r.Width * r.Height > maxGroupLabel)
                        {
                            maxGroupLabel = r.Width * r.Height;
                            outputImage = this.image.Copy();
                            outputImage.ROI = r;
                            outputImage = outputImage.Copy();
                        }
                        img2 = new Image<Gray, byte>(tempImg.Width, tempImg.Height);
                    }
                }
            }
            return this.outputImage;
        }
        public Rectangle connectedComponent(Image<Gray, byte> inp, Point p, int[,] mask, ref Image<Gray, byte> img)
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
            inp *= 255;
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
        public int getRoate(String code, databaseConnection dc)
        {
            int count = 0;
            count = dc.getCount(code);
            if(count != 0)
            {
                return dc.getRotateByCode(code);
            }
            return count;
        }

    }
}
