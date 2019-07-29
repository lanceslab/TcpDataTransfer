namespace TcpDataTransfer
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
            this.btn_start_server = new System.Windows.Forms.Button();
            this.bt_send = new System.Windows.Forms.Button();
            this.pb_upload = new System.Windows.Forms.ProgressBar();
            this.dlg_open_file = new System.Windows.Forms.OpenFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.labelResults = new System.Windows.Forms.Label();
            this.tx_recv_data = new System.Windows.Forms.TextBox();
            this.labelSocket = new System.Windows.Forms.Label();
            this.comboBoxIP = new System.Windows.Forms.ComboBox();
            this.buttonShowFiles = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_start_server
            // 
            this.btn_start_server.Location = new System.Drawing.Point(12, 12);
            this.btn_start_server.Name = "btn_start_server";
            this.btn_start_server.Size = new System.Drawing.Size(99, 23);
            this.btn_start_server.TabIndex = 0;
            this.btn_start_server.Text = "Start Server";
            this.btn_start_server.UseVisualStyleBackColor = true;
            this.btn_start_server.Click += new System.EventHandler(this.btn_start_server_Click);
            // 
            // bt_send
            // 
            this.bt_send.Enabled = false;
            this.bt_send.Location = new System.Drawing.Point(12, 104);
            this.bt_send.Name = "bt_send";
            this.bt_send.Size = new System.Drawing.Size(154, 23);
            this.bt_send.TabIndex = 2;
            this.bt_send.Text = "Select and Upload";
            this.bt_send.UseVisualStyleBackColor = true;
            this.bt_send.Click += new System.EventHandler(this.bt_send_Click);
            // 
            // pb_upload
            // 
            this.pb_upload.Location = new System.Drawing.Point(12, 75);
            this.pb_upload.Name = "pb_upload";
            this.pb_upload.Size = new System.Drawing.Size(558, 23);
            this.pb_upload.TabIndex = 3;
            // 
            // dlg_open_file
            // 
            this.dlg_open_file.FileName = "openFileDialog1";
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(201, 34);
            this.label2.TabIndex = 4;
            this.label2.Text = "Server Stoped";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // labelResults
            // 
            this.labelResults.AutoSize = true;
            this.labelResults.Location = new System.Drawing.Point(358, 107);
            this.labelResults.Name = "labelResults";
            this.labelResults.Size = new System.Drawing.Size(55, 17);
            this.labelResults.TabIndex = 5;
            this.labelResults.Text = "Results";
            // 
            // tx_recv_data
            // 
            this.tx_recv_data.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tx_recv_data.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tx_recv_data.Location = new System.Drawing.Point(0, 165);
            this.tx_recv_data.Multiline = true;
            this.tx_recv_data.Name = "tx_recv_data";
            this.tx_recv_data.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tx_recv_data.Size = new System.Drawing.Size(590, 135);
            this.tx_recv_data.TabIndex = 6;
            this.tx_recv_data.Text = "Received message";
            // 
            // labelSocket
            // 
            this.labelSocket.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSocket.Location = new System.Drawing.Point(228, 39);
            this.labelSocket.Name = "labelSocket";
            this.labelSocket.Size = new System.Drawing.Size(342, 33);
            this.labelSocket.TabIndex = 7;
            this.labelSocket.Text = "Socket : ";
            this.labelSocket.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // comboBoxIP
            // 
            this.comboBoxIP.FormattingEnabled = true;
            this.comboBoxIP.Location = new System.Drawing.Point(230, 12);
            this.comboBoxIP.Name = "comboBoxIP";
            this.comboBoxIP.Size = new System.Drawing.Size(340, 24);
            this.comboBoxIP.TabIndex = 8;
            // 
            // buttonShowFiles
            // 
            this.buttonShowFiles.Location = new System.Drawing.Point(173, 105);
            this.buttonShowFiles.Name = "buttonShowFiles";
            this.buttonShowFiles.Size = new System.Drawing.Size(118, 23);
            this.buttonShowFiles.TabIndex = 9;
            this.buttonShowFiles.Text = "Show Contents";
            this.buttonShowFiles.UseVisualStyleBackColor = true;
            this.buttonShowFiles.Click += new System.EventHandler(this.buttonShowFiles_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(117, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 23);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Stop Server";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(590, 300);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.buttonShowFiles);
            this.Controls.Add(this.comboBoxIP);
            this.Controls.Add(this.labelSocket);
            this.Controls.Add(this.tx_recv_data);
            this.Controls.Add(this.labelResults);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pb_upload);
            this.Controls.Add(this.bt_send);
            this.Controls.Add(this.btn_start_server);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LancesLab - File Transfer from Android";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_start_server;
        private System.Windows.Forms.Button bt_send;
        private System.Windows.Forms.ProgressBar pb_upload;
        private System.Windows.Forms.OpenFileDialog dlg_open_file;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelResults;
        private System.Windows.Forms.TextBox tx_recv_data;
        private System.Windows.Forms.Label labelSocket;
        private System.Windows.Forms.ComboBox comboBoxIP;
        private System.Windows.Forms.Button buttonShowFiles;
        private System.Windows.Forms.Button btnClose;
    }
}

