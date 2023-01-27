using Entidades.Usuarios;
using LogicaNegocio.Usuarios;
using ProyectoPrueba.Utilidades;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProyectoPrueba.Principal
{
    public partial class FrmUsuario : Form
    {
        //Importamos clases, inicializamos
        private ClsUsuario objUsuario = null;
        private readonly ClsUsuarioLn objUsuarioLn = new ClsUsuarioLn();
        private ClsUtilidades objUtilidades = new ClsUtilidades();  

        public FrmUsuario() //se inicializan los componentes de la ventana
        {
            InitializeComponent();
            CargarListaUsuario();
            LimpiarCampos();
        }

        private void validarCampos()
        {
            objUtilidades = new ClsUtilidades() //llamamos obj utilidades y inicializamos, tambien la lista 
            {
                LstTxtBox = new System.Collections.Generic.List<TextBox>()
            };

            objUtilidades.LstTxtBox.Add(TxtNombre);
            objUtilidades.LstTxtBox.Add(TxtApellido1);
            //objUtilidades.LstTxtBox.Add(TxtApellido2);

            objUtilidades.validarTexBox(objUtilidades.LstTxtBox);

            /*if (TxtNombre.Text.Equals(string.Empty))
            {
                MessageBox.Show(" Campo nombre vacio");
            }*/
        }

        private void LimpiarCampos()
        {
            TxtNombre.Text = string.Empty;
            TxtApellido1.Text = string.Empty;
            TxtApellido2.Text = string.Empty;   
            DtpFechaNacimiento.Value = DateTime.Now;
            ChkEstado.Checked = true;
            LblIdUsuario.Text = string.Empty;

            BtnActualizar.Enabled= false;
            BtnEliminar.Enabled = false;
            BtnAgregar.Enabled = true;
        }

        private void CargarListaUsuario()
        {
            objUsuario = new ClsUsuario();
            objUsuarioLn.Index(ref objUsuario);
            if(objUsuario.MensajeError == null) //si no hay error de usuario
            {
                DgvUsuarios.DataSource= objUsuario.DtResultados;
                objUtilidades.FormatoDataGridView(ref DgvUsuarios); //aplico formato al DATAGridView Usuario
                DgvUsuarios.Columns[0].DisplayIndex = DgvUsuarios.ColumnCount - 1; //colocar la primera columna en la ultima posicion
            }
            else
            {
                MessageBox.Show(objUsuario.MensajeError,"Mensaje de error", MessageBoxButtons.OK,MessageBoxIcon.Error);

            }
        }

        private void FrmUsuario_Load(object sender, EventArgs e)
        {

        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            validarCampos();

            if(objUtilidades.MensajeError==null) //validar mensaje de error
            {
                objUsuario = new ClsUsuario()
                {
                    Nombre = TxtNombre.Text,
                    Apellido1 = TxtApellido1.Text,
                    Apellido2 = TxtApellido2.Text,
                    FechaNacimiento = DtpFechaNacimiento.Value,
                    Estado = ChkEstado.Checked
                };

                objUsuarioLn.Create(ref objUsuario);
                if (objUsuario.MensajeError == null)
                {

                    MessageBox.Show("El ID: " + objUsuario.ValorScalar + ",fue agregado correctamente");
                    CargarListaUsuario();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show(objUsuario.MensajeError, "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(objUtilidades.MensajeError.ToString(), "Mensaje de Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);  
            }
            
         

        }

        private void BtnActualizar_Click(object sender, EventArgs e)
        {
            DialogResult respuesta = MessageBox.Show("¿Seguro de actualizar el registro " + LblIdUsuario.Text + "?", "Mensaje de consulta", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (respuesta == DialogResult.OK)
            {

                objUsuario = new ClsUsuario()
                {
                    IdUsuario = Convert.ToByte(LblIdUsuario.Text),
                    Nombre = TxtNombre.Text,
                    Apellido1 = TxtApellido1.Text,
                    Apellido2 = TxtApellido2.Text,
                    FechaNacimiento = DtpFechaNacimiento.Value,
                    Estado = ChkEstado.Checked
                };

                objUsuarioLn.Update(ref objUsuario);
                if (objUsuario.MensajeError == null)
                {
                    MessageBox.Show("El Usuario fue actualizado correctamente");
                    CargarListaUsuario();
                    LimpiarCampos();
                }
                else
                {
                    MessageBox.Show(objUsuario.MensajeError, "Mensaje de error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void DgvUsuarios_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (DgvUsuarios.Columns[e.ColumnIndex].Name == "Editar")
                {
                    objUsuario = new ClsUsuario()
                    {
                        IdUsuario = Convert.ToByte(DgvUsuarios.Rows[e.RowIndex].Cells["IdUsuario"].Value.ToString()) 
                    };

                    LblIdUsuario.Text = objUsuario.IdUsuario.ToString();

                    objUsuarioLn.Read(ref objUsuario);

                    TxtNombre.Text = objUsuario.Nombre;
                    TxtApellido1.Text = objUsuario.Apellido1;
                    TxtApellido2.Text = objUsuario.Apellido2;
                    DtpFechaNacimiento.Value = objUsuario.FechaNacimiento;
                    ChkEstado.Checked = objUsuario.Estado;

                    BtnActualizar.Enabled = true;
                    BtnEliminar.Enabled = true;
                    BtnAgregar.Enabled = false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void BtnEliminar_Click(object sender, EventArgs e)
        {
            //
            DialogResult respuesta = MessageBox.Show("¿Seguro de eliminar el registro " +LblIdUsuario.Text+ "?", "Mensaje de consulta", MessageBoxButtons.OKCancel,MessageBoxIcon.Question);

            if(respuesta == DialogResult.OK)
            {
                objUsuario = new ClsUsuario()
                {
                    IdUsuario = Convert.ToByte(LblIdUsuario.Text)
                };

                objUsuarioLn.Delete(ref objUsuario);
                CargarListaUsuario();
                LimpiarCampos();
            }
        }
    }
}
