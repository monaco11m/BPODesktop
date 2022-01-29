using BPOBackend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BPODesktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            ddlUser.DataSource = AspNetUserBl.Instance.GetAspNetUser();
            ddlUser.DisplayMember="UserName";
            ddlUser.ValueMember ="Id";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DownloadAndMergeAsync();
        }
        private async Task DownloadAndMergeAsync()
        {
            List<String> list = LabelStorageUrlsBl.Instance.GetUrls();

            await LabelStorageUrlsBl.Instance.DownloadListAsync(list);
            LabelStorageUrlsBl.Instance.MergePdf(list, true);
        }

        private void dtStartDate_ValueChanged(object sender, EventArgs e)
        {
            LoadGroupIds();
        }
        private void dtEndDate_ValueChanged(object sender, EventArgs e)
        {
            LoadGroupIds();
        }

        private void LoadGroupIds()
        {
            DateTime strStartDate = dtStartDate.Value;
            List<AutomateLabel> result = AutomateLabelsBl.Instance.GetIdsByUserIdAndDate((String)ddlUser.SelectedValue, dtStartDate.Value, dtEndDate.Value);
            ddlGroupId.DataSource = result;
            ddlGroupId.DisplayMember = "Id";
            ddlGroupId.ValueMember = "Id";
        }

        
    }
}
