using AccesoDatos.DataBase;
using Entidades.Usuarios;
using System;
using System.Data;

namespace LogicaNegocio.Usuarios
{
    public class ClsUsuarioLn
    {
        #region Variable privadas
        //variable para conectar a DB
        ClsDataBase objDataBase = null;

        #endregion

        #region Método Index
        public void Index(ref ClsUsuario objUsuarios)
        {
            objDataBase = new ClsDataBase()
            {
                NombreTabla = "Usuarios",
                NombreSP = "[SCH_GENERAL].[SP_Usuarios_Index]",
                Scalar = false
            };
            Ejecutar(ref objUsuarios);
        }
        #endregion

        #region CRUD Usuario
        public void Create(ref ClsUsuario objUsuarios)
        {
            objDataBase = new ClsDataBase()
            {
                NombreTabla = "Usuarios",
                NombreSP = "[SCH_GENERAL].[SP_Usuarios_Create]",
                Scalar = true
            };

            //parametros, llamamos obj de la tabla de datosDatatable de parametros
            //Agg el numero de parametro de agregarparametros dB
            objDataBase.DtParametros.Rows.Add(@"@Nombre","17",objUsuarios.Nombre);
            objDataBase.DtParametros.Rows.Add(@"@Apellido1", "17", objUsuarios.Apellido1);
            objDataBase.DtParametros.Rows.Add(@"@Apellido2", "17", objUsuarios.Apellido2);
            objDataBase.DtParametros.Rows.Add(@"@FechaNacimiento", "13", objUsuarios.FechaNacimiento);
            objDataBase.DtParametros.Rows.Add(@"@Estado", "1", objUsuarios.Estado);


            Ejecutar(ref objUsuarios);
        }

        public void Read(ref ClsUsuario objUsuarios)
        {
            objDataBase = new ClsDataBase()
            {
                NombreTabla = "Usuarios",
                NombreSP = "[SCH_GENERAL].[SP_Usuarios_Read]",
                Scalar = false
            };

            objDataBase.DtParametros.Rows.Add(@"@IdUsuario", "2", objUsuarios.IdUsuario);

            Ejecutar(ref objUsuarios);
        }

        public void Update(ref ClsUsuario objUsuarios)
        {
            objDataBase = new ClsDataBase()
            {
                NombreTabla = "Usuarios",
                NombreSP = "[SCH_GENERAL].[SP_Usuarios_Update]",
                //Si quiero que me devuelva algo
                Scalar = true
            };

            objDataBase.DtParametros.Rows.Add(@"@IdUsuario", "2", objUsuarios.IdUsuario);
            objDataBase.DtParametros.Rows.Add(@"@Nombre", "17", objUsuarios.Nombre);
            objDataBase.DtParametros.Rows.Add(@"@Apellido1", "17", objUsuarios.Apellido1);
            objDataBase.DtParametros.Rows.Add(@"@Apellido2", "17", objUsuarios.Apellido2);
            objDataBase.DtParametros.Rows.Add(@"@FechaNacimiento", "13", objUsuarios.FechaNacimiento);
            objDataBase.DtParametros.Rows.Add(@"@Estado", "1", objUsuarios.Estado);

            Ejecutar(ref objUsuarios);
        }

        public void Delete(ref ClsUsuario objUsuarios)
        {
            objDataBase = new ClsDataBase()
            {
                NombreTabla = "Usuarios",
                NombreSP = "[SCH_GENERAL].[SP_Usuarios_Delete]",
                Scalar = true
            };

            objDataBase.DtParametros.Rows.Add(@"@IdUsuario", "2", objUsuarios.IdUsuario);

            Ejecutar(ref objUsuarios);
        }



        #endregion

        #region Métodos privadas
        private void Ejecutar(ref ClsUsuario objUsuario)
        {
            
            objDataBase.CRUD(ref objDataBase);
            if(objDataBase.MensajeErrorDB == null)
            {
                if (objDataBase.Scalar)
                {
                    objUsuario.ValorScalar = objDataBase.ValorScalar;
                }
                else
                {
                    objUsuario.DtResultados = objDataBase.DsResultado.Tables[0];
                    //si solo hay un registro
                    if(objUsuario.DtResultados.Rows.Count == 1)
                    {
                        foreach (DataRow item in objUsuario.DtResultados.Rows)
                        {
                            objUsuario.IdUsuario = Convert.ToByte(item["IdUsuario"].ToString());
                            objUsuario.Nombre = item["Nombre"].ToString();
                            objUsuario.Apellido1 = item["Apellido1"].ToString();
                            objUsuario.Apellido2 = item["Apellido2"].ToString();
                            objUsuario.FechaNacimiento = Convert.ToDateTime(item["FechaNacimiento"].ToString());
                            objUsuario.Estado = Convert.ToBoolean(item["Estado"].ToString());
                        }
                    }
                }
            }
            else //si no es null
            {
                objUsuario.MensajeError = objDataBase.MensajeErrorDB;
            }
        }

        #endregion
    }
}
