using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using Mineware.Systems.Global;
using Mineware.Systems.GlobalConnect;
using System;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.LineActionManager
{
    public partial class ucLineActionManager_AdditionalUsers : BaseUserControl
    {
        public ucLineActionManager_AdditionalUsers()
        {
            InitializeComponent();
        }

        DataTable dtDragDropFilter = new DataTable();
        GridHitInfo downHitInfo = null;

        private void ucLineActionManager_AdditionalUsers_Load(object sender, EventArgs e)
        {
            barBtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;

            LoadAdditionalUser();
            LoadEmplyeeCentral();
        }


        private void LoadAdditionalUser()
        {
            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _sqlConnection.SqlStatement = "Select * from tbl_OCR_LineActionMan_Additional_users order by Mine,Sectionid desc";
            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ResultsTableName = "AdditionalUsers";
            _sqlConnection.ExecuteInstruction();
            DataTable dtReceive = new DataTable();
            dtReceive = _sqlConnection.ResultsDataTable;

            gcAdditional.DataSource = dtReceive;

            colUserSectionID.FieldName = "Sectionid";
            colUserMine.FieldName = "Mine";

            colUserUsername2.FieldName = "RespPerson";
            colUserSectionID2.FieldName = "Sectionid_1";
            colUserEmailInfo2.FieldName = "EmailInfoLvl1";

            colUserUsername3.FieldName = "RespPerson_2";
            colUserSectionID3.FieldName = "Sectionid_2";
            colUserEmailInfo3.FieldName = "EmailInfoLvl2";

            colUserUsername4.FieldName = "RespPerson_3";
            colUserSectionID4.FieldName = "Sectionid_3";
            colUserEmailInfo4.FieldName = "EmailInfoLvl3";

            colUserUsername5.FieldName = "RespPerson_4";
            colUserSectionID5.FieldName = "Sectionid_4";
            colUserEmailInfo5.FieldName = "EmailInfoLvl4";
        }

        private void LoadEmplyeeCentral()
        {
            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            _sqlConnection.SqlStatement = "Select EmployeeNo Empno,EmployeeLastName+' '+EmployeeInit SurnameInit,EmailAddress Email FROM tbl_EmployeeAll_LineActionManager " +
                                          "union " +
                                          "select 'DESKTOP-QTD7SIO', 'Redford K', 'kelvin@mineware.co.za' ";

            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ResultsTableName = "DragDrop";
            _sqlConnection.ExecuteInstruction();
            DataTable dtReceive = new DataTable();
            dtReceive = _sqlConnection.ResultsDataTable;
            dtDragDropFilter = _sqlConnection.ResultsDataTable;

            gcDragDrop.DataSource = dtReceive;

            colDragDropEmpNo.FieldName = "Empno";
            colDragDropEmail.FieldName = "Email";
            colDragDropSurInit.FieldName = "SurnameInit";
        }

        private void gcAdditional_DragDrop(object sender, DragEventArgs e)
        {
            Point p = this.gcAdditional.PointToClient(new Point(e.X, e.Y));
            int row = gvAdditional.CalcHitInfo(p.X, p.Y).RowHandle;
            if (row > -1)
            {
                if (gvAdditional.CalcHitInfo(p.X, p.Y).Column.FieldName != null)
                {
                    if (gvDragDrop.FocusedRowHandle == -1)
                    {
                        return;
                    }

                    string Empno = gvDragDrop.GetRowCellValue(gvDragDrop.FocusedRowHandle, gvDragDrop.Columns["Empno"]).ToString();
                    string Email = gvDragDrop.GetRowCellValue(gvDragDrop.FocusedRowHandle, gvDragDrop.Columns["Email"]).ToString();
                    string Name = gvDragDrop.GetRowCellValue(gvDragDrop.FocusedRowHandle, gvDragDrop.Columns["SurnameInit"]).ToString();


                    if (gvAdditional.CalcHitInfo(p.X, p.Y).Column.FieldName == "RespPerson" || gvAdditional.CalcHitInfo(p.X, p.Y).Column.FieldName == "Sectionid_1" || gvAdditional.CalcHitInfo(p.X, p.Y).Column.FieldName == "EmailInfoLvl1")
                    {
                        gvAdditional.SetRowCellValue(row, gvAdditional.Columns["Sectionid_1"], Empno);
                        gvAdditional.SetRowCellValue(row, gvAdditional.Columns["EmailInfoLvl1"], Email);
                        gvAdditional.SetRowCellValue(row, gvAdditional.Columns["RespPerson"], Name);
                    }

                    if (gvAdditional.CalcHitInfo(p.X, p.Y).Column.FieldName == "RespPerson_2" || gvAdditional.CalcHitInfo(p.X, p.Y).Column.FieldName == "Sectionid_2" || gvAdditional.CalcHitInfo(p.X, p.Y).Column.FieldName == "EmailInfoLvl2")
                    {
                        gvAdditional.SetRowCellValue(row, gvAdditional.Columns["Sectionid_2"], Empno);
                        gvAdditional.SetRowCellValue(row, gvAdditional.Columns["EmailInfoLvl2"], Email);
                        gvAdditional.SetRowCellValue(row, gvAdditional.Columns["RespPerson_2"], Name);
                    }

                    if (gvAdditional.CalcHitInfo(p.X, p.Y).Column.FieldName == "RespPerson_3" || gvAdditional.CalcHitInfo(p.X, p.Y).Column.FieldName == "Sectionid_3" || gvAdditional.CalcHitInfo(p.X, p.Y).Column.FieldName == "EmailInfoLvl3")
                    {
                        gvAdditional.SetRowCellValue(row, gvAdditional.Columns["Sectionid_3"], Empno);
                        gvAdditional.SetRowCellValue(row, gvAdditional.Columns["EmailInfoLvl3"], Email);
                        gvAdditional.SetRowCellValue(row, gvAdditional.Columns["RespPerson_3"], Name);
                    }

                    if (gvAdditional.CalcHitInfo(p.X, p.Y).Column.FieldName == "RespPerson_4" || gvAdditional.CalcHitInfo(p.X, p.Y).Column.FieldName == "Sectionid_4" || gvAdditional.CalcHitInfo(p.X, p.Y).Column.FieldName == "EmailInfoLvl4")
                    {
                        gvAdditional.SetRowCellValue(row, gvAdditional.Columns["Sectionid_4"], Empno);
                        gvAdditional.SetRowCellValue(row, gvAdditional.Columns["EmailInfoLvl4"], Email);
                        gvAdditional.SetRowCellValue(row, gvAdditional.Columns["RespPerson_4"], Name);
                    }
                }
            }
        }

        private void gcAdditional_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;

        }

        private void gcAdditional_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(DataRow)))
            {
                e.Effect = DragDropEffects.Move;
                barBtnSave.Visibility = DevExpress.XtraBars.BarItemVisibility.Always;
            }
            else
                e.Effect = DragDropEffects.None;


        }

        private void barBtnSave_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            MWDataManager.clsDataAccess _sqlConnection = new MWDataManager.clsDataAccess();
            _sqlConnection.ConnectionString = ConfigurationManager.AppSettings["AmplatsConnectionString"];
            for (int row = 0; row < gvAdditional.RowCount; row++)
            {
                string Sectionid = gvAdditional.GetRowCellValue(row, gvAdditional.Columns["Sectionid"]).ToString();
                string Mine = gvAdditional.GetRowCellValue(row, gvAdditional.Columns["Mine"]).ToString();

                string Comp_No_1 = gvAdditional.GetRowCellValue(row, gvAdditional.Columns["RespPerson"]).ToString();
                string SectionID_1 = gvAdditional.GetRowCellValue(row, gvAdditional.Columns["Sectionid_1"]).ToString().TrimEnd();
                string Email_1 = gvAdditional.GetRowCellValue(row, gvAdditional.Columns["EmailInfoLvl1"]).ToString().TrimEnd();

                string Comp_No_2 = gvAdditional.GetRowCellValue(row, gvAdditional.Columns["RespPerson_2"]).ToString();
                string SectionID_2 = gvAdditional.GetRowCellValue(row, gvAdditional.Columns["Sectionid_2"]).ToString().TrimEnd();
                string Email_2 = gvAdditional.GetRowCellValue(row, gvAdditional.Columns["EmailInfoLvl2"]).ToString().TrimEnd();

                string Comp_No_3 = gvAdditional.GetRowCellValue(row, gvAdditional.Columns["RespPerson_3"]).ToString();
                string SectionID_3 = gvAdditional.GetRowCellValue(row, gvAdditional.Columns["Sectionid_3"]).ToString().TrimEnd();
                string Email_3 = gvAdditional.GetRowCellValue(row, gvAdditional.Columns["EmailInfoLvl3"]).ToString().TrimEnd();

                string Comp_No_4 = gvAdditional.GetRowCellValue(row, gvAdditional.Columns["RespPerson_4"]).ToString();
                string SectionID_4 = gvAdditional.GetRowCellValue(row, gvAdditional.Columns["Sectionid_4"]).ToString().TrimEnd();
                string Email_4 = gvAdditional.GetRowCellValue(row, gvAdditional.Columns["EmailInfoLvl4"]).ToString().TrimEnd();

                _sqlConnection.SqlStatement = _sqlConnection.SqlStatement + "update tbl_OCR_LineActionMan_Additional_users  \r\n" +
                                          "set RespPerson = '" + Comp_No_1 + "' ,Sectionid_1 = '" + SectionID_1 + "' ,EmailInfoLvl1 = '" + Email_1 + "' \r\n" +
                                          ",RespPerson_2 = '" + Comp_No_2 + "' ,Sectionid_2 = '" + SectionID_2 + "' ,EmailInfoLvl2 = '" + Email_2 + "'  \r\n" +
                                          ",RespPerson_3 = '" + Comp_No_3 + "', Sectionid_3 = '" + SectionID_3 + "' ,EmailInfoLvl3 = '" + Email_3 + "'  \r\n" +
                                          ",RespPerson_4 = '" + Comp_No_4 + "', Sectionid_4 = '" + SectionID_4 + "' ,EmailInfoLvl4 = '" + Email_4 + "'  \r\n" +
                                          "where Sectionid = '" + Sectionid + "' and Mine = '" + Mine + "'  \r\n\r\n";
            }

            _sqlConnection.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _sqlConnection.queryReturnType = MWDataManager.ReturnType.DataTable;
            _sqlConnection.ResultsTableName = "DragDrop";
            _sqlConnection.ExecuteInstruction();

            //frmEngWaterEquipCapt Mainfrm;
            //Mainfrm = (frmEngWaterEquipCapt)this.FindForm();
            alertControl1.Show(null, "Information", "Record Saved Successfuly.");
        }

        private void gvDragDrop_MouseDown(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;

            downHitInfo = null;

            GridHitInfo hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));

            if (Control.ModifierKeys != Keys.None) return;

            if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
                downHitInfo = hitInfo;

        }

        private void gvDragDrop_MouseMove(object sender, MouseEventArgs e)
        {
            GridView view = sender as GridView;

            if (e.Button == MouseButtons.Left && downHitInfo != null)
            {
                Size dragSize = SystemInformation.DragSize;
                Rectangle dragRect = new Rectangle(new Point(downHitInfo.HitPoint.X - dragSize.Width / 2,
                    downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

                if (!dragRect.Contains(new Point(e.X, e.Y)))
                {
                    DataRow row = view.GetDataRow(downHitInfo.RowHandle);
                    view.GridControl.DoDragDrop(row, DragDropEffects.Move);
                    downHitInfo = null;
                    DevExpress.Utils.DXMouseEventArgs.GetMouseArgs(e).Handled = true;
                }
            }
        }

        private void barbtnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }

        private void btnClose_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            OnCloseTabRequest(new CloseTabArg(tabCaption));
        }
    }
}
