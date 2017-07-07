using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Drawing;
using WannaPlay.ImageClass;

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
        public int getRotate(String code, databaseConnection dc)
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
