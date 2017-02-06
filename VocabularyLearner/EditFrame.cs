using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VocabularyLearner
{
    class EditFrame : Form
    {

        public EditFrame(ResultList items)
        {
            SetBounds(300, 400, 600, 600);

            DataTable dataTable = new DataTable();
            DataColumn languageOneColumn = new DataColumn("Language 1", typeof(String));
            DataColumn languageTwoColumn = new DataColumn("Language 2", typeof(String));
            languageOneColumn.Unique = true;
            languageTwoColumn.Unique = true;
            languageOneColumn.AllowDBNull = false;
            languageTwoColumn.AllowDBNull = false;

            dataTable.Columns.Add(languageOneColumn);
            dataTable.Columns.Add(languageTwoColumn);

            foreach (Item x in items)
            {
                dataTable.Rows.Add(new String[] { x.stringOfallResults(false), x.stringOfallResults(true) });
            }


            DataGrid dataGrid = new DataGrid();
            dataGrid.SetBounds(0, 0, 600, 500);
            dataGrid.DataSource = dataTable;
            dataGrid.PreferredColumnWidth = 290;

            
            
            this.BackColor = dataGrid.BackgroundColor;

            this.Controls.Add(dataGrid);
        }
    }
}
