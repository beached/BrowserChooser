namespace BrowserChooser {
	partial class FrmSettings {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing ) {
			if( disposing && (components != null) ) {
				components.Dispose( );
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent( ) {
			this.btnMakeDefault = new System.Windows.Forms.Button( );
			this.btnSave = new System.Windows.Forms.Button( );
			this.btnClose = new System.Windows.Forms.Button( );
			this.lstRemeberedBrowsers = new System.Windows.Forms.ListBox( );
			this.SuspendLayout( );
			// 
			// btnMakeDefault
			// 
			this.btnMakeDefault.Location = new System.Drawing.Point( 12, 12 );
			this.btnMakeDefault.Name = "btnMakeDefault";
			this.btnMakeDefault.Size = new System.Drawing.Size( 127, 29 );
			this.btnMakeDefault.TabIndex = 0;
			this.btnMakeDefault.Text = "Make Default Handler";
			this.btnMakeDefault.UseVisualStyleBackColor = true;
			this.btnMakeDefault.Click += new System.EventHandler( this.btnMakeDefault_Click );
			// 
			// btnSave
			// 
			this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSave.Location = new System.Drawing.Point( 309, 158 );
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size( 75, 23 );
			this.btnSave.TabIndex = 1;
			this.btnSave.Text = "&Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler( this.btnSave_Click );
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnClose.Location = new System.Drawing.Point( 228, 158 );
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size( 75, 23 );
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "&Close";
			this.btnClose.UseVisualStyleBackColor = true;
			this.btnClose.Click += new System.EventHandler( this.btnClose_Click );
			// 
			// lstRemeberedBrowsers
			// 
			this.lstRemeberedBrowsers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.lstRemeberedBrowsers.FormattingEnabled = true;
			this.lstRemeberedBrowsers.Location = new System.Drawing.Point( 12, 47 );
			this.lstRemeberedBrowsers.Name = "lstRemeberedBrowsers";
			this.lstRemeberedBrowsers.Size = new System.Drawing.Size( 372, 108 );
			this.lstRemeberedBrowsers.TabIndex = 3;
			// 
			// frmSettings
			// 
			this.AcceptButton = this.btnSave;
			this.AutoScaleDimensions = new System.Drawing.SizeF( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size( 396, 193 );
			this.ControlBox = false;
			this.Controls.Add( this.lstRemeberedBrowsers );
			this.Controls.Add( this.btnClose );
			this.Controls.Add( this.btnSave );
			this.Controls.Add( this.btnMakeDefault );
			this.Name = "FrmSettings";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "Settings";
			this.ResumeLayout( false );

		}

		#endregion

		private System.Windows.Forms.Button btnMakeDefault;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.ListBox lstRemeberedBrowsers;
	}
}