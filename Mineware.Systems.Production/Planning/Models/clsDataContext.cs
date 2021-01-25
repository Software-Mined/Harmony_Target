using Mineware.Systems.GlobalExtensions;
using Mineware.Systems.Production.Planning.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Mineware.Systems.Production.Planning
{
    public class clsDataContext
    {

        #region Screen Declerations
        //private static ucGraphicsPreplanning _ucGraphicsPreplanning;
        public static DataTable dtQuestions;
        public static DataTable dtSubQuestions;


        public static readonly DataSet dsGlobal = new DataSet();
        public static List<Questions> questionsData { get; set; }
        public static List<SubQuestions> subQuestionsData { get; set; }

        public static List<SubQuestions> subQuestionsDataEdited { get; set; }




        #endregion


        public static void SetQuestionData(DataTable Questions)
        {
            questionsData = new List<Questions>();
            var questDataTable = Questions.GetDataFromDataTable<Questions>();
            foreach (var row in questDataTable)
            {
                questionsData.Add(row);
            }
        }

        public static void SetSubQuestionData(DataTable SubQuestions)
        {
            dtSubQuestions = SubQuestions;
            subQuestionsData = new List<SubQuestions>();
            var SubQuestDataTable = SubQuestions.GetDataFromDataTable<SubQuestions>();
            foreach (var row in SubQuestDataTable)
            {
                subQuestionsData.Add(row);
            }
        }




    }
}

