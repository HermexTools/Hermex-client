﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KSLUploader_Client.Tools
{
    public class Utilities
    {
        public static bool CheckFormIsOpened(string name)
        {
            FormCollection fc = Application.OpenForms;

            foreach(Form frm in fc)
            {
                if(frm.Text == name)
                {
                    frm.Focus();
                    return true;
                }
            }
            return false;
        }

    }
}
