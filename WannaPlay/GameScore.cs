using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace gameActionTest
{
    class GameScore
    {
        private Domino currentLeft;
        private Domino currentRight;
        private List<Domino> dominoList;
        private int total;
        private int count;
        public int playerTurn = 0;
        public static int gameCondition = 5;
        public static int gameFinalScore = 25;
        private bool isExit;
        private Domino AddDom;
        public bool isMatch = true;
        private bool isSingleDom = true;

        private int left;
        private int right;
        public bool wrongDom = false;
        public int[] score;
        SoundPlayer getPoint = new SoundPlayer(@"D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\Resources\sound\getPoint.wav");
        
        public GameScore() {
            total = 0;
            count = 0;
            dominoList = new List<Domino>();
            score = new int[2];
            AddDom = new Domino();
        }
        //Insert new domino into board
        public bool newPut(Domino newDommino, string direction)
        {
            isExit = false;
            isMatch = true;
            int x1 = newDommino.head < newDommino.tail ? newDommino.head : newDommino.tail;
            int x2 = newDommino.head < newDommino.tail ? newDommino.tail : newDommino.head;
            AddDom = new Domino(x1, x2);
            if (dominoList.FindAll(x => x.tail == AddDom.tail && x.head == AddDom.head).Count() != 0)
            {
                this.isMatch = false;
                return false;
            }

            if (currentLeft == null)
            {

                currentLeft = new Domino(newDommino.head, newDommino.tail); ;
                currentRight = new Domino(newDommino.tail, newDommino.head);

                this.left = currentLeft.tail;
                this.right = currentRight.tail;
                dominoList.Add(AddDom);
                this.count++;
                this.total = currentLeft.tail + currentRight.tail;
                if (this.total % gameCondition == 0)
                    this.score[playerTurn] += this.total;
                updatePlayerTurn();
                this.isMatch = true;
            }
            else
            {                
                if (direction == "LEFT" || direction == "TOP")
                {
                    this.isMatch = newDommino.head == left ? true : false;
                    if (isMatch)
                    {
                        this.currentLeft = newDommino;                        
                    }
                }

                if (direction == "RIGHT" || direction == "BUTTOM")
                {
                    isMatch = newDommino.head == right ? true : false;
                    if (isMatch)
                    {
                        this.currentRight = newDommino;                        
                    }
                }

                left = currentLeft.tail;
                right = currentRight.tail;


                if (isMatch)
                {
                    dominoList.Add(AddDom);
                    this.total = 0;
                    if((currentLeft.head == currentLeft.tail) && (currentRight.head == currentRight.tail))
                    {
                        this.total = currentRight.head * 2 + currentLeft.head * 2;
                    }
                    else if ((currentLeft.head == currentLeft.tail) || (currentRight.head == currentRight.tail))
                    {
                        if (currentLeft.head == currentLeft.tail)
                        {
                            this.total = (currentLeft.tail * 2) + currentRight.tail;
                        }
                        if (currentRight.head == currentRight.tail)
                        {
                            this.total = currentLeft.tail + (currentRight.tail * 2);
                        }
                    }
                    else
                        this.total += currentLeft.tail + currentRight.tail;

                    if (this.total % gameCondition == 0)
                    {
                        this.score[playerTurn] += this.total;
                        getPoint.Play();
                    }
                    updatePlayerTurn();
                }
                else
                {
                    //If domino input doesn't match
                    //this function will notice player to remove broken domino
                    MessageBox.Show("อุปส์ : โดมิโนวางไม่ถูกต้อง กรุณานำโดมิโนที่ไม่ถูกต้องออกไปด้วยค่ะ!!");
                    this.isMatch = false;
                }
            }
            return isMatch;
        }

        public void updatePlayerTurn()
        {
            playerTurn = (playerTurn + 1) % 2;
        }
        public int turn
        {
            get { return this.playerTurn; }
            set { this.playerTurn = 1; }
        }
        public bool IsMatch
        {
            get { return this.isMatch; }
            set { this.isMatch = IsMatch; }
        }
        internal void isRemoveWrongDom()
        {
            throw new NotImplementedException();
        }
        public int Total
        {
            get { return this.total; }
            set { this.total = Total; }
        }
        public int Count
        {
            get { return this.count; }
            set { this.count = Count; }
        }
        public Domino CurrentHead
        {
            get { return this.currentLeft; }
            set { this.currentLeft = CurrentHead; }
        }
        public Domino CurrentTail
        {
            get { return this.currentRight; }
            set { this.currentLeft = CurrentTail; }
        }
        public int PlayerTurn
        {
            get { return this.playerTurn; }
        }
        public int[] Score
        {
            get { return this.score; }
        }

        internal void changePlayer()
        {
            if (playerTurn == 0)
                playerTurn = 1;
            if (playerTurn == 1)
                playerTurn = 0;
        }

        public int Left {   get { return left;  }}
        public int Right    {   get { return right; }}
    }
}
