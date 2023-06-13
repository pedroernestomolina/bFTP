using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            IPosOffLine.IProvider _test = new ProvPosOffLine.Provider("");
            _test.setServidorRemoto("localhost", "mscala");


            var _fichaDto = new DtoLibPosOffLine.Servidor.MonitorCambiosBD.SubirHost.Insertar.Ficha()
            {
                fecha = DateTime.Now.Date,
                //descripcion = "ACTUALIZACION 2022/12/15, POS",
                // cmd = @"ALTER TABLE `ventas`  ADD `porct_bono_por_pago_divisa` DECIMAL(14,2) NOT NULL AFTER `cierre_ftp`,  
                //           ADD `cnt_divisa_aplica_bono_por_pago_divisa` INT NOT NULL AFTER `porct_bono_por_pago_divisa`,  
                //           ADD `monto_bono_por_pago_divisa` DECIMAL(14,2) NOT NULL AFTER `cnt_divisa_aplica_bono_por_pago_divisa`,
                //           ADD `monto_bono_en_divisa_por_pago_divisa` DECIMAL(14,2) NOT NULL AFTER `monto_bono_por_pago_divisa`,  
                //           ADD `monto_por_vuelto_en_efectivo` DECIMAL(14,2) NOT NULL AFTER `monto_bono_en_divisa_por_pago_divisa`,  
                //           ADD `monto_por_vuelto_en_divisa` DECIMAL(14,2) NOT NULL AFTER `monto_por_vuelto_en_efectivo`,  
                //           ADD `monto_por_vuelto_en_pago_movil` DECIMAL(14,2) NOT NULL AFTER `monto_por_vuelto_en_divisa`,  
                //           ADD `cnt_divisa_por_vuelto_en_divisa` INT NOT NULL AFTER `monto_por_vuelto_en_pago_movil`;",
                // cmd = @"ALTER TABLE `ventas`  ADD `estatus_bono_por_pago_divisa` VARCHAR(1) NOT NULL AFTER `cnt_divisa_por_vuelto_en_divisa`,  
                //           ADD `estatus_vuelto_por_pago_movil` VARCHAR(1) NOT NULL AFTER `estatus_bono_por_pago_divisa`;",
                // cmd = @"ALTER TABLE `pos_arqueo`  ADD `vuelto_por_pago_movil` DECIMAL(14,2) NOT NULL AFTER `cierre_numero`;",
                // cmd = @"ALTER TABLE `p_resumen`  ADD `monto_vuelto_por_efectivo` DECIMAL(14,2) NOT NULL AFTER `auto_pos_arqueo`,  
                //            ADD `monto_vuelto_por_divisa` DECIMAL(14,2) NOT NULL AFTER `monto_vuelto_por_efectivo`,  
                //            ADD `monto_vuelto_por_pago_movil` DECIMAL(14,2) NOT NULL AFTER `monto_vuelto_por_divisa`,  
                //            ADD `cnt_divisa_por_vuelto_divisa` INT NOT NULL AFTER `monto_vuelto_por_pago_movil`;",
                // cmd = @"ALTER TABLE `v_pagomovil`  ADD `cierre` VARCHAR(10) NOT NULL AFTER `nombre_agencia`,  
                //            ADD `cierre_ftp` VARCHAR(10) NOT NULL AFTER `cierre`;",
                //                cmd = @"ALTER TABLE `productos_movimientos_transito_detalle`  ADD `contEmpaqueInv` INT NOT NULL AFTER `exFisicaDestino`,
                //                            ADD `descEmpaqueInv` VARCHAR(20) NOT NULL AFTER `contEmpaqueInv`;",

                descripcion = "ACTUALIZACION 2023/05/17, POS",
                //cmd = @"ALTER TABLE `productos`  ADD `estatus_talla_color_sabor` VARCHAR(1) NOT NULL DEFAULT '0' AFTER `volumen`;",
                //cmd = @"ALTER TABLE `ventas`  ADD `estatus_fiscal` VARCHAR(1) NOT NULL DEFAULT '' AFTER `estatus_vuelto_por_pago_movil`;",
                //cmd = @"ALTER TABLE `ventas`  ADD `z_fiscal` INT NOT NULL DEFAULT '0' AFTER `estatus_fiscal`;",
                aplicaPosOffLine = "1",
                aplicaPosOnLine = "1",
            };
            //var rt1 = _test.MonitorCambiosBD_SubirHost_InsertarCambio(_fichaDto);
            //if (rt1.Result == DtoLib.Enumerados.EnumResult.isError)
            //{
            //    Console.WriteLine("ERROR:");
            //    Console.WriteLine(rt1.Mensaje);
            //}
            Console.WriteLine("PRESIONA UNA TECLA PARA CONTINUAR...");
            Console.ReadKey();

            //var rt1 = _test.Verificar_ParaPrepararCierre();
            //Console.WriteLine("PRESIONA UNA TECLA PARA CONTINUAR...");
            //Console.ReadKey();
        }
    }
}