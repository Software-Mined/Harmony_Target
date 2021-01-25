using System;
using System.Data;
using System.Linq;

namespace Mineware.Systems.Production.LineActionManager.Models
{
    public class FormsAPI
    {
        private int _FormsID;
        private string _Name;
        private string _Description;
        private string _FileName;
        private string _TableName;
        private DataTable _UniqueDataStructure;

        public int FormsID
        {
            get { return _FormsID; }
            set { _FormsID = value; }
        }
        public string Name { get { return _Name; } set { _Name = value; } }
        public string Description { get { return _Description; } set { _Description = value; } }

        public DataTable UniqueDataStructure
        {
            get
            {
                return _UniqueDataStructure;
            }

            set
            {
                _UniqueDataStructure = value;
            }
        }

        public string FileName
        {
            get
            {
                return _FileName;
            }

            set
            {
                _FileName = value;
            }
        }

        public string TableName
        {
            get
            {
                return _TableName;
            }

            set
            {
                _TableName = value;
            }
        }
    }
}