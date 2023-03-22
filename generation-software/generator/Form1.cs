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

namespace generator
{
    public partial class Form1 : Form
    {
        //DEBUG
        private int Reached;

        //Input Parameters
        public static int BASE = 9;
        public static int BASEROOT = 3;
        private bool SYMMETRY = true;
        private Technique[] Techniques;
        //private int RUNS = 10;

        private int ATTEMPTLIMIT;

        public static Random R;
        public static int[,] puzzle;
        private TextBox[,] board;

        private const int CELL_SIZE = 38;

        //0=hiddensingles
        //1=nakedsets
        //2=omission
        //3=hiddensets
        //4=xwing
        //5=swordfish
        //6=unique rectangle
        //7=ywing
        //8=finned xwing

        public Form1()
        {
            InitializeComponent();

            R = new Random();
            puzzle = new int[BASE, BASE];

            Techniques = new Technique[9];
            Techniques[0] = new Technique("Hidden Singles", Logic.HiddenSingles);
            Techniques[0].SetActive(true);
            Techniques[1] = new Technique("Naked Sets", Logic.NakedSets);
            Techniques[1].SetActive(true);
            Techniques[2] = new Technique("Omission", Logic.Omission);
            Techniques[3] = new Technique("Hidden Sets", Logic.HiddenSets);
            Techniques[4] = new Technique("X-Wing", Logic.XWing);
            Techniques[5] = new Technique("Swordfish", Logic.Swordfish);
            Techniques[6] = new Technique("Unique Rectangle", Logic.UniqueRectangle);
            Techniques[7] = new Technique("Y-Wing", Logic.YWing);
            Techniques[8] = new Technique("Finned X-Wing", Logic.FinnedXWing);

            InitializeBoard();
        }

        /// <summary>
        /// Calls the necessary functions to generate a new sudoku puzzle according to inputs, and put it on the board.
        /// </summary>
        private void CreatePuzzleButton_Click(object sender, EventArgs e)
        {
            //DEBUG
            Reached = 0;

            UpdateParameters(true, true, true);
            puzzle = new int[BASE, BASE];
            GenerateGrid();
            Unsolve(); 
            Print();

            string form = "";
            for(int i = 0; i < BASE; i++)
            {
                for(int j = 0; j < BASE; j++)
                {
                    form += puzzle[i, j].ToString() + ";";
                }
            }
            ImportInput.Text = form.Substring(0, form.Length - 1);

            //DEBUG
            TextOutput.Text += "\n" + Reached.ToString();
        }
        
        /// <summary>
        /// Does a given row contain a value?
        /// </summary>
        private bool RowContains(int row, int value)
        {
            for (int i = 0; i < BASE; i++)
            {
                if (puzzle[row, i] == value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Does a given column contain a value?
        /// </summary>
        private bool ColContains(int col, int value)
        {
            for (int i = 0; i < BASE; i++)
            {
                if (puzzle[i, col] == value)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Does the block of a given cell contain a value?
        /// </summary>
        private bool BlockContains(int row, int col, int value)
        {
            int difRow = Convert.ToInt32(Math.IEEERemainder(row, BASEROOT));
            int difCol = Convert.ToInt32(Math.IEEERemainder(col, BASEROOT));
            if (difRow < 0) { difRow += BASEROOT; }
            if (difCol < 0) { difCol += BASEROOT; }

            for (int i = row - difRow; i < row - difRow + BASEROOT; i++)
            {
                for (int j = col - difCol; j < col - difCol + BASEROOT; j++)
                {
                    if (puzzle[i, j] == value)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// Does the row, column, or block of a given cell contain a value?
        /// </summary>
        private bool HouseContains(int row, int col, int value)
        {
            return RowContains(row, value) || ColContains(col, value) || BlockContains(row, col, value);
        }
        
        /// <summary>
        /// Creates a solved puzzle using recursive helper method.
        /// </summary>
        private void GenerateGrid()
        {
            GenerateGrid(1, 0);
        }

        /// <summary>
        /// Recursive grid generation.
        /// </summary>
        private bool GenerateGrid(int value, int row)
        {
            if(value == BASE && row == BASE - 1)
            {
                for(int i = 0; i < BASE; i++)
                {
                    if(puzzle[row, i] == 0)
                    {
                        puzzle[row, i] = value;
                        return true;
                    }
                }
            }

            int error = 0;
            bool[] errors = new bool[BASE];
            int index;

            while(error < BASE)
            {
                index = R.Next(BASE);
                if(!errors[index])
                {
                    if(puzzle[row, index] == 0 && !ColContains(index, value) && !BlockContains(row, index, value))
                    {
                        puzzle[row, index] = value;
                        if(row == BASE - 1)
                        {
                            if(!GenerateGrid(value + 1, 0))
                            {
                                puzzle[row, index] = 0;
                                errors[index] = true;
                                error++;
                            }
                            else
                            {
                                return true;
                            }
                        }
                        else
                        {
                            if (!GenerateGrid(value, row + 1))
                            {
                                puzzle[row, index] = 0;
                                errors[index] = true;
                                error++;
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    else
                    {
                        error++;
                        errors[index] = true;
                    }
                }
            }
            return false;
        }
        
        /// <summary>
        /// Removes cells while ensuring puzzle is still solvable.
        /// </summary>
        private void Unsolve()
        {
            if (BASE == 1) { puzzle[0, 0] = 0; return; }
            bool notDone = true;
            int tries = 0;
            int uns = 0;

            int min = BASE * BASE / 8;
            ATTEMPTLIMIT = R.Next((BASE * BASE) - min) + min;
            if(BASE >= 10) { ATTEMPTLIMIT = R.Next(68) + 23; }

            while (notDone)
            {
                int num = ((BASE / 2) - uns) / 3;
                if(SYMMETRY) { num /= 2; }
                if(num < 1) { num = 1; }

                //DEBUG
                Reached = num;

                Point[] pts = new Point[num];

                for (int i = 0; i < pts.Length; i++)
                {
                    //choose a random cell to remove
                    int x = R.Next(BASE);
                    int y = R.Next(BASE);

                    //if cell already empty, crawl through grid to find another that isn't
                    if (puzzle[x, y] == 0)
                    {
                        for (int m = 0; m < BASE; m++)
                        {
                            for (int n = 0; n < BASE; n++)
                            {
                                int mm = x + m;
                                if (mm >= BASE) { mm -= BASE; }
                                int nn = y + n;
                                if (nn >= BASE) { nn -= BASE; }
                                if (puzzle[mm, nn] != 0)
                                {
                                    x = mm;
                                    y = nn;
                                    break;
                                }
                            }
                        }
                    }

                    pts[i] = new Point(x, y);
                }

                //temporarily store puzzle's current state before attempting to solve
                int[,] puzcopy = new int[BASE, BASE];
                Array.Copy(puzzle, puzcopy, BASE * BASE);

                for (int i = 0; i < pts.Length; i++)
                {
                    puzzle[pts[i].X, pts[i].Y] = 0;
                    if (SYMMETRY) { puzzle[BASE - (1 + pts[i].X), BASE - (1 + pts[i].Y)] = 0; }
                }
                Solve(BASE * BASE);
                bool removable = IsSolved();

                puzzle = puzcopy;

                if (removable)
                {
                    for (int i = 0; i < pts.Length; i++)
                    {
                        puzzle[pts[i].X, pts[i].Y] = 0;
                        if (SYMMETRY) { puzzle[BASE - (1 + pts[i].X), BASE - (1 + pts[i].Y)] = 0; }
                    }
                    uns += pts.Length;
                    if(SYMMETRY) { uns += pts.Length; }
                    tries = 0;
                }
                else { tries++; }

                if (tries > ATTEMPTLIMIT)
                {
                    notDone = false;
                }
            }
        }

        /// <summary>
        /// Is the current puzzle solved correctly?
        /// </summary>
        private bool IsSolved()
        {
            //check each cell
            for (int i = 0; i < BASE; i++)
            {
                for (int j = 0; j < BASE; j++)
                {
                    //does the cell have a valid value
                    if (puzzle[i, j] < 1 || puzzle[i, j] > BASE)
                    {
                        return false;
                    }

                    //do any of the cells houses contain a duplicate value
                    int tempVal = puzzle[i, j];
                    puzzle[i, j] = 0;
                    if (HouseContains(i, j, tempVal))
                    {
                        puzzle[i, j] = tempVal;
                        return false;
                    }
                    puzzle[i, j] = tempVal;
                }
            }
            return true;
        }

        /// <summary>
        /// Prints current puzzle data into the cells on the board.
        /// </summary>
        private void Print()
        {
            InitializeBoard();

            int clues = DisableFilledinCells();
            TextOutput.Text = "Clues: " + clues.ToString() + " (" + (Convert.ToDouble(clues / Convert.ToDouble(BASE * BASE)) * 100.0).ToString("0") + "%)";
        }

        private void InitializeBoard()
        {
            int inputWidth = InputBox.Width + CreatePuzzleButton.Width + 30;
            int inputHeight = InputBox.Height + 20;
            int totalWidth = 40 + (BASE * CELL_SIZE) + ((BASE - 1) * 6) + ((BASEROOT - 1) * 17) + inputWidth;
            int totalHeight = 40 + Math.Max(totalWidth - inputWidth, inputHeight);
            this.Size = new Size(totalWidth, totalHeight);

            if (board != null)
            {
                foreach (TextBox tb in board)
                {
                    tb.Dispose();
                }
            }

            //create the cells and add them to the form
            board = new TextBox[BASE, BASE];
            for (int i = 0; i < BASE; i++)
            {
                for(int j = 0; j < BASE; j++)
                {
                    TextBox addition = new TextBox();
                    addition.KeyDown += Cell_KeyDown;
                    addition.KeyUp += EndIfSolved;
                    addition.Size = new Size(38, 38);
                    addition.MaxLength = 1;
                    if(BASE > 9) { addition.MaxLength = 2; }
                    addition.Font = new Font("Microsoft Sans Serif", 20);
                    addition.TextAlign = HorizontalAlignment.Center;
                    addition.Location = new Point(Convert.ToInt32(23 + (j * 44) + ((j / BASEROOT) * 17)), Convert.ToInt32(23 + (i * 44) + ((i / BASEROOT) * 17)));
                    this.Controls.Add(addition);
                    board[i, j] = addition;
                }
            }

            //move other controls according to board size
            InputBox.Location = new Point(totalWidth - inputWidth, 10);
            CreatePuzzleButton.Location = new Point(totalWidth - inputWidth + InputBox.Width + 10, 23 + 10);
            SolveButton.Location = new Point(totalWidth - inputWidth + InputBox.Width + 10, 23 + 20 + CreatePuzzleButton.Height);
            ClearButton.Location = new Point(totalWidth - 20 - ClearButton.Width, 23 + 20 + CreatePuzzleButton.Height);
            TextOutput.Location = new Point(totalWidth - inputWidth + InputBox.Width + 10, 23 + 30 + CreatePuzzleButton.Height + SolveButton.Height);
            CreateMultiple.Location = new Point(totalWidth - inputWidth + InputBox.Width + 10, 23 + 40 + CreatePuzzleButton.Height + SolveButton.Height + TextOutput.Height);
            MultipleInput.Location = new Point(totalWidth - inputWidth + InputBox.Width + 10, 23 + 50 + CreatePuzzleButton.Height + SolveButton.Height + TextOutput.Height + CreateMultiple.Height);
            SolveOneButton.Location = new Point(totalWidth - inputWidth, 20 + InputBox.Height);
            ImportInput.Location = new Point(totalWidth - inputWidth, Math.Max(30 + InputBox.Height + SolveOneButton.Height, 83 + CreatePuzzleButton.Height + SolveButton.Height + TextOutput.Height + CreateMultiple.Height + MultipleInput.Height));
        }

        /// <summary>
        /// Handles key presses while in a cell for board navigation and input.
        /// </summary>
        private void Cell_KeyDown(object sender, KeyEventArgs e)
        {
            int x = -1;
            int y = -1;
            for (int i = 0; i < BASE; i++)
            {
                for (int j = 0; j < BASE; j++)
                {
                    if (board[i, j].Focused)
                    {
                        x = i;
                        y = j;
                    }
                }
            }
            if (x >= 0 && y >= 0)
            { 
                //arrow keys to navigate board
                if (e.KeyCode == Keys.Left && y > 0)
                {
                    board[x, y - 1].Focus();
                }
                else if (e.KeyCode == Keys.Up && x > 0)
                {
                    board[x - 1, y].Focus();
                }
                else if (e.KeyCode == Keys.Right && y < BASE - 1)
                {
                    board[x, y + 1].Focus();
                }
                else if (e.KeyCode == Keys.Down && x < BASE - 1)
                {
                    board[x + 1, y].Focus();
                }
                //number keys replace previous entry in cell
                else if(((e.KeyCode <= Keys.D9 && e.KeyCode >= Keys.D1) || (e.KeyCode <= Keys.NumPad9 && e.KeyCode >= Keys.NumPad1) && !(board[x, y].ReadOnly)) && (BASE <= 9 || board[x,y].Text.Length >= 2))
                {
                    board[x, y].Text = "";
                }
                //enter key autofills cell if all other cells in one of its houses are filled
                else if(e.KeyCode == Keys.Enter && puzzle[x, y] == 0)
                {
                    UpdatePuzzle();

                    bool[] valueFound = new bool[BASE];
                    int count = 0;

                    //check row
                    for(int j = 0; j < BASE; j++)
                    {
                        if(puzzle[x, j] > 0 && !valueFound[puzzle[x, j] - 1])
                        {
                            valueFound[puzzle[x, j] - 1] = true;
                            count++;
                        }
                    }
                    if(count != BASE - 1)
                    {
                        valueFound = new bool[BASE];
                        count = 0;

                        //check column
                        for (int i = 0; i < BASE; i++)
                        {
                            if (puzzle[i, y] > 0 && !valueFound[puzzle[i, y] - 1])
                            {
                                valueFound[puzzle[i, y] - 1] = true;
                                count++;
                            }
                        }

                        if (count != BASE - 1)
                        {
                            valueFound = new bool[BASE];
                            count = 0;

                            //check block
                            for (int m = 0; m < BASE; m++)
                            {
                                int q = ((x / BASEROOT) * BASEROOT) + (y / BASEROOT);
                                Point pt = Logic.ConvertFromBlock(q, m);
                                if (puzzle[pt.X, pt.Y] > 0 && !valueFound[puzzle[pt.X, pt.Y] - 1])
                                {
                                    valueFound[puzzle[pt.X, pt.Y] - 1] = true;
                                    count++;
                                }
                            }
                        }
                    }
                    //set the missing value
                    if (count == BASE - 1)
                    {
                        for (int index = 0; index < BASE; index++)
                        {
                            if(!valueFound[index])
                            {
                                board[x, y].Text = (index + 1).ToString();
                                break;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Applies solution techniques to eliminate possibilities and solve the puzzle.
        /// </summary>
        /// <returns>Array representing amount of times each solution technique was used to solve the puzzle.</returns>
        private int[] Solve(int max)
        {
            bool[,,] pencil = new bool[BASE, BASE, BASE];

            int solvesSoFar = 0;
            
            //make basic pencil marks
            for (int i = 0; i < BASE; i++)
            {
                for (int j = 0; j < BASE; j++)
                {
                    if (puzzle[i, j] == 0)
                    {
                        for (int k = 1; k <= BASE; k++)
                        {
                            if (!HouseContains(i, j, k))
                            {
                                pencil[i, j, k - 1] = true;
                            }
                        }
                    }
                }
            }

            bool solvedone = true;
            
            int[] typeCounts = new int[9];

            while (solvedone)
            {
                solvedone = false;

                //update basic pencil marks
                for (int i = 0; i < BASE; i++)
                {
                    for (int j = 0; j < BASE; j++)
                    {
                        if (puzzle[i, j] == 0)
                        {
                            for (int k = 1; k <= BASE; k++)
                            {
                                if (HouseContains(i, j, k))
                                {
                                    if (pencil[i, j, k - 1])
                                    {
                                        pencil[i, j, k - 1] = false;
                                        solvedone = true;
                                    }
                                }
                            }
                        }
                    }
                }

                for(int i = 0; i < Techniques.Length; i++)
                {
                    if(Techniques[i].GetActive() && Techniques[i].ApplyLogic(pencil))
                    {
                        typeCounts[i]++;
                        solvedone = true;
                        break;
                    }
                }

                solvesSoFar += SolveSingles(pencil);
                if(solvesSoFar >= max) { break; }
            }

            return typeCounts;
        }

        /// <summary>
        /// Solves any cells where we've eliminated all possible values but one.
        /// </summary>
        /// <returns>How many cells were solved.</returns>
        private int SolveSingles(bool[,,] pencil)
        {
            int solved = 0;
            for (int i = 0; i < BASE; i++)
            {
                for (int j = 0; j < BASE; j++)
                {
                    int index = -1;
                    for (int k = 0; k < BASE; k++)
                    {
                        if (pencil[i, j, k])
                        {
                            if (index == -1)
                            {
                                index = k;
                            }
                            else
                            {
                                index = -2;
                                break;
                            }
                        }
                    }
                    if (index >= 0)
                    {
                        puzzle[i, j] = index + 1;
                        Logic.Erase(pencil, i, j);
                        solved++;
                    }
                }
            }
            return solved;
        }

        /// <summary>
        /// Updates internal puzzle data to match what is in client cells. Non-valid input is treated as blank.
        /// </summary>
        private void UpdatePuzzle()
        {
            puzzle = new int[BASE, BASE];
            for(int i = 0; i < BASE; i++)
            {
                for(int j = 0; j < BASE; j++)
                {
                    try
                    {
                        puzzle[i, j] = Convert.ToInt32(board[i, j].Text);
                        if (puzzle[i, j] < 0 || puzzle[i, j] > BASE)
                        {
                            puzzle[i, j] = 0;
                        }
                    }
                    catch { }
                }
            }
        }

        /// <summary>
        /// Attempts to solve current puzzle with chosen methods, outputs amount of times each time of logic was used.
        /// </summary>
        private void SolveButton_Click(object sender, EventArgs e)
        {
            UpdatePuzzle();
            UpdateParameters(false, true, false);
            int[] typeCounts = Solve(BASE * BASE);
            Print();
            if (IsSolved())
            {
                TextOutput.Text = "Success";
            }
            else
            {
                TextOutput.Text = "Failure";
            }

            for(int i = 0; i < Techniques.Length; i++)
            {
                if(Techniques[i].GetActive())
                {
                    TextOutput.Text += "\n" + Techniques[i].Name + ": " + Techniques[i].GetCount().ToString();
                }
            }

            string form = "";
            for (int i = 0; i < BASE; i++)
            {
                for (int j = 0; j < BASE; j++)
                {
                    form += puzzle[i, j].ToString() + ";";
                }
            }
            ImportInput.Text = form.Substring(0, form.Length - 1);
        }
        
        //Function that could be used to generate puzzles in bulk and write them to a file.
        //I actually have done this and have a separate client I use to sift through the puzzles according to various parameters.
        private void CreateMultiple_Click(object sender, EventArgs e)
        {
            int RUNS = 0;
            try { RUNS = Convert.ToInt32(MultipleInput.Text); }
            catch { }

            List<string> outputs = new List<string>();
            UpdateParameters(true, true, true);
            
            for(int r = 0; r < RUNS; r++)
            {
                puzzle = new int[BASE, BASE];
                GenerateGrid();
                Unsolve();
                string puz = "";

                for(int i = 0; i < BASE; i++)
                {
                    for(int j = 0; j < BASE; j++)
                    {
                        puz += puzzle[i, j].ToString();
                        if(BASE >= 10) { puz += ";"; }
                    }
                }
                if (BASE >= 10) { puz = puz.Substring(0, puz.Length - 1); }

                int[] typeCounts = Solve(BASE * BASE);

                string puzSolved = "";

                for (int i = 0; i < BASE; i++)
                {
                    for (int j = 0; j < BASE; j++)
                    {
                        puzSolved += puzzle[i, j].ToString();
                        if (BASE >= 10) { puzSolved += ";"; }
                    }
                }
                if (BASE >= 10) { puzSolved = puzSolved.Substring(0, puzSolved.Length - 1); }

                string sym = "0";
                if(SYMMETRY) { sym = "1"; }
                outputs.Add(puz + " " + typeCounts[0] + " " + typeCounts[1] + " " + typeCounts[2] + " " + typeCounts[3] + " " + typeCounts[4] + " " + typeCounts[5] + " " + typeCounts[6] + " " + typeCounts[7] + " " + typeCounts[8] + " " + sym + " " + ATTEMPTLIMIT.ToString() + " " + puzSolved + " \n");
            }

            string pth = "..\\Generated_Sudoku_Puzzles.txt";
            if(BASE >= 10) { pth = "..\\Generated_Sudoku_Puzzles_16.txt"; }

            StreamWriter sw = new StreamWriter(pth);
            foreach (string s in outputs)
            {
                sw.WriteLine(s);
            }
            sw.Close();
        }

        /// <summary>
        /// Updates puzzle creation parameters according to input.
        /// </summary>
        private void UpdateParameters(bool UpdateSize, bool UpdateTechniques, bool UpdateSymmetry)
        {
            if (UpdateSize)
            {
                try
                {
                    BASE = Convert.ToInt32(SizeInput.Text.Split(' ')[0]);
                    BASEROOT = Convert.ToInt32(Math.Sqrt(BASE));
                }
                catch
                {
                    BASE = 9;
                    BASEROOT = 3;
                }
            }

            if (UpdateTechniques)
            {
                Techniques[0].SetActive(HiddenSinglesInput.Checked);
                Techniques[1].SetActive(NakedSetsInput.Checked);
                Techniques[2].SetActive(OmissionInput.Checked);
                Techniques[3].SetActive(HiddenSetsInput.Checked);
                Techniques[4].SetActive(XWingInput.Checked);
                Techniques[5].SetActive(SwordfishInput.Checked);
                Techniques[6].SetActive(UniqueRectangleInput.Checked);
                Techniques[7].SetActive(YWingInput.Checked);
                Techniques[8].SetActive(FinnedXWingInput.Checked);

                for(int i = 0; i < Techniques.Length; i++)
                {
                    Techniques[i].ClearCount();
                }
            }

            if (UpdateSymmetry) { SYMMETRY = SymmetryInput.Checked; }
        }

        /// <summary>
        /// Clears all cells on the board so user can input their own puzzle.
        /// </summary>
        private void ClearButton_Click(object sender, EventArgs e)
        {
            UpdateParameters(true, false, false);
            puzzle = new int[BASE, BASE];
            InitializeBoard();
        }

        /// <summary>
        /// Checks if puzzle has been solved and handles game end if so.
        /// </summary>
        private void EndIfSolved(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode <= Keys.D9 && e.KeyCode >= Keys.D1) || (e.KeyCode <= Keys.NumPad9 && e.KeyCode >= Keys.NumPad1) || e.KeyCode == Keys.Enter)
            {
                bool check = true;
                for (int i = 0; i < BASE; i++)
                {
                    if (board[R.Next(BASE), R.Next(BASE)].Text.Equals(""))
                    {
                        check = false;
                        break;
                    }
                }

                if (check)
                {
                    UpdatePuzzle();
                    if (IsSolved())
                    {
                        TextOutput.Text = "Congratulations, you've solved the puzzle!";
                        DisableFilledinCells();
                    }
                }
            }
        }

        /// <summary>
        /// Sets all cells with known puzzle values to read-only, and empty cells to allow input
        /// </summary>
        /// <returns>Amount of cells that are filled in.</returns>
        private int DisableFilledinCells()
        {
            int filledIn = 0;
            for (int i = 0; i < BASE; i++)
            {
                for (int j = 0; j < BASE; j++)
                {
                    if (puzzle[i, j] == 0)
                    {
                        board[i, j].ReadOnly = false;
                    }
                    else
                    {
                        board[i, j].Text = puzzle[i, j].ToString();
                        board[i, j].ReadOnly = true;
                        filledIn++;
                    }
                }
            }
            return filledIn;
        }

        private void ImportInput_KeyPress(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                string[] puzz = ImportInput.Text.Split(';');

                for(int i = 0; i < BASE; i++)
                {
                    for(int j = 0; j < BASE; j++)
                    {
                        string c = puzz[i * BASE + j];
                        if (!c.Equals("0"))
                        {
                            board[i, j].Text = c;
                        }
                    }
                }
                UpdatePuzzle();
                DisableFilledinCells();
            }
        }

        private void SolveOneButton_Click(object sender, EventArgs e)
        {
            UpdatePuzzle();
            UpdateParameters(false, true, false);
            int[] typeCounts = Solve(1);
            Print();

            for (int i = 0; i < Techniques.Length; i++)
            {
                if (Techniques[i].GetActive())
                {
                    TextOutput.Text += "\n" + Techniques[i].Name + ": " + Techniques[i].GetCount().ToString();
                }
            }
        }
    }
}