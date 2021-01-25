using System;
using System.Collections.Generic;
using System.Linq;

namespace Mineware.Systems.Production.LineActionManager.Models
{
    public class PrintedForm
    {
        private List<string> _PrintedFromID;
        private string _PDFLocation;
        private bool _Success;
        private string _Error;

        public List<string> PrintedFromID
        {
            get
            {
                return _PrintedFromID;
            }

            set
            {
                _PrintedFromID = value;
            }
        }

        public string PDFLocation
        {
            get
            {
                return _PDFLocation;
            }

            set
            {
                _PDFLocation = value;
            }
        }

        public string Error
        {
            get
            {
                return _Error;
            }

            set
            {
                _Error = value;
            }
        }

        public bool Success
        {
            get
            {
                return _Success;
            }

            set
            {
                _Success = value;
            }
        }
    }
}