namespace BOOSEapp
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel canvasPanel;
        private System.Windows.Forms.TextBox programTextBox;
        private System.Windows.Forms.TextBox outputTextBox;
        private System.Windows.Forms.Button executeButton;
        private System.Windows.Forms.Button clearCanvasButton;
        private System.Windows.Forms.Button clearOutputButton;
        private System.Windows.Forms.Label programLabel;
        private System.Windows.Forms.Label outputLabel;
        private System.Windows.Forms.Label canvasLabel;
        private System.Windows.Forms.SplitContainer mainSplitContainer;
        private System.Windows.Forms.SplitContainer rightSplitContainer;
        private System.Windows.Forms.Panel programPanel;
        private System.Windows.Forms.Panel outputPanel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            canvasPanel = new Panel();
            programTextBox = new TextBox();
            outputTextBox = new TextBox();
            executeButton = new Button();
            clearCanvasButton = new Button();
            clearOutputButton = new Button();
            programLabel = new Label();
            outputLabel = new Label();
            canvasLabel = new Label();
            mainSplitContainer = new SplitContainer();
            rightSplitContainer = new SplitContainer();
            programPanel = new Panel();
            outputPanel = new Panel();
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).BeginInit();
            mainSplitContainer.Panel1.SuspendLayout();
            mainSplitContainer.Panel2.SuspendLayout();
            mainSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)rightSplitContainer).BeginInit();
            rightSplitContainer.Panel1.SuspendLayout();
            rightSplitContainer.Panel2.SuspendLayout();
            rightSplitContainer.SuspendLayout();
            programPanel.SuspendLayout();
            outputPanel.SuspendLayout();
            SuspendLayout();
            // 
            // canvasPanel
            // 
            canvasPanel.BackColor = Color.White;
            canvasPanel.BorderStyle = BorderStyle.FixedSingle;
            canvasPanel.Dock = DockStyle.Fill;
            canvasPanel.Location = new Point(0, 25);
            canvasPanel.Name = "canvasPanel";
            canvasPanel.Size = new Size(600, 535);
            canvasPanel.TabIndex = 0;
            canvasPanel.Paint += canvasPanel_Paint;
            // 
            // programTextBox
            // 
            programTextBox.Dock = DockStyle.Fill;
            programTextBox.Font = new Font("Consolas", 10F);
            programTextBox.Location = new Point(0, 65);
            programTextBox.Multiline = true;
            programTextBox.Name = "programTextBox";
            programTextBox.ScrollBars = ScrollBars.Vertical;
            programTextBox.Size = new Size(396, 235);
            programTextBox.TabIndex = 1;
            programTextBox.Text = "moveto 100,150\r\npen 0,0,255\r\ncircle 150\r\npen 255,0,0\r\nmoveto 150,50\r\nrect 150,100\r\nmoveto 150,200\r\npen 0,0,255\r\ncircle 250\r\npen 255,0,0\r\nmoveto 200,250\r\nrect 200,100";
            // 
            // outputTextBox
            // 
            outputTextBox.Dock = DockStyle.Fill;
            outputTextBox.Font = new Font("Consolas", 9F);
            outputTextBox.Location = new Point(0, 25);
            outputTextBox.Multiline = true;
            outputTextBox.Name = "outputTextBox";
            outputTextBox.ReadOnly = true;
            outputTextBox.ScrollBars = ScrollBars.Vertical;
            outputTextBox.Size = new Size(396, 231);
            outputTextBox.TabIndex = 2;
            // 
            // executeButton
            // 
            executeButton.BackColor = Color.FromArgb(76, 175, 80);
            executeButton.Dock = DockStyle.Top;
            executeButton.FlatStyle = FlatStyle.Flat;
            executeButton.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            executeButton.ForeColor = Color.White;
            executeButton.Location = new Point(0, 0);
            executeButton.Name = "executeButton";
            executeButton.Size = new Size(396, 40);
            executeButton.TabIndex = 3;
            executeButton.Text = "RUN PROGRAM";
            executeButton.UseVisualStyleBackColor = false;
            executeButton.Click += ExecuteButton_Click;
            // 
            // clearCanvasButton
            // 
            clearCanvasButton.BackColor = Color.FromArgb(244, 67, 54);
            clearCanvasButton.Dock = DockStyle.Bottom;
            clearCanvasButton.FlatStyle = FlatStyle.Flat;
            clearCanvasButton.Font = new Font("Segoe UI", 9F);
            clearCanvasButton.ForeColor = Color.White;
            clearCanvasButton.Location = new Point(0, 560);
            clearCanvasButton.Name = "clearCanvasButton";
            clearCanvasButton.Size = new Size(600, 40);
            clearCanvasButton.TabIndex = 4;
            clearCanvasButton.Text = "CLEAR CANVAS";
            clearCanvasButton.UseVisualStyleBackColor = false;
            clearCanvasButton.Click += ClearCanvasButton_Click;
            // 
            // clearOutputButton
            // 
            clearOutputButton.BackColor = Color.FromArgb(33, 150, 243);
            clearOutputButton.Dock = DockStyle.Bottom;
            clearOutputButton.FlatStyle = FlatStyle.Flat;
            clearOutputButton.Font = new Font("Segoe UI", 9F);
            clearOutputButton.ForeColor = Color.White;
            clearOutputButton.Location = new Point(0, 256);
            clearOutputButton.Name = "clearOutputButton";
            clearOutputButton.Size = new Size(396, 40);
            clearOutputButton.TabIndex = 5;
            clearOutputButton.Text = "CLEAR OUTPUT";
            clearOutputButton.UseVisualStyleBackColor = false;
            clearOutputButton.Click += ClearOutputButton_Click;
            // 
            // programLabel
            // 
            programLabel.BackColor = Color.FromArgb(66, 66, 66);
            programLabel.Dock = DockStyle.Top;
            programLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            programLabel.ForeColor = Color.White;
            programLabel.Location = new Point(0, 40);
            programLabel.Name = "programLabel";
            programLabel.Size = new Size(396, 25);
            programLabel.TabIndex = 6;
            programLabel.Text = "PROGRAM INPUT";
            programLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // outputLabel
            // 
            outputLabel.BackColor = Color.FromArgb(66, 66, 66);
            outputLabel.Dock = DockStyle.Top;
            outputLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            outputLabel.ForeColor = Color.White;
            outputLabel.Location = new Point(0, 0);
            outputLabel.Name = "outputLabel";
            outputLabel.Size = new Size(396, 25);
            outputLabel.TabIndex = 7;
            outputLabel.Text = "OUTPUT / DEBUG";
            outputLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // canvasLabel
            // 
            canvasLabel.BackColor = Color.FromArgb(66, 66, 66);
            canvasLabel.Dock = DockStyle.Top;
            canvasLabel.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            canvasLabel.ForeColor = Color.White;
            canvasLabel.Location = new Point(0, 0);
            canvasLabel.Name = "canvasLabel";
            canvasLabel.Size = new Size(600, 25);
            canvasLabel.TabIndex = 8;
            canvasLabel.Text = "DRAWING CANVAS";
            canvasLabel.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // mainSplitContainer
            // 
            mainSplitContainer.Dock = DockStyle.Fill;
            mainSplitContainer.Location = new Point(0, 0);
            mainSplitContainer.Name = "mainSplitContainer";
            // 
            // mainSplitContainer.Panel1
            // 
            mainSplitContainer.Panel1.Controls.Add(canvasPanel);
            mainSplitContainer.Panel1.Controls.Add(canvasLabel);
            mainSplitContainer.Panel1.Controls.Add(clearCanvasButton);
            // 
            // mainSplitContainer.Panel2
            // 
            mainSplitContainer.Panel2.Controls.Add(rightSplitContainer);
            mainSplitContainer.Size = new Size(1000, 600);
            mainSplitContainer.SplitterDistance = 600;
            mainSplitContainer.TabIndex = 9;
            // 
            // rightSplitContainer
            // 
            rightSplitContainer.Dock = DockStyle.Fill;
            rightSplitContainer.Location = new Point(0, 0);
            rightSplitContainer.Name = "rightSplitContainer";
            rightSplitContainer.Orientation = Orientation.Horizontal;
            // 
            // rightSplitContainer.Panel1
            // 
            rightSplitContainer.Panel1.Controls.Add(programPanel);
            // 
            // rightSplitContainer.Panel2
            // 
            rightSplitContainer.Panel2.Controls.Add(outputPanel);
            rightSplitContainer.Size = new Size(396, 600);
            rightSplitContainer.SplitterDistance = 300;
            rightSplitContainer.TabIndex = 0;
            // 
            // programPanel
            // 
            programPanel.Controls.Add(programTextBox);
            programPanel.Controls.Add(programLabel);
            programPanel.Controls.Add(executeButton);
            programPanel.Dock = DockStyle.Fill;
            programPanel.Location = new Point(0, 0);
            programPanel.Name = "programPanel";
            programPanel.Size = new Size(396, 300);
            programPanel.TabIndex = 9;
            // 
            // outputPanel
            // 
            outputPanel.Controls.Add(outputTextBox);
            outputPanel.Controls.Add(outputLabel);
            outputPanel.Controls.Add(clearOutputButton);
            outputPanel.Dock = DockStyle.Fill;
            outputPanel.Location = new Point(0, 0);
            outputPanel.Name = "outputPanel";
            outputPanel.Size = new Size(396, 296);
            outputPanel.TabIndex = 10;
            // 
            // MainForm
            // 
            ClientSize = new Size(1000, 600);
            Controls.Add(mainSplitContainer);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "BOOSE Drawing Program";
            FormClosing += MainForm_FormClosing;
            mainSplitContainer.Panel1.ResumeLayout(false);
            mainSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)mainSplitContainer).EndInit();
            mainSplitContainer.ResumeLayout(false);
            rightSplitContainer.Panel1.ResumeLayout(false);
            rightSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)rightSplitContainer).EndInit();
            rightSplitContainer.ResumeLayout(false);
            programPanel.ResumeLayout(false);
            programPanel.PerformLayout();
            outputPanel.ResumeLayout(false);
            outputPanel.PerformLayout();
            ResumeLayout(false);
        }

    }
}
