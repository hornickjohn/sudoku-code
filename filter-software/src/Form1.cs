using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace src
{
    public partial class Form1 : Form
    {
        List<Puzzle> data = new List<Puzzle>();
        int TOTALTECHNIQUES = 9;
        Random R;

        List<Puzzle> savelist = new List<Puzzle>();

        public Form1()
        {
            InitializeComponent();
            R = new Random();
            readInfo();
        }

        struct Puzzle : IComparable<Puzzle>
        {
            public string puzzle;

            public int hiddensingles;
            public int nakedsets;
            public int omissions;
            public int hiddensets;
            public int xwings;
            public int swordfishes;
            public int uniquerectangles;
            public int ywings;
            public int finnedxwing;

            public int attemptlimit;

            public bool symmetrical;

            public double comparison;
            public int unsolved;
            public int average;

            public double difficulty;

            public Puzzle(string puz, int h, int ns, int o, int hs, int x, int s, int ur, int y, int fx, bool sym, int atmptlmt)
            {
                puzzle = puz;
                hiddensingles = h;
                nakedsets = ns;
                omissions = o;
                hiddensets = hs;
                xwings = x;
                swordfishes = s;
                uniquerectangles = ur;
                ywings = y;
                finnedxwing = fx;
                symmetrical = sym;
                attemptlimit = atmptlmt;

                int count = 0;
                for(int i = 0; i < puzzle.Length; i++)
                {
                    if(puzzle.ElementAt(i).Equals('0'))
                    {
                        count++;
                    }
                }
                unsolved = count;

                //set difficulty
                int[] vals = new int[9];
                vals[0] = hiddensingles;
                vals[1] = nakedsets;
                vals[2] = omissions;
                vals[3] = hiddensets;
                vals[4] = xwings;
                vals[5] = swordfishes;
                vals[6] = uniquerectangles;
                vals[7] = ywings;
                vals[8] = finnedxwing;
                //ADD NEW TECHNIQUE HERE

                average = unsolved / vals.Sum();

                int maxMethod = 1;
                for(int i = 1; i < vals.Length; i++)
                {
                    if (vals[i] > 0) { maxMethod = i + 1; }
                }
                
                int difIndex = ((unsolved - 25) * 2) + ((10 - average) * 10);
                if (difIndex < 90 && maxMethod <= 1)
                {
                    difficulty = 1.0;
                    difficulty += 0.1 * (hiddensingles / 81.0);
                    difficulty += 0.9 * (difIndex / 90.0);
                }
                else if (difIndex < 102 && maxMethod <= 1)
                {
                    difficulty = 2.0;
                    difficulty += 0.1 * (hiddensingles / 81.0);
                    difficulty += 0.9 * ((difIndex - 90.0) / 12.0);
                }
                else if (difIndex < 114 && maxMethod <= 1)
                {
                    difficulty = 3.0;
                    difficulty += 0.1 * (hiddensingles / 81.0);
                    difficulty += 0.9 * ((difIndex - 102.0) / 12.0);
                }
                else if (difIndex < 126 && maxMethod <= 1)
                {
                    difficulty = 4.0;
                    difficulty += 0.1 * (hiddensingles / 81.0);
                    difficulty += 0.9 * ((difIndex - 114.0) / 12.0);
                }
                else if (difIndex < 142 && difIndex >= 110 && maxMethod <= 2)
                {
                    difficulty = 5.0;
                    difficulty += 0.1 * (hiddensingles / 81.0);
                    difficulty += 0.45 * Math.Min((nakedsets / 40.0), 1.0);
                    difficulty += 0.45 * ((difIndex - 110.0) / 32.0);
                }
                else
                {
                    if (maxMethod > 2 || difIndex >= 142)
                    {
                        difficulty = 6.0;
                    }
                    else
                    {
                        difficulty = 0.0;
                    }
                }
                comparison = difficulty;
                //comparison = (1000.0 * fx) + (100.0 * y) + (10.0 * ur) + (1.0 * s) + (0.1 * x) + (0.01 * hs) + (0.001 * o) + (0.00001 * ns) + (0.0000001 * h) + (0.000000001 * unsolved) + (0.00000000001 * (81 - avg));
            }

            public int CompareTo(Puzzle other)
            {
                if (comparison > other.comparison) { return -1; }
                else if (comparison < other.comparison) { return 1; }
                return 0;
            }

            public override string ToString()
            {
                return difficulty.ToString("0.00") + " - " + puzzle + "   " + unsolved + "   " + hiddensingles + " " + nakedsets + " " + omissions + " " + hiddensets + " " + xwings + " " + swordfishes + " " + uniquerectangles + " " + ywings + " " + finnedxwing + "   " + average + "   " + attemptlimit;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int[] mins = new int[TOTALTECHNIQUES];
            int[] maxes = new int[TOTALTECHNIQUES];
            int mintech = 0;
            int maxtech = TOTALTECHNIQUES;

            int minunsolved = 0;
            int maxunsolved = 81;
            try { minunsolved = Convert.ToInt32(textBox21.Text); }
            catch { minunsolved = 0; }
            try { maxunsolved = Convert.ToInt32(textBox22.Text); }
            catch { maxunsolved = 81; }
            if (minunsolved < 0 || minunsolved > 81) { minunsolved = 0; }
            if (maxunsolved < minunsolved || maxunsolved > 81) { maxunsolved = 81; }

            int minaverage = 0;
            int maxaverage = 81;
            try { minaverage = Convert.ToInt32(textBox23.Text); }
            catch { minaverage = 0; }
            try { maxaverage = Convert.ToInt32(textBox24.Text); }
            catch { maxaverage = 81; }
            if (minaverage < 0 || minaverage > 81) { minaverage = 0; }
            if (maxaverage < minaverage || maxaverage > 81) { maxaverage = 81; }

            int minattempt = 10;
            int maxattempt = 81;
            try { minattempt = Convert.ToInt32(textBox28.Text); }
            catch { minattempt = 0; }
            try { maxattempt = Convert.ToInt32(textBox29.Text); }
            catch { maxattempt = 81; }
            if (minattempt < 0 || minattempt > 81) { minattempt = 0; }
            if (maxattempt < minattempt || maxattempt > 81) { maxattempt = 81; }

            double mindiff = 0.0;
            double maxdiff = 10.0;
            try { mindiff = Convert.ToDouble(textBox25.Text); }
            catch { mindiff = 0.0; }
            try { maxdiff = Convert.ToDouble(textBox26.Text); }
            catch { maxdiff = 10.0; }
            if (mindiff < 0.0 || mindiff > 10.0) { mindiff = 0.0; }
            if (maxdiff < mindiff || maxdiff > 10.0) { maxdiff = 10.0; }

            try { mintech = Convert.ToInt32(textBox17.Text); }
            catch { mintech = 0; }
            try { maxtech = Convert.ToInt32(textBox18.Text); }
            catch { maxtech = TOTALTECHNIQUES; }
            if(mintech < 0 || mintech > TOTALTECHNIQUES) { mintech = 0; }
            if(maxtech < mintech || maxtech > TOTALTECHNIQUES) { maxtech = TOTALTECHNIQUES; }

            try { mins[0] = Convert.ToInt32(textBox1.Text); }
            catch { mins[0] = 0; }
            try { mins[1] = Convert.ToInt32(textBox2.Text); }
            catch { mins[1] = 0; }
            try { mins[2] = Convert.ToInt32(textBox3.Text); }
            catch { mins[2] = 0; }
            try { mins[3] = Convert.ToInt32(textBox4.Text); }
            catch { mins[3] = 0; }
            try { mins[4] = Convert.ToInt32(textBox5.Text); }
            catch { mins[4] = 0; }
            try { mins[5] = Convert.ToInt32(textBox6.Text); }
            catch { mins[5] = 0; }
            try { mins[6] = Convert.ToInt32(textBox7.Text); }
            catch { mins[6] = 0; }
            try { mins[7] = Convert.ToInt32(textBox8.Text); }
            catch { mins[7] = 0; }
            try { mins[8] = Convert.ToInt32(textBox19.Text); }
            catch { mins[8] = 0; }
            //ADD NEW MIN HERE
            try { maxes[0] = Convert.ToInt32(textBox9.Text); }
            catch { maxes[0] = 81; }
            try { maxes[1] = Convert.ToInt32(textBox10.Text); }
            catch { maxes[1] = 81; }
            try { maxes[2] = Convert.ToInt32(textBox11.Text); }
            catch { maxes[2] = 81; }
            try { maxes[3] = Convert.ToInt32(textBox12.Text); }
            catch { maxes[3] = 81; }
            try { maxes[4] = Convert.ToInt32(textBox13.Text); }
            catch { maxes[4] = 81; }
            try { maxes[5] = Convert.ToInt32(textBox14.Text); }
            catch { maxes[5] = 81; }
            try { maxes[6] = Convert.ToInt32(textBox15.Text); }
            catch { maxes[6] = 81; }
            try { maxes[7] = Convert.ToInt32(textBox16.Text); }
            catch { maxes[7] = 81; }
            try { maxes[8] = Convert.ToInt32(textBox20.Text); }
            catch { maxes[8] = 81; }
            //ADD NEW MAX HERE
            for (int i = 0; i < TOTALTECHNIQUES; i++)
            {
                if(mins[i] < 0 || mins[i] > 81)
                {
                    mins[i] = 0;
                }
            }
            for (int i = 0; i < TOTALTECHNIQUES; i++)
            {
                if (maxes[i] < mins[i] || mins[i] > 81)
                {
                    maxes[i] = 81;
                }
            }

            List<Puzzle> results = new List<Puzzle>();
            foreach(Puzzle p in data)
            {
                int[] vals = returnVals(p);
                
                bool fit = true;
                int techs = 0;
                for(int i = 0; i < TOTALTECHNIQUES; i++)
                {
                    if(vals[i] > 0)
                    {
                        techs++;
                    }
                    if(vals[i] > maxes[i] || vals[i] < mins[i])
                    {
                        fit = false;
                        break;
                    }
                }
                if(fit && (techs > maxtech || techs < mintech))
                {
                    fit = false;
                }
                if (fit && (p.unsolved > maxunsolved || p.unsolved < minunsolved))
                {
                    fit = false;
                }
                if (fit && (p.attemptlimit > maxattempt || p.attemptlimit < minattempt))
                {
                    fit = false;
                }
                if (fit && (p.average > maxaverage || p.average < minaverage))
                {
                    fit = false;
                }
                if(fit && (p.difficulty > maxdiff || p.difficulty < mindiff))
                {
                    fit = false;
                }
                if(fit && ((!checkBox1.Checked && p.symmetrical) || (!checkBox2.Checked && !p.symmetrical)))
                {
                    fit = false;
                }
                if (fit)
                {
                    results.Add(p);
                }
            }

            panel1.Controls.Clear();

            int max = 0;
            try { max = Convert.ToInt32(textBox27.Text); }
            catch { max = results.Count; }
            if(max > results.Count || max < 0) { max = results.Count; }
            int y = 0;
            int cused = 0;
            int ctotal = 0;
            foreach (Puzzle p in results)
            {
                if (cused < max)
                {
                    if(!(cused >= max) && R.Next(results.Count - ctotal) < (max - cused))
                    {
                        cused++;

                        //add puzzle to output
                        Panel gb = new Panel();
                        gb.Width = panel1.Width - 30;
                        gb.Height = 20;
                        gb.Location = new Point(0, y);
                        TextBox tb = new TextBox();
                        tb.ReadOnly = true;
                        tb.Width = panel1.Width - 120;
                        tb.Location = new Point(0, 0);
                        tb.Text = p.ToString();
                        if(p.symmetrical)
                        {
                            tb.BackColor = Color.Aqua;
                        }
                        gb.Controls.Add(tb);

                        //give it a list-add button
                        bool notfound = true;
                        foreach (Puzzle pp in savelist)
                        {
                            if(pp.ToString().Equals(p.ToString()))
                            {
                                notfound = false;
                            }
                        }
                        if(notfound)
                        {
                            Button b = new Button();
                            b.Width = 50;
                            b.Text = "Add";
                            b.Height = tb.Height;
                            b.Location = new Point(tb.Location.X + tb.Width, 0);
                            b.Click += AddClicked;
                            gb.Controls.Add(b);
                        }
                        
                        panel1.Controls.Add(gb);

                        y += 20;
                    }
                }
                ctotal++;
            }

            label11.Text = "Results - " + cused + " / " + data.Count;
        }

        private void AddClicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            foreach(object tb in b.Parent.Controls)
            {
                if (tb.GetType().Name.Equals("TextBox"))
                {
                    string s = ((TextBox)tb).Text;
                    bool sym = ((TextBox)tb).BackColor == Color.Aqua;
                    string[] split = s.Split(' ');
                    savelist.Add(new Puzzle(split[2], Convert.ToInt32(split[8]), Convert.ToInt32(split[9]), Convert.ToInt32(split[10]),
                        Convert.ToInt32(split[11]), Convert.ToInt32(split[12]), Convert.ToInt32(split[13]), Convert.ToInt32(split[14]),
                        Convert.ToInt32(split[15]), Convert.ToInt32(split[16]), sym, Convert.ToInt32(split[22])));
                }
            }
            b.Parent.Controls.Remove(b);
        }

        private int[] returnVals(Puzzle p)
        {
            int[] vals = new int[TOTALTECHNIQUES];
            vals[0] = p.hiddensingles;
            vals[1] = p.nakedsets;
            vals[2] = p.omissions;
            vals[3] = p.hiddensets;
            vals[4] = p.xwings;
            vals[5] = p.swordfishes;
            vals[6] = p.uniquerectangles;
            vals[7] = p.ywings;
            vals[8] = p.finnedxwing;
            //ADD NEW TECHNIQUE HERE
            return vals;
        }

        private void readInfo()
        {
            StreamReader sr = new StreamReader(".\\Generated_Sudoku_Puzzles.txt");

            while (!sr.EndOfStream)
            {
                string s = sr.ReadLine();
                string[] broken = s.Split(' ');
                if (broken.Length >= 12)
                {
                    data.Add(new Puzzle(broken[0], Convert.ToInt32(broken[1]), Convert.ToInt32(broken[2]), Convert.ToInt32(broken[3]), Convert.ToInt32(broken[4]), Convert.ToInt32(broken[5]), Convert.ToInt32(broken[6]), Convert.ToInt32(broken[7]), Convert.ToInt32(broken[8]), Convert.ToInt32(broken[9]), Convert.ToInt32(broken[10]) != 0, Convert.ToInt32(broken[11])));
                }
            }

            sr.Close();

            StreamReader sr2 = new StreamReader(".\\SAVED_PUZZLES.txt");

            while (!sr2.EndOfStream)
            {
                string s = sr2.ReadLine();
                string[] broken = s.Split(' ');
                if (broken.Length >= 12)
                {
                    savelist.Add(new Puzzle(broken[0], Convert.ToInt32(broken[1]), Convert.ToInt32(broken[2]), Convert.ToInt32(broken[3]), Convert.ToInt32(broken[4]), Convert.ToInt32(broken[5]), Convert.ToInt32(broken[6]), Convert.ToInt32(broken[7]), Convert.ToInt32(broken[8]), Convert.ToInt32(broken[9]), Convert.ToInt32(broken[10]) != 0, Convert.ToInt32(broken[11])));
                }
            }

            sr2.Close();

            data.Sort();
            savelist.Sort();
            
            label11.Text = "Results - 0 / " + data.Count;
        }

        private void saveListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StreamWriter sw = new StreamWriter(".\\SAVED_PUZZLES.txt");

            foreach(Puzzle p in savelist)
            {
                string s = p.puzzle + " ";
                s += p.hiddensingles + " ";
                s += p.nakedsets + " ";
                s += p.omissions + " ";
                s += p.hiddensets + " ";
                s += p.xwings + " ";
                s += p.swordfishes + " ";
                s += p.uniquerectangles + " ";
                s += p.ywings + " ";
                s += p.finnedxwing + " ";
                s += p.average + " ";
                if (p.symmetrical)
                {
                    s += "1 \n";
                }
                else
                {
                    s += "0 \n";
                }

                sw.WriteLine(s);
            }

            sw.Close();
        }

        private void loadListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel1.Controls.Clear();
            savelist.Sort();
            int y = 0;
            label11.Text = "Saved List - " + savelist.Count;
            foreach (Puzzle p in savelist)
            {
                Panel gb = new Panel();
                gb.Width = panel1.Width - 30;
                gb.Height = 20;
                gb.Location = new Point(0, y);
                TextBox tb = new TextBox();
                tb.ReadOnly = true;
                tb.Width = panel1.Width - 120;
                tb.Location = new Point(0, 0);
                tb.Text = p.ToString();
                if (p.symmetrical)
                {
                    tb.BackColor = Color.Aqua;
                }
                gb.Controls.Add(tb);

                //give it a list-remove button
                Button b = new Button();
                b.Width = 70;
                b.Text = "Remove";
                b.Height = tb.Height;
                b.Location = new Point(tb.Location.X + tb.Width, 0);
                b.Click += RemoveClicked;
                gb.Controls.Add(b);

                panel1.Controls.Add(gb);

                y += 20;
            }
        }

        private void RemoveClicked(object sender, EventArgs e)
        {
            Button b = (Button)sender;
            foreach (object tb in b.Parent.Controls)
            {
                if (tb.GetType().Name.Equals("TextBox"))
                {
                    string s = ((TextBox)tb).Text;
                    foreach(Puzzle p in savelist)
                    {
                        if(p.puzzle.Equals(s.Split(' ')[2]))
                        {
                            savelist.Remove(p);
                            break;
                        }
                    }
                }
            }
            bool found = false;
            foreach(Panel pan in b.Parent.Parent.Controls)
            {
                if(found)
                {
                    pan.Location = new Point(pan.Location.X, pan.Location.Y - 20);
                }
                else
                {
                    if(pan == b.Parent)
                    {
                        found = true;
                    }
                }
            }
            b.Parent.Parent.Controls.Remove(b.Parent);
            label11.Text = "Saved List - " + savelist.Count;
        }
    }
}
