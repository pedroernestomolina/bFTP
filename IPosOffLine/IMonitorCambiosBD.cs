using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IPosOffLine
{

    public interface IMonitorCambiosBD
    {

        DtoLib.ResultadoEntidad<int>
            MonitorCambiosBD_GetId_UltimoCambioRegistrado();
        DtoLib.Resultado
            MonitorCambiosBD_ProcesarCambios(List<DtoLibPosOffLine.Servidor.MonitorCambiosBD.NevoCambio.Ficha> lst);


        DtoLib.ResultadoLista<DtoLibPosOffLine.Servidor.MonitorCambiosBD.NevoCambio.Ficha>
            MonitorCambiosBD_Host_GetLista_NuevosCambios_APartirDel_IdRef(int id);


        DtoLib.Resultado
            MonitorCambiosBD_Host_Insertar_NuevosCambios(string cmd);

    }

}