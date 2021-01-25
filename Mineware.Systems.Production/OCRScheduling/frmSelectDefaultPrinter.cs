using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.Drawing.Printing;

namespace Mineware.Systems.HarmonyPAS.SysAdminScreens.OCRScheduling
{
    public partial class frmSelectDefaultPrinter : Form
    {
        public frmSelectDefaultPrinter()
        {
            InitializeComponent();
        }

        private void frmSelectDefaultPrinter_Load(object sender, EventArgs e)
        {
            listAllPrinters();
        }

        private void listAllPrinters()
        {
            //foreach (var item in PrinterSettings.InstalledPrinters)
            //{
            //    this.listBox1.Items.Add(item.ToString());
            //}
            //ManagementScope objScope = new ManagementScope(ManagementPath.DefaultPath); //For the local Access
            //objScope.Connect();

            //SelectQuery selectQuery = new SelectQuery();
            //selectQuery.QueryString = "Select * from win32_Printer";
            //ManagementObjectSearcher MOS = new ManagementObjectSearcher(objScope, selectQuery);
            //ManagementObjectCollection MOC = MOS.Get();
            //foreach (ManagementObject mo in MOC)
            //{
            //    listBox1.Items.Add(mo["Name"].ToString());
            //}


            //            System.Management.ObjectQuery oq = new System.Management.ObjectQuery
            //("SELECT * FROM Win32_PrintJob");
            //            ManagementObjectSearcher query1 = new ManagementObjectSearcher(oq);
            //            ManagementObjectCollection queryCollection1 = query1.Get();
            //            foreach (ManagementObject mo in queryCollection1)
            //            {
            //                listBox1.Items.Add(mo["Name"].ToString());
            //            }


            //foreach (var print in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            //{
            //    listBox1.Items.Add(print);
            //}


            //String pkInstalledPrinters;
            //for (int i = 0; i < PrinterSettings.InstalledPrinters.Count; i++)
            //{
            //    pkInstalledPrinters = PrinterSettings.InstalledPrinters[i];
            //    listBox1.Items.Add(pkInstalledPrinters);
            //}


            PrintDialog printDlg = new PrintDialog();

            PrintDocument printDoc = new PrintDocument();

            printDoc.DocumentName = "Print Document";

            printDlg.Document = printDoc;

            printDlg.AllowSelection = true;

            printDlg.AllowSomePages = true;

            //Call ShowDialog

            if (printDlg.ShowDialog() == DialogResult.OK)

                printDoc.Print();

            // return retVal;
        }
    }
}
