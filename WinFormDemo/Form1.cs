using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinFormDemo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        protected override void WndProc(ref Message m)
        {
            #region U盘播放
            if (m.Msg == 0x0219)//WM_DEVICECHANGE
            {
                switch (m.WParam.ToInt32())
                {
                    case 0x8000://DBT_DEVICEARRIVAL
                        {
                            Console.WriteLine("设备接入");
                            break;
                        }
                    case 0x8004://DBT_DEVICEREMOVECOMPLETE
                        {
                            Console.WriteLine("设备拔出");
                            break;
                        }
                }
            }
            #endregion
            base.WndProc(ref m);
        }

    }
}
