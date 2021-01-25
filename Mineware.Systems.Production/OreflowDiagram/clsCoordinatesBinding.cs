using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Mineware.Systems.Production.OreflowDiagram
{
    public class clsCoordinatesBinding
    {

        public class ObjectCoordinates : INotifyPropertyChanged
        {
            public event PropertyChangedEventHandler PropertyChanged;

            private int? _X1;
            public int? X1Coord { get { return _X1; } set { _X1 = value; NotifyPropertyChanged(); } }
            private int? _Y1;
            public int? Y1Coord { get { return _Y1; } set { _Y1 = value; NotifyPropertyChanged(); } }
            private int? _X2;
            public int? X2Coord { get { return _X2; } set { _X2 = value; NotifyPropertyChanged(); } }
            private int? _Y2;
            public int? Y2Coord { get { return _Y2; } set { _Y2 = value; NotifyPropertyChanged(); } }
            private string _OreflowCode;
            public string OreflowCode { get { return _OreflowCode; } set { _OreflowCode = value; NotifyPropertyChanged(); } }
            private string _OreflowID;
            public string OreflowID { get { return _OreflowID; } set { _OreflowID = value; NotifyPropertyChanged(); } }
            public ObjectCoordinates()
            {

            }

            private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
            {
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

    }
}
