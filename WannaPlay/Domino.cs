using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gameActionTest
{
    class Domino
    {
        private int left;
        private int right;
        private int sum;

        public Domino()
        {
            
        }
        public Domino(int head, int tail)
        {
            this.left = head;
            this.right = tail;
            this.sum = head + tail;
        }

        public int head
        {
            get { return this.left; }
            set { this.left = head; }
        }

        public int tail {
            get { return this.right; }
            set { this.right = tail; }
        }

        public int Sum
        {
            get { return this.sum; }
        }
        
        override
        public string ToString()
        {
            return "Head : " + head + " Tail : " + tail;
        }

        public void inverse()
        {
            int temp = this.left;
            this.left = this.right;
            this.right = temp;
        }
    }
}
 