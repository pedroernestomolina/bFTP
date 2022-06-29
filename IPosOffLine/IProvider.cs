using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IPosOffLine
{

    public interface IProvider: IServidor, IMonitorBoletin, IMonitorCambiosBD
    {

        void 
            setServidorRemoto(string instancia, string baseDatos, string usuario="root");
        //
        DtoLib.Resultado 
            TestBD_Local();
        DtoLib.Resultado 
            TestBD_Remoto();
        DtoLib.Resultado 
            TestBD_Nube();

    }

}