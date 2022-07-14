using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProvPosOffLine
{
    
    public partial class Provider : IPosOffLine.IProvider
    {

        public DtoLib.ResultadoEntidad<int> 
            MonitorCambiosBD_GetId_UltimoCambioRegistrado()
        {
            var result = new DtoLib.ResultadoEntidad<int>();

            try
            {
                using (var cn = new MySqlConnection(_cnn2.ConnectionString))
                {
                    cn.Open();

                    try
                    {
                        var sql0 = "";
                        MySqlCommand comando1;
                        int rt = 0;

                        sql0 = "select max(idRef) ultId from monitor_cambios_bd";
                        comando1 = new MySqlCommand(sql0, cn);
                        var reader = comando1.ExecuteReader();
                        while (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                rt = reader.GetInt32("ultId");
                            }
                        }
                        reader.Close();
                        result.Entidad = rt;
                    }
                    catch (Exception ex1)
                    {
                        result.Mensaje = ex1.Message;
                        result.Result = DtoLib.Enumerados.EnumResult.isError;
                    }
                };
            }
            catch (MySqlException ex2)
            {
                result.Mensaje = ex2.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }

            return result;
        }
        public DtoLib.Resultado 
            MonitorCambiosBD_ProcesarCambios(List<DtoLibPosOffLine.Servidor.MonitorCambiosBD.NevoCambio.Ficha> lst)
        {
            var result = new DtoLib.Resultado();

            try
            {
                using (var cn = new MySqlConnection(_cnn2.ConnectionString))
                {
                    cn.Open();
                    MySqlTransaction tr = null;
                    try
                    {
                        tr = cn.BeginTransaction();

                        var sql1 = @"INSERT INTO monitor_cambios_bd (
                                        id,
                                        idRef,
                                        fecha,
                                        descripcion,
                                        cmd,
                                        estatusProcesado)
                                    VALUES (
                                        NULL , ?idRef, ?fecha, ?descripcion, ?cmd, '1')";
                        var comando1 = new MySqlCommand(sql1, cn, tr);
                        var comando2 = new MySqlCommand("", cn, tr);

                        foreach (var rg in lst) 
                        {
                            comando2.Parameters.Clear();
                            comando2.CommandText = rg.cmd;
                            var rt = comando2.ExecuteNonQuery();
                            if (rt == 0)
                            {
                                var msg = "PROBLEMA AL EJECUTAR ACTUALIZACION ";
                                new Exception(msg);
                            }

                            comando1.Parameters.Clear();
                            comando1.Parameters.AddWithValue("?idRef", rg.id);
                            comando1.Parameters.AddWithValue("?fecha", rg.fecha);
                            comando1.Parameters.AddWithValue("?descripcion", rg.descripcion);
                            comando1.Parameters.AddWithValue("?cmd", rg.cmd);
                            rt = comando1.ExecuteNonQuery();
                            if (rt == 0) 
                            {
                                var msg = "PROBLEMA AL INSERTAR NUEVA ACTUALIZACION"; 
                                new Exception(msg);
                            }
                        }
                        tr.Commit();
                    }
                    catch (Exception ex1)
                    {
                        tr.Rollback();
                        result.Mensaje = ex1.Message;
                        result.Result = DtoLib.Enumerados.EnumResult.isError;
                    }
                };
            }
            catch (Exception e)
            {
                result.Mensaje = e.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }

            return result;
        }


        // PARA EL HOST
        public DtoLib.ResultadoLista<DtoLibPosOffLine.Servidor.MonitorCambiosBD.NevoCambio.Ficha> 
            MonitorCambiosBD_Host_GetLista_NuevosCambios_APartirDel_IdRef(int id)
        {
            var result = new DtoLib.ResultadoLista<DtoLibPosOffLine.Servidor.MonitorCambiosBD.NevoCambio.Ficha>();

            try
            {
                using (var cn = new MySqlConnection(_cnn3.ConnectionString))
                {
                    cn.Open();

                    try
                    {
                        var sql0 = "";
                        MySqlCommand comando1;

                        var lst = new List<DtoLibPosOffLine.Servidor.MonitorCambiosBD.NevoCambio.Ficha>();
                        sql0 = @"select id, fecha, descripcion, cmd, 
                                            aplica_pos_offline as aplicaPosOffLine,
                                            aplica_pos_online as aplicaPosOnLine from monitor_cambios_bd where id>?id";
                        comando1 = new MySqlCommand(sql0, cn);
                        comando1.Parameters.AddWithValue("?id",id);
                        var reader = comando1.ExecuteReader();
                        while (reader.Read())
                        {
                            var nr = new DtoLibPosOffLine.Servidor.MonitorCambiosBD.NevoCambio.Ficha()
                            {
                                id = reader.GetInt32("id"),
                                fecha = reader.GetDateTime("fecha"),
                                descripcion = reader.GetString("descripcion"),
                                cmd = reader.GetString("cmd"),
                                aplicaPosOffLine = reader.GetString("aplicaPosOffLine"),
                                aplicaPosOnLine = reader.GetString("aplicaPosOnLine"),
                            };
                            lst.Add(nr);
                        }
                        reader.Close();
                        result.Lista = lst;
                    }
                    catch (Exception ex1)
                    {
                        result.Mensaje = ex1.Message;
                        result.Result = DtoLib.Enumerados.EnumResult.isError;
                    }
                };
            }
            catch (MySqlException ex2)
            {
                result.Mensaje = ex2.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }

            return result;
        }
        public DtoLib.Resultado 
            MonitorCambiosBD_SubirHost_InsertarCambio(DtoLibPosOffLine.Servidor.MonitorCambiosBD.SubirHost.Insertar.Ficha ficha)
        {
            var result = new DtoLib.Resultado();

            try
            {
                using (var cn = new MySqlConnection(_cnn3.ConnectionString))
                {
                    cn.Open();
                    MySqlTransaction tr = null;
                    try
                    {
                        tr = cn.BeginTransaction();

                        var sql0 = @"INSERT INTO `monitor_cambios_bd`
                                    (`id`,
                                    `fecha`,
                                    `descripcion`,
                                    `cmd`,
                                    `aplica_pos_offline`,
                                    `aplica_pos_online`)
                                VALUES
                                    (@id,
                                    @fecha,
                                    @descripcion,
                                    @cmd,
                                    @aplica_pos_offline,
                                    @aplica_pos_online)";
                        var comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.AddWithValue("@id", DBNull.Value);
                        comando1.Parameters.AddWithValue("@fecha", ficha.fecha );
                        comando1.Parameters.AddWithValue("@descripcion", ficha.descripcion);
                        comando1.Parameters.AddWithValue("@cmd", ficha.cmd);
                        comando1.Parameters.AddWithValue("@aplica_pos_offline", ficha.aplicaPosOffLine);
                        comando1.Parameters.AddWithValue("@aplica_pos_online", ficha.aplicaPosOnLine);
                        comando1.ExecuteNonQuery();
                        tr.Commit();
                    }
                    catch (Exception ex1)
                    {
                        tr.Rollback();
                        result.Mensaje = ex1.Message;
                        result.Result = DtoLib.Enumerados.EnumResult.isError;
                    }
                };
            }
            catch (Exception e)
            {
                result.Mensaje = e.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }

            return result;
        }

    }

}