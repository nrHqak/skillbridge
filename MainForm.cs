using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Collections.Generic;

namespace SkillBridgeApp
{
    public partial class MainForm : Form
    {
        private ListView lvFeed;
        private ComboBox cmbCategoryFilter;
        private TextBox txtSearch;
        private Button btnRespond;

        private TextBox txtPostDescription;
        private ComboBox cmbPostCategory;
        private RadioButton rbOffer;
        private RadioButton rbRequest;
        private TextBox txtWantedSkill;
        private Button btnPublish;

        private Button btnFindMatches;
        private ListBox lbMatches;
        private Label lblMatchCount;
        private Button btnOpenChat;

        public MainForm()
        {
            InitializeComponent();
            SetupTheming();
            LoadFeed();
        }

        private void InitializeComponent()
        {
            this.Text = "SkillBridge - Платформа обмена навыками";
            this.ClientSize = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            TabControl tabControl = new TabControl { Dock = DockStyle.Fill };
            TabPage feedTab = new TabPage("Лента") { Name = "feedTab" };
            TabPage createPostTab = new TabPage("Создать объявление") { Name = "createPostTab" };
            TabPage matchesTab = new TabPage("Совпадения") { Name = "matchesTab" };
            TabPage chatTab = new TabPage("Чат") { Name = "chatTab" };

            tabControl.Controls.Add(feedTab);
            tabControl.Controls.Add(createPostTab);
            tabControl.Controls.Add(matchesTab);
            tabControl.Controls.Add(chatTab);

            this.Controls.Add(tabControl);

            // Feed Tab
            lvFeed = new ListView { Dock = DockStyle.Fill, View = View.Details, FullRowSelect = true, Location = new Point(0, 40) };
            lvFeed.Columns.Add("Тип", 80);
            lvFeed.Columns.Add("Категория", 100);
            lvFeed.Columns.Add("Описание", 250);
            lvFeed.Columns.Add("Автор", 100);
            lvFeed.Columns.Add("Рейтинг", 80);
            feedTab.Controls.Add(lvFeed);

            cmbCategoryFilter = new ComboBox { Location = new Point(10, 10), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbCategoryFilter.Items.AddRange(new string[] { "Все", "IT", "Арт", "Языки", "Ремонт" });
            cmbCategoryFilter.SelectedIndex = 0;
            cmbCategoryFilter.SelectedIndexChanged += (sender, e) => LoadFeed();
            feedTab.Controls.Add(cmbCategoryFilter);

            txtSearch = new TextBox { Location = new Point(170, 10), Width = 200 };
            txtSearch.TextChanged += (sender, e) => LoadFeed();
            feedTab.Controls.Add(txtSearch);

            btnRespond = new Button { Text = "Откликнуться", Location = new Point(380, 10), Width = 100 };
            btnRespond.Click += BtnRespond_Click;
            feedTab.Controls.Add(btnRespond);

            btnOpenChat = new Button { Text = "Открыть чат", Location = new Point(490, 10), Width = 110 };
            btnOpenChat.Click += (sender, e) => OpenChat();
            feedTab.Controls.Add(btnOpenChat);

            // Create Post Tab
            txtPostDescription = new TextBox { Location = new Point(10, 10), Width = 300, Height = 100, Multiline = true };
            createPostTab.Controls.Add(txtPostDescription);

            cmbPostCategory = new ComboBox { Location = new Point(10, 120), Width = 150, DropDownStyle = ComboBoxStyle.DropDownList };
            cmbPostCategory.Items.AddRange(new string[] { "IT", "Арт", "Языки", "Ремонт" });
            cmbPostCategory.SelectedIndex = 0;
            createPostTab.Controls.Add(cmbPostCategory);

            rbOffer = new RadioButton { Text = "Предлагаю", Location = new Point(10, 160), Checked = true };
            rbRequest = new RadioButton { Text = "Ищу", Location = new Point(120, 160) };
            rbRequest.CheckedChanged += (sender, e) => txtWantedSkill.Visible = rbRequest.Checked;
            createPostTab.Controls.Add(rbOffer);
            createPostTab.Controls.Add(rbRequest);

            txtWantedSkill = new TextBox { Location = new Point(250, 120), Width = 150,Visible = false };
            createPostTab.Controls.Add(txtWantedSkill);

            btnPublish = new Button { Text = "Опубликовать", Location = new Point(10, 200), Width = 150 };
            btnPublish.Click += BtnPublish_Click;
            createPostTab.Controls.Add(btnPublish);

            // Matches Tab
            btnFindMatches = new Button { Text = "Найти совпадения", Location = new Point(10, 10), Width = 150 };
            btnFindMatches.Click += BtnFindMatches_Click;
            matchesTab.Controls.Add(btnFindMatches);

            lblMatchCount = new Label { Text = "Найдено совпадений: 0", Location = new Point(170, 15), AutoSize = true };
            matchesTab.Controls.Add(lblMatchCount);

            lbMatches = new ListBox { Location = new Point(10, 40), Width = 700, Height = 400 };
            matchesTab.Controls.Add(lbMatches);

            Label lblChatInfo = new Label
            {
                Text = "Откройте чат и выберите собеседника из списка.",
                Location = new Point(10, 20),
                AutoSize = true
            };
            chatTab.Controls.Add(lblChatInfo);

            Button btnOpenChatFromTab = new Button { Text = "Перейти в чат", Location = new Point(10, 55), Width = 180 };
            btnOpenChatFromTab.Click += (sender, e) => OpenChat();
            chatTab.Controls.Add(btnOpenChatFromTab);
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
            else if (control is RadioButton radioButton)
            {
                radioButton.ForeColor = ColorTranslator.FromHtml("#e2e8f0");
            }
            else if (control is Label label)
            {
                label.ForeColor = ColorTranslator.FromHtml("#e2e8f0");
                if (label.Font != null && label.Font.Style.HasFlag(FontStyle.Bold))
                {
                    label.ForeColor = ColorTranslator.FromHtml("#7c3aed");
                }
            }
            else if (control is TabControl tabControl)
            {
                tabControl.BackColor = ColorTranslator.FromHtml("#1a1a2e");
                foreach (TabPage page in tabControl.TabPages)
                {
                    page.BackColor = ColorTranslator.FromHtml("#1a1a2e");
                    foreach (Control childControl in page.Controls)
                    {
                        ApplyTheme(childControl);
                    }
                }
            }
            else if (control is ListView listView)
            {
                listView.BackColor = ColorTranslator.FromHtml("#0f3460");
                listView.ForeColor = ColorTranslator.FromHtml("#e2e8f0");
            }
            else if (control is ListBox listBox)
            {
                listBox.BackColor = ColorTranslator.FromHtml("#0f3460");
                listBox.ForeColor = ColorTranslator.FromHtml("#e2e8f0");
            }
        }

        private void LoadFeed()
        {
            lvFeed.Items.Clear();
            var filteredPosts = DataStore.Posts.AsEnumerable();

            string selectedCategory = cmbCategoryFilter.SelectedItem?.ToString();
            if (selectedCategory != "Все" && !string.IsNullOrEmpty(selectedCategory))
            {
                filteredPosts = filteredPosts.Where(p => p.Category == selectedCategory);
            }

            string searchTerm = txtSearch.Text.ToLower();
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                filteredPosts = filteredPosts.Where(p => p.Description.ToLower().Contains(searchTerm) || p.Author.Name.ToLower().Contains(searchTerm));
            }

            foreach (var post in filteredPosts.OrderByDescending(p => p.CreatedAt))
            {
                ListViewItem item = new ListViewItem(post is OfferPost ? "Предлагаю" : "Ищу");
                item.SubItems.Add(post.Category);
                item.SubItems.Add(post.GetDisplayText());
                item.SubItems.Add(post.Author.Name);
                item.SubItems.Add(post.Author.Rating.ToString("F1"));
                item.Tag = post; // Store the post object for later use
                lvFeed.Items.Add(item);
            }
        }

        private void BtnRespond_Click(object sender, EventArgs e)
        {
            if (lvFeed.SelectedItems.Count > 0)
            {
                BasePost selectedPost = lvFeed.SelectedItems[0].Tag as BasePost;
                if (selectedPost != null && selectedPost.Author != DataStore.CurrentUser)
                {
                    // Open ChatForm with the author of the selected post
                    ChatForm chatForm = new ChatForm(DataStore.CurrentUser, selectedPost.Author);
                    chatForm.ShowDialog();
                }
                else if (selectedPost.Author == DataStore.CurrentUser)
                {
                    MessageBox.Show("Вы не можете откликнуться на свое собственное объявление.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите объявление, чтобы откликнуться.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OpenChat()
        {
            if (DataStore.CurrentUser == null)
            {
                MessageBox.Show("Для доступа к чату необходимо войти в систему.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            ChatForm chatForm = new ChatForm(DataStore.CurrentUser);
            chatForm.ShowDialog();
        }

        private void BtnPublish_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPostDescription.Text))
            {
                MessageBox.Show("Описание навыка не может быть пустым.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (DataStore.CurrentUser == null)
            {
                MessageBox.Show("Для публикации объявления необходимо войти в систему.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int newId = DataStore.Posts.Any() ? DataStore.Posts.Max(p => p.Id) + 1 : 1;
            string category = cmbPostCategory.SelectedItem.ToString();
            string description = txtPostDescription.Text;

            if (rbOffer.Checked)
            {
                DataStore.Posts.Add(new OfferPost(newId, DataStore.CurrentUser, category, description));
            }
            else // Request.Checked
            {
                if (string.IsNullOrWhiteSpace(txtWantedSkill.Text))
                {
                    MessageBox.Show("Для объявления типа 'Ищу' необходимо указать, что вы хотите получить взамен.", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DataStore.Posts.Add(new RequestPost(newId, DataStore.CurrentUser, category, description, txtWantedSkill.Text));
            }

            MessageBox.Show("Объявление успешно опубликовано!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            txtPostDescription.Clear();
            txtWantedSkill.Clear();
            rbOffer.Checked = true;
            txtWantedSkill.Visible = false;
            LoadFeed(); // Refresh the feed
        }

        private void BtnFindMatches_Click(object sender, EventArgs e)
        {
            if (DataStore.CurrentUser == null)
            {
                MessageBox.Show("Для поиска совпадений необходимо войти в систему.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lbMatches.Items.Clear();
            var matches = DataStore.FindMatches(DataStore.CurrentUser);
            lblMatchCount.Text = $"Найдено совпадений: {matches.Count}";

            if (matches.Any())
            {
                foreach (var match in matches)
                {
                    lbMatches.Items.Add(
                        $"{match.offeringUser.Name} предлагает '{match.offer.Category}', " +
                        $"а {match.requestingUser.Name} ищет '{match.offer.Category}'. " +
                        $"{match.requestingUser.Name} предлагает '{match.request.Category}', " +
                        $"а {match.offeringUser.Name} ищет '{match.request.Category}'.");
                }
            }
            else
            {
                lbMatches.Items.Add("Совпадений не найдено.");
            }
        }
    }
}
