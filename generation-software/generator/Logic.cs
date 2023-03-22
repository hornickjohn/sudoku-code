using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace generator
{
    class Logic
    {
        /// <summary>
        /// Eliminates pencil marks where possible based on the Hidden Singles technique.
        /// </summary>
        /// <returns>Number of changes made.</returns>
        public static int HiddenSingles(bool[,,] pencil)
        {
            int changes = 0;

            for (int k = 0; k < Form1.BASE; k++)
            {
                //go through rows
                for (int i = 0; i < Form1.BASE; i++)
                {
                    int count = 0;
                    int index = 0;
                    for (int j = 0; j < Form1.BASE; j++)
                    {
                        if (pencil[i, j, k])
                        {
                            count++;
                            index = j;
                        }
                    }
                    if (count == 1)
                    {
                        Erase(pencil, i, index);
                        pencil[i, index, k] = true;
                        changes++;
                    }
                }

                //go through columns
                for (int j = 0; j < Form1.BASE; j++)
                {
                    int count = 0;
                    int index = 0;
                    for (int i = 0; i < Form1.BASE; i++)
                    {
                        if (pencil[i, j, k])
                        {
                            count++;
                            index = i;
                        }
                    }
                    if (count == 1)
                    {
                        Erase(pencil, index, j);
                        pencil[index, j, k] = true;
                        changes++;
                    }
                }

                //go through blocks
                for (int q = 0; q < Form1.BASE; q++)
                {
                    int count = 0;
                    Point index = new Point(0, 0);
                    for (int i = (q / Form1.BASEROOT) * Form1.BASEROOT; i < ((q / Form1.BASEROOT) * Form1.BASEROOT) + Form1.BASEROOT; i++)
                    {
                        int rem = Convert.ToInt32(Math.IEEERemainder(q, Form1.BASEROOT));
                        if (rem < 0) { rem += Form1.BASEROOT; }
                        int start = rem * Form1.BASEROOT;
                        for (int j = start; j < start + Form1.BASEROOT; j++)
                        {
                            if (pencil[i, j, k])
                            {
                                count++;
                                index.X = i;
                                index.Y = j;
                            }
                        }
                    }
                    if (count == 1)
                    {
                        Erase(pencil, index.X, index.Y);
                        pencil[index.X, index.Y, k] = true;
                        changes++;
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// Eliminates pencil marks where possible based on the Naked Sets technique.
        /// </summary>
        /// <returns>Number of changes made.</returns>
        public static int NakedSets(bool[,,] pencil)
        {
            /*
            Naked Sets:
            In each house, check each combination of two, three, or four cells.
            If that cell-group has exactly two, three, or four possible values respectively, then those values cannot exist elsewhere in that house.
            */

            int changes = 0;

            //Check sets of two cells.
            //rows
            for (int i = 0; i < Form1.BASE; i++)
            {
                for (int a = 0; a < Form1.BASE; a++)
                {
                    for (int b = a + 1; b < Form1.BASE; b++)
                    {
                        if (Form1.puzzle[i, a] == 0 && Form1.puzzle[i, b] == 0)
                        {
                            int counter = 0;
                            int[] vals = new int[2];

                            //check how many different pencil marks there are across empty cells [i,a] and [i,b]... 
                            for (int check = 0; check < Form1.BASE; check++)
                            {
                                if (pencil[i, a, check] || pencil[i, b, check])
                                {
                                    if (counter < 2)
                                    {
                                        vals[counter] = check;
                                        counter++;
                                    }
                                    else
                                    {
                                        counter++;
                                        break;
                                    }
                                }
                            }
                            //if there are exactly two possible values, remove those from other cells in the same house
                            if (counter == 2)
                            {
                                bool changed = false;
                                for (int removecell = 0; removecell < Form1.BASE; removecell++)
                                {
                                    if (removecell != a && removecell != b)
                                    {
                                        if (pencil[i, removecell, vals[0]] || pencil[i, removecell, vals[1]])
                                        {
                                            pencil[i, removecell, vals[0]] = false;
                                            pencil[i, removecell, vals[1]] = false;
                                            changed = true;
                                        }
                                    }
                                }
                                if (changed) { changes++; }
                            }
                        }
                    }
                }
            }
            //cols
            for (int j = 0; j < Form1.BASE; j++)
            {
                for (int a = 0; a < Form1.BASE; a++)
                {
                    for (int b = a + 1; b < Form1.BASE; b++)
                    {
                        if (Form1.puzzle[a, j] == 0 && Form1.puzzle[b, j] == 0)
                        {
                            int counter = 0;
                            int[] vals = new int[2];

                            //check how many different pencil marks there are across empty cells [a,j] and [b,j]... 
                            for (int check = 0; check < Form1.BASE; check++)
                            {
                                if (pencil[a, j, check] || pencil[b, j, check])
                                {
                                    if (counter < 2)
                                    {
                                        vals[counter] = check;
                                        counter++;
                                    }
                                    else
                                    {
                                        counter++;
                                        break;
                                    }
                                }
                            }
                            //if there are exactly two possible values, remove those from other cells in the same house
                            if (counter == 2)
                            {
                                bool changed = false;
                                for (int removecell = 0; removecell < Form1.BASE; removecell++)
                                {
                                    if (removecell != a && removecell != b)
                                    {
                                        if (pencil[removecell, j, vals[0]] || pencil[removecell, j, vals[1]])
                                        {
                                            pencil[removecell, j, vals[0]] = false;
                                            pencil[removecell, j, vals[1]] = false;
                                            changed = true;
                                        }
                                    }
                                }
                                if (changed) { changes++; }
                            }
                        }
                    }
                }
            }
            //blocks
            for (int q = 0; q < Form1.BASE; q++)
            {
                for (int a = 0; a < Form1.BASE; a++)
                {
                    for (int b = a + 1; b < Form1.BASE; b++)
                    {
                        Point c1 = ConvertFromBlock(q, a);
                        Point c2 = ConvertFromBlock(q, b);

                        if (Form1.puzzle[c1.X, c1.Y] == 0 && Form1.puzzle[c2.X, c2.Y] == 0)
                        {
                            int counter = 0;
                            int[] vals = new int[2];

                            //check how many different pencil marks there are across empty cells c1 and c2... 
                            for (int check = 0; check < Form1.BASE; check++)
                            {
                                if (pencil[c1.X, c1.Y, check] || pencil[c2.X, c2.Y, check])
                                {
                                    if (counter < 2)
                                    {
                                        vals[counter] = check;
                                        counter++;
                                    }
                                    else
                                    {
                                        counter++;
                                        break;
                                    }
                                }
                            }
                            //if there are exactly two possible values, remove those from other cells in the same house
                            if (counter == 2)
                            {
                                bool changed = false;
                                for (int removecell = 0; removecell < Form1.BASE; removecell++)
                                {
                                    if (removecell != a && removecell != b)
                                    {
                                        Point rc = ConvertFromBlock(q, removecell);

                                        if (pencil[rc.X, rc.Y, vals[0]] || pencil[rc.X, rc.Y, vals[1]])
                                        {
                                            pencil[rc.X, rc.Y, vals[0]] = false;
                                            pencil[rc.X, rc.Y, vals[1]] = false;
                                            changed = true;
                                        }
                                    }
                                }
                                if (changed) { changes++; }
                            }
                        }
                    }
                }
            }

            //Check sets of three cells.
            //rows
            for (int i = 0; i < Form1.BASE; i++)
            {
                for (int a = 0; a < Form1.BASE; a++)
                {
                    for (int b = a + 1; b < Form1.BASE; b++)
                    {
                        for (int c = b + 1; c < Form1.BASE; c++)
                        {
                            if (Form1.puzzle[i, a] == 0 && Form1.puzzle[i, b] == 0 && Form1.puzzle[i, c] == 0)
                            {
                                int counter = 0;
                                int[] vals = new int[3];

                                //check how many different pencil marks there are across empty cells [i,a] and [i,b] and [i,c]... 
                                for (int check = 0; check < Form1.BASE; check++)
                                {
                                    if (pencil[i, a, check] || pencil[i, b, check] || pencil[i, c, check])
                                    {
                                        if (counter < 3)
                                        {
                                            vals[counter] = check;
                                            counter++;
                                        }
                                        else
                                        {
                                            counter++;
                                            break;
                                        }
                                    }
                                }
                                //if there are exactly three different values, remove those from other cells in the same house
                                if (counter == 3)
                                {
                                    bool changed = false;
                                    for (int removecell = 0; removecell < Form1.BASE; removecell++)
                                    {
                                        if (removecell != a && removecell != b && removecell != c)
                                        {
                                            if (pencil[i, removecell, vals[0]] || pencil[i, removecell, vals[1]] || pencil[i, removecell, vals[2]])
                                            {
                                                pencil[i, removecell, vals[0]] = false;
                                                pencil[i, removecell, vals[1]] = false;
                                                pencil[i, removecell, vals[2]] = false;
                                                changed = true;
                                            }
                                        }
                                    }
                                    if (changed) { changes++; }
                                }
                            }
                        }
                    }
                }
            }
            //cols
            for (int j = 0; j < Form1.BASE; j++)
            {
                for (int a = 0; a < Form1.BASE; a++)
                {
                    for (int b = a + 1; b < Form1.BASE; b++)
                    {
                        for (int c = b + 1; c < Form1.BASE; c++)
                        {
                            if (Form1.puzzle[a, j] == 0 && Form1.puzzle[b, j] == 0 && Form1.puzzle[c, j] == 0)
                            {
                                int counter = 0;
                                int[] vals = new int[3];

                                //check how many different pencil marks there are across empty cells [a,j] and [b,j] and [c,j]... 
                                for (int check = 0; check < Form1.BASE; check++)
                                {
                                    if (pencil[a, j, check] || pencil[b, j, check] || pencil[c, j, check])
                                    {
                                        if (counter < 3)
                                        {
                                            vals[counter] = check;
                                            counter++;
                                        }
                                        else
                                        {
                                            counter++;
                                            break;
                                        }
                                    }
                                }
                                //if there are exactly three different values, remove those from other cells in the same house
                                if (counter == 3)
                                {
                                    bool changed = false;
                                    for (int removecell = 0; removecell < Form1.BASE; removecell++)
                                    {
                                        if (removecell != a && removecell != b && removecell != c)
                                        {
                                            if (pencil[removecell, j, vals[0]] || pencil[removecell, j, vals[1]] || pencil[removecell, j, vals[2]])
                                            {
                                                pencil[removecell, j, vals[0]] = false;
                                                pencil[removecell, j, vals[1]] = false;
                                                pencil[removecell, j, vals[2]] = false;
                                                changed = true;
                                            }
                                        }
                                    }
                                    if (changed) { changes++; }
                                }
                            }
                        }
                    }
                }
            }
            //blocks
            for (int q = 0; q < Form1.BASE; q++)
            {
                for (int a = 0; a < Form1.BASE; a++)
                {
                    for (int b = a + 1; b < Form1.BASE; b++)
                    {
                        for (int c = b + 1; c < Form1.BASE; c++)
                        {
                            Point c1 = ConvertFromBlock(q, a);
                            Point c2 = ConvertFromBlock(q, b);
                            Point c3 = ConvertFromBlock(q, c);

                            //check how many different pencil marks there are across empty cells c1, c2, and c3
                            if (Form1.puzzle[c1.X, c1.Y] == 0 && Form1.puzzle[c2.X, c2.Y] == 0 && Form1.puzzle[c3.X, c3.Y] == 0)
                            {
                                int counter = 0;
                                int[] vals = new int[3];
                                for (int check = 0; check < Form1.BASE; check++)
                                {
                                    if (pencil[c1.X, c1.Y, check] || pencil[c2.X, c2.Y, check] || pencil[c3.X, c3.Y, check])
                                    {
                                        if (counter < 3)
                                        {
                                            vals[counter] = check;
                                            counter++;
                                        }
                                        else
                                        {
                                            counter++;
                                            break;
                                        }
                                    }
                                }
                                //if there are exactly three different values, remove those from other cells in the same house
                                if (counter == 3)
                                {
                                    bool changed = false;
                                    for (int removecell = 0; removecell < Form1.BASE; removecell++)
                                    {
                                        if (removecell != a && removecell != b && removecell != c)
                                        {
                                            Point rc = ConvertFromBlock(q, removecell);

                                            if (pencil[rc.X, rc.Y, vals[0]] || pencil[rc.X, rc.Y, vals[1]] || pencil[rc.X, rc.Y, vals[2]])
                                            {
                                                pencil[rc.X, rc.Y, vals[0]] = false;
                                                pencil[rc.X, rc.Y, vals[1]] = false;
                                                pencil[rc.X, rc.Y, vals[2]] = false;
                                                changed = true;
                                            }
                                        }
                                    }
                                    if (changed) { changes++; }
                                }
                            }
                        }
                    }
                }
            }

            //Check sets of four cells.
            //rows
            for (int i = 0; i < Form1.BASE; i++)
            {
                for (int a = 0; a < Form1.BASE; a++)
                {
                    for (int b = a + 1; b < Form1.BASE; b++)
                    {
                        for (int c = b + 1; c < Form1.BASE; c++)
                        {
                            for (int d = c + 1; d < Form1.BASE; d++)
                            {
                                if (Form1.puzzle[i, a] == 0 && Form1.puzzle[i, b] == 0 && Form1.puzzle[i, c] == 0 && Form1.puzzle[i, d] == 0)
                                {
                                    int counter = 0;
                                    int[] vals = new int[4];

                                    //check how many different pencil marks there are across empty cells [i,a] and [i,b] and [i,c] and [i,d]...
                                    for (int check = 0; check < Form1.BASE; check++)
                                    {
                                        if (pencil[i, a, check] || pencil[i, b, check] || pencil[i, c, check] || pencil[i, d, check])
                                        {
                                            if (counter < 4)
                                            {
                                                vals[counter] = check;
                                                counter++;
                                            }
                                            else
                                            {
                                                counter++;
                                                break;
                                            }
                                        }
                                    }
                                    //if there are exactly four possible values across the four cells, remove any instances of those values across other cells in the same house
                                    if (counter == 4)
                                    {
                                        bool changed = false;
                                        for (int removecell = 0; removecell < Form1.BASE; removecell++)
                                        {
                                            if (removecell != a && removecell != b && removecell != c && removecell != d)
                                            {
                                                if (pencil[i, removecell, vals[0]] || pencil[i, removecell, vals[1]] || pencil[i, removecell, vals[2]] || pencil[i, removecell, vals[3]])
                                                {
                                                    pencil[i, removecell, vals[0]] = false;
                                                    pencil[i, removecell, vals[1]] = false;
                                                    pencil[i, removecell, vals[2]] = false;
                                                    pencil[i, removecell, vals[3]] = false;
                                                    changed = true;
                                                }
                                            }
                                        }
                                        if (changed) { changes++; }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //cols
            for (int j = 0; j < Form1.BASE; j++)
            {
                for (int a = 0; a < Form1.BASE; a++)
                {
                    for (int b = a + 1; b < Form1.BASE; b++)
                    {
                        for (int c = b + 1; c < Form1.BASE; c++)
                        {
                            for (int d = c + 1; d < Form1.BASE; d++)
                            {
                                if (Form1.puzzle[a, j] == 0 && Form1.puzzle[b, j] == 0 && Form1.puzzle[c, j] == 0 && Form1.puzzle[d, j] == 0)
                                {
                                    int counter = 0;
                                    int[] vals = new int[4];

                                    //check how many different pencil marks there are across empty cells [a,j] and [b,j] and [c,j] and [d,j]... 
                                    for (int check = 0; check < Form1.BASE; check++)
                                    {
                                        if (pencil[a, j, check] || pencil[b, j, check] || pencil[c, j, check] || pencil[d, j, check])
                                        {
                                            if (counter < 4)
                                            {
                                                vals[counter] = check;
                                                counter++;
                                            }
                                            else
                                            {
                                                counter++;
                                                break;
                                            }
                                        }
                                    }
                                    //if there are exactly four possible values across these cells, remove them from other cells in the same house
                                    if (counter == 4)
                                    {
                                        bool changed = false;
                                        for (int removecell = 0; removecell < Form1.BASE; removecell++)
                                        {
                                            if (removecell != a && removecell != b && removecell != c && removecell != d)
                                            {
                                                if (pencil[removecell, j, vals[0]] || pencil[removecell, j, vals[1]] || pencil[removecell, j, vals[2]] || pencil[removecell, j, vals[3]])
                                                {
                                                    pencil[removecell, j, vals[0]] = false;
                                                    pencil[removecell, j, vals[1]] = false;
                                                    pencil[removecell, j, vals[2]] = false;
                                                    pencil[removecell, j, vals[3]] = false;
                                                    changed = true;
                                                }
                                            }
                                        }
                                        if (changed) { changes++; }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //blocks
            for (int q = 0; q < Form1.BASE; q++)
            {
                for (int a = 0; a < Form1.BASE; a++)
                {
                    for (int b = a + 1; b < Form1.BASE; b++)
                    {
                        for (int c = b + 1; c < Form1.BASE; c++)
                        {
                            for (int d = c + 1; d < Form1.BASE; d++)
                            {
                                Point c1 = ConvertFromBlock(q, a);
                                Point c2 = ConvertFromBlock(q, b);
                                Point c3 = ConvertFromBlock(q, c);
                                Point c4 = ConvertFromBlock(q, d);

                                if (Form1.puzzle[c1.X, c1.Y] == 0 && Form1.puzzle[c2.X, c2.Y] == 0 && Form1.puzzle[c3.X, c3.Y] == 0 && Form1.puzzle[c4.X, c4.Y] == 0)
                                {
                                    int counter = 0;
                                    int[] vals = new int[4];

                                    //check how many different pencil marks there are across empty cells c1, c2, c3, and c4
                                    for (int check = 0; check < Form1.BASE; check++)
                                    {
                                        if (pencil[c1.X, c1.Y, check] || pencil[c2.X, c2.Y, check] || pencil[c3.X, c3.Y, check] || pencil[c4.X, c4.Y, check])
                                        {
                                            if (counter < 4)
                                            {
                                                vals[counter] = check;
                                                counter++;
                                            }
                                            else
                                            {
                                                counter++;
                                                break;
                                            }
                                        }
                                    }
                                    //if there are exactly four values across these cells, remove those values from other cells in the same house
                                    if (counter == 4)
                                    {
                                        bool changed = false;
                                        for (int removecell = 0; removecell < Form1.BASE; removecell++)
                                        {
                                            if (removecell != a && removecell != b && removecell != c && removecell != d)
                                            {
                                                Point rc = ConvertFromBlock(q, removecell);

                                                if (pencil[rc.X, rc.Y, vals[0]] || pencil[rc.X, rc.Y, vals[1]] || pencil[rc.X, rc.Y, vals[2]] || pencil[rc.X, rc.Y, vals[3]])
                                                {
                                                    pencil[rc.X, rc.Y, vals[0]] = false;
                                                    pencil[rc.X, rc.Y, vals[1]] = false;
                                                    pencil[rc.X, rc.Y, vals[2]] = false;
                                                    pencil[rc.X, rc.Y, vals[3]] = false;
                                                    changed = true;
                                                }
                                            }
                                        }
                                        if (changed) { changes++; }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// Eliminates pencil marks where possible based on the Omission technique.
        /// </summary>
        /// <returns>Number of changes made.</returns>
        public static int Omission(bool[,,] pencil)
        {
            /*
            Omission:
            If all pencil marks for one value in one house are contained in a different overlapping house,
            remove all pencil marks for that value in the rest of that other house.
            */

            int changes = 0;

            //check for rows with all of one value in the same block
            for (int i = 0; i < Form1.BASE; i++)
            {
                for (int k = 0; k < Form1.BASE; k++)
                {
                    int count = 0;
                    int found = -1;
                    //see how many blocks value k appears in on this row
                    for (int j = 0; j < Form1.BASE; j++)
                    {
                        if (pencil[i, j, k] && !(found == j / Form1.BASEROOT))
                        {
                            found = j / Form1.BASEROOT;
                            count++;
                        }
                    }
                    //if it's exactly one, remove k from any other cells in that block
                    if (count == 1)
                    {
                        bool changed = false;
                        int q = ((i / Form1.BASEROOT) * Form1.BASEROOT) + found;
                        for (int m = 0; m < Form1.BASE; m++)
                        {
                            Point cell = ConvertFromBlock(q, m);
                            if (!(cell.X == i) && pencil[cell.X, cell.Y, k])
                            {
                                pencil[cell.X, cell.Y, k] = false;
                                changed = true;
                            }
                        }
                        if (changed) { changes++; }
                    }
                }
            }
            //check for columns with all of one value in the same block
            for (int j = 0; j < Form1.BASE; j++)
            {
                for (int k = 0; k < Form1.BASE; k++)
                {
                    int count = 0;
                    int found = -1;
                    //see how many blocks value k appears in on this column
                    for (int i = 0; i < Form1.BASE; i++)
                    {
                        if (pencil[i, j, k] && !(found == i / Form1.BASEROOT))
                        {
                            found = i / Form1.BASEROOT;
                            count++;
                        }
                    }
                    //if it's exactly one, remove k from any other cells in that block
                    if (count == 1)
                    {
                        bool changed = false;
                        int q = (found * Form1.BASEROOT) + (j / Form1.BASEROOT);
                        for (int m = 0; m < Form1.BASE; m++)
                        {
                            Point cell = ConvertFromBlock(q, m);
                            if (!(cell.Y == j) && pencil[cell.X, cell.Y, k])
                            {
                                pencil[cell.X, cell.Y, k] = false;
                                changed = true;
                            }
                        }
                        if (changed) { changes++; }
                    }
                }
            }
            //check for blocks with all of one value in the same row or column
            for (int q = 0; q < Form1.BASE; q++)
            {
                for (int k = 0; k < Form1.BASE; k++)
                {
                    int countR = 0;
                    int countC = 0;
                    bool[] foundR = new bool[Form1.BASEROOT];
                    bool[] foundC = new bool[Form1.BASEROOT];
                    int startcol = Convert.ToInt32(Math.IEEERemainder(q, Form1.BASEROOT));
                    if (startcol < 0) { startcol += Form1.BASEROOT; }
                    startcol *= Form1.BASEROOT;
                    int startrow = ((q / Form1.BASEROOT) * Form1.BASEROOT);

                    //go through block q and keep track of in how many rows and how many columns k appears
                    for (int i = startrow; i < startrow + Form1.BASEROOT; i++)
                    {
                        for (int j = startcol; j < startcol + Form1.BASEROOT; j++)
                        {
                            if (pencil[i, j, k])
                            {
                                int translatedi = i - startrow;
                                int translatedj = j - startcol;
                                if (!foundR[translatedi])
                                {
                                    foundR[translatedi] = true;
                                    countR++;
                                }
                                if (!foundC[translatedj])
                                {
                                    foundC[translatedj] = true;
                                    countC++;
                                }
                            }
                        }
                    }
                    //if it's exactly one row or column, remove k from any other cells in that row or column
                    if (countR == 1 || countC == 1)
                    {
                        bool changed = false;
                        for (int f = 0; f < Form1.BASEROOT; f++)
                        {
                            if (foundR[f] && countR == 1)
                            {
                                int r = f + startrow;
                                for (int clear = 0; clear < Form1.BASE; clear++)
                                {
                                    if (!(clear < startcol + Form1.BASEROOT && clear >= startcol) && pencil[r, clear, k])
                                    {
                                        pencil[r, clear, k] = false;
                                        changed = true;
                                    }
                                }
                            }
                            if (foundC[f] && countC == 1)
                            {
                                int c = f + startcol;
                                for (int clear = 0; clear < Form1.BASE; clear++)
                                {
                                    if (!(clear < startrow + Form1.BASEROOT && clear >= startrow) && pencil[clear, c, k])
                                    {
                                        pencil[clear, c, k] = false;
                                        changed = true;
                                    }
                                }
                            }
                        }
                        if (changed) { changes++; }
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// Eliminates pencil marks where possible based on the Hidden Sets technique.
        /// </summary>
        /// <returns>Number of changes made.</returns>
        public static int HiddenSets(bool[,,] pencil)
        {
            int changes = 0;

            //Hidden sets of two values
            //rows
            for (int i = 0; i < Form1.BASE; i++)
            {
                for (int v1 = 0; v1 < Form1.BASE; v1++)
                {
                    for (int v2 = v1 + 1; v2 < Form1.BASE; v2++)
                    {
                        int count = 0;
                        bool found1 = false;
                        bool found2 = false;
                        int[] ind = new int[2];

                        //check each cell in row i for values v1 and v2
                        for (int j = 0; j < Form1.BASE; j++)
                        {
                            if (pencil[i, j, v1] || pencil[i, j, v2])
                            {
                                if (pencil[i, j, v1]) { found1 = true; }
                                if (pencil[i, j, v2]) { found2 = true; }
                                if (count < 2)
                                {
                                    ind[count] = j;
                                }
                                count++;
                            }
                        }
                        //if only two cells have either value, then remove other values from those cells
                        if (count == 2 && found1 && found2)
                        {
                            bool changed = false;
                            for (int e = 0; e < Form1.BASE; e++)
                            {
                                if (e != v1 && e != v2 && (pencil[i, ind[0], e] || pencil[i, ind[1], e]))
                                {
                                    pencil[i, ind[0], e] = false;
                                    pencil[i, ind[1], e] = false;
                                    changed = true;
                                }
                            }
                            if (changed) { changes++; }
                        }
                    }
                }
            }
            //cols
            for (int j = 0; j < Form1.BASE; j++)
            {
                for (int v1 = 0; v1 < Form1.BASE; v1++)
                {
                    for (int v2 = v1 + 1; v2 < Form1.BASE; v2++)
                    {
                        int count = 0;
                        bool found1 = false;
                        bool found2 = false;
                        int[] ind = new int[2];

                        //check each cell in column j for values v1 and v2
                        for (int i = 0; i < Form1.BASE; i++)
                        {
                            if (pencil[i, j, v1] || pencil[i, j, v2])
                            {
                                if (pencil[i, j, v1]) { found1 = true; }
                                if (pencil[i, j, v2]) { found2 = true; }
                                if (count < 2)
                                {
                                    ind[count] = i;
                                }
                                count++;
                            }
                        }
                        //if only two cells have either value, then remove other values from those cells
                        if (count == 2 && found1 && found2)
                        {
                            bool changed = false;
                            for (int e = 0; e < Form1.BASE; e++)
                            {
                                if (e != v1 && e != v2 && (pencil[ind[0], j, e] || pencil[ind[1], j, e]))
                                {
                                    pencil[ind[0], j, e] = false;
                                    pencil[ind[1], j, e] = false;
                                    changed = true;
                                }
                            }
                            if (changed) { changes++; }
                        }
                    }
                }
            }
            //blocks
            for (int q = 0; q < Form1.BASE; q++)
            {
                for (int v1 = 0; v1 < Form1.BASE; v1++)
                {
                    for (int v2 = v1 + 1; v2 < Form1.BASE; v2++)
                    {
                        int count = 0;
                        bool found1 = false;
                        bool found2 = false;
                        Point[] ind = new Point[2];

                        //check each cell in block q for values v1 and v2
                        for (int slot = 0; slot < Form1.BASE; slot++)
                        {
                            Point pt = ConvertFromBlock(q, slot);

                            if (pencil[pt.X, pt.Y, v1] || pencil[pt.X, pt.Y, v2])
                            {
                                if (pencil[pt.X, pt.Y, v1]) { found1 = true; }
                                if (pencil[pt.X, pt.Y, v2]) { found2 = true; }
                                if (count < 2)
                                {
                                    ind[count] = new Point(pt.X, pt.Y);
                                }
                                count++;
                            }
                        }
                        //if only two cells have either value, then remove other values from those cells
                        if (count == 2 && found1 && found2)
                        {
                            bool changed = false;
                            for (int e = 0; e < Form1.BASE; e++)
                            {
                                if (e != v1 && e != v2 && (pencil[ind[0].X, ind[0].Y, e] || pencil[ind[1].X, ind[1].Y, e]))
                                {
                                    pencil[ind[0].X, ind[0].Y, e] = false;
                                    pencil[ind[1].X, ind[1].Y, e] = false;
                                    changed = true;
                                }
                            }
                            if (changed) { changes++; }
                        }
                    }
                }
            }
            //Hidden sets of three values
            //rows
            for (int i = 0; i < Form1.BASE; i++)
            {
                for (int v1 = 0; v1 < Form1.BASE; v1++)
                {
                    for (int v2 = v1 + 1; v2 < Form1.BASE; v2++)
                    {
                        for (int v3 = v2 + 1; v3 < Form1.BASE; v3++)
                        {
                            int count = 0;
                            bool found1 = false;
                            bool found2 = false;
                            bool found3 = false;
                            int[] ind = new int[3];

                            //check each cell in row i for values v1, v2, and v3
                            for (int j = 0; j < Form1.BASE; j++)
                            {
                                if (pencil[i, j, v1] || pencil[i, j, v2] || pencil[i, j, v3])
                                {
                                    if (pencil[i, j, v1]) { found1 = true; }
                                    if (pencil[i, j, v2]) { found2 = true; }
                                    if (pencil[i, j, v3]) { found3 = true; }
                                    if (count < 3)
                                    {
                                        ind[count] = j;
                                    }
                                    count++;
                                }
                            }
                            //if only three cells have any of the values, then remove other values from those cells
                            if (count == 3 && found1 && found2 && found3)
                            {
                                bool changed = false;
                                for (int e = 0; e < Form1.BASE; e++)
                                {
                                    if (e != v1 && e != v2 && e != v3 && (pencil[i, ind[0], e] || pencil[i, ind[1], e] || pencil[i, ind[2], e]))
                                    {
                                        pencil[i, ind[0], e] = false;
                                        pencil[i, ind[1], e] = false;
                                        pencil[i, ind[2], e] = false;
                                        changed = true;
                                    }
                                }
                                if (changed) { changes++; }
                            }
                        }
                    }
                }
            }
            //cols
            for (int j = 0; j < Form1.BASE; j++)
            {
                for (int v1 = 0; v1 < Form1.BASE; v1++)
                {
                    for (int v2 = v1 + 1; v2 < Form1.BASE; v2++)
                    {
                        for (int v3 = v2 + 1; v3 < Form1.BASE; v3++)
                        {
                            int count = 0;
                            bool found1 = false;
                            bool found2 = false;
                            bool found3 = false;
                            int[] ind = new int[3];

                            //check each cell in column j for values v1, v2, and v3
                            for (int i = 0; i < Form1.BASE; i++)
                            {
                                if (pencil[i, j, v1] || pencil[i, j, v2] || pencil[i, j, v3])
                                {
                                    if (pencil[i, j, v1]) { found1 = true; }
                                    if (pencil[i, j, v2]) { found2 = true; }
                                    if (pencil[i, j, v3]) { found3 = true; }
                                    if (count < 3)
                                    {
                                        ind[count] = i;
                                    }
                                    count++;
                                }
                            }
                            //if only three cells have any of the values, then remove other values from those cells
                            if (count == 3 && found1 && found2 && found3)
                            {
                                bool changed = false;
                                for (int e = 0; e < Form1.BASE; e++)
                                {
                                    if (e != v1 && e != v2 && e != v3 && (pencil[ind[0], j, e] || pencil[ind[1], j, e] || pencil[ind[2], j, e]))
                                    {
                                        pencil[ind[0], j, e] = false;
                                        pencil[ind[1], j, e] = false;
                                        pencil[ind[2], j, e] = false;
                                        changed = true;
                                    }
                                }
                                if (changed) { changes++; }
                            }
                        }
                    }
                }
            }
            //blocks
            for (int q = 0; q < Form1.BASE; q++)
            {
                for (int v1 = 0; v1 < Form1.BASE; v1++)
                {
                    for (int v2 = v1 + 1; v2 < Form1.BASE; v2++)
                    {
                        for (int v3 = v2 + 1; v3 < Form1.BASE; v3++)
                        {
                            int count = 0;
                            bool found1 = false;
                            bool found2 = false;
                            bool found3 = false;
                            Point[] ind = new Point[3];

                            //check each cell in block q for values v1, v2, and v3
                            for (int slot = 0; slot < Form1.BASE; slot++)
                            {
                                Point pt = ConvertFromBlock(q, slot);

                                if (pencil[pt.X, pt.Y, v1] || pencil[pt.X, pt.Y, v2] || pencil[pt.X, pt.Y, v3])
                                {
                                    if (pencil[pt.X, pt.Y, v1]) { found1 = true; }
                                    if (pencil[pt.X, pt.Y, v2]) { found2 = true; }
                                    if (pencil[pt.X, pt.Y, v3]) { found3 = true; }
                                    if (count < 3)
                                    {
                                        ind[count] = new Point(pt.X, pt.Y);
                                    }
                                    count++;
                                }
                            }
                            //if only three cells have any of the values, then remove other values from those cells
                            if (count == 3 && found1 && found2 && found3)
                            {
                                bool changed = false;
                                for (int e = 0; e < Form1.BASE; e++)
                                {
                                    if (e != v1 && e != v2 && e != v3 && (pencil[ind[0].X, ind[0].Y, e] || pencil[ind[1].X, ind[1].Y, e] || pencil[ind[2].X, ind[2].Y, e]))
                                    {
                                        pencil[ind[0].X, ind[0].Y, e] = false;
                                        pencil[ind[1].X, ind[1].Y, e] = false;
                                        pencil[ind[2].X, ind[2].Y, e] = false;
                                        changed = true;
                                    }
                                }
                                if (changed) { changes++; }
                            }
                        }
                    }
                }
            }
            //4
            //rows
            for (int i = 0; i < Form1.BASE; i++)
            {
                for (int v1 = 0; v1 < Form1.BASE; v1++)
                {
                    for (int v2 = v1 + 1; v2 < Form1.BASE; v2++)
                    {
                        for (int v3 = v2 + 1; v3 < Form1.BASE; v3++)
                        {
                            for (int v4 = v3 + 1; v4 < Form1.BASE; v4++)
                            {
                                int count = 0;
                                bool found1 = false;
                                bool found2 = false;
                                bool found3 = false;
                                bool found4 = false;
                                int[] ind = new int[4];

                                //check each cell in row i for values v1, v2, v3, and v4
                                for (int j = 0; j < Form1.BASE; j++)
                                {
                                    if (pencil[i, j, v1] || pencil[i, j, v2] || pencil[i, j, v3] || pencil[i, j, v4])
                                    {
                                        if (pencil[i, j, v1]) { found1 = true; }
                                        if (pencil[i, j, v2]) { found2 = true; }
                                        if (pencil[i, j, v3]) { found3 = true; }
                                        if (pencil[i, j, v4]) { found4 = true; }
                                        if (count < 4)
                                        {
                                            ind[count] = j;
                                        }
                                        count++;
                                    }
                                }
                                //if only four cells have any of the values, then remove other values from those cells
                                if (count == 4 && found1 && found2 && found3 && found4)
                                {
                                    bool changed = false;
                                    for (int e = 0; e < Form1.BASE; e++)
                                    {
                                        if (e != v1 && e != v2 && e != v3 && e != v4 && (pencil[i, ind[0], e] || pencil[i, ind[1], e] || pencil[i, ind[2], e] || pencil[i, ind[3], e]))
                                        {
                                            pencil[i, ind[0], e] = false;
                                            pencil[i, ind[1], e] = false;
                                            pencil[i, ind[2], e] = false;
                                            pencil[i, ind[3], e] = false;
                                            changed = true;
                                        }
                                    }
                                    if (changed) { changes++; }
                                }
                            }
                        }
                    }
                }
            }
            //cols
            for (int j = 0; j < Form1.BASE; j++)
            {
                for (int v1 = 0; v1 < Form1.BASE; v1++)
                {
                    for (int v2 = v1 + 1; v2 < Form1.BASE; v2++)
                    {
                        for (int v3 = v2 + 1; v3 < Form1.BASE; v3++)
                        {
                            for (int v4 = v3 + 1; v4 < Form1.BASE; v4++)
                            {
                                int count = 0;
                                bool found1 = false;
                                bool found2 = false;
                                bool found3 = false;
                                bool found4 = false;
                                int[] ind = new int[4];

                                //check each cell in column j for values v1, v2, v3, and v4
                                for (int i = 0; i < Form1.BASE; i++)
                                {
                                    if (pencil[i, j, v1] || pencil[i, j, v2] || pencil[i, j, v3] || pencil[i, j, v4])
                                    {
                                        if (pencil[i, j, v1]) { found1 = true; }
                                        if (pencil[i, j, v2]) { found2 = true; }
                                        if (pencil[i, j, v3]) { found3 = true; }
                                        if (pencil[i, j, v4]) { found4 = true; }
                                        if (count < 4)
                                        {
                                            ind[count] = i;
                                        }
                                        count++;
                                    }
                                }
                                //if only four cells have any of the values, then remove other values from those cells
                                if (count == 4 && found1 && found2 && found3 && found4)
                                {
                                    bool changed = false;
                                    for (int e = 0; e < Form1.BASE; e++)
                                    {
                                        if (e != v1 && e != v2 && e != v3 && e != v4 && (pencil[ind[0], j, e] || pencil[ind[1], j, e] || pencil[ind[2], j, e] || pencil[ind[3], j, e]))
                                        {
                                            pencil[ind[0], j, e] = false;
                                            pencil[ind[1], j, e] = false;
                                            pencil[ind[2], j, e] = false;
                                            pencil[ind[3], j, e] = false;
                                            changed = true;
                                        }
                                    }
                                    if (changed) { changes++; }
                                }
                            }
                        }
                    }
                }
            }
            //blocks
            for (int q = 0; q < Form1.BASE; q++)
            {
                for (int v1 = 0; v1 < Form1.BASE; v1++)
                {
                    for (int v2 = v1 + 1; v2 < Form1.BASE; v2++)
                    {
                        for (int v3 = v2 + 1; v3 < Form1.BASE; v3++)
                        {
                            for (int v4 = v3 + 1; v4 < Form1.BASE; v4++)
                            {
                                int count = 0;
                                bool found1 = false;
                                bool found2 = false;
                                bool found3 = false;
                                bool found4 = false;
                                Point[] ind = new Point[4];

                                //check each cell in block q for values v1, v2, v3, and v4
                                for (int slot = 0; slot < Form1.BASE; slot++)
                                {
                                    Point pt = ConvertFromBlock(q, slot);

                                    if (pencil[pt.X, pt.Y, v1] || pencil[pt.X, pt.Y, v2] || pencil[pt.X, pt.Y, v3] || pencil[pt.X, pt.Y, v4])
                                    {
                                        if (pencil[pt.X, pt.Y, v1]) { found1 = true; }
                                        if (pencil[pt.X, pt.Y, v2]) { found2 = true; }
                                        if (pencil[pt.X, pt.Y, v3]) { found3 = true; }
                                        if (pencil[pt.X, pt.Y, v4]) { found4 = true; }
                                        if (count < 4)
                                        {
                                            ind[count] = new Point(pt.X, pt.Y);
                                        }
                                        count++;
                                    }
                                }
                                //if only four cells have any of the values, then remove other values from those cells
                                if (count == 4 && found1 && found2 && found3 && found4)
                                {
                                    bool changed = false;
                                    for (int e = 0; e < Form1.BASE; e++)
                                    {
                                        if (e != v1 && e != v2 && e != v3 && e != v4 && (pencil[ind[0].X, ind[0].Y, e] || pencil[ind[1].X, ind[1].Y, e] || pencil[ind[2].X, ind[2].Y, e] || pencil[ind[3].X, ind[3].Y, e]))
                                        {
                                            pencil[ind[0].X, ind[0].Y, e] = false;
                                            pencil[ind[1].X, ind[1].Y, e] = false;
                                            pencil[ind[2].X, ind[2].Y, e] = false;
                                            pencil[ind[3].X, ind[3].Y, e] = false;
                                            changed = true;
                                        }
                                    }
                                    if (changed) { changes++; }
                                }
                            }
                        }
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// Eliminates pencil marks where possible based on the X-Wing technique.
        /// </summary>
        /// <returns>Number of changes made.</returns>
        public static int XWing(bool[,,] pencil)
        {
            int changes = 0;

            //go through rows
            for (int i = 0; i < Form1.BASE - 1; i++)
            {
                for (int k = 0; k < Form1.BASE; k++)
                {
                    int count = 0;
                    int[] ind = new int[2];

                    //check if row has exactly two instances of value k
                    for (int j = 0; j < Form1.BASE; j++)
                    {
                        if (pencil[i, j, k])
                        {
                            if (count < 2)
                            {
                                ind[count] = j;
                            }
                            count++;
                        }
                    }

                    if (count == 2)
                    {
                        //check if another row also has value k in only the same columns
                        for (int i2 = i + 1; i2 < Form1.BASE; i2++)
                        {
                            bool failed = !(pencil[i2, ind[0], k] && pencil[i2, ind[1], k]);
                            if (!failed)
                            {
                                for (int m = 0; m < Form1.BASE; m++)
                                {
                                    if (m != ind[0] && m != ind[1] && pencil[i2, m, k])
                                    {
                                        failed = true;
                                        break;
                                    }
                                }
                            }
                            if (!failed)
                            {
                                //remove any other instances of value k in those columns
                                bool changed = false;
                                for (int m = 0; m < Form1.BASE; m++)
                                {
                                    if (m != i && m != i2)
                                    {
                                        if (pencil[m, ind[0], k])
                                        {
                                            pencil[m, ind[0], k] = false;
                                            changed = true;
                                        }
                                        if (pencil[m, ind[1], k])
                                        {
                                            pencil[m, ind[1], k] = false;
                                            changed = true;
                                        }
                                    }
                                }
                                if (changed) { changes++; }
                            }
                        }
                    }
                }
            }
            //go through columns
            for (int j = 0; j < Form1.BASE - 1; j++)
            {
                for (int k = 0; k < Form1.BASE; k++)
                {
                    int count = 0;
                    int[] ind = new int[2];

                    //check if column has exactly two instances of value k
                    for (int i = 0; i < Form1.BASE; i++)
                    {
                        if (pencil[i, j, k])
                        {
                            if (count < 2)
                            {
                                ind[count] = i;
                            }
                            count++;
                        }
                    }

                    if (count == 2)
                    {
                        //check if another column also has value k in only the same rows
                        for (int j2 = j + 1; j2 < Form1.BASE; j2++)
                        {
                            bool failed = !(pencil[ind[0], j2, k] && pencil[ind[1], j2, k]);
                            if (!failed)
                            {
                                for (int m = 0; m < Form1.BASE; m++)
                                {
                                    if (m != ind[0] && m != ind[1] && pencil[m, j2, k])
                                    {
                                        failed = true;
                                        break;
                                    }
                                }
                            }
                            if (!failed)
                            {
                                //remove any other instances of value k in those rows
                                bool changed = false;
                                for (int m = 0; m < Form1.BASE; m++)
                                {
                                    if (m != j && m != j2)
                                    {
                                        if (pencil[ind[0], m, k])
                                        {
                                            pencil[ind[0], m, k] = false;
                                            changed = true;
                                        }
                                        if (pencil[ind[1], m, k])
                                        {
                                            pencil[ind[1], m, k] = false;
                                            changed = true;
                                        }
                                    }
                                }
                                if (changed) { changes++; }
                            }
                        }
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// Eliminates pencil marks where possible based on the Swordfish technique.
        /// </summary>
        /// <returns>Number of changes made.</returns>
        public static int Swordfish(bool[,,] pencil)
        {
            int changes = 0;

            //go through rows
            for (int i = 0; i < Form1.BASE - 1; i++)
            {
                for (int k = 0; k < Form1.BASE; k++)
                {
                    int count = 0;
                    int[] ind = new int[3];

                    //check if row has exactly three instances of value k
                    for (int j = 0; j < Form1.BASE; j++)
                    {
                        if (pencil[i, j, k])
                        {
                            if (count < 3)
                            {
                                ind[count] = j;
                            }
                            count++;
                        }
                    }

                    if (count == 3)
                    {
                        //check if another two rows also have value k only in the same columns
                        for (int i3 = i + 1; i3 < Form1.BASE; i3++)
                        {
                            for (int i2 = i3 + 1; i2 < Form1.BASE; i2++)
                            {
                                bool failed = !((pencil[i2, ind[0], k] || pencil[i2, ind[1], k] || pencil[i2, ind[2], k]) && (pencil[i3, ind[0], k] || pencil[i3, ind[1], k] || pencil[i3, ind[2], k]));
                                if (!failed)
                                {
                                    for (int m = 0; m < Form1.BASE; m++)
                                    {
                                        if (m != ind[0] && m != ind[1] && m != ind[2] && (pencil[i2, m, k] || pencil[i3, m, k]))
                                        {
                                            failed = true;
                                            break;
                                        }
                                    }
                                }
                                if (!failed)
                                {
                                    //remove any other instances of value k in those columns
                                    bool changed = false;
                                    for (int m = 0; m < Form1.BASE; m++)
                                    {
                                        if (m != i && m != i2 && m != i3)
                                        {
                                            for (int var = 0; var < 3; var++)
                                            {
                                                if (pencil[m, ind[var], k])
                                                {
                                                    pencil[m, ind[var], k] = false;
                                                    changed = true;
                                                }
                                            }
                                        }
                                    }
                                    if (changed) { changes++; }
                                }
                            }
                        }
                    }
                }
            }
            //go through columns
            for (int j = 0; j < Form1.BASE - 1; j++)
            {
                for (int k = 0; k < Form1.BASE; k++)
                {
                    int count = 0;
                    int[] ind = new int[3];

                    //check if column has exactly three instances of value k
                    for (int i = 0; i < Form1.BASE; i++)
                    {
                        if (pencil[i, j, k])
                        {
                            if (count < 3)
                            {
                                ind[count] = i;
                            }
                            count++;
                        }
                    }

                    if (count == 3)
                    {
                        //check if another two columns also have value k only in the same rows
                        for (int j3 = j + 1; j3 < Form1.BASE; j3++)
                        {
                            for (int j2 = j3 + 1; j2 < Form1.BASE; j2++)
                            {
                                bool failed = !((pencil[ind[0], j2, k] || pencil[ind[1], j2, k] || pencil[ind[2], j2, k]) && (pencil[ind[0], j3, k] || pencil[ind[1], j3, k] || pencil[ind[2], j3, k]));
                                if (!failed)
                                {
                                    for (int m = 0; m < Form1.BASE; m++)
                                    {
                                        if (m != ind[0] && m != ind[1] && m != ind[2] && (pencil[m, j2, k] || pencil[m, j3, k]))
                                        {
                                            failed = true;
                                            break;
                                        }
                                    }
                                }
                                if (!failed)
                                {
                                    //remove any other instances of value k in those rows
                                    bool changed = false;
                                    for (int m = 0; m < Form1.BASE; m++)
                                    {
                                        if (m != j && m != j2 && m != j3)
                                        {
                                            for (int var = 0; var < 3; var++)
                                            {
                                                if (pencil[ind[var], m, k])
                                                {
                                                    pencil[ind[var], m, k] = false;
                                                    changed = true;
                                                }
                                            }
                                        }
                                    }
                                    if (changed) { changes++; }
                                }
                            }
                        }
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// Eliminates pencil marks where possible based on the Unique Rectangle technique.
        /// </summary>
        /// <returns>Number of changes made.</returns>
        public static int UniqueRectangle(bool[,,] pencil)
        {
            /*
            Unique Rectangle:
            When four cells fall into exactly two rows, two columns, and two blocks, while three of them have the same two pencil marks,
            you can eliminate both of those pencil marks from the fourth cell.
            */

            int changes = 0;

            //horizontal rectangles
            for (int startrow = 0; startrow < Form1.BASE; startrow += Form1.BASEROOT)
            {
                //check each valid rectangle in the same vertical third of the grid
                for (int j1 = 0; j1 < Form1.BASE - Form1.BASEROOT; j1++)
                {
                    int startsecondcol = Convert.ToInt32(Math.IEEERemainder(j1, Form1.BASEROOT));
                    if (startsecondcol < 0) { startsecondcol += Form1.BASEROOT; }
                    startsecondcol = j1 - startsecondcol + Form1.BASEROOT;

                    for (int j2 = startsecondcol; j2 < Form1.BASE; j2++)
                    {
                        for (int i1 = startrow; i1 < startrow + Form1.BASEROOT - 1; i1++)
                        {
                            for (int i2 = i1 + 1; i2 < startrow + Form1.BASEROOT; i2++)
                            {
                                Point[] corners = new Point[4];
                                corners[0] = new Point(i1, j1);
                                corners[1] = new Point(i2, j1);
                                corners[2] = new Point(i1, j2);
                                corners[3] = new Point(i2, j2);

                                //check for pairs of values found in three of the corners
                                for (int k1 = 0; k1 < Form1.BASE - 1; k1++)
                                {
                                    for (int k2 = k1 + 1; k2 < Form1.BASE; k2++)
                                    {
                                        int count = 0;
                                        int changem = -1; //will set index of corner that has extra pencil marks
                                        for (int m = 0; m < 4; m++)
                                        {
                                            bool failed = false;
                                            for (int check = 0; check < Form1.BASE; check++)
                                            {
                                                if (((check != k1 && check != k2) && pencil[corners[m].X, corners[m].Y, check]) || ((check == k1 || check == k2) && !pencil[corners[m].X, corners[m].Y, check]))
                                                {
                                                    failed = true;
                                                    break;
                                                }
                                            }
                                            if (!failed) { count++; }
                                            else { changem = m; }
                                        }
                                        if (count == 3)
                                        {
                                            //remove the two values from the corner that had extra pencil marks
                                            if (pencil[corners[changem].X, corners[changem].Y, k1] || pencil[corners[changem].X, corners[changem].Y, k2])
                                            {
                                                pencil[corners[changem].X, corners[changem].Y, k1] = false;
                                                pencil[corners[changem].X, corners[changem].Y, k2] = false;
                                                changes++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //vertical rectangles
            for (int startcol = 0; startcol < Form1.BASE; startcol += Form1.BASEROOT)
            {
                //check each valid rectangle in the same horizontal third of the grid
                for (int i1 = 0; i1 < Form1.BASE - Form1.BASEROOT; i1++)
                {
                    int startsecondrow = Convert.ToInt32(Math.IEEERemainder(i1, Form1.BASEROOT));
                    if (startsecondrow < 0) { startsecondrow += Form1.BASEROOT; }
                    startsecondrow = i1 - startsecondrow + Form1.BASEROOT;
                    for (int i2 = startsecondrow; i2 < Form1.BASE; i2++)
                    {
                        for (int j1 = startcol; j1 < startcol + Form1.BASEROOT - 1; j1++)
                        {
                            for (int j2 = j1 + 1; j2 < startcol + Form1.BASEROOT; j2++)
                            {
                                Point[] corners = new Point[4];
                                corners[0] = new Point(i1, j1);
                                corners[1] = new Point(i2, j1);
                                corners[2] = new Point(i1, j2);
                                corners[3] = new Point(i2, j2);

                                //check for pairs of values found in three of the corners
                                for (int k1 = 0; k1 < Form1.BASE - 1; k1++)
                                {
                                    for (int k2 = k1 + 1; k2 < Form1.BASE; k2++)
                                    {
                                        int count = 0;
                                        int changem = -1; //will set index of corner that has extra pencil marks
                                        for (int m = 0; m < 4; m++)
                                        {
                                            bool failed = false;
                                            for (int check = 0; check < Form1.BASE; check++)
                                            {
                                                if (((check != k1 && check != k2) && pencil[corners[m].X, corners[m].Y, check]) || ((check == k1 || check == k2) && !pencil[corners[m].X, corners[m].Y, check]))
                                                {
                                                    failed = true;
                                                    break;
                                                }
                                            }
                                            if (!failed) { count++; }
                                            else { changem = m; }
                                        }
                                        if (count == 3)
                                        {
                                            //remove the two values from the corner that had extra pencil marks
                                            if (pencil[corners[changem].X, corners[changem].Y, k1] || pencil[corners[changem].X, corners[changem].Y, k2])
                                            {
                                                pencil[corners[changem].X, corners[changem].Y, k1] = false;
                                                pencil[corners[changem].X, corners[changem].Y, k2] = false;
                                                changes++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// Eliminates pencil marks where possible Form1.BASEd on the Y-Wing technique.
        /// </summary>
        /// <returns>Number of changes made.</returns>
        public static int YWing(bool[,,] pencil)
        {
            int changes = 0;

            //look for cells with two values
            for (int i1 = 0; i1 < Form1.BASE; i1++)
            {
                for (int j1 = 0; j1 < Form1.BASE; j1++)
                {
                    int count1 = 0;
                    int[] vals1 = new int[2];
                    for (int b = 0; b < Form1.BASE; b++)
                    {
                        if (pencil[i1, j1, b])
                        {
                            if (count1 < 2)
                            {
                                vals1[count1] = b;
                            }
                            count1++;
                        }
                    }
                    if (count1 == 2)
                    {
                        //look for second spot intersecting first with two values, one shared
                        for (int i2 = 0; i2 < Form1.BASE; i2++)
                        {
                            for (int j2 = 0; j2 < Form1.BASE; j2++)
                            {
                                if ((j2 != j1 || i2 != i1) && Intersects(i1, j1, i2, j2))
                                {
                                    int count2 = 0;
                                    int[] vals2 = new int[2];
                                    for (int b = 0; b < Form1.BASE; b++)
                                    {
                                        if (pencil[i2, j2, b])
                                        {
                                            if (count2 < 2)
                                            {
                                                vals2[count2] = b;
                                            }
                                            count2++;
                                        }
                                    }
                                    if (count2 == 2)
                                    {
                                        if (vals1[0] == vals2[1] || vals1[1] == vals2[0])
                                        {
                                            int store1 = vals1[1];
                                            vals1[1] = vals1[0];
                                            vals1[0] = store1;
                                        }
                                        if (vals1[1] == vals2[1])
                                        {
                                            int store1 = vals1[1];
                                            vals1[1] = vals1[0];
                                            vals1[0] = store1;
                                            int store2 = vals2[1];
                                            vals2[1] = vals2[0];
                                            vals2[0] = store2;
                                        }
                                        if (vals1[0] == vals2[0] && vals1[1] != vals2[1])
                                        {
                                            //look for third spot sharing each of the unshared values between the first two, and intersecting both
                                            for (int i3 = 0; i3 < Form1.BASE; i3++)
                                            {
                                                for (int j3 = 0; j3 < Form1.BASE; j3++)
                                                {
                                                    if (!Intersects(i1, j1, i3, j3) && Intersects(i2, j2, i3, j3))
                                                    {
                                                        bool fits = (pencil[i3, j3, vals1[1]] && pencil[i3, j3, vals2[1]]);
                                                        if (fits)
                                                        {
                                                            for (int mark = 0; mark < Form1.BASE; mark++)
                                                            {
                                                                if (mark != vals1[1] && mark != vals2[1] && pencil[i3, j3, mark])
                                                                {
                                                                    fits = false;
                                                                    break;
                                                                }
                                                            }
                                                        }
                                                        if (fits)
                                                        {
                                                            //remove value shared by first and third cell from all cells that intersect both
                                                            bool changed = false;
                                                            for (int clearX = 0; clearX < Form1.BASE; clearX++)
                                                            {
                                                                for (int clearY = 0; clearY < Form1.BASE; clearY++)
                                                                {
                                                                    if (Intersects(i1, j1, clearX, clearY) && Intersects(i3, j3, clearX, clearY))
                                                                    {
                                                                        if (pencil[clearX, clearY, vals1[1]])
                                                                        {
                                                                            pencil[clearX, clearY, vals1[1]] = false;
                                                                            changed = true;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                            if (changed) { changes++; }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// Eliminates pencil marks where possible based on the Finned X-Wing technique.
        /// </summary>
        /// <returns>Number of changes made.</returns>
        public static int FinnedXWing(bool[,,] pencil)
        {
            int changes = 0;

            //rows
            for (int k = 0; k < Form1.BASE; k++)
            {
                //find rows with two instances of value k, store the columns where those occur
                for (int i1 = 0; i1 < Form1.BASE - 1; i1++)
                {
                    int count = 0;
                    int[] j1 = new int[2];
                    for (int j = 0; j < Form1.BASE; j++)
                    {
                        if (pencil[i1, j, k])
                        {
                            if (count < 2)
                            {
                                j1[count] = j;
                            }
                            count++;
                        }
                    }
                    if (count == 2)
                    {
                        //find another row with two instances, store those columns as well
                        for (int i2 = i1 + 1; i2 < Form1.BASE; i2++)
                        {
                            int count2 = 0;
                            int[] j2 = new int[2];
                            for (int j = 0; j < Form1.BASE; j++)
                            {
                                if (pencil[i2, j, k])
                                {
                                    if (count2 < 2)
                                    {
                                        j2[count2] = j;
                                    }
                                    count2++;
                                }
                            }
                            if (count2 == 2)
                            {
                                //if two of the cells on different rows intersect, find cells that intersect both of the other two, and remove k from them
                                bool changed = false;
                                for (int cleanX = 0; cleanX < Form1.BASE; cleanX++)
                                {
                                    for (int cleanY = 0; cleanY < Form1.BASE; cleanY++)
                                    {
                                        for (int a = 0; a < 2; a++)
                                        {
                                            for (int b = 0; b < 2; b++)
                                            {
                                                if (Intersects(i1, j1[a], i2, j2[b]) && Intersects(cleanX, cleanY, i1, j1[1 - a]) && Intersects(cleanX, cleanY, i2, j2[1 - b]))
                                                {
                                                    if (pencil[cleanX, cleanY, k])
                                                    {
                                                        pencil[cleanX, cleanY, k] = false;
                                                        changed = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (changed) { changes++; break; }
                            }
                        }
                    }
                }
            }

            //cols
            for (int k = 0; k < Form1.BASE; k++)
            {
                //find columns with two instances of value k, store the rows where those occur
                for (int j1 = 0; j1 < Form1.BASE - 1; j1++)
                {
                    int count = 0;
                    int[] i1 = new int[2];
                    for (int i = 0; i < Form1.BASE; i++)
                    {
                        if (pencil[i, j1, k])
                        {
                            if (count < 2)
                            {
                                i1[count] = i;
                            }
                            count++;
                        }
                    }
                    if (count == 2)
                    {
                        //find another column with two instances, store those rows as well
                        for (int j2 = j1 + 1; j2 < Form1.BASE; j2++)
                        {
                            int count2 = 0;
                            int[] i2 = new int[2];
                            for (int i = 0; i < Form1.BASE; i++)
                            {
                                if (pencil[i, j2, k])
                                {
                                    if (count2 < 2)
                                    {
                                        i2[count2] = i;
                                    }
                                    count2++;
                                }
                            }
                            if (count2 == 2)
                            {
                                //if two of the cells on different columns intersect, find cells that intersect both of the other two, and remove k from them
                                bool changed = false;
                                for (int cleanX = 0; cleanX < Form1.BASE; cleanX++)
                                {
                                    for (int cleanY = 0; cleanY < Form1.BASE; cleanY++)
                                    {
                                        for (int a = 0; a < 2; a++)
                                        {
                                            for (int b = 0; b < 2; b++)
                                            {
                                                if (Intersects(i1[a], j1, i2[b], j2) && Intersects(cleanX, cleanY, i1[1 - a], j1) && Intersects(cleanX, cleanY, i2[1 - b], j2))
                                                {
                                                    if (pencil[cleanX, cleanY, k])
                                                    {
                                                        pencil[cleanX, cleanY, k] = false;
                                                        changed = true;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                if (changed) { changes++; break; }
                            }
                        }
                    }
                }
            }

            return changes;
        }

        /// <summary>
        /// Erases all pencil marks from a cell.
        /// </summary>
        public static void Erase(bool[,,] pencil, int row, int col)
        {
            for (int i = 0; i < Form1.BASE; i++)
            {
                pencil[row, col, i] = false;
            }
        }

        /// <summary>
        /// Gives the overall grid coordinates for a cell provided with which block it is in, and which cell within that block.
        /// </summary>
        /// <param name="block">Which block, from left to right and top to bottom.</param>
        /// <param name="index">Which cell within the block, left to right and top to bottom.</param>
        /// <returns>Point with x and y values corresponding to the cell relative to the overall grid.</returns>
        public static Point ConvertFromBlock(int block, int index)
        {
            int x = ((block / Form1.BASEROOT) * Form1.BASEROOT) + (index / Form1.BASEROOT);
            int y = Convert.ToInt32(block % Form1.BASEROOT);
            if (y < 0) { y += Form1.BASEROOT; }
            y *= Form1.BASEROOT;
            int smalladd = Convert.ToInt32(index % Form1.BASEROOT);
            if (smalladd < 0) { smalladd += Form1.BASEROOT; }
            y += smalladd;

            return new Point(x, y);
        }

        /// <summary>
        /// Do cells x1,y1 and x2,y2 share a row, column, or block?
        /// </summary>
        private static bool Intersects(int x1, int y1, int x2, int y2)
        {
            if (x1 == x2 || y1 == y2)
            {
                return true;
            }
            return ((x1 / Form1.BASEROOT == x2 / Form1.BASEROOT) && (y1 / Form1.BASEROOT == y2 / Form1.BASEROOT));
        }
    }
}
