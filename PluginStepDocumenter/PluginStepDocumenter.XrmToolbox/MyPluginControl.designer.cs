namespace PluginStepDocumenter.XrmToolbox
{
    partial class MyPluginControl
    {
        /// <summary> 
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur de composants

        /// <summary> 
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas 
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStripMenu = new System.Windows.Forms.ToolStrip();
            this.tsbClose = new System.Windows.Forms.ToolStripButton();
            this.tssSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.loadAssembliesBtn = new System.Windows.Forms.ToolStripButton();
            this.jsonTextBox = new System.Windows.Forms.TextBox();
            this.assemblyComboBox = new System.Windows.Forms.ComboBox();
            this.assemblyListLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStripMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMenu
            // 
            this.toolStripMenu.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStripMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadAssembliesBtn,
            this.tssSeparator1,
            this.tsbClose});
            this.toolStripMenu.Location = new System.Drawing.Point(0, 0);
            this.toolStripMenu.Name = "toolStripMenu";
            this.toolStripMenu.Size = new System.Drawing.Size(559, 25);
            this.toolStripMenu.TabIndex = 4;
            this.toolStripMenu.Text = "toolStrip1";
            // 
            // tsbClose
            // 
            this.tsbClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbClose.Name = "tsbClose";
            this.tsbClose.Size = new System.Drawing.Size(86, 22);
            this.tsbClose.Text = "Close this tool";
            this.tsbClose.Click += new System.EventHandler(this.tsbClose_Click);
            // 
            // tssSeparator1
            // 
            this.tssSeparator1.Name = "tssSeparator1";
            this.tssSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // loadAssembliesBtn
            // 
            this.loadAssembliesBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.loadAssembliesBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.loadAssembliesBtn.Name = "loadAssembliesBtn";
            this.loadAssembliesBtn.Size = new System.Drawing.Size(99, 22);
            this.loadAssembliesBtn.Text = "Load Assemblies";
            this.loadAssembliesBtn.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.loadAssembliesBtn.Click += new System.EventHandler(this.loadAssembliesBtn_Click);
            // 
            // jsonTextBox
            // 
            this.jsonTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.jsonTextBox.Location = new System.Drawing.Point(3, 89);
            this.jsonTextBox.Multiline = true;
            this.jsonTextBox.Name = "jsonTextBox";
            this.jsonTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.jsonTextBox.Size = new System.Drawing.Size(553, 208);
            this.jsonTextBox.TabIndex = 5;
            // 
            // assemblyComboBox
            // 
            this.assemblyComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.assemblyComboBox.FormattingEnabled = true;
            this.assemblyComboBox.Location = new System.Drawing.Point(3, 45);
            this.assemblyComboBox.Name = "assemblyComboBox";
            this.assemblyComboBox.Size = new System.Drawing.Size(553, 21);
            this.assemblyComboBox.TabIndex = 6;
            this.assemblyComboBox.SelectedIndexChanged += new System.EventHandler(this.assemblyComboBox_SelectedIndexChanged);
            // 
            // assemblyListLabel
            // 
            this.assemblyListLabel.AutoSize = true;
            this.assemblyListLabel.Location = new System.Drawing.Point(4, 29);
            this.assemblyListLabel.Name = "assemblyListLabel";
            this.assemblyListLabel.Size = new System.Drawing.Size(51, 13);
            this.assemblyListLabel.TabIndex = 7;
            this.assemblyListLabel.Text = "Assembly";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Step JSON";
            // 
            // MyPluginControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.assemblyListLabel);
            this.Controls.Add(this.assemblyComboBox);
            this.Controls.Add(this.jsonTextBox);
            this.Controls.Add(this.toolStripMenu);
            this.Name = "MyPluginControl";
            this.Size = new System.Drawing.Size(559, 300);
            this.Load += new System.EventHandler(this.MyPluginControl_Load);
            this.toolStripMenu.ResumeLayout(false);
            this.toolStripMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStripMenu;
        private System.Windows.Forms.ToolStripButton tsbClose;
        private System.Windows.Forms.ToolStripSeparator tssSeparator1;
        private System.Windows.Forms.ToolStripButton loadAssembliesBtn;
        private System.Windows.Forms.TextBox jsonTextBox;
        private System.Windows.Forms.ComboBox assemblyComboBox;
        private System.Windows.Forms.Label assemblyListLabel;
        private System.Windows.Forms.Label label2;
    }
}
