using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;
using System.Windows.Forms;

namespace getDomino
{
    class databaseConnection
    {
        private String connectionString;
        DataTable dt;
        public databaseConnection()
        {
            connectionString = @"Data source=D:\งาน\project\NSC\App\16022017 final\WannaPlay\WannaPlay\bin\Debug\save\KNNDB.db";
            dt = new DataTable();
        }
        public int getCount(string str)
        {
            int countInRow = 0;
            string statement = "SELECT COUNT(*) AS C FROM knnDB WHERE code = '" + str + "'";
            DataTable temp = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    SQLiteHelper sh = new SQLiteHelper(cmd);
                    temp = sh.Select(statement);
                    conn.Close();
                }
            }
            foreach (DataRow rows in temp.Rows)
            {
                string t = rows["C"].ToString();
                countInRow = int.Parse(t);
            }
            return countInRow;

        }
        public bool isExitIntable(String code, int rotate)
        {
            string statement = "SELECT count(*) AS C FROM knnDB WHERE code = '" + code + "' AND rotate = '" + rotate + "'";
            DataTable temp = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    SQLiteHelper sh = new SQLiteHelper(cmd);
                    temp = sh.Select(statement);
                    conn.Close();
                }
            }

            foreach (DataRow rows in temp.Rows)
            {
                string t = rows["C"].ToString();
                int count = int.Parse(t);
                if (count != 0)
                    return true;
            }
            return false;
        }
        public bool saveRotateCode(string code, int rotate)
        {
            bool isComplate = false;
            var dic = new Dictionary<string, object>();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    SQLiteHelper sh = new SQLiteHelper(cmd);
                    dic["code"] = code;
                    dic["rotate"] = rotate;
                    sh.Insert("knnDB", dic);
                    conn.Close();
                    isComplate = true;
                }
            }
            return isComplate;
        }
        public int getRotateByCode(string code)
        {
            MessageBox.Show(code);
            string statement = "SELECT rotate AS R FROM knnDB WHERE code = '" + code + "'";
            DataTable temp = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    SQLiteHelper sh = new SQLiteHelper(cmd);
                    temp = sh.Select(statement);
                    conn.Close();
                }
            }

            foreach (DataRow rows in temp.Rows)
            {
                string val = rows["R"].ToString();
                int count = int.Parse(val);
                return count;
            }
            return 0;
        }
        public int knnProcess(string code)
        {
            string statement = "SELECT code AS C, rotate AS R FROM knnDB";
            DataTable temp = new DataTable();
            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();
                    SQLiteHelper sh = new SQLiteHelper(cmd);
                    temp = sh.Select(statement);
                    conn.Close();
                }
            }

            int C = temp.Rows.Count;
            string[] myKeys = new string[C];
            int[] myValues = new int[C];
            int i = 0;
            foreach (DataRow rows in temp.Rows)
            {
                string val = rows["R"].ToString();
                string text = rows["C"].ToString();                
                int differentValue = 0;
                int counting = 0;
                foreach (char item in text)
                {
                    differentValue += Math.Abs(int.Parse(item.ToString()) - int.Parse(code[counting].ToString()));
                    counting++;
                }
                myKeys[i] = val;
                myValues[i] = differentValue;
                i++;
            }
            int[] myArray = new int[5];
            Array.Sort(myValues, myKeys);
            myArray[0] = int.Parse(myKeys[0]);
            myArray[1] = int.Parse(myKeys[1]);
            myArray[2] = int.Parse(myKeys[2]);
            myArray[3] = int.Parse(myKeys[3]);
            myArray[4] = int.Parse(myKeys[4]);
            int mode = myArray
                    .GroupBy(x => x)
                    .OrderByDescending(g => g.Count())
                    .First() // throws InvalidOperationException if myArray is empty
                    .Key;
            return mode;
        }
    }
}
