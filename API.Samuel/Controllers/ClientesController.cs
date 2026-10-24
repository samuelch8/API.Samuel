using API.Samuel.ENTIDADES;
using API.Samuel.LOGICA.NEGOCIO;
using Microsoft.AspNetCore.Mvc;

namespace API.Samuel.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController : ControllerBase
    {

        //Instancias

        private readonly ClientesBLL _ClientesBLL;

        public ClientesController(ClientesBLL clientesBLL)
        {
            _ClientesBLL = clientesBLL;
        }


        // POST api/Clientes  -> Agregar un cliente nuevo
        [HttpPost]
        public Respuesta<ClienteDto> InsertarCliente([FromBody] ClienteDto ObjCliente)
        {

            Respuesta<ClienteDto> respuesta = new Respuesta<ClienteDto>();

            try
            {
                var reply = _ClientesBLL.InsertarCliente(ObjCliente);

                if (reply != null)
                {
                    respuesta = reply;
                }
            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa del controlador ClientesController en el método InsertarCliente {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }


        // GET api/Clientes  -> Consultar todos los clientes
        [HttpGet]
        public Respuesta<List<ClienteDto>> ObtenerClientes()
        {

            Respuesta<List<ClienteDto>> respuesta = new Respuesta<List<ClienteDto>>();

            try
            {
                var reply = _ClientesBLL.ObtenerClientes();

                if (reply != null)
                {
                    respuesta = reply;
                }
            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa del controlador ClientesController en el método ObtenerClientes {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }


        // GET api/Clientes/5  -> Consultar un cliente por su Id
        [HttpGet("{id:int}")]
        public Respuesta<ClienteDto> ObtenerClientePorId(int id)
        {

            Respuesta<ClienteDto> respuesta = new Respuesta<ClienteDto>();

            try
            {
                var reply = _ClientesBLL.ObtenerClientePorId(id);

                if (reply != null)
                {
                    respuesta = reply;
                }
            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa del controlador ClientesController en el método ObtenerClientePorId {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }


        // PUT api/Clientes/5  -> Actualizar un cliente existente
        [HttpPut("{id:int}")]
        public Respuesta<ClienteDto> ActualizarCliente(int id, [FromBody] ClienteDto ObjCliente)
        {

            Respuesta<ClienteDto> respuesta = new Respuesta<ClienteDto>();

            try
            {
                // El Id que manda es el de la ruta
                ObjCliente.IdCliente = id;

                var reply = _ClientesBLL.ActualizarCliente(ObjCliente);

                if (reply != null)
                {
                    respuesta = reply;
                }
            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa del controlador ClientesController en el método ActualizarCliente {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }


        // DELETE api/Clientes/5  -> Eliminar un cliente por su Id
        [HttpDelete("{id:int}")]
        public Respuesta<ClienteDto> EliminarCliente(int id)
        {

            Respuesta<ClienteDto> respuesta = new Respuesta<ClienteDto>();

            try
            {
                var reply = _ClientesBLL.EliminarCliente(id);

                if (reply != null)
                {
                    respuesta = reply;
                }
            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa del controlador ClientesController en el método EliminarCliente {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

    }
}
