using Mineware.Systems.GlobalConnect;
using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.LineActionManager
{
    public partial class frmLineActionManagerAdhoc : DevExpress.XtraEditors.XtraForm
    {
        public frmLineActionManagerAdhoc()
        {
            InitializeComponent();
        }

        #region Public variables
        public string UserCurrentInfoConnection;
        #endregion

        #region Private variables
        private DataTable dtOP = new DataTable();
        private DataTable dtSection = new DataTable();
        private DataTable dtDis = new DataTable();
        private DataTable dtPro = new DataTable();
        private DataTable dtAct = new DataTable();
        private DataTable dtReported = new DataTable();
        private DataTable dtAcc = new DataTable();
        private DataTable dtResp = new DataTable();


        private string Operation;
        private string Section;
        private string Workplace;
        private string Hazard;
        private int HazardRating;
        private DateTime CaptureDate;
        private DateTime RequiredDate;
        private string ReportedBy;
        private string ReportedByCoy;
        private string Discipline;
        private string Process;
        private string Activity;
        private string Dificiency;
        private string RemedialAction;
        private string Accountable;
        private string AccountableCoy;
        private string Responsible;
        private string ResponsibleCoy;
        #endregion

        private void frmLineActionManagerAdhoc_Load(object sender, EventArgs e)
        {
            dtOP = LoadDetail(0, "0");
            sleOperation.Properties.DataSource = dtOP;
            sleOperation.Properties.ValueMember = "Operation_Description";
            sleOperation.Properties.DisplayMember = "Operation_Description";
            sleOperation.EditValue = dtOP.Rows[0]["Operation_Description"].ToString();

            dtDis = LoadDetail(2, "0");
            sleDiscipline.Properties.DataSource = dtDis;
            sleDiscipline.Properties.ValueMember = "Discipline_ID";
            sleDiscipline.Properties.DisplayMember = "Discipline_Description";
            sleDiscipline.Properties.PopulateViewColumns();
            sleDiscipline.Properties.View.Columns[0].Visible = false;

            dtReported = LoadDetail(5, "0");
            dtAcc = dtReported.Copy();
            dtResp = dtReported.Copy();

            sleReportedBy.Properties.DataSource = dtReported;
            sleReportedBy.Properties.ValueMember = "Employee_Number";
            sleReportedBy.Properties.DisplayMember = "Name";
            sleReportedBy.Properties.PopulateViewColumns();
            sleReportedBy.Properties.View.Columns[2].Visible = false;
            sleReportedBy.Properties.View.Columns[0].Width = 8;
            sleReportedBy.Properties.View.Columns[1].Width = 15;

            slePersonAccountable.Properties.DataSource = dtAcc;
            slePersonAccountable.Properties.ValueMember = "Employee_Number";
            slePersonAccountable.Properties.DisplayMember = "Name";
            slePersonAccountable.Properties.PopulateViewColumns();
            slePersonAccountable.Properties.View.Columns[2].Visible = false;
            slePersonAccountable.Properties.View.Columns[0].Width = 8;
            slePersonAccountable.Properties.View.Columns[1].Width = 15;

            slePersonResponsible.Properties.DataSource = dtResp;
            slePersonResponsible.Properties.ValueMember = "Employee_Number";
            slePersonResponsible.Properties.DisplayMember = "Name";
            slePersonResponsible.Properties.PopulateViewColumns();
            slePersonResponsible.Properties.View.Columns[2].Visible = false;
            slePersonResponsible.Properties.View.Columns[0].Width = 8;
            slePersonResponsible.Properties.View.Columns[1].Width = 15;

            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfoConnection);
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.SqlStatement = "Select 'High' Descript,'H' val \r\n" +
                                   "union  \r\n" +
                                   "Select 'Medium' Descript,'M' val  \r\n" +
                                   "union  \r\n" +
                                   "Select 'Low' Descript,'L' val  \r\n" +
                                   " ";
            theData.ExecuteInstruction();
            DataTable dtHaz = theData.ResultsDataTable;

            sleHazard.Properties.DataSource = dtHaz;
            sleHazard.Properties.ValueMember = "val";
            sleHazard.Properties.DisplayMember = "Descript";
        }

        private DataTable LoadDetail(int ID, string Val)
        {
            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfoConnection);
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.SqlStatement = "exec [dbo].[sp_LineActionManager_AdHoc_Load] '" + ID + "', '" + Val + "' ";
            theData.ExecuteInstruction();

            if (theData.ResultsDataTable.Rows.Count > 0)
            {
                return theData.ResultsDataTable;
            }
            else
            {
                return null;
            }
        }

        private void sleOperation_EditValueChanged(object sender, EventArgs e)
        {
            dtSection.Clear();
            dtSection = LoadDetail(1, sleOperation.EditValue.ToString());
            sleSection.Properties.DataSource = dtSection;
            sleSection.Properties.ValueMember = "Section";
            sleSection.Properties.DisplayMember = "Section";

            Operation = sleOperation.EditValue.ToString();
        }

        private void sleDiscipline_EditValueChanged(object sender, EventArgs e)
        {
            dtPro.Clear();
            dtPro = LoadDetail(3, sleDiscipline.EditValue.ToString());
            sleProcess.Properties.DataSource = dtPro;
            sleProcess.Properties.ValueMember = "Process_ID";
            sleProcess.Properties.DisplayMember = "Process_Description";
            sleProcess.Properties.PopulateViewColumns();
            sleProcess.Properties.View.Columns[0].Visible = false;
            sleProcess.Properties.View.Columns[1].Visible = false;

            Discipline = sleDiscipline.EditValue.ToString();
            sleProcess.Enabled = true;
            sleProcess.Focus();
        }

        private void sleProcess_EditValueChanged(object sender, EventArgs e)
        {
            dtAct.Clear();
            dtAct = LoadDetail(4, sleProcess.EditValue.ToString());
            sleActivity.Properties.DataSource = dtAct;
            sleActivity.Properties.ValueMember = "Activity_ID";
            sleActivity.Properties.DisplayMember = "Activity_Description";
            sleActivity.Properties.PopulateViewColumns();
            sleActivity.Properties.View.Columns[0].Visible = false;
            sleActivity.Properties.View.Columns[1].Visible = false;

            Process = sleProcess.EditValue.ToString();
            sleActivity.Enabled = true;
            sleActivity.Focus();
        }

        private void sleActivity_EditValueChanged(object sender, EventArgs e)
        {
            Activity = sleActivity.EditValue.ToString();
        }

        private void sleSection_EditValueChanged(object sender, EventArgs e)
        {
            Section = sleSection.EditValue.ToString();
        }

        private void deCaptureDate_EditValueChanged(object sender, EventArgs e)
        {
            CaptureDate = (DateTime)deCaptureDate.EditValue;
        }

        private void deRequiredDate_EditValueChanged(object sender, EventArgs e)
        {
            RequiredDate = (DateTime)deRequiredDate.EditValue;
        }

        private void sleReportedBy_EditValueChanged(object sender, EventArgs e)
        {
            ReportedBy = sleReportedBy.EditValue.ToString();
            ReportedByCoy = sleReportedBy.Text.ToString();
        }

        private void meDeficiency_EditValueChanged(object sender, EventArgs e)
        {
            Dificiency = meDeficiency.EditValue.ToString();
        }

        private void meAction_EditValueChanged(object sender, EventArgs e)
        {
            RemedialAction = meAction.EditValue.ToString();
        }

        private void slePersonAccountable_EditValueChanged(object sender, EventArgs e)
        {
            Accountable = slePersonAccountable.EditValue.ToString();
            AccountableCoy = slePersonAccountable.Text.ToString();
        }

        private void slePersonResponsible_EditValueChanged(object sender, EventArgs e)
        {
            Responsible = slePersonResponsible.EditValue.ToString();
            ResponsibleCoy = slePersonResponsible.Text.ToString();
        }

        private void txtWorkplace_EditValueChanged(object sender, EventArgs e)
        {
            Workplace = txtWorkplace.EditValue.ToString();
        }

        //private bool Save(string Operation, string Section, string Workplace,
        //string Hazard, string HazardRating, string CaptureDate,
        //string RequiredDate, string ReportedBy, string ReportedByCoy,
        //string Discipline, string Process, string Activity,
        //string Dificiency, string RemedialAction, string Accountable,
        //string AccountableCoy, string Responsible, string ResponsibleCoy)
        //{
        //    MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
        //    theData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfoConnection);
        //    theData.queryExecutionType = MWDataManager.ExecutionType.StoreProcedure;
        //    theData.queryReturnType = MWDataManager.ReturnType.DataTable;
        //    theData.SqlStatement = "[sp_LineActionManager_AdHoc_Create]";
        //    var _paramCollection = new SqlParameter[]
        //    {
        //        theData.CreateParameter("@Operation",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                Operation),
        //        theData.CreateParameter("@Section",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                Section),
        //        theData.CreateParameter("@Workplace",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                Workplace),
        //        theData.CreateParameter("@Hazard",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                Hazard),
        //        theData.CreateParameter("@HazardRating",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                0),
        //        theData.CreateParameter("@CaptureDate",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                CaptureDate),
        //        theData.CreateParameter("@RequiredDate",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                RequiredDate),
        //        theData.CreateParameter("@ReportedBy",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                ReportedBy),
        //        theData.CreateParameter("@ReportedByCoy",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                ReportedByCoy),
        //        theData.CreateParameter("@Discipline",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                Discipline),
        //        theData.CreateParameter("@Process",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                Process),
        //        theData.CreateParameter("@Activity",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                Activity),
        //        theData.CreateParameter("@Dificiency",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                Dificiency),
        //        theData.CreateParameter("@RemedialAction",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                RemedialAction),
        //        theData.CreateParameter("@Accountable",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                Accountable),
        //        theData.CreateParameter("@AccountableCoy",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                AccountableCoy),
        //        theData.CreateParameter("@Responsible",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                Responsible),
        //        theData.CreateParameter("@ResponsibleCoy",
        //                                SqlDbType.VarChar,
        //                                0,
        //                                ResponsibleCoy)
        //    };
        //    theData.ParamCollection = _paramCollection;
        //    theData.ExecuteInstruction();

        //    if (theData.ResultsDataTable.Rows.Count > 0)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}



        private void barBtnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void barBtnSave_Click(object sender, EventArgs e)
        {
            if (txtWorkplace.EditValue == null)
            {
                MessageBox.Show("Please Type in a workplace before Adding Adhoc Action");
                return;
            }

            if (sleSection.EditValue == null)
            {
                MessageBox.Show("Please Select a Section before Adding Adhoc Action");
                return;
            }

            if (sleHazard.EditValue == null)
            {
                MessageBox.Show("Please Select a Hazard before Adding Adhoc Action");
                return;
            }

            if (deCaptureDate.EditValue == null)
            {
                MessageBox.Show("Please Select a Capture Date before Adding Adhoc Action");
                return;
            }

            if (deRequiredDate.EditValue == null)
            {
                MessageBox.Show("Please Select a Required Date Adding Adhoc Action");
                return;
            }

            if (sleReportedBy.EditValue == null)
            {
                MessageBox.Show("Please Select a Report by Person Adding Adhoc Action");
                return;
            }

            if (slePersonAccountable.EditValue == null)
            {
                MessageBox.Show("Please Select a Accountable Person Adding Adhoc Action");
                return;
            }

            if (slePersonResponsible.EditValue == null)
            {
                MessageBox.Show("Please Select a Responsible Person Adding Adhoc Action");
                return;
            }

            string Operation = sleOperation.EditValue.ToString();
            string Section = sleSection.EditValue.ToString();
            string Workplace = txtWorkplace.EditValue.ToString();
            string Hazard = sleHazard.EditValue.ToString();
            string HazardRating = sleHazard.EditValue.ToString();
            string Capturedate = String.Format("{0:yyyy-MM-dd}", deCaptureDate.EditValue);
            string RequiredDate = String.Format("{0:yyyy-MM-dd}", deRequiredDate.EditValue);
            string ReportBy = sleReportedBy.Text;
            string ReportByCoy = sleReportedBy.EditValue.ToString();
            //string Discipline = sleDiscipline.EditValue.ToString();
            //string Process = sleProcess.EditValue.ToString();
            //string Acitvity = sleActivity.EditValue.ToString();
            string Dificiency = meDeficiency.EditValue.ToString();
            string RemedialAction = meAction.EditValue.ToString();
            string Accountable = slePersonAccountable.Text.ToString();
            string AccountableCoy = slePersonAccountable.EditValue.ToString();
            string Responsible = slePersonResponsible.Text.ToString();
            string ResponsibleCoy = slePersonResponsible.EditValue.ToString();

            MWDataManager.clsDataAccess theData = new MWDataManager.clsDataAccess();
            theData.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfoConnection);
            theData.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

            theData.SqlStatement = "INSERT INTO [dbo].[tbl_Incidents]  \r\n" +
           "([Operation],[Workplace],[Mineware_Action_ID],[ActionDate],[Enablon_Action_ID]  \r\n" +
           ",[Action_Title],[Action_Status]  \r\n" +
           ",[Responsible_Person],[Responsible_Person_Badge],[Responsible_Person_Feedback]  \r\n" +
           ",[Action_Created_By],[Action_Created_By_Badge]  \r\n" +
           ",[Start_Date],[Target_Date],[Complete_Date]  \r\n" +
           ",[Action_Progress],[Action_Parent_Type],[Action_Complete]  \r\n" +
           ",[Document_Link],[Image_Link]  \r\n" +
           ",[Action_Closed_By],[Action_Closed_By_Badge],[Action_Close_Date]  \r\n" +
           ",[Application_Origin],[WPRiskRating],[SUE]  \r\n" +
           ",[CriticalControl],[ActInSpectionDate]  \r\n" +
           ",[Hazard],[Step],[Section])  \r\n" +
           "VALUES  \r\n" +
           "('Amandelbult','" + Workplace + "', \r\n" +
           "    ( select top 1( 'ADHOC' + SUBSTRING( convert(varchar(20), CONVERT(decimal(18,0), substring(Mineware_Action_ID,6,8))+1+1000000),2,8)) \r\n" +
           "    from(select Mineware_Action_ID from tbl_Incidents where Mineware_Action_ID like 'ADHOC'  \r\n" +
           "    union  \r\n" +
           "    select 'ADHOC000001')a order by Mineware_Action_ID desc)  \r\n" +
           ",'" + Capturedate + "',0  \r\n" +
           ",'" + RemedialAction + "',''  \r\n" +
           ",'" + Responsible + "','" + ResponsibleCoy + "',''  \r\n" +
           ",'" + ReportBy + "','" + ReportByCoy + "'  \r\n" +
           ",'" + Capturedate + "','" + RequiredDate + "','1900-01-01 00:00:00.000'  \r\n" +
           ",0,'',0  \r\n" +
           ",'',''  \r\n" +
           ",'','','1900-01-01 00:00:00.000'  \r\n" +
           ",'' ,0,''  \r\n" +
           ",'','1900-01-01 00:00:00.000'  \r\n" +
           ",'" + Hazard + "','" + Dificiency + "','" + Section + "')";

            theData.queryReturnType = MWDataManager.ReturnType.DataTable;
            theData.ExecuteInstruction();
        }
    }
}
