using System;
using System.Drawing;
using System.Windows.Forms;

namespace SkillBridgeApp
{
    public partial class LoginForm : Form
    {
        public User LoggedInUser { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            SetupTheming();
        }

        private void InitializeComponent()
        {
            this.Text = "SkillBridge - Вход/Регистрация";
            this.ClientSize = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Controls
            Label lblTitle = new Label { Text = "SkillBridge — платформа для обмена навыками", Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = true, Location = new Point(20, 20) };
            Label lblName = new Label { Text = "Имя пользователя:", AutoSize = true, Location = new Point(20, 70) };
            TextBox txtName = new TextBox { Name = "txtName", Location = new Point(150, 70), Width = 200 };
            Label lblEmail = new Label { Text = "Email:", AutoSize = true, Location = new Point(20, 110) };
            TextBox txtEmail = new TextBox { Name = "txtEmail", Location = new Point(150, 110), Width = 200 };

            RadioButton rbLogin = new RadioButton { Text = "Войти", Name = "rbLogin", AutoSize = true, Location = new Point(150, 150), Checked = true };
            RadioButton rbRegister = new RadioButton { Text = "Зарегистрироваться", Name = "rbRegister", AutoSize = true, Location = new Point(250, 150) };

            Button btnSubmit = new Button { Text = "Войти", Name = "btnSubmit", Location = new Point(150, 190), Width = 100, Height = 30 };

            // Event Handlers
            rbLogin.CheckedChanged += (sender, e) => { btnSubmit.Text = rbLogin.Checked ? "Войти" : "Зарегистрироваться"; };
            btnSubmit.Click += (sender, e) => { HandleSubmit(txtName.Text, txtEmail.Text, rbLogin.Checked); };

            // Add Controls
            this.Controls.Add(lblTitle);
            this.Controls.Add(lblName);
            this.Controls.Add(txtName);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(rbLogin);
            this.Controls.Add(rbRegister);
            this.Controls.Add(btnSubmit);
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
                else if (control is RadioButton radioButton)
                {
                    radioButton.ForeColor = ColorTranslator.FromHtml("#e2e8f0");
                }
                else if (control is Label label)
                {
                    label.ForeColor = ColorTranslator.FromHtml("#e2e8f0");
                    if (label.Font.Style.HasFlag(FontStyle.Bold))
                    {
                        label.ForeColor = ColorTranslator.FromHtml("#7c3aed"); // Accent color for titles
                    }
                }
            }
        }

        private void HandleSubmit(string name, string email, bool isLogin)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Имя пользователя и Email не могут быть пустыми.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (isLogin)
            {
                LoggedInUser = DataStore.Users.Find(u => u.Name == name && u.Email == email);
                if (LoggedInUser == null)
                {
                    MessageBox.Show("Пользователь не найден. Проверьте имя и Email или зарегистрируйтесь.", "Ошибка входа", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DataStore.CurrentUser = LoggedInUser;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            else // Register
            {
                if (DataStore.Users.Any(u => u.Email == email))
                {
                    MessageBox.Show("Пользователь с таким Email уже существует.", "Ошибка регистрации", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    int newId = DataStore.Users.Any() ? DataStore.Users.Max(u => u.Id) + 1 : 1;
                    LoggedInUser = new User(newId, name, email);
                    DataStore.Users.Add(LoggedInUser);
                    DataStore.CurrentUser = LoggedInUser;
                    MessageBox.Show("Регистрация прошла успешно!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
        }
    }
}
