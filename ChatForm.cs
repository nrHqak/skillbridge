using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SkillBridgeApp
{
    public partial class ChatForm : Form
    {
        private User _currentUser;
        private User _chatPartner;
        private ComboBox cmbChatPartners;
        private ListBox lbChatHistory;
        private TextBox txtMessageInput;
        private Button btnSendMessage;

        public ChatForm(User currentUser, User initialChatPartner = null)
        {
            _currentUser = currentUser;
            _chatPartner = initialChatPartner;
            InitializeComponent();
            SetupTheming();
            LoadChatPartners();
            if (_chatPartner != null)
            {
                cmbChatPartners.SelectedItem = _chatPartner.Name;
                LoadChatHistory();
            }
        }

        private void InitializeComponent()
        {
            this.Text = "SkillBridge - Чат";
            this.ClientSize = new Size(500, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            Label lblChatWith = new Label { Text = "Чат с:", AutoSize = true, Location = new Point(10, 10) };
            cmbChatPartners = new ComboBox { Location = new Point(60, 10), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbChatPartners.SelectedIndexChanged += (sender, e) => 
            {
                _chatPartner = DataStore.Users.FirstOrDefault(u => u.Name == cmbChatPartners.SelectedItem.ToString());
                LoadChatHistory();
            };

            lbChatHistory = new ListBox { Location = new Point(10, 40), Width = 460, Height = 350 };

            txtMessageInput = new TextBox { Location = new Point(10, 400), Width = 380 };
            btnSendMessage = new Button { Text = "Отправить", Location = new Point(400, 400), Width = 80 };
            btnSendMessage.Click += BtnSendMessage_Click;

            this.Controls.Add(lblChatWith);
            this.Controls.Add(cmbChatPartners);
            this.Controls.Add(lbChatHistory);
            this.Controls.Add(txtMessageInput);
            this.Controls.Add(btnSendMessage);
        }

        private void SetupTheming()
        {
            this.BackColor = ColorTranslator.FromHtml("#1a1a2e");
            foreach (Control control in this.Controls)
            {
                ApplyTheme(control);
            }
        }

        private void ApplyTheme(Control control)
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
            else if (control is ComboBox comboBox)
            {
                comboBox.BackColor = ColorTranslator.FromHtml("#0f3460");
                comboBox.ForeColor = ColorTranslator.FromHtml("#e2e8f0");
            }
            else if (control is ListBox listBox)
            {
                listBox.BackColor = ColorTranslator.FromHtml("#0f3460");
                listBox.ForeColor = ColorTranslator.FromHtml("#e2e8f0");
            }
            else if (control is Label label)
            {
                label.ForeColor = ColorTranslator.FromHtml("#e2e8f0");
            }
        }

        private void LoadChatPartners()
        {
            cmbChatPartners.Items.Clear();
            foreach (var user in DataStore.Users.Where(u => u != _currentUser))
            {
                cmbChatPartners.Items.Add(user.Name);
            }
        }

        private void LoadChatHistory()
        {
            lbChatHistory.Items.Clear();
            if (_chatPartner == null) return;

            var chatHistory = DataStore.ChatMessages
                .Where(m => (m.From == _currentUser && m.To == _chatPartner) || (m.From == _chatPartner && m.To == _currentUser))
                .OrderBy(m => m.Time)
                .ToList();

            foreach (var message in chatHistory)
            {
                lbChatHistory.Items.Add($"{message.From.Name}: {message.Text}");
            }
        }

        private void BtnSendMessage_Click(object sender, EventArgs e)
        {
            if (_chatPartner == null)
            {
                MessageBox.Show("Пожалуйста, выберите собеседника.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtMessageInput.Text))
            {
                MessageBox.Show("Сообщение не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            ChatMessage newMessage = new ChatMessage(_currentUser, _chatPartner, txtMessageInput.Text);
            DataStore.ChatMessages.Add(newMessage);
            txtMessageInput.Clear();
            LoadChatHistory();
        }
    }
}
