using Emgu.CV;
using Emgu.CV.Structure;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace WannaPlay.ImageClass
{
    class ImageSegmentation
    {
        private Image<Gray, byte> grayImage;
        private Image<Gray, byte> tempBinaryImage;
        private Image<Gray, byte> bwImage;
        private List<Image<Gray, byte>> output;
        private List<Rectangle> outputRec;
        private Rectangle maxRectangle;
        private Image<Gray, byte> maxAreaImage;
        public ImageSegmentation(Image<Gray, byte> input)
        {
            this.grayImage = input.Convert<Gray, byte>();
            this.bwImage = input;
        }
        /***
         * Label region by split groups of image and return values as list of image
         * Input is binary image value between 0 - 1 
         */
        public List<Image<Gray, byte>> bwLabel()
        {
            this.output = new List<Image<Gray, byte>>();
            this.outputRec = new List<Rectangle>();
            Image<Gray, byte> tempBw = this.bwImage.Copy();
            tempBinaryImage = new Image<Gray, byte>(bwImage.Width, bwImage.Height);
            maxAreaImage = new Image<Gray, byte>(bwImage.Width, bwImage.Height);
            maxRectangle = new Rectangle();
            for (int i = 0; i < bwImage.Height; i++)
            {
                for (int J = 0; J < bwImage.Width; J++)
                {
                    //If whatever pixel is white
                    if (bwImage.Data[i, J, 0] != 0)
                    {
                        tempBinaryImage.Data[i, J, 0] = 1;
                        tempBw = connectedComponent(tempBinaryImage, new Point(i, J));
                        if (maxAreaImage.CountNonzero()[0] < tempBw.CountNonzero()[0])
                            this.maxAreaImage = tempBw.Copy();
                        Rectangle r = getRectangle(tempBw.Copy());
                        this.output.Add(tempBw.Copy());
                        if ((maxRectangle.Width * maxRectangle.Height) < (r.Width * r.Height))
                            this.maxRectangle = r;
                        this.outputRec.Add(r);
                        bwImage = bwImage.Sub(tempBw);
                    }
                }
            }
            return output;
        }
        /***
         * Label region by split groups of image and return values as rectangle
         * Input is binary image value between 0 - 1 
         */
        public List<Rectangle> bwLabelRec()
        {
            if (this.outputRec != null)
            {
                return this.outputRec;
            }
            this.outputRec = new List<Rectangle>();
            bwLabel();
            return this.outputRec;
        }
        public Rectangle getMaxAreaRec()    {   return this.maxRectangle;   }
        public Image<Gray, byte> getMaxArea()   {
            return this.maxAreaImage.Copy();
        }
        public List<Rectangle> getBoudingBox() { return this.outputRec; }
        public List<Image<Gray, byte>> getListImage() { return this.output; }
        private Image<Gray, byte> connectedComponent(Image<Gray, byte> tempBinaryImage, Point point)
        {
            int diffCount = 99;
            Image<Gray, byte> previous = tempBinaryImage;
            Image<Gray, byte> current = tempBinaryImage;
            while (diffCount != 0)
            {
                previous = tempBinaryImage.Copy();
                tempBinaryImage._Dilate(1);
                tempBinaryImage._And(this.bwImage);
                current = tempBinaryImage.Sub(previous);
                diffCount = current.CountNonzero()[0];
            }
            return tempBinaryImage;
        }
        private Rectangle getRectangle(Image<Gray, byte> binaryImage)
        {
            int top = -1;
            int left = -1;
            int buttom = binaryImage.Height + 1;
            int right = binaryImage.Width + 1;
            //Get top
            for (int i = 0; i < buttom; i++)
            {
                for (int j = 0; j < right - 1; j++)
                {
                    if (binaryImage.Data[i, j, 0] != 0)
                        top = i;
                }
                if (top != -1) break;
            }

            //Get Buttom
            for (int i = buttom - 2; i > top; i--)
            {
                for (int j = 0; j < right - 1; j++)
                {
                    if (binaryImage.Data[i, j, 0] != 0)
                        buttom = i;
                }
                if (buttom != binaryImage.Height + 1) break;
            }

            //Get Left
            for (int i = 0; i < right - 1; i++)
            {
                for (int j = top; j < buttom - 2; j++)
                {
                    if (binaryImage.Data[j, i, 0] != 0)
                        left = i;
                }
                if (left != -1)
                    break;
            }

            //Get left
            for (int i = right - 2; i > left; i--)
            {
                for (int j = top; j < buttom - 1; j++)
                {
                    if (binaryImage.Data[j, i, 0] != 0)
                        right = i;
                }
                if (right != binaryImage.Width + 1)
                    break;
            }
            Rectangle r = new Rectangle(new Point(left, top), new Size((right - left), (buttom - top)));
            return r;
        }
    }
}
