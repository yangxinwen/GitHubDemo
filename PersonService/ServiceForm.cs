using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;

namespace Host
{
    public partial class ServiceForm : Form
    {
        ServiceHost serviceHost = null;
        public ServiceForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (serviceHost == null|| serviceHost.State!=CommunicationState.Opened)
            {
                serviceHost = new ServiceHost(typeof(PersonManageService));
                serviceHost.Open();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (serviceHost != null)
            {
                serviceHost.Close();
                serviceHost = null;
            }
        }
    }
}
