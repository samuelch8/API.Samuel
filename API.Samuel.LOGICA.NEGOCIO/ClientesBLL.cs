using API.Samuel.DAPPER;
using API.Samuel.ENTIDADES;

namespace API.Samuel.LOGICA.NEGOCIO
{
    public class ClientesBLL
    {

        //Instancia

        private readonly ClientesDAL _ClientesDAL;


        //Constructor

        public ClientesBLL(ClientesDAL clientesDAL)
        {
            _ClientesDAL = clientesDAL;
        }


        // Revisa que vengan todos los datos obligatorios del cliente.
        // Devuelve el mensaje de error, o null si todo está bien.
        private static string? ValidarCliente(ClienteDto ObjCliente)
        {
            if (string.IsNullOrWhiteSpace(ObjCliente.Correo) || string.IsNullOrWhiteSpace(ObjCliente.Contrasena) ||
                string.IsNullOrWhiteSpace(ObjCliente.Nombre) || string.IsNullOrWhiteSpace(ObjCliente.Apellido1) ||
                string.IsNullOrWhiteSpace(ObjCliente.Apellido2) || string.IsNullOrWhiteSpace(ObjCliente.Telefono) ||
                string.IsNullOrWhiteSpace(ObjCliente.Provincia))
            {
                return "Uno o varios de los valores están vacíos, favor revisar!";
            }

            if (!ObjCliente.Correo.Contains('@'))
            {
                return "El correo no tiene un formato válido";
            }

            if (ObjCliente.IdTipoSesion <= 0)
            {
                return "Debe indicar un tipo de sesión válido (IdTipoSesion)";
            }

            return null;
        }


        public Respuesta<ClienteDto> InsertarCliente(ClienteDto ObjCliente)
        {

            Respuesta<ClienteDto> respuesta = new Respuesta<ClienteDto>();

            try
            {

                string? Error = ValidarCliente(ObjCliente);

                if (Error == null)
                {
                    var reply = _ClientesDAL.InsertarCliente(ObjCliente);

                    if (reply != null)
                    {
                        respuesta = reply;
                    }
                }
                else
                {
                    respuesta.Ok = false;
                    respuesta.Mensaje = Error;
                    respuesta.ValorRetorno = null;
                }

            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa BLL en el método InsertarCliente {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }


        public Respuesta<List<ClienteDto>> ObtenerClientes()
        {

            Respuesta<List<ClienteDto>> respuesta = new Respuesta<List<ClienteDto>>();

            try
            {

                var reply = _ClientesDAL.ObtenerClientes();

                if (reply != null)
                {
                    respuesta = reply;
                }

            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa BLL en el método ObtenerClientes {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }


        public Respuesta<ClienteDto> ObtenerClientePorId(int IdCliente)
        {

            Respuesta<ClienteDto> respuesta = new Respuesta<ClienteDto>();

            try
            {

                if (IdCliente > 0)
                {
                    var reply = _ClientesDAL.ObtenerClientePorId(IdCliente);

                    if (reply != null)
                    {
                        respuesta = reply;
                    }
                }
                else
                {
                    respuesta.Ok = false;
                    respuesta.Mensaje = "El Id del cliente debe ser mayor a 0";
                    respuesta.ValorRetorno = null;
                }

            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa BLL en el método ObtenerClientePorId {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }


        public Respuesta<ClienteDto> ActualizarCliente(ClienteDto ObjCliente)
        {

            Respuesta<ClienteDto> respuesta = new Respuesta<ClienteDto>();

            try
            {

                string? Error = ObjCliente.IdCliente > 0 ? ValidarCliente(ObjCliente) : "El Id del cliente debe ser mayor a 0";

                if (Error == null)
                {
                    var reply = _ClientesDAL.ActualizarCliente(ObjCliente);

                    if (reply != null)
                    {
                        respuesta = reply;
                    }
                }
                else
                {
                    respuesta.Ok = false;
                    respuesta.Mensaje = Error;
                    respuesta.ValorRetorno = null;
                }

            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa BLL en el método ActualizarCliente {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }


        public Respuesta<ClienteDto> EliminarCliente(int IdCliente)
        {

            Respuesta<ClienteDto> respuesta = new Respuesta<ClienteDto>();

            try
            {

                if (IdCliente > 0)
                {
                    var reply = _ClientesDAL.EliminarCliente(IdCliente);

                    if (reply != null)
                    {
                        respuesta = reply;
                    }
                }
                else
                {
                    respuesta.Ok = false;
                    respuesta.Mensaje = "El Id del cliente debe ser mayor a 0";
                    respuesta.ValorRetorno = null;
                }

            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en la capa BLL en el método EliminarCliente {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

    }
}
