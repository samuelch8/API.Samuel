using API.Samuel.DAPPER;
using API.Samuel.ENTIDADES;

namespace API.Samuel.LOGICA.NEGOCIO
{
    public class InicioSesionBLL
    {

        //Instancia

        private readonly InicioSesionDAL _InicioSesionDAL;


        //Constructor

        public InicioSesionBLL(InicioSesionDAL inicioSesionDAL)
        {
            _InicioSesionDAL = inicioSesionDAL;
        }


        public Respuesta<InicioSesionDto> IniciarSesion(InicioSesionDto ObjCredenciales)
        {

            Respuesta<InicioSesionDto> respuesta = new Respuesta<InicioSesionDto>();

            try
            {

                if (!string.IsNullOrWhiteSpace(ObjCredenciales.Correo) && !string.IsNullOrWhiteSpace(ObjCredenciales.Contrasena))
                {
                    var reply = _InicioSesionDAL.IniciarSesion(ObjCredenciales);

                    if (reply != null)
                    {
                        respuesta = reply;
                    }
                }
                else
                {
                    respuesta.Ok = false;
                    respuesta.Mensaje = "Debe indicar el correo y la contraseña";
                    respuesta.ValorRetorno = null;
                }

            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa BLL en el método IniciarSesion {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

    }
}
