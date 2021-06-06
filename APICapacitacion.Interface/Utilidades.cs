using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace APICapacitacion.Clases
{
    public static class Utilidades
    {
        public static List<Dictionary<string, object>> ConvertirDataTableADiccionario(DataTable datos) {
            List<Dictionary<string, object>> rows = new List<Dictionary<string, object>>();
            Dictionary<string, object> row;
            foreach (DataRow dr in datos.Rows)
            {
                row = new Dictionary<string, object>();
                foreach (DataColumn col in datos.Columns)
                {
                    row.Add(col.ColumnName, dr[col]);
                }
                rows.Add(row);
            }
            return rows;
        }
    }
}
