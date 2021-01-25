using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;

namespace DocumentManager
{
    public partial class SplashScreenLoad : XtraForm
    {
        public SplashScreenLoad()
        {
            InitializeComponent();
            this.labelCopyright.Text = "Copyright Mineware © 1998-" + DateTime.Now.Year.ToString();
        }

        #region Overrides

       

        #endregion

        public enum SplashScreenCommand
        {
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            newtimerlbl.Text = Convert.ToString(Convert.ToInt32(newtimerlbl.Text) + 1);

            if (newtimerlbl.Text == "2")
            {
                timer1.Enabled = false;

                //MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                //_dbMan.ConnectionString = ConfigurationSettings.AppSettings["SQLConnectionStr"];
                //_dbMan.SqlStatement = "Select * from [vw_Vent_Auth_Users] where USERID = '"+ SysSettings.LocalUsername + "' ";
                //_dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                //_dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                //_dbMan.ExecuteInstruction();


                SysSettings sys = new SysSettings();
                sys.GetSysSettings();

                //frmMainLoad MainFrm = new frmMainLoad();
                //MainFrm.WindowState = FormWindowState.Maximized;
                //MainFrm.Show();

                frmMainLoad MainFrm = (frmMainLoad)IsFormAlreadyOpen(typeof(frmMainLoad));
                if (MainFrm == null)
                {
                    MainFrm = new frmMainLoad();
                    MainFrm.WindowState = FormWindowState.Maximized;
                    MainFrm.Show();
                }
                else
                {
                    MainFrm.WindowState = FormWindowState.Maximized;
                    MainFrm.Select();
                }

                this.Hide();
            }
        }

        public static Form IsFormAlreadyOpen(Type FormType)
        {
            foreach (Form OpenForm in Application.OpenForms)
            {
                if (OpenForm.GetType() == FormType)
                    return OpenForm;
            }

            return null;
        }

        private void SplashScreenLoad_Load(object sender, EventArgs e)
        {
            SysSettings.LocalUsername = Environment.UserDomainName;
            //lblUser.Text = Environment.UserName;
            clsUserInfo.UserID = Environment.UserDomainName;
            timer1.Enabled = true;
        }
    }
}