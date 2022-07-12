using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DtoLibPosOffLine.Servidor.MonitorCambiosBD.NevoCambio
{
    
    public class Ficha
    {

        public int id { get; set; }
        public DateTime fecha { get; set; }
        public string descripcion { get; set; }
        public string cmd { get; set; }
        public string aplicaPosOffLine { get; set; }
        public string aplicaPosOnLine { get; set; }


        public Ficha() 
        {
            id = -1;
            fecha = DateTime.Now.Date;
            descripcion = "";
            cmd = "";
            aplicaPosOffLine = "";
            aplicaPosOnLine = "";
        }

    }

}