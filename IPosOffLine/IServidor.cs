using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IPosOffLine
{
    
    public interface IServidor
    {

        DtoLib.Resultado 
            Servidor_Test();
        DtoLib.Resultado 
            Servidor_Principal_CrearBoletin(string pathDestino, DateTime fechaMovInv);
        DtoLib.Resultado 
            Servidor_Principal_InsertarCierre(string pathOrigen);
        DtoLib.Resultado 
            Servidor_Principal_PreprararCierre(string codigoEmpresa, string pathLeonuxBandeja, string pathLeonuxFtpBandejaData);
        DtoLib.Resultado 
            Servidor_Principal_InsertarBoletin(string codigoSuc, string rutaArchivoTxt);

        //

        DtoLib.ResultadoEntidad<string>
            Sucursal_GetIdDepositoPrincipal_ByCodigoSucursal(string codSucursal);
        DtoLib.Resultado
            Servidor_Principal_EliminarMovimientosKardexExcluyeDeposito(string idDeposito);
        DtoLib.Resultado
            Servidor_Principal_ActualizarInventarioDeposito();

    }

}