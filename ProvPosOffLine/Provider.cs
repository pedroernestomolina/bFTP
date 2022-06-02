using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProvPosOffLine
{
    
    public partial class Provider : IPosOffLine.IProvider
    {

        static EntityConnectionStringBuilder _cnn;
        static MySqlConnectionStringBuilder _cnn2;
        static MySqlConnectionStringBuilder _cnn3;
        static string _bdRemotoInstancia;
        static string _bdRemotaBaseDatos;
        static string _bdLocal;


        public Provider(string pathDB) 
        {
            _bdLocal = pathDB;
            _cnn = new EntityConnectionStringBuilder()
            {
                Metadata = @"res://*/ModelPos.csdl|res://*/ModelPos.ssdl|res://*/ModelPos.msl",
                Provider = @"System.Data.SQLite.EF6",
                ProviderConnectionString = new SqlConnectionStringBuilder()
                {
                    DataSource =pathDB,
                }.ConnectionString
            };

            //var _usuario = "root";
            //var _password = "123";
            //var _instancia = "localhost";
            //var _baseDatos = "bogagalpon";
            //var cadena="Database="+_baseDatos+"; Data Source="+_instancia+"; User Id=" +_usuario+"; Password="+_password+"";
            //_cnn2 = new MySqlConnectionStringBuilder();
            //_cnn2.Database = _baseDatos;
            //_cnn2.UserID = _usuario;
            //_cnn2.Password = _password;
            //_cnn2.Server = _instancia;
        }

        public void
            setServidorRemoto(string instancia, string baseDatos, string usuario="root")
        {
            _bdRemotoInstancia = instancia;
            _bdRemotaBaseDatos = baseDatos;

            var _usuario = usuario;
            var _password = "123";
            var _instancia = instancia ;
            var _baseDatos = baseDatos;
            var cadena = "Database=" + _baseDatos + "; Data Source=" + _instancia + "; User Id=" + _usuario + "; Password=" + _password + "";
            _cnn2 = new MySqlConnectionStringBuilder();
            _cnn2.Database = _baseDatos;
            _cnn2.UserID = _usuario;
            _cnn2.Password = _password;
            _cnn2.Server = _instancia;

            _cnn3 = new MySqlConnectionStringBuilder();
            _cnn3.Database = "pitaTest";
            _cnn3.UserID = "leonuxBD";
            _cnn3.Password = "ghx_k!kibx+D";
            _cnn3.Server = "107.180.50.172";
        }
        public DtoLib.Resultado
            TestBD_Local()
        {
            var result = new DtoLib.Resultado();

            try
            {
            }
            catch (Exception e)
            {
                result.Mensaje = e.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }

            return result;
        }
        public DtoLib.Resultado 
            TestBD_Remoto()
        {
            var result = new DtoLib.Resultado(); 

            try
            {
                using (var cn = new MySqlConnection(_cnn2.ConnectionString))
                {
                    cn.Open();
                };
            }
            catch (MySqlException ex)
            {
                result.Mensaje = ex.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }

            return result;
        }
        public DtoLib.Resultado
            TestBD_Nube()
        {
            var result = new DtoLib.Resultado();

            try
            {
                using (var cn = new MySqlConnection(_cnn3.ConnectionString))
                {
                    cn.Open();
                };
            }
            catch (MySqlException ex)
            {
                result.Mensaje = ex.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }

            return result;
        }
    }

}