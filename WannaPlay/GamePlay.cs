using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WannaPlay
{
    class GamePlay
    {
        private DominoClass currentLeft = new DominoClass(0, 0);
        private DominoClass currentRight = new DominoClass(0, 0);
        private List<DominoClass> dominoList = new List<DominoClass>();
        private int leftScore;
        private int rightScore;
        private int totalScore = 0;
        private int[] playerScore = new int[2];

        public GamePlay(int leftScore, int rightScore)
        {
            playerScore[0] = leftScore;
            playerScore[1] = rightScore;
        }

        public void newPut(DominoClass newDom, int playerTurn)
        {

            bool isInList = false;
            foreach (DominoClass item in dominoList)
            {
                if (DominoClass.isEqual(item, newDom))
                    isInList = true;
            }

            //If input domino is not in list
            if (!isInList)
            {
                if (dominoList.Count == 0)
                {
                    //If in field have only one domino
                    currentLeft = newDom;
                    currentRight = newDom.inserseDom();
                    leftScore = currentLeft.getScore()[0];
                    rightScore = currentRight.getScore()[1];
                    dominoList.Add(newDom);
                    int p = (playerTurn + 1) % 2;
                    if ((currentLeft.getEdge + currentRight.getEdge) % 5 == 0)
                        playerScore[p] += (currentLeft.getEdge + currentRight.getEdge);
                } else
                {
                    //Check if new conected domino is same values with current left left or right
                    //Then player score up                 
                    if (newDom.getConnected == currentRight.getEdge)
                    {
                        //Check right first
                        totalScore = currentLeft.getEdge + newDom.getEdge;
                        if (newDom.getConnected == newDom.getEdge)
                            totalScore = currentLeft.getEdge + newDom.getEdge;
                        currentRight = newDom;
                        dominoList.Add(currentRight);
                    } else if (newDom.getConnected == currentLeft.getEdge)
                    {
                        //Then check left
                        totalScore = currentRight.getEdge + newDom.getEdge;
                        if (newDom.getConnected == newDom.getEdge)
                            totalScore = currentLeft.getEdge + newDom.getEdge;
                        currentLeft = newDom;
                        dominoList.Add(newDom);
                    }

                    if (totalScore % 5 == 0)
                    {
                        MessageBox.Show("isFive");
                        int p = (playerTurn + 1) % 2;
                        playerScore[p] += totalScore;
                    }
                }
                //dominoList.Add(newDom);
                //currentLeft = newDom;
            } else
            {
                //MessageBox.Show(newDom.toString() +  "\nThis domino is in list!!");
            }
        }

        public int[] getPlayerScore
        {
            get
            {
                return this.playerScore;
            }
        }
        public int currentScore
        {
            get
            {
                return currentRight.getEdge + currentLeft.getEdge;
            }
        }
        public string getLeftDom
        {
            get
            {
                return this.currentLeft.ToString();
            }
        }
        public string getRight
        {
            get
            {
                return this.currentRight.ToString();
            }
        }
    }
}
