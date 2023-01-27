using System;
using System.Data;
using System.Data.SqlClient;

namespace AccesoDatos.DataBase
{
    public class ClsDataBase
    {
        #region Variables privadas

        private SqlConnection _objSqlConnection;
        private SqlDataAdapter _objSqlDataAdapter;
        private SqlCommand _objSqlCommand;
        private DataSet _dsResultado;
        private DataTable _dtParametros;
        private string _nombreTabla, _nombreSP, _mensajeErrorDB, _valorScalar,_nombreDB;
        private bool _scalar;

        #endregion

        #region Variables públicas
        public SqlConnection ObjSqlConnection { get => _objSqlConnection; set => _objSqlConnection = value; }
        public SqlDataAdapter ObjSqlDataAdapter { get => _objSqlDataAdapter; set => _objSqlDataAdapter = value; }
        public SqlCommand ObjSqlCommand { get => _objSqlCommand; set => _objSqlCommand = value; }
        public DataSet DsResultado { get => _dsResultado; set => _dsResultado = value; }
        public DataTable DtParametros { get => _dtParametros; set => _dtParametros = value; }
        public string NombreTabla { get => _nombreTabla; set => _nombreTabla = value; }
        public string NombreSP { get => _nombreSP; set => _nombreSP = value; }
        public string MensajeErrorDB { get => _mensajeErrorDB; set => _mensajeErrorDB = value; }
        public string ValorScalar { get => _valorScalar; set => _valorScalar = value; }
        public string NombreDB { get => _nombreDB; set => _nombreDB = value; }
        public bool Scalar { get => _scalar; set => _scalar = value; }

        #endregion

        #region Constructores
        public ClsDataBase()
        {
            DtParametros = new DataTable("SpParametros");
            DtParametros.Columns.Add("Nombre");
            DtParametros.Columns.Add("TipoDato");
            DtParametros.Columns.Add("Valor");

            //NombreDB = string.Empty;
            NombreDB = "BasePruebas";
        }

        #endregion

        #region Métodos privados
        private void CrearConexionBaseDatos(ref ClsDataBase objdataBase)
        {
            switch (objdataBase.NombreDB)
            {
                case "BasePruebas":
                    objdataBase._objSqlConnection= new SqlConnection(Properties.Settings.Default.cadenaConeccion_BasePruebas);
                    break;
                default: 
                    break;
            }
        }

        private void validarConexionBaseDatos(ref ClsDataBase objdataBase)
        {
            if (objdataBase.ObjSqlConnection.State == ConnectionState.Closed)
            {
                objdataBase.ObjSqlConnection.Open(); //abrilo
            }
            else
            {
                objdataBase.ObjSqlConnection.Close(); //cerrarlo
                objdataBase.ObjSqlConnection.Dispose(); //quitarlo de memoria
            }
        }

        private void AgregarParametros(ref ClsDataBase objdataBase)
        {
            if(objdataBase.DtParametros != null)
            {
                SqlDbType TipoDatoSQL = new SqlDbType();
                foreach (DataRow item in objdataBase.DtParametros.Rows)
                {
                    //validar columna uno
                    switch (item[1])
                    {
                        case "1":
                            TipoDatoSQL = SqlDbType.Bit;
                            break;
                        case "2":
                            TipoDatoSQL = SqlDbType.TinyInt;
                            break;
                        case "3":
                            TipoDatoSQL = SqlDbType.SmallInt;
                            break;
                        case "4":
                            TipoDatoSQL = SqlDbType.Int;
                            break;
                        case "5":
                            TipoDatoSQL = SqlDbType.BigInt;
                            break;
                        case "6":
                            TipoDatoSQL = SqlDbType.Decimal;
                            break;
                        case "7":
                            TipoDatoSQL = SqlDbType.SmallMoney;
                            break;
                        case "8":
                            TipoDatoSQL = SqlDbType.Money;
                            break;
                        case "9":
                            TipoDatoSQL = SqlDbType.Float;
                            break;
                        case "10":
                            TipoDatoSQL = SqlDbType.Real;
                            break;
                        case "11":
                            TipoDatoSQL = SqlDbType.Date;
                            break;
                        case "12":
                            TipoDatoSQL = SqlDbType.Time;
                            break;
                        case "13":
                            TipoDatoSQL = SqlDbType.SmallDateTime;
                            break;
                        case "14":
                            TipoDatoSQL = SqlDbType.DateTime;
                            break;
                        case "15":
                            TipoDatoSQL = SqlDbType.Char;
                            break;
                        case "16":
                            TipoDatoSQL = SqlDbType.NChar;
                            break;
                        case "17":
                            TipoDatoSQL = SqlDbType.VarChar;
                            break;
                        case "18":
                            TipoDatoSQL = SqlDbType.NVarChar;
                            break;
                        default:
                            break;
                    }
                    //si es escalar,agregamos el parametro a sqlcommand, si no a sqladapter
                    if (objdataBase.Scalar)
                    {
                        if (item[2].ToString().Equals(string.Empty)) //si el valor esta vacio
                        {
                            objdataBase.ObjSqlCommand.Parameters.Add(item[0].ToString(),TipoDatoSQL).Value = DBNull.Value;
                        }
                        else //si no es null, le pasamos el item en la posision dos
                        {
                            objdataBase.ObjSqlCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = item[2].ToString();
                        }
                    }
                    else //si no es escalar
                    {
                        if (item[2].ToString().Equals(string.Empty))
                        {
                            objdataBase.ObjSqlDataAdapter.SelectCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = DBNull.Value;
                        }
                        else //si no es null, le pasamos el item en la posision dos
                        {
                            objdataBase.ObjSqlDataAdapter.SelectCommand.Parameters.Add(item[0].ToString(), TipoDatoSQL).Value = item[2].ToString();
                        }
                    }
                }
            }
        }

        private void PrepararConexionBaseDatos(ref ClsDataBase objdataBase)
        {

            CrearConexionBaseDatos(ref objdataBase);
            validarConexionBaseDatos(ref objdataBase);
        }

        private void EjecutarDataAdapter(ref ClsDataBase objdataBase)
        {
            try
            {
                PrepararConexionBaseDatos(ref objdataBase);
                //llamamos al adapter
                objdataBase.ObjSqlDataAdapter = new SqlDataAdapter(objdataBase.NombreSP,objdataBase.ObjSqlConnection);
                //volvemos a llamar a adapter,que tipo de comando le voy a mandar
                objdataBase.ObjSqlDataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
                AgregarParametros(ref objdataBase);
                //rellenar datos en la tabla
                objdataBase.DsResultado = new DataSet();
                objdataBase.ObjSqlDataAdapter.Fill(objdataBase.DsResultado, objdataBase.NombreTabla);

            }
            catch (Exception ex)
            {
                objdataBase.MensajeErrorDB = ex.Message.ToString();
            }
            finally
            {
                //si la conexion de la db esta abierta 
                if (objdataBase.ObjSqlConnection.State == ConnectionState.Open) 
                {
                    validarConexionBaseDatos(ref objdataBase);
                }
            }
        }

        private void EjecutarCommand(ref ClsDataBase objdataBase)
        {
            try
            {
                PrepararConexionBaseDatos(ref objdataBase);
                //llamamos al objcomand, le pasamos sus parametros
                objdataBase.ObjSqlCommand = new SqlCommand(objdataBase.NombreSP, objdataBase.ObjSqlConnection)
                {
                    //le digo que es un procedimiento almacena
                    CommandType = CommandType.StoredProcedure
                };
                AgregarParametros(ref objdataBase);
                //si el objeto de db scalar es true

                if (objdataBase.Scalar)
                {
                    objdataBase.ValorScalar = objdataBase.ObjSqlCommand.ExecuteScalar().ToString().Trim();
                }
                else
                {
                    objdataBase.ObjSqlCommand.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                objdataBase.MensajeErrorDB = ex.Message.ToString();
            }
            finally
            {
                if (objdataBase.ObjSqlConnection.State == ConnectionState.Open)
                {
                    validarConexionBaseDatos(ref objdataBase);
                }
            }
        }

        #endregion

        #region Métodos públicos
        public void CRUD(ref ClsDataBase objdataBase)
        {
            if (objdataBase.Scalar)
            {
                EjecutarCommand(ref objdataBase);
            }
            else
            {
                EjecutarDataAdapter(ref objdataBase);
            }
        }

        #endregion
    }
}
