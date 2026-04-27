using System.Drawing;
using System.Windows.Forms;

namespace SkillBridgeApp
{
    public partial class InputBoxForm : Form
    {
        public string InputText { get; private set; }

        public InputBoxForm(string title, string prompt)
        {
            InitializeComponent(title, prompt);
            SetupTheming();
        }

        private void InitializeComponent(string title, string prompt)
        {
            this.Text = title;
            this.ClientSize = new Size(300, 150);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.AcceptButton = btnOK;
            this.CancelButton = btnCancel;

            Label lblPrompt = new Label { Text = prompt, AutoSize = true, Location = new Point(10, 10) };
            TextBox txtInput = new TextBox { Name = "txtInput", Location = new Point(10, 40), Width = 260 };
            Button btnOK = new Button { Text = "OK", Location = new Point(100, 80), Width = 70, DialogResult = DialogResult.OK };
            Button btnCancel = new Button { Text = "Отмена", Location = new Point(180, 80), Width = 70, DialogResult = DialogResult.Cancel };

            btnOK.Click += (sender, e) => { InputText = txtInput.Text; };

            this.Controls.Add(lblPrompt);
            this.Controls.Add(txtInput);
            this.Controls.Add(btnOK);
            this.Controls.Add(btnCancel);
        }

        private void SetupTheming()
        {
            this.BackColor = ColorTranslator.FromHtml("#1a1a2e");
            foreach (Control control in this.Controls)
            {
                control.ForeColor = ColorTranslator.FromHtml("#e2e8f0");
                if (control is Button button)
                {
                    button.BackColor = ColorTranslator.FromHtml("#7c3aed");
                    button.FlatStyle = FlatStyle.Flat;
                    button.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#7c3aed");
                    button.ForeColor = Color.White;
                }
                else if (control is TextBox textBox)
                {
                    textBox.BackColor = ColorTranslator.FromHtml("#0f3460");
                    textBox.BorderStyle = BorderStyle.FixedSingle;
                    textBox.ForeColor = ColorTranslator.FromHtml("#e2e8f0");
                }
            }
        }
    }
}
