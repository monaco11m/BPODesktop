﻿using BPOBackend;
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

    }
}
