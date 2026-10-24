using API.Samuel.ENTIDADES;
using API.Samuel.LOGICA.NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace API.Samuel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InicioSesionController : ControllerBase
    {

        //Instancias

        private readonly InicioSesionBLL _InicioSesionBLL;

        public InicioSesionController(InicioSesionBLL inicioSesionBLL)
        {
            _InicioSesionBLL = inicioSesionBLL;
        }


        // POST api/InicioSesion  -> Validar correo y contraseña contra la base de datos
        [HttpPost]
        public Respuesta<InicioSesionDto> IniciarSesion([FromBody] InicioSesionDto ObjCredenciales)
        {

            Respuesta<InicioSesionDto> respuesta = new Respuesta<InicioSesionDto>();

            try
            {
                var reply = _InicioSesionBLL.IniciarSesion(ObjCredenciales);

                if (reply != null)
                {
                    respuesta = reply;
                }
            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa del controlador InicioSesionController en el método IniciarSesion {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

    }
}
