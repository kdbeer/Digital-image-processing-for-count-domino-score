using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace getDomino
{
    class DomOperation
    {
        Image<Bgr, byte> floorImage;
        Image<Gray, byte> tempImage;
        int theresholdVal = 90;
        const int scoreScale = 5;
        int[,] mappingImage;
        string[] DIRECTIONSTRING = { "RIGHT", "TOP", "BUTTOM", "LEFT" };
        public DomOperation(Image<Bgr, byte> inp, ref Point p)
        {
            this.floorImage = inp;
            tempImage = this.floorImage.Copy().Convert<Gray, byte>().ThresholdBinary(new Gray(theresholdVal), new Gray(255));
        }
        public int[,] dominoMappingImage()
        {
            mappingImage = new int[tempImage.Height / scoreScale + 1, tempImage.Width / scoreScale + 1];
            for (int i = 0; i < tempImage.Height; i += scoreScale)
            {
                if (i >= tempImage.Height)
                    break;

                for (int j = 0; j < tempImage.Width; j += scoreScale)
                {
                    if (j >= tempImage.Width)
                        break;

                    if (mappingImage[i / scoreScale, j / scoreScale] == 1)
                        break;

                    if (tempImage.Height - i < scoreScale || tempImage.Width - j < scoreScale)
                    {
                        mappingImage[i / scoreScale, j / scoreScale] = 0;
                    }
                    int left = 0;
                    int top = 0;

                    //Inner loop for counting black pixel
                    int blackPixelCount = 0;
                    for (int x = j; x < j + scoreScale; x++)
                    {
                        if (x >= tempImage.Width)
                            break;
                        for (int y = i; y < i + scoreScale; y++)
                        {
                            if (y >= tempImage.Height)
                                break;

                            if (tempImage.Data[y, x, 0] == 0)
                            {
                                if (top == 0)
                                    top = i;
                                if (left == 0)
                                    left = x;
                                blackPixelCount++;
                            }
                        }
                    }
                    if (blackPixelCount / Math.Pow(scoreScale, 2) > 0.05)
                        mappingImage[i / scoreScale, j / scoreScale] = 1;
                }
            }
            return mappingImage;
        }
        private static MappingPoint getNextNode(int[,] input, int x, int y, MappingPoint previous)
        {
            if (x < 0 || y < 0)
                return new MappingPoint(x, y);
            if (x >= input.GetLength(1) || y >= input.GetLength(0))
                return previous;
            if (input[y, x] == 0)
                return previous;

            MappingPoint map = new MappingPoint(x, y);
            map.pathCost = previous.pathCost;

            input[y, x] = 0;

            //Recurev self function
            previous.x = x;
            previous.y = y;

            MappingPoint left = getNextNode(input, x, y - 1, map);
            MappingPoint right = getNextNode(input, x, y + 1, map);
            MappingPoint down = getNextNode(input, x + 1, y, map);
            MappingPoint up = getNextNode(input, x - 1, y, map);

            MappingPoint topLeft = getNextNode(input, x - 1, y - 1, map);
            MappingPoint topRight = getNextNode(input, x - 1, y + 1, map);
            MappingPoint downLeft = getNextNode(input, x + 1, y - 1, map);
            MappingPoint downRight = getNextNode(input, x + 1, y + 1, map);

            MappingPoint returnPoint = previous;

            if (right.pathCost > returnPoint.pathCost)
                returnPoint = right;
            if (left.pathCost > returnPoint.pathCost)
                returnPoint = left;
            if (down.pathCost > returnPoint.pathCost)
                returnPoint = down;
            if (up.pathCost > returnPoint.pathCost)
                returnPoint = up;
            if (topLeft.pathCost > returnPoint.pathCost)
                returnPoint = topLeft;
            if (topRight.pathCost > returnPoint.pathCost)
                returnPoint = topRight;
            if (downLeft.pathCost > returnPoint.pathCost)
                returnPoint = downLeft;
            if (downRight.pathCost > returnPoint.pathCost)
                returnPoint = downRight;
            returnPoint.pathCost++;
            return returnPoint;
        }
        public List<MappingPoint> getDominoEdge(int[,] inp)
        {
            int[,] arr = inp;
            int[,] temp = arr.Clone() as int[,];
            MappingPoint[] dominoFace = new MappingPoint[8];
            int N = 0, M = 0;

            int left = 0;
            for (int i = 0; i < arr.GetLength(1); i++)
            {
                for(int j = 0; j < arr.GetLength(0); j++)
                {
                    if (mappingImage[j, i] == 1)
                        left = i;
                }
                if (left != 0)
                    break;
            }
            int right = 0;
            for (int i = arr.GetLength(1) - 1; i > 0; i--)
            {
                for (int j = 0; j < arr.GetLength(0); j++)
                {
                    if (mappingImage[j, i] == 1)
                        right = i;
                }
                if (right != 0)
                    break;
            }

            int top = 0;
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    if (mappingImage[i, j] == 1)
                        top = i;
                }
                if (top != 0)
                    break;
            }

            int btm = 0;
            for (int i = arr.GetLength(0) - 1; i > 0; i--)
            {
                for (int j = 0; j < arr.GetLength(1) - 1; j++)
                {
                    if (mappingImage[i, j] == 1)
                        btm = i;
                }
                if (btm != 0)
                    break;
            }
            List<MappingPoint> domFaceFinal = new List<MappingPoint>();
            if (right - left > 5 || btm - top > 5)
            {
                if (Math.Abs(right - left) > Math.Abs(btm - top))
                {
                    int mid = (left + right) / 2;
                    do
                    {
                        mid++;
                        for (int i = 0; i < arr.GetLength(0); i++)
                        {
                            if (arr[i, mid] == 1)
                            {
                                N = i;
                                M = mid;
                                break;
                            }
                        }
                    } while (mappingImage[N, M + 1] + mappingImage[N, M - 1] != 2 && mappingImage[N - 1, M] + mappingImage[N + 1, M] != 2);
                }
                else
                {
                    int mid = ((top + btm) / 2) - 1;
                    do
                    {
                        mid++;
                        for (int i = 0; i < arr.GetLength(1); i++)
                        {
                            if (arr[mid, i] == 1)
                            {
                                N = mid;
                                M = i;
                                break;
                            }
                        }
                    } while (mappingImage[N, M + 1] + mappingImage[N, M - 1] != 2 && mappingImage[N - 1, M] + mappingImage[N + 1, M] != 2);
                }
                arr[N, M] = 0;
                dominoFace[0] = getNextNode(arr, M + 1, N, new MappingPoint(N, M));             //
                dominoFace[1] = getNextNode(arr, M, N - 1, new MappingPoint(N, M));
                dominoFace[2] = getNextNode(arr, M, N + 1, new MappingPoint(N, M));
                dominoFace[3] = getNextNode(arr, M - 1, N, new MappingPoint(N, M));
                dominoFace[4] = getNextNode(arr, M + 1, N + 1, new MappingPoint(N, M));
                dominoFace[5] = getNextNode(arr, M + 1, N - 1, new MappingPoint(N, M));
                dominoFace[6] = getNextNode(arr, M - 1, N + 1, new MappingPoint(N, M));
                dominoFace[7] = getNextNode(arr, M - 1, N - 1, new MappingPoint(N, M));

                int direction = 0;
                foreach (MappingPoint P in dominoFace)
                {
                    if (P.x != N || P.y != M)
                    {
                        if (P.x >= 1 && P.y >= 1)
                        {
                            if (P.x + 1 < temp.GetLength(1))
                                if (temp[P.y, P.x + 1] == 1)
                                    P.headFace = "Right";
                            if (P.x - 1 >= 0)
                                if (temp[P.y, P.x - 1] == 1)
                                    P.headFace = "Left";
                            if (P.y + 1 < temp.GetLength(0))
                                if (temp[P.y + 1, P.x] == 1)
                                    P.headFace = "Buttom";
                            if (P.y - 2 >= 0)
                                if (temp[P.y - 1, P.x] == 1)
                                    P.headFace = "Top";
                            P.x = P.x * scoreScale;
                            P.y = P.y * scoreScale;

                            P.direction = DIRECTIONSTRING[direction];
                            domFaceFinal.Add(P);
                        }
                    }
                    direction++;
                }
            }
            else if (right - left > 0)
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    int sum = 0;
                    for (int j = 0; j < arr.GetLength(1); j++)
                    {
                        if (mappingImage[i, j] != 0)
                            sum++;
                    }
                    if (sum != 0)
                    {
                        MappingPoint P = new MappingPoint(left, i);
                        P.x = (P.x) * scoreScale;
                        P.y = (P.y) * scoreScale;
                        domFaceFinal.Add(P);
                        break;
                    }
                }
            }
            else if (right - left == 0)
            {
                for (int i = 0; i < arr.GetLength(0); i++)
                {
                    int sum = 0;
                    for (int j = 0; j < arr.GetLength(1); j++)
                    {
                        if (mappingImage[i, j] != 0)
                            sum++;
                    }
                    if (sum != 0)
                    {
                        MappingPoint P = new MappingPoint(left, i);
                        P.x = (P.x) * scoreScale;
                        P.y = (P.y) * scoreScale;
                        domFaceFinal.Add(P);
                        break;
                    }
                }
            }
            return domFaceFinal;
        }
    }
}