using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DtoLibPosOffLine.Servidor.MonitorCambiosBD.SubirHost.Insertar
{
    
    public class Ficha
    {

        public DateTime fecha { get; set; }
        public string descripcion { get; set; }
        public string cmd { get; set; }
        public string aplicaPosOffLine { get; set; }
        public string aplicaPosOnLine { get; set; }


        public Ficha()
        {
            fecha = DateTime.Now.Date;
            descripcion = "";
            cmd = "";
            aplicaPosOffLine = "";
            aplicaPosOnLine = "";
        }

    }

}