using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SkillBridgeApp
{
    public partial class ProfileForm : Form
    {
        private User _user;
        private Label lblUserName;
        private Label lblUserRating;
        private ListBox lbUserPosts;
        private Button btnLeaveReview;

        public ProfileForm(User user)
        {
            _user = user;
            InitializeComponent();
            SetupTheming();
            LoadProfileData();
        }

        private void InitializeComponent()
        {
            this.Text = $"Профиль пользователя: {_user.Name}";
            this.ClientSize = new Size(400, 500);
            this.StartPosition = FormStartPosition.CenterScreen;

            lblUserName = new Label { Text = $"Имя: {_user.Name}", Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = true, Location = new Point(20, 20) };
            lblUserRating = new Label { Text = $"Рейтинг: {_user.Rating:F1} ★", Font = new Font("Segoe UI", 12), AutoSize = true, Location = new Point(20, 60) };

            Label lblMyPosts = new Label { Text = "Мои объявления:", Font = new Font("Segoe UI", 12, FontStyle.Bold), AutoSize = true, Location = new Point(20, 100) };
            lbUserPosts = new ListBox { Location = new Point(20, 130), Width = 350, Height = 200 };

            btnLeaveReview = new Button { Text = "Оставить отзыв", Location = new Point(20, 350), Width = 150 };
            btnLeaveReview.Click += BtnLeaveReview_Click;

            this.Controls.Add(lblUserName);
            this.Controls.Add(lblUserRating);
            this.Controls.Add(lblMyPosts);
            this.Controls.Add(lbUserPosts);
            this.Controls.Add(btnLeaveReview);
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
            else if (control is Label label)
            {
                label.ForeColor = ColorTranslator.FromHtml("#e2e8f0");
                if (label.Font != null && label.Font.Style.HasFlag(FontStyle.Bold))
                {
                    label.ForeColor = ColorTranslator.FromHtml("#7c3aed");
                }
                if (label.Text.Contains("Рейтинг"))
                {
                    label.ForeColor = ColorTranslator.FromHtml("#f59e0b"); // Rating color
                }
            }
            else if (control is ListBox listBox)
            {
                listBox.BackColor = ColorTranslator.FromHtml("#0f3460");
                listBox.ForeColor = ColorTranslator.FromHtml("#e2e8f0");
            }
        }

        private void LoadProfileData()
        {
            lblUserName.Text = $"Имя: {_user.Name}";
            lblUserRating.Text = $"Рейтинг: {_user.Rating:F1} ★";

            lbUserPosts.Items.Clear();
            foreach (var post in DataStore.Posts.Where(p => p.Author == _user))
            {
                lbUserPosts.Items.Add(post.GetDisplayText());
            }
        }

        private void BtnLeaveReview_Click(object sender, EventArgs e)
        {
            if (DataStore.CurrentUser == null)
            {
                MessageBox.Show("Для оставления отзыва необходимо войти в систему.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (DataStore.CurrentUser == _user)
            {
                MessageBox.Show("Вы не можете оставить отзыв самому себе.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Use custom InputBoxForm for review comment
            InputBoxForm commentBox = new InputBoxForm("Оставить отзыв", "Оставьте комментарий к отзыву:");
            if (commentBox.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(commentBox.InputText))
            {
                MessageBox.Show("Комментарий не может быть пустым.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string reviewComment = commentBox.InputText;

            // Use custom InputBoxForm for stars rating
            InputBoxForm starsBox = new InputBoxForm("Оставить отзыв", "Поставьте оценку (1-5):");
            if (starsBox.ShowDialog() != DialogResult.OK || !int.TryParse(starsBox.InputText, out int stars) || stars < 1 || stars > 5)
            {
                MessageBox.Show("Оценка должна быть числом от 1 до 5.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Review newReview = new Review(DataStore.CurrentUser, _user, stars, reviewComment);
            DataStore.Reviews.Add(newReview);
            MessageBox.Show("Отзыв успешно оставлен!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
