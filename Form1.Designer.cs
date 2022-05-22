
namespace Валюты
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prediction = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FormulaPredict = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.buttonBuy = new System.Windows.Forms.Button();
            this.buttonSell = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PredictionBuy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PredictionFormula = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.textBoxCount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxBalance = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.buttonSellAuto = new System.Windows.Forms.Button();
            this.buttonBuyAuto = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SimplePrediction = new System.Windows.Forms.RadioButton();
            this.FormulaPrediction = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Prediction,
            this.FormulaPredict});
            this.dataGridView1.Location = new System.Drawing.Point(8, 64);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(855, 535);
            this.dataGridView1.TabIndex = 0;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Наименование";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 125;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Цена";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.Width = 125;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Валюта";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.Width = 125;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "Перевод";
            this.Column4.MinimumWidth = 6;
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Width = 125;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Валюта перевода";
            this.Column5.MinimumWidth = 6;
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.Width = 125;
            // 
            // Prediction
            // 
            this.Prediction.HeaderText = "Простой прогноз (прирост)";
            this.Prediction.MinimumWidth = 6;
            this.Prediction.Name = "Prediction";
            this.Prediction.ReadOnly = true;
            this.Prediction.Width = 125;
            // 
            // FormulaPredict
            // 
            this.FormulaPredict.HeaderText = "Прогноз по формуле";
            this.FormulaPredict.Name = "FormulaPredict";
            this.FormulaPredict.ReadOnly = true;
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(949, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(269, 28);
            this.button2.TabIndex = 2;
            this.button2.Text = "Сделать простой прогноз";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // buttonBuy
            // 
            this.buttonBuy.Enabled = false;
            this.buttonBuy.Location = new System.Drawing.Point(340, 32);
            this.buttonBuy.Name = "buttonBuy";
            this.buttonBuy.Size = new System.Drawing.Size(110, 26);
            this.buttonBuy.TabIndex = 3;
            this.buttonBuy.Text = "Купить";
            this.buttonBuy.UseVisualStyleBackColor = true;
            this.buttonBuy.Click += new System.EventHandler(this.buttonBuy_Click);
            // 
            // buttonSell
            // 
            this.buttonSell.Enabled = false;
            this.buttonSell.Location = new System.Drawing.Point(470, 33);
            this.buttonSell.Name = "buttonSell";
            this.buttonSell.Size = new System.Drawing.Size(110, 25);
            this.buttonSell.TabIndex = 4;
            this.buttonSell.Text = "Продать";
            this.buttonSell.UseVisualStyleBackColor = true;
            this.buttonSell.Click += new System.EventHandler(this.buttonSell_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column7,
            this.Column8,
            this.PredictionBuy,
            this.PredictionFormula});
            this.dataGridView2.Location = new System.Drawing.Point(869, 64);
            this.dataGridView2.MultiSelect = false;
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.Size = new System.Drawing.Size(349, 535);
            this.dataGridView2.TabIndex = 5;
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Название";
            this.Column6.MinimumWidth = 6;
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Width = 125;
            // 
            // Column7
            // 
            this.Column7.HeaderText = "Дата последней покупки";
            this.Column7.MinimumWidth = 6;
            this.Column7.Name = "Column7";
            this.Column7.ReadOnly = true;
            this.Column7.Width = 150;
            // 
            // Column8
            // 
            this.Column8.HeaderText = "Количество";
            this.Column8.MinimumWidth = 6;
            this.Column8.Name = "Column8";
            this.Column8.ReadOnly = true;
            this.Column8.Width = 125;
            // 
            // PredictionBuy
            // 
            this.PredictionBuy.HeaderText = "Простой прогноз";
            this.PredictionBuy.MinimumWidth = 6;
            this.PredictionBuy.Name = "PredictionBuy";
            this.PredictionBuy.ReadOnly = true;
            this.PredictionBuy.Width = 125;
            // 
            // PredictionFormula
            // 
            this.PredictionFormula.HeaderText = "Прогноз по формуле";
            this.PredictionFormula.Name = "PredictionFormula";
            this.PredictionFormula.ReadOnly = true;
            // 
            // textBoxCount
            // 
            this.textBoxCount.Location = new System.Drawing.Point(476, 3);
            this.textBoxCount.Name = "textBoxCount";
            this.textBoxCount.Size = new System.Drawing.Size(104, 26);
            this.textBoxCount.TabIndex = 6;
            this.textBoxCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(336, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 19);
            this.label1.TabIndex = 7;
            this.label1.Text = "Количество акций";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 19);
            this.label2.TabIndex = 8;
            this.label2.Text = "Баланс:";
            // 
            // textBoxBalance
            // 
            this.textBoxBalance.Enabled = false;
            this.textBoxBalance.Location = new System.Drawing.Point(77, 12);
            this.textBoxBalance.Name = "textBoxBalance";
            this.textBoxBalance.Size = new System.Drawing.Size(211, 26);
            this.textBoxBalance.TabIndex = 9;
            this.textBoxBalance.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(294, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 19);
            this.label3.TabIndex = 10;
            this.label3.Text = "руб.";
            // 
            // buttonSellAuto
            // 
            this.buttonSellAuto.Enabled = false;
            this.buttonSellAuto.Location = new System.Drawing.Point(586, 32);
            this.buttonSellAuto.Name = "buttonSellAuto";
            this.buttonSellAuto.Size = new System.Drawing.Size(185, 26);
            this.buttonSellAuto.TabIndex = 12;
            this.buttonSellAuto.Text = "Продать по автомату";
            this.buttonSellAuto.UseVisualStyleBackColor = true;
            this.buttonSellAuto.Click += new System.EventHandler(this.buttonSellAuto_Click);
            // 
            // buttonBuyAuto
            // 
            this.buttonBuyAuto.Enabled = false;
            this.buttonBuyAuto.Location = new System.Drawing.Point(586, 3);
            this.buttonBuyAuto.Name = "buttonBuyAuto";
            this.buttonBuyAuto.Size = new System.Drawing.Size(185, 26);
            this.buttonBuyAuto.TabIndex = 11;
            this.buttonBuyAuto.Text = "Купить по автомату";
            this.buttonBuyAuto.UseVisualStyleBackColor = true;
            this.buttonBuyAuto.Click += new System.EventHandler(this.buttonBuyAuto_Click);
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(949, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(269, 28);
            this.button1.TabIndex = 13;
            this.button1.Text = "Сделать прогноз по формуле";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // SimplePrediction
            // 
            this.SimplePrediction.AutoSize = true;
            this.SimplePrediction.Enabled = false;
            this.SimplePrediction.Location = new System.Drawing.Point(779, 2);
            this.SimplePrediction.Name = "SimplePrediction";
            this.SimplePrediction.Size = new System.Drawing.Size(134, 23);
            this.SimplePrediction.TabIndex = 14;
            this.SimplePrediction.TabStop = true;
            this.SimplePrediction.Text = "Просто прогноз";
            this.SimplePrediction.UseVisualStyleBackColor = true;
            // 
            // FormulaPrediction
            // 
            this.FormulaPrediction.AutoSize = true;
            this.FormulaPrediction.Enabled = false;
            this.FormulaPrediction.Location = new System.Drawing.Point(779, 35);
            this.FormulaPrediction.Name = "FormulaPrediction";
            this.FormulaPrediction.Size = new System.Drawing.Size(164, 23);
            this.FormulaPrediction.TabIndex = 15;
            this.FormulaPrediction.TabStop = true;
            this.FormulaPrediction.Text = "Прогноз по формуле";
            this.FormulaPrediction.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1230, 607);
            this.Controls.Add(this.FormulaPrediction);
            this.Controls.Add(this.SimplePrediction);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonSellAuto);
            this.Controls.Add(this.buttonBuyAuto);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxBalance);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxCount);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.buttonSell);
            this.Controls.Add(this.buttonBuy);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Name = "Form1";
            this.Text = "Курс акций";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button buttonBuy;
        private System.Windows.Forms.Button buttonSell;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.TextBox textBoxCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxBalance;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonSellAuto;
        private System.Windows.Forms.Button buttonBuyAuto;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column8;
        private System.Windows.Forms.DataGridViewTextBoxColumn PredictionBuy;
        private System.Windows.Forms.DataGridViewTextBoxColumn PredictionFormula;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prediction;
        private System.Windows.Forms.DataGridViewTextBoxColumn FormulaPredict;
        private System.Windows.Forms.RadioButton SimplePrediction;
        private System.Windows.Forms.RadioButton FormulaPrediction;
    }
}

