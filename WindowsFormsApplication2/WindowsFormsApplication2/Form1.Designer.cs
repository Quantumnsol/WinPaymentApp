namespace WindowsFormsApplication2
{
    public partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        public void InitializeComponent()
        {
            this.BTN_PAYMENT = new System.Windows.Forms.Button();
            this.textBox_send = new System.Windows.Forms.TextBox();
            this.textBox_recv = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.combobox_port = new System.Windows.Forms.ComboBox();
            this.combobox_baudrate = new System.Windows.Forms.ComboBox();
            this.comboBox_req = new System.Windows.Forms.ComboBox();
            this.combobox_type = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox_amount = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox_sign = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_auth_number = new System.Windows.Forms.TextBox();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnReload = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BTN_PAYMENT
            // 
            this.BTN_PAYMENT.Enabled = false;
            this.BTN_PAYMENT.Location = new System.Drawing.Point(2171, 10);
            this.BTN_PAYMENT.Name = "BTN_PAYMENT";
            this.BTN_PAYMENT.Size = new System.Drawing.Size(267, 53);
            this.BTN_PAYMENT.TabIndex = 0;
            this.BTN_PAYMENT.Text = "거래요청";
            this.BTN_PAYMENT.UseVisualStyleBackColor = true;
            this.BTN_PAYMENT.Click += new System.EventHandler(this.BTN_PAYMENT_Click);
            // 
            // textBox_send
            // 
            this.textBox_send.Location = new System.Drawing.Point(86, 9);
            this.textBox_send.Multiline = true;
            this.textBox_send.Name = "textBox_send";
            this.textBox_send.Size = new System.Drawing.Size(350, 26);
            this.textBox_send.TabIndex = 1;
            // 
            // textBox_recv
            // 
            this.textBox_recv.Location = new System.Drawing.Point(607, 9);
            this.textBox_recv.Multiline = true;
            this.textBox_recv.Name = "textBox_recv";
            this.textBox_recv.ReadOnly = true;
            this.textBox_recv.Size = new System.Drawing.Size(391, 26);
            this.textBox_recv.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Request";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(519, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Response";
            // 
            // combobox_port
            // 
            this.combobox_port.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combobox_port.FormattingEnabled = true;
            this.combobox_port.Location = new System.Drawing.Point(1803, 8);
            this.combobox_port.Name = "combobox_port";
            this.combobox_port.Size = new System.Drawing.Size(132, 25);
            this.combobox_port.TabIndex = 0;
            // 
            // combobox_baudrate
            // 
            this.combobox_baudrate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combobox_baudrate.FormattingEnabled = true;
            this.combobox_baudrate.Items.AddRange(new object[] {
            "9600",
            "38400",
            "57600",
            "115200"});
            this.combobox_baudrate.Location = new System.Drawing.Point(2030, 8);
            this.combobox_baudrate.Name = "combobox_baudrate";
            this.combobox_baudrate.Size = new System.Drawing.Size(132, 25);
            this.combobox_baudrate.TabIndex = 6;
            // 
            // comboBox_req
            // 
            this.comboBox_req.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_req.FormattingEnabled = true;
            this.comboBox_req.Items.AddRange(new object[] {
            "Z0",
            "Z6",
            "Z8",
            "Z9",
            "ZA"});
            this.comboBox_req.Location = new System.Drawing.Point(1803, 38);
            this.comboBox_req.Name = "comboBox_req";
            this.comboBox_req.Size = new System.Drawing.Size(132, 25);
            this.comboBox_req.TabIndex = 7;
            // 
            // combobox_type
            // 
            this.combobox_type.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combobox_type.FormattingEnabled = true;
            this.combobox_type.Items.AddRange(new object[] {
            "S0 : 신용승인",
            "S1 : 신용취소",
            "S2 : 은련승인",
            "S3 : 은련취소",
            "C0 : 현금승인",
            "C1 : 현금취소"});
            this.combobox_type.Location = new System.Drawing.Point(2030, 38);
            this.combobox_type.Name = "combobox_type";
            this.combobox_type.Size = new System.Drawing.Size(132, 25);
            this.combobox_type.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(1759, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 17);
            this.label3.TabIndex = 9;
            this.label3.Text = "포트";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1978, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "속도";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(1725, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 17);
            this.label5.TabIndex = 11;
            this.label5.Text = "전문구분";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(1948, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(76, 17);
            this.label6.TabIndex = 12;
            this.label6.Text = "거래유형";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(1515, 12);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(76, 17);
            this.label7.TabIndex = 13;
            this.label7.Text = "승인금액";
            // 
            // textBox_amount
            // 
            this.textBox_amount.Location = new System.Drawing.Point(1597, 8);
            this.textBox_amount.Name = "textBox_amount";
            this.textBox_amount.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBox_amount.Size = new System.Drawing.Size(111, 27);
            this.textBox_amount.TabIndex = 14;
            this.textBox_amount.Text = "1100";
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(14, 91);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(2529, 1614);
            this.panel1.TabIndex = 15;
            // 
            // checkBox_sign
            // 
            this.checkBox_sign.AutoSize = true;
            this.checkBox_sign.Location = new System.Drawing.Point(1474, 12);
            this.checkBox_sign.Name = "checkBox_sign";
            this.checkBox_sign.Size = new System.Drawing.Size(18, 17);
            this.checkBox_sign.TabIndex = 16;
            this.checkBox_sign.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(1426, 12);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(42, 17);
            this.label8.TabIndex = 17;
            this.label8.Text = "서명";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1515, 46);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(76, 17);
            this.label9.TabIndex = 18;
            this.label9.Text = "승인번호";
            // 
            // textBox_auth_number
            // 
            this.textBox_auth_number.Location = new System.Drawing.Point(1597, 38);
            this.textBox_auth_number.Name = "textBox_auth_number";
            this.textBox_auth_number.Size = new System.Drawing.Size(111, 27);
            this.textBox_auth_number.TabIndex = 19;
            this.textBox_auth_number.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnBack
            // 
            this.btnBack.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.btnBack.Location = new System.Drawing.Point(13, 46);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(84, 39);
            this.btnBack.TabIndex = 22;
            this.btnBack.Text = "<";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNext
            // 
            this.btnNext.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.btnNext.Location = new System.Drawing.Point(105, 46);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(87, 39);
            this.btnNext.TabIndex = 23;
            this.btnNext.Text = ">";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnReload
            // 
            this.btnReload.Font = new System.Drawing.Font("굴림", 10F, System.Drawing.FontStyle.Bold);
            this.btnReload.Location = new System.Drawing.Point(198, 46);
            this.btnReload.Name = "btnReload";
            this.btnReload.Size = new System.Drawing.Size(108, 39);
            this.btnReload.TabIndex = 24;
            this.btnReload.Text = "새로고침";
            this.btnReload.UseVisualStyleBackColor = true;
            this.btnReload.Click += new System.EventHandler(this.btnReload_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2455, 1415);
            this.Controls.Add(this.btnReload);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.btnBack);
            this.Controls.Add(this.textBox_auth_number);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.checkBox_sign);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.textBox_amount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.combobox_type);
            this.Controls.Add(this.comboBox_req);
            this.Controls.Add(this.combobox_baudrate);
            this.Controls.Add(this.combobox_port);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox_recv);
            this.Controls.Add(this.textBox_send);
            this.Controls.Add(this.BTN_PAYMENT);
            this.Font = new System.Drawing.Font("굴림", 10F);
            this.Name = "Form1";
            this.Text = "웹 결제 테스트 - TEST";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button BTN_PAYMENT;
        public System.Windows.Forms.TextBox textBox_recv;
        public System.Windows.Forms.TextBox textBox_send;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.ComboBox combobox_port;
        public System.Windows.Forms.ComboBox combobox_baudrate;
        public System.Windows.Forms.ComboBox comboBox_req;
        public System.Windows.Forms.ComboBox combobox_type;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.TextBox textBox_amount;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.CheckBox checkBox_sign;
        public System.Windows.Forms.Label label8;
        public System.Windows.Forms.Label label9;
        public System.Windows.Forms.TextBox textBox_auth_number;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnReload;
    }
}

