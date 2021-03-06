using System;
using System.Windows.Forms;
using tabler.Logic.Classes;
using tabler.Logic.Helper;

namespace tabler
{
    public partial class SettingsForm : Form
    {
        private readonly GridUI _myParent;

        public SettingsForm(GridUI myParent)
        {
            _myParent = myParent;
            InitializeComponent();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            var configHelper = new ConfigHelper();

            var settings = configHelper.GetSettings();

            if (settings != null)
            {
                if (settings.IndentationSettings == IndentationSettings.Tabs)
                {
                    rbIndentTabs.Checked = true;
                }

                tbIndentation.Text = settings.TabSize.ToString();
            }
        }


        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void rbIndentTabs_CheckedChanged(object sender, EventArgs e)
        {
            var me = (RadioButton) sender;
            tbIndentation.Enabled = !me.Checked;
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            var configHelper = new ConfigHelper();
            var newSettings = new Settings();

            if (rbIndentTabs.Checked)
            {
                newSettings.IndentationSettings = IndentationSettings.Tabs;
            }

            if (rbIndentSpaces.Checked)
            {
                newSettings.IndentationSettings = IndentationSettings.Spaces;
                //newSettings.TabSize = 
                int tabSizeValue;
                if (int.TryParse(tbIndentation.Text, out tabSizeValue))
                {
                    newSettings.TabSize = tabSizeValue;
                }
                else
                {
                    tbIndentation.Text = "4";
                    return;
                }
            }

            configHelper.SaveSettings(newSettings);
            Close();
        }
    }
}
