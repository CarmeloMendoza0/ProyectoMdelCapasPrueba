using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoPrueba.Utilidades
{
    public class ClsUtilidades
    {
        private string _mensajeError; //variable global
        private List<TextBox> _lstTxtBox;

        public string MensajeError { get => _mensajeError; set => _mensajeError = value; }
        public List<TextBox> LstTxtBox { get => _lstTxtBox; set => _lstTxtBox = value; }

        public void FormatoDataGridView(ref DataGridView Dgv) //recibe obj por referencia DataGridView 
        {
            //encabezado de las columnas en Negrita
            DataGridViewCellStyle estilo = Dgv.ColumnHeadersDefaultCellStyle; //objeto para cambiar el encabezado de las columnas
            estilo.Alignment = DataGridViewContentAlignment.MiddleCenter; //invocamos el obj estilo y lo ponemos en el centro
            estilo.Font = new Font(Dgv.Font, FontStyle.Bold); //Estilo de funete Negrita 
            Dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill; //que rellene todo, sin espacio en blanco columnas y filas
            Dgv.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;

            Dgv.AllowUserToAddRows = false; //dataGrid usuario para que agregue lineas - falso
            Dgv.AllowUserToDeleteRows = false; //datagRID Agregar linea que permita eliminar de la filas - falso
            Dgv.ReadOnly = true; //dataDrid solo para leer - verdadero
        }

        public void validarTexBox(List<TextBox> LstTxtBox)
        {
            MensajeError = null;

            foreach(TextBox txt in LstTxtBox) //recorrer lista de textbox
            {
                if (txt.Text.Equals(string.Empty))
                {
                    MensajeError = MensajeError + "\n" + txt.Name.Remove(0,3)+ ", No puede estar en blanco";
                }
            }
        }
    }
}
