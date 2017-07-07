using System;

namespace getDomino
{
    class MappingPoint
    {
        public int x, y;
        public int pathCost = 0;
        public string headFace = "Top";
        public string direction = "LEFT";
        public MappingPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        override
        public string ToString()
        {
            return ("X = " + x + " , " + "Y = " + y + "\n");
        }

        /// <summary>
        /// Calculate distance from 2 point
        /// </summary>
        /// <param name="compare">point to compare with this point</param>
        /// <returns>distance of 2 point</returns>
        public double getDistance(MappingPoint compare)
        {
            return Math.Sqrt(Math.Pow(compare.x - this.x, 2) + Math.Pow(compare.y - this.y, 2));
        }
    }
}
