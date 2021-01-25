using Mineware.Systems.GlobalConnect;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Mineware.Systems.Production.OreflowDiagram
{
    public partial class frmAddOreflow : DevExpress.XtraEditors.XtraForm
    {
        public string _OreflowType;
        public string _theSystemDBTag;
        public string _UserCurrentInfoConnection;

        public frmAddOreflow()
        {
            InitializeComponent();
        }

        private void frmAddOreflow_Load(object sender, EventArgs e)
        {
            this.Text = "Add " + _OreflowType;
            LoadgridInfo();
        }

        private void LoadgridInfo()
        {
            if (_OreflowType == "Pachuca")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan.SqlStatement = " select PacName DescOreflow, PacID OreflowID from tbl_Backfill_Pachuca";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                gcOreflowentities.DataSource = null;
                gcOreflowentities.DataSource = _dbMan.ResultsDataTable;
                colDescription.FieldName = "DescOreflow";
                colID.FieldName = "OreflowID";
            }

            if (_OreflowType == "Tank")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan.SqlStatement = "select TankName DescOreflow, TankID OreflowID from tbl_Backfill_Tanks";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                gcOreflowentities.DataSource = null;
                gcOreflowentities.DataSource = _dbMan.ResultsDataTable;
                colDescription.FieldName = "DescOreflow";
                colID.FieldName = "OreflowID";
            }

            if (_OreflowType == "Transfer Range")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan.SqlStatement = "select TRangeID OreflowID,TRangeName DescOreflow from tbl_Backfill_TransferRange";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                gcOreflowentities.DataSource = null;
                gcOreflowentities.DataSource = _dbMan.ResultsDataTable;
                colDescription.FieldName = "DescOreflow";
                colID.FieldName = "OreflowID";
            }

            if (_OreflowType == "Dam")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan.SqlStatement = "select DamID OreflowID,DamName DescOreflow from tbl_Backfill_Dam";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                gcOreflowentities.DataSource = null;
                gcOreflowentities.DataSource = _dbMan.ResultsDataTable;
                colDescription.FieldName = "DescOreflow";
                colID.FieldName = "OreflowID";
            }

            if (_OreflowType == "Internal Orepass")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan.SqlStatement = "select InteralOrepassID OreflowID,InteralOrepassName DescOreflow from tbl_Backfill_InternalOrepass";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                gcOreflowentities.DataSource = null;
                gcOreflowentities.DataSource = _dbMan.ResultsDataTable;
                colDescription.FieldName = "DescOreflow";
                colID.FieldName = "OreflowID";
            }

            if (_OreflowType == "Shaft")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan.SqlStatement = "select ShaftID OreflowID,ShaftName DescOreflow from tbl_Backfill_Shaft where ShaftType = '" + _OreflowType + "' ";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                gcOreflowentities.DataSource = null;
                gcOreflowentities.DataSource = _dbMan.ResultsDataTable;
                colDescription.FieldName = "DescOreflow";
                colID.FieldName = "OreflowID";
            }

            if (_OreflowType == "SubShaft")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan.SqlStatement = "select ShaftID OreflowID,ShaftName DescOreflow from tbl_Backfill_Shaft where ShaftType = '" + _OreflowType + "'";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                gcOreflowentities.DataSource = null;
                gcOreflowentities.DataSource = _dbMan.ResultsDataTable;
                colDescription.FieldName = "DescOreflow";
                colID.FieldName = "OreflowID";
            }

            if (_OreflowType == "TerShaft")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan.SqlStatement = "select ShaftID OreflowID,ShaftName DescOreflow from tbl_Backfill_Shaft where ShaftType = '" + _OreflowType + "'";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                gcOreflowentities.DataSource = null;
                gcOreflowentities.DataSource = _dbMan.ResultsDataTable;
                colDescription.FieldName = "DescOreflow";
                colID.FieldName = "OreflowID";
            }

            if (_OreflowType == "InclineShaft")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan.SqlStatement = "select ShaftID OreflowID,ShaftName DescOreflow from tbl_Backfill_Shaft where ShaftType = '" + _OreflowType + "'";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                gcOreflowentities.DataSource = null;
                gcOreflowentities.DataSource = _dbMan.ResultsDataTable;
                colDescription.FieldName = "DescOreflow";
                colID.FieldName = "OreflowID";
            }

            if (_OreflowType == "Level")
            {
                MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
                _dbMan.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbMan.SqlStatement = "select LevelID OreflowID,LevelName DescOreflow from tbl_Backfill_Level";
                _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbMan.ExecuteInstruction();

                gcOreflowentities.DataSource = null;
                gcOreflowentities.DataSource = _dbMan.ResultsDataTable;
                colDescription.FieldName = "DescOreflow";
                colID.FieldName = "OreflowID";
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (_OreflowType == "Pachuca")
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Please Fill in a Pachuca Name ");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_Backfill_Pachuca where PacName = '" + txtName.Text + "'  ";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + "Insert Into tbl_Backfill_Pachuca Values \r\n" +
                                           "( (select top 1( 'P' + SUBSTRING( convert(varchar(10), CONVERT(decimal(18,0), substring(PacID,2,5))+1+10000),2,5)) \r\n" +
                                           "from (select * from dbo.tbl_Backfill_Pachuca  \r\n" +
                                           "union  \r\n" +
                                           "select 'P0001', '" + txtName.Text + "') a order by PacID desc) , '" + txtName.Text + "' )";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbBackfill.queryReturnType = MWDataManager.ReturnType.DataTable;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Tank")
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Please Fill in a Tank Name ");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_Backfill_Tanks where TankName = '" + txtName.Text + "'  ";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + "Insert Into tbl_Backfill_Tanks Values \r\n" +
                                           "( (select top 1( 'T' + SUBSTRING( convert(varchar(10), CONVERT(decimal(18,0), substring(TankID,2,5))+1+10000),2,5)) \r\n" +
                                           "from (select * from tbl_Backfill_Tanks \r\n" +
                                           "union   \r\n" +
                                           "select 'T0001', '" + txtName.Text + "') a order by TankID desc) , '" + txtName.Text + "' ) ";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbBackfill.queryReturnType = MWDataManager.ReturnType.DataTable;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Transfer Range")
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Please Fill in a Transfer Range Name");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_Backfill_TransferRange where TRangeName = '" + txtName.Text + "'  ";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + "Insert Into tbl_Backfill_TransferRange Values \r\n" +
                                           "( (select top 1( 'TR' + SUBSTRING( convert(varchar(10), CONVERT(decimal(18,0), substring(TRangeID,3,8))+1+100000),3,8))  \r\n" +
                                           "from (select * from tbl_Backfill_TransferRange  \r\n" +
                                           "union \r\n" +
                                           "select 'TR0001', '" + txtName.Text + "') a order by TRangeID desc) , '" + txtName.Text + "' )";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbBackfill.queryReturnType = MWDataManager.ReturnType.DataTable;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Dam")
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Please Fill in a Dam Name");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_Backfill_Dam where DamName = '" + txtName.Text + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + "Insert Into[tbl_Backfill_Dam] Values \r\n" +
                                                       "( (select top 1( 'D' + SUBSTRING( convert(varchar(10), CONVERT(decimal(18,0), substring(DamID,2,5))+1+10000),2,5)) \r\n" +
                                                       "from (select * from dbo.tbl_Backfill_Dam union  \r\n" +
                                                       "select 'D0001', '" + txtName.Text + "') a order by DamID desc) , '" + txtName.Text + "' )";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbBackfill.queryReturnType = MWDataManager.ReturnType.DataTable;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Internal Orepass")
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Please Fill in a Internal Orepass Name");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_Backfill_InternalOrepass where InteralOrepassName = '" + txtName.Text + "'  ";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + "Insert Into tbl_Backfill_InternalOrepass Values \r\n" +
                                           "( (select top 1( 'IP' + SUBSTRING( convert(varchar(10), CONVERT(decimal(18,0), substring(InteralOrepassID,3,8))+1+100000),3,8))  \r\n" +
                                           "from (select * from tbl_Backfill_InternalOrepass  \r\n" +
                                           "union \r\n" +
                                           "select 'IP0001', '" + txtName.Text + "') a order by InteralOrepassID desc) , '" + txtName.Text + "' )";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbBackfill.queryReturnType = MWDataManager.ReturnType.DataTable;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Shaft")
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Please Fill in a Shaft Name");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_Backfill_Shaft where ShaftName = '" + txtName.Text + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + "Insert Into tbl_Backfill_Shaft Values \r\n" +
                                                       "( (select top 1( 'S' + SUBSTRING( convert(varchar(10), CONVERT(decimal(18,0), substring(ShaftID,2,5))+1+10000),2,5)) \r\n" +
                                                       "from (select * from dbo.tbl_Backfill_Shaft union  \r\n" +
                                                       "select 'S0001', '" + txtName.Text + "','" + _OreflowType + "') a order by ShaftID desc) , '" + txtName.Text + "','" + _OreflowType + "' )";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbBackfill.queryReturnType = MWDataManager.ReturnType.DataTable;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "SubShaft")
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Please Fill in a Sub Shaft Name");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_Backfill_Shaft where ShaftName = '" + txtName.Text + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + "Insert Into tbl_Backfill_Shaft Values \r\n" +
                                                       "( (select top 1( 'S' + SUBSTRING( convert(varchar(10), CONVERT(decimal(18,0), substring(ShaftID,2,5))+1+10000),2,5)) \r\n" +
                                                       "from (select * from dbo.tbl_Backfill_Shaft union  \r\n" +
                                                       "select 'S0001', '" + txtName.Text + "','" + _OreflowType + "') a order by ShaftID desc) , '" + txtName.Text + "','" + _OreflowType + "' )";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbBackfill.queryReturnType = MWDataManager.ReturnType.DataTable;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "TerShaft")
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Please Fill in a Ter Shaft Name");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_Backfill_Shaft where ShaftName = '" + txtName.Text + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + "Insert Into tbl_Backfill_Shaft Values \r\n" +
                                                       "( (select top 1( 'S' + SUBSTRING( convert(varchar(10), CONVERT(decimal(18,0), substring(ShaftID,2,5))+1+10000),2,5)) \r\n" +
                                                       "from (select * from dbo.tbl_Backfill_Shaft union  \r\n" +
                                                       "select 'S0001', '" + txtName.Text + "','" + _OreflowType + "') a order by ShaftID desc) , '" + txtName.Text + "','" + _OreflowType + "' )";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbBackfill.queryReturnType = MWDataManager.ReturnType.DataTable;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "InclineShaft")
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Please Fill in a incline Shaft Name");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_Backfill_Shaft where ShaftName = '" + txtName.Text + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + "Insert Into tbl_Backfill_Shaft Values \r\n" +
                                                       "( (select top 1( 'S' + SUBSTRING( convert(varchar(10), CONVERT(decimal(18,0), substring(ShaftID,2,5))+1+10000),2,5)) \r\n" +
                                                       "from (select * from dbo.tbl_Backfill_Shaft union  \r\n" +
                                                       "select 'S0001', '" + txtName.Text + "','" + _OreflowType + "') a order by ShaftID desc) , '" + txtName.Text + "','" + _OreflowType + "' )";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbBackfill.queryReturnType = MWDataManager.ReturnType.DataTable;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Level")
            {
                if (string.IsNullOrEmpty(txtName.Text))
                {
                    MessageBox.Show("Please Fill in a Level Name");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_Backfill_Level where LevelName = '" + txtName.Text + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + "Insert Into tbl_Backfill_Level Values \r\n" +
                                                       "( (select top 1( 'L' + SUBSTRING( convert(varchar(10), CONVERT(decimal(18,0), substring(LevelID,2,5))+1+10000),2,5)) \r\n" +
                                                       "from (select * from dbo.tbl_Backfill_Level union  \r\n" +
                                                       "select 'L0001', '" + txtName.Text + "') a order by LevelID desc) , '" + txtName.Text + "' )";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbBackfill.queryReturnType = MWDataManager.ReturnType.DataTable;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            LoadgridInfo();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (_OreflowType == "Pachuca")
            {
                if (string.IsNullOrEmpty(gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString()))
                {
                    MessageBox.Show("No Record Selected!");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_OreFlowEntities where oreflowid = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + " Delete from tbl_Backfill_Pachuca where PacID = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  ";

                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Deleted", "Data Deleted", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Tank")
            {
                if (string.IsNullOrEmpty(gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString()))
                {
                    MessageBox.Show("No Record Selected!");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_OreFlowEntities where oreflowid = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + " Delete from tbl_Backfill_Tanks where TankID = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  ";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Deleted", "Data Deleted", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Transfer Range")
            {
                if (string.IsNullOrEmpty(gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString()))
                {
                    MessageBox.Show("No Record Selected!");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_OreFlowEntities where oreflowid = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + " Delete from tbl_Backfill_TransferRange where TRangeID = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  ";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Deleted", "Data Deleted", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Dam")
            {
                if (string.IsNullOrEmpty(gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString()))
                {
                    MessageBox.Show("No Record Selected!");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_OreFlowEntities where oreflowid = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + " Delete from tbl_Backfill_Dam where DamID = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  ";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Deleted", "Data Deleted", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Internal Orepass")
            {
                if (string.IsNullOrEmpty(gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString()))
                {
                    MessageBox.Show("No Record Selected!");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_OreFlowEntities where oreflowid = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + " Delete from tbl_Backfill_InternalOrepass where InteralOrepassID = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  ";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Deleted", "Data Deleted", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Shaft")
            {
                if (string.IsNullOrEmpty(gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString()))
                {
                    MessageBox.Show("No Record Selected!");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_OreFlowEntities where oreflowid = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + " Delete from tbl_Backfill_Shaft where ShaftID = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  ";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Deleted", "Data Deleted", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "SubShaft")
            {
                if (string.IsNullOrEmpty(gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString()))
                {
                    MessageBox.Show("No Record Selected!");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_OreFlowEntities where oreflowid = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + " Delete from tbl_Backfill_Shaft where ShaftID = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  ";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Deleted", "Data Deleted", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "TerShaft")
            {
                if (string.IsNullOrEmpty(gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString()))
                {
                    MessageBox.Show("No Record Selected!");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_OreFlowEntities where oreflowid = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + " Delete from tbl_Backfill_Shaft where ShaftID = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  ";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Deleted", "Data Deleted", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "InclineShaft")
            {
                if (string.IsNullOrEmpty(gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString()))
                {
                    MessageBox.Show("No Record Selected!");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_OreFlowEntities where oreflowid = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + " Delete from tbl_Backfill_Shaft where ShaftID = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  ";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Deleted", "Data Deleted", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Level")
            {
                if (string.IsNullOrEmpty(gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString()))
                {
                    MessageBox.Show("No Record Selected!");
                    return;
                }

                MWDataManager.clsDataAccess _dbBackfill = new MWDataManager.clsDataAccess();
                _dbBackfill.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbBackfill.SqlStatement = " Delete from tbl_OreFlowEntities where oreflowid = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  \r\n";

                _dbBackfill.SqlStatement = _dbBackfill.SqlStatement + " Delete from tbl_Backfill_Level where LevelID = '" + gvOreflowentities.GetRowCellValue(gvOreflowentities.FocusedRowHandle, gvOreflowentities.Columns[1]).ToString() + "'  ";
                _dbBackfill.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbBackfill.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Deleted", "Data Deleted", Color.CornflowerBlue);
                }
            }

            LoadgridInfo();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_OreflowType == "Pachuca")
            {
                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManSave.SqlStatement = " insert into tbl_OreFlowEntities (OreFlowID,Name,OreFlowCode,Inactive) \r\n" +
                                               " select PacID, PacName, 'Pachuca', '1' from \r\n" +
                                               " [tbl_Backfill_Pachuca] where PacID not in (select OreFlowID from[tbl_OreFlowEntities]) ";
                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbManSave.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Tank")
            {
                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManSave.SqlStatement = "insert into tbl_OreFlowEntities (OreFlowID,Name,OreFlowCode,Inactive)   \r\n " +
                                           "select TankID, TankName, 'Tank', '1' from  \r\n " +
                                           "[tbl_Backfill_Tanks] where TankID not in (select OreFlowID from [tbl_OreFlowEntities])";
                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                var result = _dbManSave.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Transfer Range")
            {
                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManSave.SqlStatement = "insert into tbl_OreFlowEntities (OreFlowID,Name,OreFlowCode,Inactive)  \r\n" +
                                          "select TRangeID, TRangeName, 'TRange', '1' from  \r\n " +
                                          "[tbl_Backfill_TransferRange] where TRangeID not in (select OreFlowID from [tbl_OreFlowEntities])";
                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbManSave.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Dam")
            {
                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManSave.SqlStatement = "insert into tbl_OreFlowEntities (OreFlowID,Name,OreFlowCode,Inactive)  \r\n" +
                                          "select DamID, DamName, 'Dam', '1' from  \r\n " +
                                          "[tbl_Backfill_Dam] where DamID not in (select OreFlowID from [tbl_OreFlowEntities])";
                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbManSave.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Internal Orepass")
            {
                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManSave.SqlStatement = "insert into tbl_OreFlowEntities (OreFlowID,Name,OreFlowCode,Inactive)  \r\n" +
                                          "select InteralOrepassID, InteralOrepassName, 'IOrePass', '1' from  \r\n " +
                                          "[tbl_Backfill_InternalOrepass] where InteralOrepassID not in (select OreFlowID from [tbl_OreFlowEntities])";
                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbManSave.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Shaft")
            {
                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManSave.SqlStatement = "insert into tbl_OreFlowEntities (OreFlowID,Name,OreFlowCode,Inactive)  \r\n" +
                                          "select ShaftID, ShaftName, 'Shaft', '1' from  \r\n " +
                                          "[tbl_Backfill_Shaft] where ShaftID not in (select OreFlowID from [tbl_OreFlowEntities]) and ShaftType = '" + _OreflowType + "'";
                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbManSave.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "SubShaft")
            {
                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManSave.SqlStatement = "insert into tbl_OreFlowEntities (OreFlowID,Name,OreFlowCode,Inactive)  \r\n" +
                                          "select ShaftID, ShaftName, 'SubShaft', '1' from  \r\n " +
                                          "[tbl_Backfill_Shaft] where ShaftID not in (select OreFlowID from [tbl_OreFlowEntities]) and ShaftType = '" + _OreflowType + "'";
                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbManSave.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "TerShaft")
            {
                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManSave.SqlStatement = "insert into tbl_OreFlowEntities (OreFlowID,Name,OreFlowCode,Inactive)  \r\n" +
                                          "select ShaftID, ShaftName, 'TerShaft', '1' from  \r\n " +
                                          "[tbl_Backfill_Shaft] where ShaftID not in (select OreFlowID from [tbl_OreFlowEntities]) and ShaftType = '" + _OreflowType + "'";
                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbManSave.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "InclineShaft")
            {
                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManSave.SqlStatement = "insert into tbl_OreFlowEntities (OreFlowID,Name,OreFlowCode,Inactive)  \r\n" +
                                          "select ShaftID, ShaftName, 'InclShaft', '1' from  \r\n " +
                                          "[tbl_Backfill_Shaft] where ShaftID not in (select OreFlowID from [tbl_OreFlowEntities]) and ShaftType = '" + _OreflowType + "'";
                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbManSave.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            if (_OreflowType == "Level")
            {
                MWDataManager.clsDataAccess _dbManSave = new MWDataManager.clsDataAccess();
                _dbManSave.ConnectionString = TConnections.GetConnectionString(_theSystemDBTag, _UserCurrentInfoConnection);
                _dbManSave.SqlStatement = "insert into tbl_OreFlowEntities (OreFlowID,Name,OreFlowCode,Inactive)  \r\n" +
                                          "select LevelID, LevelName, 'Lvl', '1' from  \r\n " +
                                          "[tbl_Backfill_Level] where LevelID not in (select OreFlowID from [tbl_OreFlowEntities])";
                _dbManSave.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;

                var result = _dbManSave.ExecuteInstruction();
                if (result.success)
                {
                    Global.sysNotification.TsysNotification.showNotification("Data Saved", "Data Saved", Color.CornflowerBlue);
                }
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}