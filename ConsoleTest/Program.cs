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
            _test.setServidorRemoto("localhost", "00000001");

            var _fichaDto = new DtoLibPosOffLine.Servidor.MonitorCambiosBD.SubirHost.Insertar.Ficha()
            {
                fecha = DateTime.Now.Date,
                descripcion = "ACTUALIZACION 2022/08/10",
                cmd = @"ALTER TABLE `p_resumen` CHANGE `m_otros` `m_otros` DECIMAL(14,2) NOT NULL",
                aplicaPosOffLine = "0",
                aplicaPosOnLine = "1",
            };
            //var rt1 = _test.MonitorCambiosBD_SubirHost_InsertarCambio(_fichaDto);
            //if (rt1.Result == DtoLib.Enumerados.EnumResult.isError)
            //{
            //    Console.WriteLine("ERROR:");
            //    Console.WriteLine(rt1.Mensaje);
            //}
            //Console.ReadKey();
        }
    }
}