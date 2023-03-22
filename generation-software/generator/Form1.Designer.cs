namespace generator
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.CreatePuzzleButton = new System.Windows.Forms.Button();
            this.TextOutput = new System.Windows.Forms.Label();
            this.SolveButton = new System.Windows.Forms.Button();
            this.SizeInput = new System.Windows.Forms.ComboBox();
            this.HiddenSinglesInput = new System.Windows.Forms.CheckBox();
            this.NakedSetsInput = new System.Windows.Forms.CheckBox();
            this.HiddenSetsInput = new System.Windows.Forms.CheckBox();
            this.SymmetryInput = new System.Windows.Forms.CheckBox();
            this.XWingInput = new System.Windows.Forms.CheckBox();
            this.SwordfishInput = new System.Windows.Forms.CheckBox();
            this.YWingInput = new System.Windows.Forms.CheckBox();
            this.UniqueRectangleInput = new System.Windows.Forms.CheckBox();
            this.FinnedXWingInput = new System.Windows.Forms.CheckBox();
            this.OmissionInput = new System.Windows.Forms.CheckBox();
            this.InputBox = new System.Windows.Forms.GroupBox();
            this.ClearButton = new System.Windows.Forms.Button();
            this.CreateMultiple = new System.Windows.Forms.Button();
            this.MultipleInput = new System.Windows.Forms.TextBox();
            this.ImportInput = new System.Windows.Forms.TextBox();
            this.SolveOneButton = new System.Windows.Forms.Button();
            this.InputBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // CreatePuzzleButton
            // 
            this.CreatePuzzleButton.Location = new System.Drawing.Point(942, 57);
            this.CreatePuzzleButton.Name = "CreatePuzzleButton";
            this.CreatePuzzleButton.Size = new System.Drawing.Size(122, 69);
            this.CreatePuzzleButton.TabIndex = 1;
            this.CreatePuzzleButton.Text = "Create Puzzle";
            this.CreatePuzzleButton.UseVisualStyleBackColor = true;
            this.CreatePuzzleButton.Click += new System.EventHandler(this.CreatePuzzleButton_Click);
            // 
            // TextOutput
            // 
            this.TextOutput.Location = new System.Drawing.Point(942, 194);
            this.TextOutput.Name = "TextOutput";
            this.TextOutput.Size = new System.Drawing.Size(122, 150);
            this.TextOutput.TabIndex = 3;
            this.TextOutput.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // SolveButton
            // 
            this.SolveButton.Location = new System.Drawing.Point(942, 132);
            this.SolveButton.Name = "SolveButton";
            this.SolveButton.Size = new System.Drawing.Size(58, 55);
            this.SolveButton.TabIndex = 9;
            this.SolveButton.Text = "Solve";
            this.SolveButton.UseVisualStyleBackColor = true;
            this.SolveButton.Click += new System.EventHandler(this.SolveButton_Click);
            // 
            // SizeInput
            // 
            this.SizeInput.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SizeInput.FormattingEnabled = true;
            this.SizeInput.Items.AddRange(new object[] {
            "4 x 4",
            "9 x 9",
            "16 x 16"});
            this.SizeInput.Location = new System.Drawing.Point(6, 51);
            this.SizeInput.MaxDropDownItems = 3;
            this.SizeInput.Name = "SizeInput";
            this.SizeInput.Size = new System.Drawing.Size(128, 21);
            this.SizeInput.TabIndex = 13;
            // 
            // HiddenSinglesInput
            // 
            this.HiddenSinglesInput.AutoSize = true;
            this.HiddenSinglesInput.Checked = true;
            this.HiddenSinglesInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.HiddenSinglesInput.Location = new System.Drawing.Point(6, 90);
            this.HiddenSinglesInput.Name = "HiddenSinglesInput";
            this.HiddenSinglesInput.Size = new System.Drawing.Size(97, 17);
            this.HiddenSinglesInput.TabIndex = 14;
            this.HiddenSinglesInput.Text = "Hidden Singles";
            this.HiddenSinglesInput.UseVisualStyleBackColor = true;
            // 
            // NakedSetsInput
            // 
            this.NakedSetsInput.AutoSize = true;
            this.NakedSetsInput.Checked = true;
            this.NakedSetsInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.NakedSetsInput.Location = new System.Drawing.Point(6, 113);
            this.NakedSetsInput.Name = "NakedSetsInput";
            this.NakedSetsInput.Size = new System.Drawing.Size(82, 17);
            this.NakedSetsInput.TabIndex = 15;
            this.NakedSetsInput.Text = "Naked Sets";
            this.NakedSetsInput.UseVisualStyleBackColor = true;
            // 
            // HiddenSetsInput
            // 
            this.HiddenSetsInput.AutoSize = true;
            this.HiddenSetsInput.Location = new System.Drawing.Point(6, 159);
            this.HiddenSetsInput.Name = "HiddenSetsInput";
            this.HiddenSetsInput.Size = new System.Drawing.Size(84, 17);
            this.HiddenSetsInput.TabIndex = 16;
            this.HiddenSetsInput.Text = "Hidden Sets";
            this.HiddenSetsInput.UseVisualStyleBackColor = true;
            // 
            // SymmetryInput
            // 
            this.SymmetryInput.AutoSize = true;
            this.SymmetryInput.Checked = true;
            this.SymmetryInput.CheckState = System.Windows.Forms.CheckState.Checked;
            this.SymmetryInput.Location = new System.Drawing.Point(6, 19);
            this.SymmetryInput.Name = "SymmetryInput";
            this.SymmetryInput.Size = new System.Drawing.Size(71, 17);
            this.SymmetryInput.TabIndex = 17;
            this.SymmetryInput.Text = "Symmetry";
            this.SymmetryInput.UseVisualStyleBackColor = true;
            // 
            // XWingInput
            // 
            this.XWingInput.AutoSize = true;
            this.XWingInput.Location = new System.Drawing.Point(6, 182);
            this.XWingInput.Name = "XWingInput";
            this.XWingInput.Size = new System.Drawing.Size(61, 17);
            this.XWingInput.TabIndex = 18;
            this.XWingInput.Text = "X-Wing";
            this.XWingInput.UseVisualStyleBackColor = true;
            // 
            // SwordfishInput
            // 
            this.SwordfishInput.AutoSize = true;
            this.SwordfishInput.Location = new System.Drawing.Point(6, 205);
            this.SwordfishInput.Name = "SwordfishInput";
            this.SwordfishInput.Size = new System.Drawing.Size(72, 17);
            this.SwordfishInput.TabIndex = 19;
            this.SwordfishInput.Text = "Swordfish";
            this.SwordfishInput.UseVisualStyleBackColor = true;
            // 
            // YWingInput
            // 
            this.YWingInput.AutoSize = true;
            this.YWingInput.Location = new System.Drawing.Point(6, 251);
            this.YWingInput.Name = "YWingInput";
            this.YWingInput.Size = new System.Drawing.Size(61, 17);
            this.YWingInput.TabIndex = 20;
            this.YWingInput.Text = "Y-Wing";
            this.YWingInput.UseVisualStyleBackColor = true;
            // 
            // UniqueRectangleInput
            // 
            this.UniqueRectangleInput.AutoSize = true;
            this.UniqueRectangleInput.Location = new System.Drawing.Point(6, 228);
            this.UniqueRectangleInput.Name = "UniqueRectangleInput";
            this.UniqueRectangleInput.Size = new System.Drawing.Size(112, 17);
            this.UniqueRectangleInput.TabIndex = 21;
            this.UniqueRectangleInput.Text = "Unique Rectangle";
            this.UniqueRectangleInput.UseVisualStyleBackColor = true;
            // 
            // FinnedXWingInput
            // 
            this.FinnedXWingInput.AutoSize = true;
            this.FinnedXWingInput.Location = new System.Drawing.Point(6, 274);
            this.FinnedXWingInput.Name = "FinnedXWingInput";
            this.FinnedXWingInput.Size = new System.Drawing.Size(96, 17);
            this.FinnedXWingInput.TabIndex = 22;
            this.FinnedXWingInput.Text = "Finned X-Wing";
            this.FinnedXWingInput.UseVisualStyleBackColor = true;
            // 
            // OmissionInput
            // 
            this.OmissionInput.AutoSize = true;
            this.OmissionInput.Location = new System.Drawing.Point(6, 136);
            this.OmissionInput.Name = "OmissionInput";
            this.OmissionInput.Size = new System.Drawing.Size(68, 17);
            this.OmissionInput.TabIndex = 23;
            this.OmissionInput.Text = "Omission";
            this.OmissionInput.UseVisualStyleBackColor = true;
            // 
            // InputBox
            // 
            this.InputBox.Controls.Add(this.SymmetryInput);
            this.InputBox.Controls.Add(this.OmissionInput);
            this.InputBox.Controls.Add(this.SizeInput);
            this.InputBox.Controls.Add(this.FinnedXWingInput);
            this.InputBox.Controls.Add(this.HiddenSinglesInput);
            this.InputBox.Controls.Add(this.UniqueRectangleInput);
            this.InputBox.Controls.Add(this.NakedSetsInput);
            this.InputBox.Controls.Add(this.YWingInput);
            this.InputBox.Controls.Add(this.HiddenSetsInput);
            this.InputBox.Controls.Add(this.SwordfishInput);
            this.InputBox.Controls.Add(this.XWingInput);
            this.InputBox.Location = new System.Drawing.Point(713, 57);
            this.InputBox.Name = "InputBox";
            this.InputBox.Size = new System.Drawing.Size(223, 301);
            this.InputBox.TabIndex = 24;
            this.InputBox.TabStop = false;
            this.InputBox.Text = "Settings";
            // 
            // ClearButton
            // 
            this.ClearButton.Location = new System.Drawing.Point(1006, 132);
            this.ClearButton.Name = "ClearButton";
            this.ClearButton.Size = new System.Drawing.Size(58, 55);
            this.ClearButton.TabIndex = 25;
            this.ClearButton.Text = "Clear";
            this.ClearButton.UseVisualStyleBackColor = true;
            this.ClearButton.Click += new System.EventHandler(this.ClearButton_Click);
            // 
            // CreateMultiple
            // 
            this.CreateMultiple.Location = new System.Drawing.Point(945, 347);
            this.CreateMultiple.Name = "CreateMultiple";
            this.CreateMultiple.Size = new System.Drawing.Size(119, 40);
            this.CreateMultiple.TabIndex = 26;
            this.CreateMultiple.Text = "Create Multiple";
            this.CreateMultiple.UseVisualStyleBackColor = true;
            this.CreateMultiple.Click += new System.EventHandler(this.CreateMultiple_Click);
            // 
            // MultipleInput
            // 
            this.MultipleInput.Location = new System.Drawing.Point(945, 388);
            this.MultipleInput.Name = "MultipleInput";
            this.MultipleInput.Size = new System.Drawing.Size(119, 20);
            this.MultipleInput.TabIndex = 27;
            // 
            // ImportInput
            // 
            this.ImportInput.Location = new System.Drawing.Point(713, 414);
            this.ImportInput.Name = "ImportInput";
            this.ImportInput.Size = new System.Drawing.Size(351, 20);
            this.ImportInput.TabIndex = 28;
            this.ImportInput.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ImportInput_KeyPress);
            // 
            // SolveOneButton
            // 
            this.SolveOneButton.Location = new System.Drawing.Point(719, 367);
            this.SolveOneButton.Name = "SolveOneButton";
            this.SolveOneButton.Size = new System.Drawing.Size(217, 41);
            this.SolveOneButton.TabIndex = 29;
            this.SolveOneButton.Text = "Solve One";
            this.SolveOneButton.UseVisualStyleBackColor = true;
            this.SolveOneButton.Click += new System.EventHandler(this.SolveOneButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1272, 592);
            this.Controls.Add(this.SolveOneButton);
            this.Controls.Add(this.ImportInput);
            this.Controls.Add(this.MultipleInput);
            this.Controls.Add(this.CreateMultiple);
            this.Controls.Add(this.ClearButton);
            this.Controls.Add(this.InputBox);
            this.Controls.Add(this.SolveButton);
            this.Controls.Add(this.TextOutput);
            this.Controls.Add(this.CreatePuzzleButton);
            this.Name = "Form1";
            this.Text = "Sudoku Generator";
            this.InputBox.ResumeLayout(false);
            this.InputBox.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button CreatePuzzleButton;
        private System.Windows.Forms.Label TextOutput;
        private System.Windows.Forms.Button SolveButton;
        private System.Windows.Forms.ComboBox SizeInput;
        private System.Windows.Forms.CheckBox HiddenSinglesInput;
        private System.Windows.Forms.CheckBox NakedSetsInput;
        private System.Windows.Forms.CheckBox HiddenSetsInput;
        private System.Windows.Forms.CheckBox SymmetryInput;
        private System.Windows.Forms.CheckBox XWingInput;
        private System.Windows.Forms.CheckBox SwordfishInput;
        private System.Windows.Forms.CheckBox YWingInput;
        private System.Windows.Forms.CheckBox UniqueRectangleInput;
        private System.Windows.Forms.CheckBox FinnedXWingInput;
        private System.Windows.Forms.CheckBox OmissionInput;
        private System.Windows.Forms.GroupBox InputBox;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button CreateMultiple;
        private System.Windows.Forms.TextBox MultipleInput;
        private System.Windows.Forms.TextBox ImportInput;
        private System.Windows.Forms.Button SolveOneButton;
    }
}

