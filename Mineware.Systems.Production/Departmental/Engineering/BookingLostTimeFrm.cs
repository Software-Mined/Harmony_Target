using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using Mineware.Systems.GlobalConnect;

namespace Mineware.Systems.Production.Departmental.Engineering
{
    public partial class BookingLostTimeFrm : DevExpress.XtraEditors.XtraForm
    {
        //public BookingPropEq BookingPropEq;
        public BookingLostTimeFrm()
        {
            InitializeComponent();

        }

        BindingSource bs1 = new BindingSource();
        public string VisiblePanel = "";

        public string theSystemDBTag;
        public string UserCurrentInfo;

        private void BookingLostTimeFrm_Load(object sender, EventArgs e)
        {
            //this.Icon = PAS.Properties.Resources.testbutton3;
            ProbGroupGrid.Visible = false;
            ProbGroupGrid.ColumnCount = 1;
            ProbGroupGrid.RowCount = 1000;
            ProbGroupGrid.ColumnCount = 1;


            ProbGroupGrid.Columns[0].Width = 250;
            ProbGroupGrid.Columns[0].HeaderText = "Problem Groups";
            ProbGroupGrid.Columns[0].ReadOnly = true;
            ProbGroupGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            ProbGroupGrid.Columns[0].Frozen = true;

            MWDataManager.clsDataAccess _dbMan = new MWDataManager.clsDataAccess();
            _dbMan.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

            _dbMan.SqlStatement = " select convert(numeric(10),SUBSTRING(description,1,CharIndex('.',description)-1)) id, * from tbl_Code_ProblemGroup order by id  \r\n";
            _dbMan.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbMan.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbMan.ExecuteInstruction();


            ProbGroupGrid.Rows[0].Cells[0].Value = "All";

            int x = 1;

            foreach (DataRow dr in _dbMan.ResultsDataTable.Rows)
            {
                ProbGroupGrid.Rows[x].Cells[0].Value = dr["Description"].ToString();

                x = x + 1;
            }

            ProbGroupGrid.RowCount = x;
            ProbGroupGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;

            //LHDGrid.BackgroundColor = Color.White;
            ProbGroupGrid.BorderStyle = BorderStyle.FixedSingle;

            ProbGroupGrid.Visible = true;

            /////////////////////////////// Load all Problems

            ProbGrid.Visible = false;
            //ProbGrid.ColumnCount = 1;
            //ProbGrid.RowCount = 1000;
            //ProbGrid.ColumnCount = 1;


           

            MWDataManager.clsDataAccess _dbManProb = new MWDataManager.clsDataAccess();
            _dbManProb.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

            _dbManProb.SqlStatement = " select ProblemDesc [Description] from tbl_Code_Problem_Main \r\n";
            _dbManProb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManProb.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManProb.ExecuteInstruction();

            bs1.DataSource = _dbManProb.ResultsDataTable;

            int y = 0;

            ProbGrid.DataSource = bs1;

            ProbGrid.Columns[0].Width = 250;
            ProbGrid.Columns[0].HeaderText = "Problem Groups";
            ProbGrid.Columns[0].ReadOnly = true;
            ProbGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
            ProbGrid.Columns[0].Frozen = true;

            ProbGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;

            ProbGrid.BorderStyle = BorderStyle.FixedSingle;

            ProbGrid.Visible = true;
            
           
        }

        private void CloseBtn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void button1_Click(object sender, EventArgs e)
        {
            LostTimeGrid.Visible = false;

            LostTimeGrid.RowCount = 1;
            LostTimeGrid.ColumnCount = 100;

            for (int i = 0; i < 100; i++)
            {
                LostTimeGrid.Columns[i].Visible = false;
            }

            int x = 0;
            int z = 0;
            int bb = 0;

            for (int i = 0; i <= 14; i++)
            {
                if (Shiftlbl.Text == "Night")
                {
                    bb = i + 20;
                }

                if (Shiftlbl.Text == "Morning")
                {
                    bb = i + 6;
                }

                if (Shiftlbl.Text == "Afternoon")
                {
                    bb = i + 13;
                }

                if (bb == 24)
                {
                    bb = 0;
                }
                if (bb == 25)
                {
                    bb = 1;
                }
                if (bb == 26)
                {
                    bb = 2;
                }
                if (bb == 27)
                {
                    bb = 3;
                }
                if (bb == 28)
                {
                    bb = 4;
                }
                if (bb == 29)
                {
                    bb = 5;
                }
                if (bb == 30)
                {
                    bb = 6;
                }
                if (bb == 31)
                {
                    bb = 7;
                }
                if (bb == 32)
                {
                    bb = 8;
                }
                if (bb == 33)
                {
                    bb = 9;
                }
                if (bb == 34)
                {
                    bb = 10;
                }

                if (bb < 10)
                {
                    LostTimeGrid.Columns[x].HeaderText = "0" + Convert.ToString(bb) + ":00";
                    LostTimeGrid.Columns[x].Visible = true;
                    LostTimeGrid.Columns[x].Width = 35;
                    LostTimeGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    LostTimeGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                }
                else
                {
                    LostTimeGrid.Columns[x].HeaderText = "" + Convert.ToString(bb) + ":00";
                    LostTimeGrid.Columns[x].Visible = true;
                    LostTimeGrid.Columns[x].Width = 35;
                    LostTimeGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    LostTimeGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                }

                LostTimeGrid.Columns[x + 1].HeaderText = "" + Convert.ToString(bb) + ":15";
                LostTimeGrid.Columns[x + 1].Visible = true;
                LostTimeGrid.Columns[x+1].Width = 35;
                LostTimeGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                LostTimeGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                LostTimeGrid.Columns[x + 2].HeaderText = "" + Convert.ToString(bb) + ":30";
                LostTimeGrid.Columns[x + 2].Visible = true;
                LostTimeGrid.Columns[x+2].Width = 35;
                LostTimeGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                LostTimeGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                LostTimeGrid.Columns[x + 3].HeaderText = "" + Convert.ToString(bb) + ":45";
                LostTimeGrid.Columns[x + 3].Visible = true;
                LostTimeGrid.Columns[x+3].Width = 35;
                LostTimeGrid.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                LostTimeGrid.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                




                x = x + 4;

            }

            LostTimeGrid.Visible = true;

            StartTxt.Text = String.Format("{0:hh:ss}", Convert.ToDateTime(LostTimeGrid.Columns[0].HeaderText).ToShortTimeString().Substring(0,5));
            EndTxt.Text = "00:00";
        }

        private void LostTimeGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            

               
                    //if (e.RowIndex == -1)
                    //{

                    //    e.PaintBackground(e.CellBounds, true);
                    //    //e.Graphics.TranslateTransform(e.CellBounds.Left, e.CellBounds.Bottom);
                    //    e.Graphics.RotateTransform(90);
                    //    //e.Graphics.DrawString(LostTimeGrid.Columns[i].HeaderText, e.CellStyle.Font, Brushes.Black, 5, 5);
                    //    //e.Graphics.ResetTransform();
                    //    //e.Handled = true;
                    //}
               
            
        }

      

        private void button2_Click(object sender, EventArgs e)
        {
            //textBox3.Text = Convert.ToString( * 15)
            DurationTxt.Text = Convert.ToString(((LostTimeGrid.SelectedCells.Count-1) * 15));

            if (Convert.ToDecimal(DurationTxt.Text) < 0)
            {
                DurationTxt.Text = "0";
            }

            EndTxt.Text = String.Format("{0:hh:mm}", Convert.ToDateTime(StartTxt.Text).AddMinutes(Convert.ToDouble(DurationTxt.Text)) );

            
        }

        private void LostTimeGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == 0)
            {
                StartTxt.Text = String.Format("{0:hh:nn}", Convert.ToDateTime(LostTimeGrid.Columns[e.ColumnIndex].HeaderText).ToShortTimeString().Substring(0, 5));
            }
        }

        private void LostTimeGrid_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            button2_Click(null, null);
        }

        private void LostTimeGrid_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void ProbGroupGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (ProbGroupGrid.CurrentRow.Cells[0].Value.ToString() == "All")
            {
                ProbGrid.Visible = false;


                MWDataManager.clsDataAccess _dbManProb = new MWDataManager.clsDataAccess();
                _dbManProb.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

                _dbManProb.SqlStatement = " select Description [Description] from PROBLEM  \r\n";
                _dbManProb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManProb.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManProb.ExecuteInstruction();
                //bs1.Clear();

                bs1.DataSource = _dbManProb.ResultsDataTable;

                ProbGrid.Columns[0].Width = 250;
                ProbGrid.Columns[0].HeaderText = "Problem Groups";
                ProbGrid.Columns[0].ReadOnly = true;
                ProbGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                ProbGrid.Columns[0].Frozen = true;

                int y = 0;

                ProbGrid.DataSource = bs1;
                ProbGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;


                ProbGrid.BorderStyle = BorderStyle.FixedSingle;

                ProbGrid.Visible = true;
            }
            else
            {
                MWDataManager.clsDataAccess _dbManProb = new MWDataManager.clsDataAccess();
                _dbManProb.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

                _dbManProb.SqlStatement = " select ProblemDesc [Description] from tbl_Code_Problem_Main where ProblemGroup = '" + ProbGroupGrid.CurrentRow.Cells[0].Value + "'  \r\n";
                _dbManProb.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
                _dbManProb.queryReturnType = MWDataManager.ReturnType.DataTable;
                _dbManProb.ExecuteInstruction();
               

                bs1.DataSource = _dbManProb.ResultsDataTable;

                ProbGrid.Columns[0].Width = 250;
                ProbGrid.Columns[0].HeaderText = "Problem Groups";
                ProbGrid.Columns[0].ReadOnly = true;
                ProbGrid.Columns[0].SortMode = DataGridViewColumnSortMode.NotSortable;
                ProbGrid.Columns[0].Frozen = true;

                int y = 0;

                ProbGrid.DataSource = bs1;
                ProbGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightCyan;


                ProbGrid.BorderStyle = BorderStyle.FixedSingle;

                ProbGrid.Visible = true;
            }
        }

        private void FilterTxt_TextChanged(object sender, EventArgs e)
        {
            
            if (FilterTxt.Text == "")
                bs1.Filter = bs1.Filter;// + string.Format("and [Employee Number] LIKE '{0}%'", '%');
            else
                bs1.Filter = string.Format("[Description] LIKE '%{0}%'", FilterTxt.Text);


        }

        private void SaveBtn_Click(object sender, EventArgs e)
        {
            MWDataManager.clsDataAccess _dbManDelete = new MWDataManager.clsDataAccess();
            _dbManDelete.ConnectionString = TConnections.GetConnectionString(ProductionRes.systemDBTag, UserCurrentInfo);

            _dbManDelete.SqlStatement = " delete from Booking_Delays where bookdate = '" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dateLbl.Text).ToShortDateString()) + "' \r\n";
            _dbManDelete.SqlStatement = _dbManDelete.SqlStatement + " and equipno = '"+EquiNo.Text+"' and starttime = '"+StartTxt.Text+"' and problem = '"+ProbGrid.CurrentRow.Cells[0].Value.ToString()+"' ";
            _dbManDelete.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDelete.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDelete.ExecuteInstruction();

            _dbManDelete.SqlStatement = " insert into Booking_Delays values( '" + String.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dateLbl.Text).ToShortDateString()) + "' , '" + EquiNo.Text + "', '" + Shiftlbl.Text + "', \r\n";
            _dbManDelete.SqlStatement = _dbManDelete.SqlStatement + " '"+StartTxt.Text+"', '"+EndTxt.Text+"','"+DurationTxt.Text+"','"+ProbGrid.CurrentRow.Cells[0].Value+"', '"+Remarkstxt.Text+"' )";
            _dbManDelete.queryExecutionType = MWDataManager.ExecutionType.GeneralSQLStatement;
            _dbManDelete.queryReturnType = MWDataManager.ReturnType.DataTable;
            _dbManDelete.ExecuteInstruction();

            if ((EquiNo.Text.Substring(0, 2) == "LH") || (EquiNo.Text.Substring(0, 2) == "Du"))
            {

            }
            else
            {
                
            }
            
        }

        private void ProbGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SaveBtn.Enabled = true;
        }
    }
}
