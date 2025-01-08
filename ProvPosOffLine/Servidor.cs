using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProvPosOffLine
{
    public class DataKardexResumen
    {
        public string autoProducto { get; set; }
        public string autoDeposito { get; set; }
        public decimal cnt { get; set; }
    }

    public partial class Provider : IPosOffLine.IProvider
    {

        public DtoLib.Resultado 
            Servidor_Test()
        {
            var result = new DtoLib.ResultadoEntidad<int>();

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
            Servidor_Principal_CrearBoletin(string pathDestino, DateTime fechaMovInv)
        {
            var result = new DtoLib.Resultado();

            try
            {
                using (var cn = new MySqlConnection(_cnn2.ConnectionString))
                {
                    cn.Open();
                    var sql0 = "";
                    MySqlCommand comando1;
                    var rt = -1;

                    //SE ENVIA FECHA GESTION DE MOV INVENTARIO, PARA LAS SUCURSALES
                    var fechaMov=fechaMovInv;
                    sql0 = "select @fechaMov, now() as fechaBoletin into outfile \"" + pathDestino + "fecha_gestion_ftp.txt\"";
                    comando1 = new MySqlCommand(sql0, cn);
                    comando1.Parameters.AddWithValue("@fechaMov", fechaMovInv.Date);
                    rt = comando1.ExecuteNonQuery();
                    //

                    
                    sql0 = "select * into outfile \"" + pathDestino + "usuarios_grupo.txt\" from usuarios_grupo";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "usuarios.txt\" from usuarios";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "usuarios_grupo_permisos.txt\" from usuarios_grupo_permisos";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "empresa.txt\" from empresa";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "empresa_departamentos.txt\" from empresa_departamentos";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "empresa_tasas.txt\" from empresa_tasas";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "sistema_configuracion.txt\" from sistema_configuracion";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "productos.txt\" from productos";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "productos_alterno.txt\" from productos_alterno";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "productos_deposito.txt\" from productos_deposito";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "productos_grupo.txt\" from productos_grupo";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "productos_lista.txt\" from productos_lista";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "productos_marca.txt\" from productos_marca";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "proveeodres_grupo.txt\" from proveedores_grupo";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "proveeodres.txt\" from proveedores";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "compras.txt\" from compras where tipo='04' and  FECHA >= DATE_SUB(CURDATE(), INTERVAL 2 MONTH)";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "compras_detalle.txt\" from compras_detalle where tipo='04' and  FECHA >= DATE_SUB(CURDATE(), INTERVAL 2 MONTH)";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    ////NO ES NECESARIO ENVIAR HISTORICO DE PRECIOS
                    sql0 = "select * into outfile \"" + pathDestino + "productos_precios.txt\" from productos_precios where FECHA >= DATE_SUB(CURDATE(), INTERVAL 1 WEEK)";
                    //sql0 = "select * into outfile \"" + pathDestino + "productos_precios.txt\" from productos_precios where 1=0";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    ////NO ES NECESARIO ENVIAR CLIENTES
                    //sql0 = "select * into outfile \"" + pathDestino + "clientes.txt\" FROM clientes where auto >'0900000001'";
                    sql0 = "select * into outfile \"" + pathDestino + "clientes.txt\" FROM clientes where 1=0";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    //SE ENVIAN MOVIMIENTOS A TRAVEZ DE PARAMETRO DE ENTRADA
                    //sql0 = "select * into outfile \"" + pathDestino + "productos_kardex.txt\" FROM productos_kardex where modulo<>'Ventas' and fecha>='2022/01/01'";
                    sql0 = "select * into outfile \"" + pathDestino + "productos_kardex.txt\" FROM productos_kardex where modulo<>'Ventas' and fecha>=@fechaMov";
                    comando1 = new MySqlCommand(sql0, cn);
                    comando1.Parameters.AddWithValue("@fechaMov",fechaMov);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "empresa_grupo.txt\" FROM empresa_grupo";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "empresa_sucursal.txt\" FROM empresa_sucursal";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "sistema_menu.txt\" FROM sistema_menu";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "sistema_funciones.txt\" FROM sistema_funciones";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "empresa_depositos.txt\" FROM empresa_depositos";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    //SE ENVIAN MOVIMIENTOS A TRAVEZ DE PARAMETRO DE ENTRADA
                    //sql0 = "select * into outfile \"" + pathDestino + "productos_movimientos.txt\" FROM productos_movimientos where fecha>='2022/01/01' ";
                    sql0 = "select * into outfile \"" + pathDestino + "productos_movimientos.txt\" FROM productos_movimientos where fecha>=@fechaMov";
                    comando1 = new MySqlCommand(sql0, cn);
                    comando1.Parameters.AddWithValue("@fechaMov", fechaMov);
                    rt = comando1.ExecuteNonQuery();

                    //SE ENVIAN MOVIMIENTOS A TRAVEZ DE PARAMETRO DE ENTRADA
                    //sql0 = "select * into outfile \"" + pathDestino + "productos_movimientos_detalle.txt\" FROM productos_movimientos_detalle where fecha>='2022/01/01' ";
                    sql0 = "select * into outfile \"" + pathDestino + "productos_movimientos_detalle.txt\" FROM productos_movimientos_detalle where fecha>=@fechaMov";
                    comando1 = new MySqlCommand(sql0, cn);
                    comando1.Parameters.AddWithValue("@fechaMov", fechaMov);
                    rt = comando1.ExecuteNonQuery();

                    //SE ENVIAN MOVIMIENTOS A TRAVEZ DE PARAMETRO DE ENTRADA
                    sql0 = "select movExt.* into outfile \"" + pathDestino + "productos_movimientos_extra.txt\" FROM productos_movimientos_extra as movExt join productos_movimientos as mov on movExt.auto_movimiento=mov.auto where fecha>=@fechaMov";
                    comando1 = new MySqlCommand(sql0, cn);
                    comando1.Parameters.AddWithValue("@fechaMov", fechaMov);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "productos_medida.txt\" FROM productos_medida";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "productos_ext.txt\" FROM productos_ext";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "empresa_hnd_precios.txt\" FROM empresa_hnd_precios";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "empresa_grupo_ext.txt\" FROM empresa_grupo_ext";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "empresa_sucursal_ext.txt\" FROM empresa_sucursal_ext";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    //
                    sql0 = "select * into outfile \"" + pathDestino + "empresa_depositos_ext.txt\" FROM empresa_depositos_ext";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "tomainv_solicitud.txt\" FROM tomainv_solicitud";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();

                    sql0 = "select * into outfile \"" + pathDestino + "tomainv_solicitud_detalle.txt\" FROM tomainv_solicitud_detalle";
                    comando1 = new MySqlCommand(sql0, cn);
                    rt = comando1.ExecuteNonQuery();
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
            Servidor_Principal_InsertarCierre(string pathOrigen)
        {
            var result = new DtoLib.Resultado();
            var listMv = new List<DataKardexResumen>();

            try
            {
                using (var cn = new MySqlConnection(_cnn2.ConnectionString))
                {
                    cn.Open();

                    MySqlTransaction tr = null;
                    try
                    {
                        tr = cn.BeginTransaction();


                        var sql0 = "";
                        MySqlCommand comando1;
                        var rt = -1;


                        sql0 = "SET FOREIGN_KEY_CHECKS=0";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //VENTAS
                        sql0 = "load data infile \"" + pathOrigen + "ventas.txt\" into table ventas";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathOrigen + "ventas_detalle.txt\" into table ventas_detalle";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //CXC
                        sql0 = "load data infile \"" + pathOrigen + "cxc.txt\" into table cxc";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathOrigen + "cxc_recibos.txt\" into table cxc_recibos";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathOrigen + "cxc_documentos.txt\" into table cxc_documentos";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathOrigen + "cxc_medio_pago.txt\" into table cxc_medio_pago";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //KARDEX
                        sql0 = "load data infile \"" + pathOrigen + "productos_kardex.txt\" into table productos_kardex";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //ARQUEO
                        sql0 = "load data infile \"" + pathOrigen + "pos_arqueo.txt\" into table pos_arqueo";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //COMPRAS
                        sql0 = "load data infile \"" + pathOrigen + "compras.txt\" into table compras";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathOrigen + "compras_detalle.txt\" into table compras_detalle";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //PRODUCTOS MOVIMIENTOS
                        sql0 = "load data infile \"" + pathOrigen + "productos_movimientos.txt\" into table productos_movimientos";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathOrigen + "productos_movimientos_detalle.txt\" into table productos_movimientos_detalle";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //ACTULIZAR DEPOSITO
                        sql0 = "delete from productos_kardex_resumen";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathOrigen + "productos_kardex_resumen.txt\" into table productos_kardex_resumen";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "select auto_producto, auto_deposito, cnt from productos_kardex_resumen";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        var reader = comando1.ExecuteReader();
                        while (reader.Read())
                        {
                            var nr = new DataKardexResumen()
                            {
                                autoProducto = reader.GetString("auto_producto"),
                                autoDeposito = reader.GetString("auto_deposito"),
                                cnt = reader.GetDecimal("cnt")
                            };
                            listMv.Add(nr);
                        }
                        reader.Close();

                        sql0 = "update productos_deposito set fisica=fisica+?cnt, disponible=disponible+?cnt where auto_producto=?ap and auto_deposito=?ad";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        foreach (var mv in listMv)
                        {
                            comando1.Parameters.Clear();
                            comando1.Parameters.AddWithValue("?cnt", mv.cnt);
                            comando1.Parameters.AddWithValue("?ap", mv.autoProducto);
                            comando1.Parameters.AddWithValue("?ad", mv.autoDeposito);
                            rt = comando1.ExecuteNonQuery();
                        }


                        //TOMAS DE INVENTARIO
                        sql0 = "load data infile \"" + pathOrigen + "tomainv.txt\" into table tomainv";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        //rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathOrigen + "tomainv_detalle.txt\" into table tomainv_detalle";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        //rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathOrigen + "tomainv_conteo.txt\" into table tomainv_conteo";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        //rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathOrigen + "tomainv_result.txt\" into table tomainv_result";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        //rt = comando1.ExecuteNonQuery();
                        //


                        sql0 = "SET FOREIGN_KEY_CHECKS=1";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

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
            catch (MySqlException ex2)
            {
                result.Mensaje = ex2.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }

            return result;
        }
        public DtoLib.Resultado 
            Servidor_Principal_PreprararCierre(string codigoEmpresa, string rutaLeonuxBandeja, string rutaLeonuxFtpBandejaData)
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
                        var sql0 = "";
                        MySqlCommand comando1;
                        var rt = -1;

                        var pathBandeja = rutaLeonuxBandeja;
                        var pathTemp = rutaLeonuxBandeja + @"/temp";
                        var pathDestino = rutaLeonuxBandeja + @"/temp/";
                        var pathFtpData = rutaLeonuxFtpBandejaData;

                        tr = cn.BeginTransaction();

                        //VENTAS
                        sql0 = "select * into outfile \"" + pathDestino + "ventas.txt\" FROM ventas where (tipo='01' or tipo='02' or tipo='03' or tipo='04') and cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "select * into outfile \"" + pathDestino + "ventas_detalle.txt\" FROM ventas_detalle where (tipo='01' or tipo='02' or tipo='03' or tipo='04') and cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //CXC
                        sql0 = "select * into outfile \"" + pathDestino + "cxc.txt\" FROM cxc where cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "select * into outfile \"" + pathDestino + "cxc_recibos.txt\" FROM cxc_recibos where cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "select * into outfile \"" + pathDestino + "cxc_documentos.txt\" FROM cxc_documentos where cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "select * into outfile \"" + pathDestino + "cxc_medio_pago.txt\" FROM cxc_medio_pago where cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //MOV KARDEX
                        sql0 = "select * into outfile \"" + pathDestino + "productos_kardex.txt\" FROM productos_kardex where  modulo='Ventas' and cierre_ftp='' ";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //JORNADA
                        sql0 = "SELECT NULL as id,fecha,estatus_cierre INTO OUTFILE \"" + pathDestino + "pos_jornadas.txt\" FROM pos_jornadas where cierre_ftp='' ";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //ARQUEO
                        sql0 = "select * into OUTFILE \"" + pathDestino + "pos_arqueo.txt\" FROM pos_arqueo where cierre_ftp='' ";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //COMPRAS
                        sql0 = "select * into OUTFILE \"" + pathDestino + "compras.txt\" FROM compras where tipo='05' and cierre_ftp='' ";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "select * into OUTFILE \"" + pathDestino + "compras_detalle.txt\" FROM compras_detalle where tipo='05' and cierre_ftp='' ";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //KARDEX RESUMEN
                        sql0 = "select auto_producto, sum(cantidad_und*signo) as cnt, auto_deposito " +
                               "into outfile \"" + pathDestino + "productos_kardex_resumen.txt\"" +
                               "FROM productos_kardex where estatus_anulado='0' and modulo='Ventas' and cierre_ftp='' " +
                               "group by auto_producto, auto_deposito";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //PRODUCTOS MOVIMIENTOS
                        sql0 = "select * into OUTFILE \"" + pathDestino + "productos_movimientos.txt\" FROM productos_movimientos where tipo='05' and cierre_ftp='' ";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "select * into OUTFILE \"" + pathDestino + "productos_movimientos_detalle.txt\" FROM productos_movimientos_detalle where tipo='05' and cierre_ftp='' ";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //TOMAS DE INVENTARIO
                        sql0 = "select * into OUTFILE \"" + pathDestino + "tomainv.txt\" FROM tomainv where estatusProcesado='1' and cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "select detalle.* into OUTFILE \"" + pathDestino + "tomainv_detalle.txt\" FROM " + @"tomainv_detalle as detalle 
                                                                                                            join tomainv as toma on toma.auto=detalle.auto_tomainv
                                                                                                            where toma.estatusProcesado='1' 
                                                                                                            and toma.cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "select conteo.* into OUTFILE \"" + pathDestino + "tomainv_conteo.txt\" FROM " + @"tomainv_conteo as conteo 
                                                                                                            join tomainv as toma on toma.auto=conteo.auto_tomainv
                                                                                                            where toma.estatusProcesado='1' 
                                                                                                            and toma.cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "select result.* into OUTFILE \"" + pathDestino + "tomainv_result.txt\" FROM " + @"tomainv_result as result 
                                                                                                            join tomainv as toma on toma.auto=result.auto_tomainv
                                                                                                            where toma.estatusProcesado='1' 
                                                                                                            and toma.cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //EMPAQUETAR CIERRE
                        var fecha = DateTime.Now;
                        var df = "data" + codigoEmpresa + "_";
                        df += fecha.Year.ToString() + "_";
                        df += fecha.Month.ToString().Trim().PadLeft(2, '0') + "_";
                        df += fecha.Day.ToString().Trim().PadLeft(2, '0') + "_";
                        df += "h_" + fecha.Hour.ToString().Trim().PadLeft(2, '0') + "_";
                        df += fecha.Minute.ToString().Trim().PadLeft(2, '0');
                        df += ".zip";

                        var destino = "";
                        destino += pathBandeja + @"/" + df;
                        ZipFile.CreateFromDirectory(pathTemp, destino, CompressionLevel.Fastest, false);


                        //TRASLADAR ARCHIVO A DESTINO
                        string sourceFile = System.IO.Path.Combine(pathBandeja, df);
                        string destFile = System.IO.Path.Combine(pathFtpData, df);
                        System.IO.File.Copy(sourceFile, destFile, true);


                        //ACTUALIZAR TABLAS 
                        sql0 = "update sistema_contadores set a_cierre_ftp=a_cierre_ftp+1";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "select a_cierre_ftp from sistema_contadores";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        var v = comando1.ExecuteScalar();
                        if (v == null)
                        {
                        }

                        var cierre = int.Parse(v.ToString());
                        var aCierre = cierre.ToString().Trim().PadLeft(10, '0');


                        //VENTAS
                        sql0 = "update ventas set cierre_ftp=?cierre where (tipo='01' or tipo='02' or tipo='03' or tipo='04') and cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "update ventas_detalle set cierre_ftp=?cierre where (tipo='01' or tipo='02' or tipo='03' or tipo='04') and cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();


                        //CXC
                        sql0 = "update cxc set cierre_ftp=?cierre  where cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "update cxc_recibos set cierre_ftp=?cierre  where cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "update cxc_documentos set cierre_ftp=?cierre  where cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "update cxc_medio_pago set cierre_ftp=?cierre  where cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();


                        //PRODUCTOS KARDEX
                        sql0 = "update productos_kardex set cierre_ftp=?cierre  where modulo='Ventas' and cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();


                        //POS JORNADAS 
                        sql0 = "update pos_jornadas set cierre_ftp=?cierre where cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();


                        //POS ARQUEO
                        sql0 = "update pos_arqueo set cierre_ftp=?cierre where cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();


                        //COMPRAS
                        sql0 = "update compras set cierre_ftp=?cierre where tipo='05' and cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "update compras_detalle set cierre_ftp=?cierre where tipo='05' and cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();


                        //PRODUCTOS MOVIMIENTOS
                        sql0 = "update productos_movimientos set cierre_ftp=?cierre where tipo='05' and cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "update productos_movimientos_detalle set cierre_ftp=?cierre where tipo='05' and cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();

                        //TOMAS INVENTARIO
                        sql0 = "update tomainv set cierre_ftp=?cierre where estatusProcesado='1' and cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = @"update 
                                tomainv_detalle as detalle 
                                join tomainv as toma on toma.auto=detalle.auto_tomainv
                                set detalle.cierre_ftp=?cierre 
                                where toma.estatusProcesado='1' and detalle.cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = @"update 
                                tomainv_conteo as conteo
                                join tomainv as toma on toma.auto=conteo.auto_tomainv
                                set conteo.cierre_ftp=?cierre 
                                where toma.estatusProcesado='1' and conteo.cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = @"update 
                                tomainv_result as result
                                join tomainv as toma on toma.auto=result.auto_tomainv
                                set result.cierre_ftp=?cierre 
                                where toma.estatusProcesado='1' and result.cierre_ftp=''";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?cierre", aCierre);
                        rt = comando1.ExecuteNonQuery();

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
            catch (MySqlException ex2)
            {
                result.Mensaje = ex2.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }

            return result;
        }
        public DtoLib.Resultado 
            Servidor_Principal_InsertarBoletin(string codigoSuc, string rutaArchivoTxt)
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
                        var sql0 = "";
                        MySqlCommand comando1;
                        var rt = -1;

                        var pathData = rutaArchivoTxt;

                        tr = cn.BeginTransaction();

                        //DESACTIVAR RESTRICCIONES FORANEAS
                        sql0 = "SET FOREIGN_KEY_CHECKS=0";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        //TABLA TEMPORAL PARA USO DEL FTP, TRAE LA FECHA QUE INDICA DESDE CUANDO SE PODRAN ELIMINAR LOS MOV DE INVENTARIO
                        sql0 = "DROP TABLE IF EXISTS gestion_ftp;";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "CREATE TABLE gestion_ftp (`fecha_mov_inv` DATE NOT NULL, `fecha_boletin` DATETIME NOT NULL) ENGINE = InnoDB;";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/fecha_gestion_ftp.txt\" into table gestion_ftp";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        var fecMovInv = DateTime.Now.Date;
                        sql0 = @"select fecha_mov_inv as fechaMovInv from gestion_ftp";
                        comando1 = new MySqlCommand(sql0, cn);
                        var reader = comando1.ExecuteReader();
                        while (reader.Read())
                        {
                            fecMovInv = reader.GetDateTime("fechaMovInv").Date;
                        }
                        reader.Close();
                        //


                        //LIMPIANDO TABLAS
                        sql0 = "delete from sistema_configuracion";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from usuarios_grupo_permisos";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from usuarios";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from usuarios_grupo";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from empresa";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from empresa_tasas";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from empresa_departamentos";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from productos_alterno";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from productos_deposito";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from productos";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from productos_grupo";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from productos_marca";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from productos_lista";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        //

                        sql0 = "delete from proveedores_grupo";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from proveedores";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from compras_detalle WHERE TIPO='04'";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from compras WHERE TIPO='04'";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        //SI HACE FALTA RECIBIR HISTORICO DE PRECIOS
                        sql0 = "delete from productos_precios";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        ////NO HACE FALTA RECIBIR CLIENTES
                        //sql0 = "delete from clientes where auto > '0900000001'";
                        //comando1 = new MySqlCommand(sql0, cn, tr);
                        //comando1.CommandTimeout = int.MaxValue;
                        //rt = comando1.ExecuteNonQuery();

                        //

                        //sql0 = "delete from productos_kardex where modulo <> 'Ventas' and fecha>='2022/01/01'";
                        sql0 = "delete from productos_kardex where modulo <> 'Ventas' and fecha>=@fechaMov";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.AddWithValue("@fechaMov", fecMovInv);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from empresa_grupo";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from empresa_sucursal";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from sistema_menu";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from sistema_funciones";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from empresa_depositos";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        //MOVIMIENTOS EXTRA
                        sql0 = @"delete movExt
                                    from productos_movimientos_extra as movExt 
                                    join productos_movimientos as mov on movExt.auto_movimiento=mov.auto
                                    where mov.fecha>=@fechaMov";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.AddWithValue("@fechaMov", fecMovInv);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        //MOVIMIENTOS DETALLE
                        sql0 = "delete from productos_movimientos_detalle where fecha>=@fechaMov";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.AddWithValue("@fechaMov", fecMovInv);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        //sql0 = "delete from productos_movimientos where fecha>='2022/01/01'";
                        sql0 = "delete from productos_movimientos where fecha>=@fechaMov";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.Parameters.AddWithValue("@fechaMov", fecMovInv);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from productos_medida";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from productos_ext";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from empresa_hnd_precios";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from empresa_grupo_ext";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from empresa_sucursal_ext";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from empresa_depositos_ext";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        //
                        sql0 = "delete from tomainv_solicitud_detalle";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from tomainv_solicitud";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();


                        // PROCESO DE INSERTAR
                        sql0 = "load data infile \"" + pathData + "/sistema_configuracion.txt\" into table sistema_configuracion";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/usuarios_grupo.txt\" into table usuarios_grupo";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/usuarios.txt\" into table usuarios";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/usuarios_grupo_permisos.txt\" into table usuarios_grupo_permisos";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/empresa.txt\" into table empresa";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/empresa_tasas.txt\" into table empresa_tasas";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/empresa_departamentos.txt\" into table empresa_departamentos";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/productos_alterno.txt\" into table productos_alterno";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/productos_deposito.txt\" into table productos_deposito";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/productos.txt\" into table productos";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/productos_grupo.txt\" into table productos_grupo";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/productos_marca.txt\" into table productos_marca";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/productos_lista.txt\" into table productos_lista";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        //

                        sql0 = "load data infile \"" + pathData + "/proveeodres_grupo.txt\" into table proveedores_grupo";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/proveeodres.txt\" into table proveedores";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/compras.txt\" into table compras";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/compras_detalle.txt\" into table compras_detalle";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        //SI HACE FALTA RECIBIR HISTORICO DE PRECIOS
                        sql0 = "load data infile \"" + pathData + "/productos_precios.txt\" into table productos_precios";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        ////NO HACE FALTA RECIBIR CLIENTES
                        //sql0 = "load data infile \"" + pathData + "/clientes.txt\" into table clientes";
                        //comando1 = new MySqlCommand(sql0, cn, tr);
                        //comando1.CommandTimeout = int.MaxValue;
                        //rt = comando1.ExecuteNonQuery();

                        //
                        sql0 = "load data infile \"" + pathData + "/productos_kardex.txt\" into table productos_kardex";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        //// NO PUEDE ELIMINAR MOVIMIENTOS BASANDOME EN CODIGO DE LA SUCURSAL, DEBE SER CON EL AUTO DEL DEPOSITO
                        //sql0 = "delete from productos_kardex where codigo_sucursal<>?codigoSucursal";
                        //comando1 = new MySqlCommand(sql0, cn, tr);
                        //comando1.CommandTimeout = int.MaxValue;
                        //comando1.Parameters.Clear();
                        //comando1.Parameters.AddWithValue("?codigoSucursal", codigoSuc);
                        //rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/empresa_grupo.txt\" into table empresa_grupo";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/empresa_sucursal.txt\" into table empresa_sucursal";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/sistema_menu.txt\" into table sistema_menu";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/sistema_funciones.txt\" into table sistema_funciones";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/empresa_depositos.txt\" into table empresa_depositos";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        //AGREGAR TABLA NUEVA
                        sql0 = "load data infile \"" + pathData + "/empresa_depositos_ext.txt\" into table empresa_depositos_ext";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        //
                        sql0 = "load data infile \"" + pathData + "/tomainv_solicitud.txt\" into table tomainv_solicitud";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        //rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/tomainv_solicitud_detalle.txt\" into table tomainv_solicitud_detalle";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        //rt = comando1.ExecuteNonQuery();

                        sql0 = @"delete detalle 
                                    from tomainv_solicitud_detalle as detalle
                                    join tomainv_solicitud as solicitud on detalle.auto_solicitud=solicitud.auto 
                                    where solicitud.codigoSucursal<>?codigoSucursal";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?codigoSucursal", codigoSuc);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = @"delete solicitud
                                    from tomainv_solicitud as solicitud
                                    where solicitud.codigoSucursal<>?codigoSucursal";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?codigoSucursal", codigoSuc);
                        rt = comando1.ExecuteNonQuery();
                        //


                        sql0 = @"delete dpExt
                                    from empresa_depositos_ext as dpExt 
                                    join empresa_depositos as dp on dp.auto=dpExt.auto_deposito 
                                    where dp.codigo_sucursal<>?codigoSucursal";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?codigoSucursal", codigoSuc);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "delete from empresa_depositos where codigo_sucursal<>?codigoSucursal";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?codigoSucursal", codigoSuc);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/productos_movimientos.txt\" into table productos_movimientos";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();


                        ////NO PUEDE ELIMINAR MOVIMIENTOS BASANDOME EN EL CODIGO DE LA SUCURSAL
                        //sql0 = "delete from productos_movimientos where codigo_sucursal<>?codigoSucursal";
                        //comando1 = new MySqlCommand(sql0, cn, tr);
                        //comando1.CommandTimeout = int.MaxValue;
                        //comando1.Parameters.Clear();
                        //comando1.Parameters.AddWithValue("?codigoSucursal", codigoSuc);
                        ////rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/productos_movimientos_detalle.txt\" into table productos_movimientos_detalle";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/productos_movimientos_extra.txt\" into table productos_movimientos_extra";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();


                        sql0 = "load data infile \"" + pathData + "/productos_medida.txt\" into table productos_medida";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        //nuevo
                        sql0 = "load data infile \"" + pathData + "/productos_ext.txt\" into table productos_ext";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/empresa_hnd_precios.txt\" into table empresa_hnd_precios";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/empresa_grupo_ext.txt\" into table empresa_grupo_ext";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "load data infile \"" + pathData + "/empresa_sucursal_ext.txt\" into table empresa_sucursal_ext";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();
                        
                        sql0 = "update sistema set deposito_principal=(select autodepositoprincipal from empresa_sucursal where codigo=?codigoSucursal)";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?codigoSucursal", codigoSuc);
                        rt = comando1.ExecuteNonQuery();

                        //ESTADO NORMAL RESTRICCIONES FORANEAS
                        sql0 = "SET FOREIGN_KEY_CHECKS=1";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        rt = comando1.ExecuteNonQuery();

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
            catch (MySqlException ex2)
            {
                result.Mensaje = ex2.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }

            return result;
        }

        //

        public DtoLib.Resultado 
            Servidor_Principal_EliminarMovimientosKardexExcluyeDeposito(string idDeposito)
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
                        var sql0 = "";
                        MySqlCommand comando1;
                        var rt = -1;

                        tr = cn.BeginTransaction();
                        //
                        sql0 = "delete from productos_kardex where auto_deposito<>?idDeposito";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        comando1.CommandTimeout = int.MaxValue;
                        comando1.Parameters.Clear();
                        comando1.Parameters.AddWithValue("?idDeposito", idDeposito);
                        rt = comando1.ExecuteNonQuery();
                        //
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
            catch (MySqlException ex2)
            {
                result.Mensaje = ex2.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }

            return result;
        }
        public DtoLib.ResultadoEntidad<string> 
            Sucursal_GetIdDepositoPrincipal_ByCodigoSucursal(string codSucursal)
        {
            var result = new DtoLib.ResultadoEntidad<string>();

            try
            {
                var rt = "";
                using (var cn = new MySqlConnection(_cnn2.ConnectionString))
                {
                    cn.Open();
                    var sql0 = "";
                    MySqlCommand comando1;

                    sql0 = @"select autoDepositoPrincipal as autoDeposito from empresa_sucursal where codigo=@codSucursal";
                    comando1 = new MySqlCommand(sql0, cn);
                    comando1.Parameters.AddWithValue("@codSucursal", codSucursal);
                    var reader = comando1.ExecuteReader();
                    while (reader.Read())
                    {
                        rt = reader.GetString("autoDeposito");
                    }
                    reader.Close();
                };
                result.Entidad = rt;
            }
            catch (MySqlException ex2)
            {
                result.Mensaje = ex2.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }

            return result;
        }
        public DtoLib.Resultado
            Servidor_Principal_ActualizarInventarioDeposito()
        {
            var result = new DtoLib.Resultado();
            var listMv = new List<DataKardexResumen>();

            try
            {
                using (var cn = new MySqlConnection(_cnn2.ConnectionString))
                {
                    cn.Open();

                    MySqlTransaction tr = null;
                    try
                    {
                        MySqlCommand comando1;
                        var rt = -1;
                        tr = cn.BeginTransaction();

                        var sql0 = @"select 
                                        autoPrd, autoDep, cnt 
                                        from
                                        (
                                            SELECT 
                                                vd.auto_producto as autoPrd, 
                                                vd.auto_deposito autoDep,
		                                        sum(vd.cantidad_und*v.signo) as cnt
		                                        FROM ventas_detalle as vd 
		                                        join ventas as v on vd.auto_documento=v.auto
		                                        where (v.cierre in 
                                                    (
                                                        SELECT r.auto_pos_arqueo as cierre
                                                        from p_operador as o 
                                                        join p_resumen as r on r.id_p_operador=o.id
                                                        where o.estatus='A'
                                                    )
                                                    or v.cierre_ftp='')
                                                    and v.estatus_anulado='0'
                                                    and v.tipo in ('01','03','04')
                                                    group by vd.auto_producto, vd.auto_deposito
                                        ) as v1";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        var reader = comando1.ExecuteReader();
                        while (reader.Read())
                        {
                            var nr = new DataKardexResumen()
                            {
                                autoProducto = reader.GetString("autoPrd"),
                                autoDeposito = reader.GetString("autoDep"),
                                cnt = reader.GetDecimal("cnt")
                            };
                            listMv.Add(nr);
                        }
                        reader.Close();

                        sql0 = @"update productos_deposito 
                                        set fisica=fisica-?cnt, 
                                            disponible=disponible-?cnt 
                                        where auto_producto=?ap and auto_deposito=?ad";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        foreach (var mv in listMv)
                        {
                            comando1.Parameters.Clear();
                            comando1.Parameters.AddWithValue("?cnt", mv.cnt);
                            comando1.Parameters.AddWithValue("?ap", mv.autoProducto);
                            comando1.Parameters.AddWithValue("?ad", mv.autoDeposito);
                            rt = comando1.ExecuteNonQuery();
                            if (rt == 0)
                            {
                                var msg = @"PROBLEMA AL ACTUALIZAR PRODUCTO DEPOSITO: " + Environment.NewLine + "Deposito: " + mv.autoDeposito + ", Producto: " + mv.autoProducto;
                                new Exception(msg);
                            }
                        }

                        listMv.Clear();
                        sql0 = @"SELECT 
                                    auto_deposito as autoDep, 
                                    auto_producto as autoPrd, 
                                    sum(cantidad*empaqueContenido) as cnt
                                FROM p_venta
                                group by auto_deposito, auto_producto";
                        var comando2 = new MySqlCommand(sql0, cn, tr);
                        var reader2 = comando2.ExecuteReader();
                        while (reader2.Read())
                        {
                            var nr = new DataKardexResumen()
                            {
                                autoProducto = reader2.GetString("autoPrd"),
                                autoDeposito = reader2.GetString("autoDep"),
                                cnt = reader2.GetDecimal("cnt")
                            };
                            listMv.Add(nr);
                        }
                        reader2.Close();
                        sql0 = @"update productos_deposito 
                                        set reservada=reservada+?cnt, 
                                            disponible=disponible-?cnt 
                                        where auto_producto=?ap and auto_deposito=?ad";
                        comando2 = new MySqlCommand(sql0, cn, tr);
                        foreach (var mv in listMv)
                        {
                            comando2.Parameters.Clear();
                            comando2.Parameters.AddWithValue("?cnt", mv.cnt);
                            comando2.Parameters.AddWithValue("?ap", mv.autoProducto);
                            comando2.Parameters.AddWithValue("?ad", mv.autoDeposito);
                            rt = comando2.ExecuteNonQuery();
                            if (rt == 0)
                            {
                                var msg = @"PROBLEMA AL ACTUALIZAR PRODUCTO DEPOSITO: " + Environment.NewLine + "Deposito: " + mv.autoDeposito + ", Producto: " + mv.autoProducto;
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
            catch (MySqlException ex2)
            {
                result.Mensaje = ex2.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }

            return result;

        }



        // 
        // VERIFICAR SI SE PUEDE PREPARAR EL CIERRE DE VENTAS
        public DtoLib.ResultadoEntidad<int> 
            Verificar_ParaPrepararCierre()
        {
            var result = new DtoLib.ResultadoEntidad<int>();

            try
            {
                var rt = -1;
                using (var cn = new MySqlConnection(_cnn2.ConnectionString))
                {
                    cn.Open();
                    var sql0 = "";
                    MySqlCommand comando1;

                    sql0 = @"SELECT 
                                count(*) as cnt
                                FROM p_operador as op
                                join p_resumen as r on r.id_p_operador=op.id
                                join ventas as v on v.cierre=r.auto_pos_arqueo
                                where estatus='A'";
                    comando1 = new MySqlCommand(sql0, cn);
                    var reader = comando1.ExecuteReader();
                    while (reader.Read())
                    {
                        rt = reader.GetInt32("cnt");
                    }
                    reader.Close();
                };
                result.Entidad = rt;
            }
            catch (MySqlException ex2)
            {
                result.Mensaje = ex2.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }
            return result;
        }
       

        //
        // PROCESO PARA REVERSAR UN CIERRE, SOLO DE MANERA INTERNA
        public DtoLib.Resultado
            Servidor_Principal_ReversarCierre()
        {
            var result = new DtoLib.Resultado();
            var listMv = new List<DataKardexResumen>();

            try
            {
                using (var cn = new MySqlConnection(_cnn2.ConnectionString))
                {
                    cn.Open();

                    MySqlTransaction tr = null;
                    try
                    {
                        tr = cn.BeginTransaction();


                        var sql0 = "";
                        MySqlCommand comando1;
                        var rt = -1;


                        sql0 = "SET FOREIGN_KEY_CHECKS=0";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        //VENTAS
                        sql0 = @"delete vd
                                    from ventas_detalle as vd
                                    join ventas as v on vd.auto_documento=v.auto
                                    where v.fecha='2022/11/02'
                                    and v.codigo_sucursal='03'
                                    and v.cierre in ('0302000250', '0304000251')";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = @"delete from ventas 
                                    where fecha='2022/11/02'
                                    and codigo_sucursal='03'
                                    and cierre in ('0302000250', '0304000251')";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        
                        sql0 = @"delete from pos_arqueo
                                    where auto_cierre in ('0302000250', '0304000251')";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        sql0 = @"delete from cxc_medio_pago 
                                    WHERE codigo_sucursal='03' and
                                    fecha='2022/11/02'";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = @"delete from 
                                    cxc_documentos
                                    WHERE codigo_sucursal='03' and
                                    fecha='2022/11/02'";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = @"delete from 
                                    cxc_recibos
                                    where fecha='2022/11/02' and
                                    codigo_sucursal='03' and 
                                    cierre in ('0302000250', '0304000251')";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = @"delete from 
                                    cxc
                                    WHERE codigo_sucursal='03' and
                                    fecha='2022/11/02'";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();


                        sql0 = @"SELECT 
                                    auto_producto, 
                                    sum(cantidad_und*signo) cnt, 
                                    auto_deposito
                                FROM productos_kardex
                                WHERE codigo_sucursal='03' and 
                                fecha='2022/11/02' and 
                                modulo='Ventas' and
                                estatus_anulado='0'
                                group by auto_producto, auto_deposito";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        var reader = comando1.ExecuteReader();
                        while (reader.Read())
                        {
                            var nr = new DataKardexResumen()
                            {
                                autoProducto = reader.GetString("auto_producto"),
                                autoDeposito = reader.GetString("auto_deposito"),
                                cnt = reader.GetDecimal("cnt")
                            };
                            listMv.Add(nr);
                        }
                        reader.Close();

                        sql0 = "update productos_deposito set fisica=fisica-?cnt, disponible=disponible-?cnt where auto_producto=?ap and auto_deposito=?ad";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        foreach (var mv in listMv)
                        {
                            comando1.Parameters.Clear();
                            comando1.Parameters.AddWithValue("?cnt", mv.cnt);
                            comando1.Parameters.AddWithValue("?ap", mv.autoProducto);
                            comando1.Parameters.AddWithValue("?ad", mv.autoDeposito);
                            rt = comando1.ExecuteNonQuery();
                        }

                        sql0 = @"delete from 
                                    productos_kardex
                                    WHERE codigo_sucursal='03' and 
                                    fecha='2022/11/02' and 
                                    modulo='Ventas'";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

                        sql0 = "SET FOREIGN_KEY_CHECKS=1";
                        comando1 = new MySqlCommand(sql0, cn, tr);
                        rt = comando1.ExecuteNonQuery();

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
            catch (MySqlException ex2)
            {
                result.Mensaje = ex2.Message;
                result.Result = DtoLib.Enumerados.EnumResult.isError;
            }

            return result;
        }
    }
}