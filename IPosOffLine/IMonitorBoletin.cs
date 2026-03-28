using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IPosOffLine
{
    public interface IMonitorBoletin
    {
        DtoLib.Resultado 
            MonitorBoletin_Actualizar(string boletin, int idNegocio);
        DtoLib.ResultadoEntidad<string>
            MonitorBoletin_Info(int idNegocio);
    }
}
