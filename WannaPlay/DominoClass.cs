using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WannaPlay
{
    class DominoClass
    {
        private int edge = 0;
        private int connected = 0;
        public DominoClass(int leftFace, int rightFace)
        {
            this.edge = leftFace;
            this.connected = rightFace;
        }

        public int[] getScore()
        {
            int[] a = new int[2];
            a[0] = edge;
            a[1] = connected;
            return a;
        }

        public override string ToString()
        {
            return "Dom : Edge --> " + edge + " Connected --> " + connected; 
        }

        public static bool isEqual(DominoClass d1, DominoClass d2)
        {
            if (d1.edge == d2.edge && d1.connected == d2.connected)
                return true;
            return false;
        }

        public int getConnected
        {
            get
            {
                return this.connected;
            } set
            {
                this.connected = value;
            }
        }

        public int getEdge
        {
            get
            {
                return this.edge;
            }
            set
            {
                this.edge = value;
            }
        }



        //Set
        public DominoClass inserseDom()
        {
            int a = connected;
            int b = edge;
            return new DominoClass(a, b);
        }
    }
}
