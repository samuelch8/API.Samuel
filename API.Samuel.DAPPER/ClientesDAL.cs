using API.Samuel.ENTIDADES;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace API.Samuel.DAPPER
{
    public class ClientesDAL
    {

        // Procedimientos almacenados

        private const string sp_InsertarCliente = "spInsertarCliente";
        private const string sp_ObtenerClientes = "spObtenerClientes";
        private const string sp_ObtenerClientePorId = "spObtenerClientePorId";
        private const string sp_ActualizarCliente = "spActualizarCliente";
        private const string sp_EliminarCliente = "spEliminarCliente";


        //Instancias

        private readonly IConfiguration _Config;


        public ClientesDAL(IConfiguration configuration)
        {
            _Config = configuration;
        }


        // Convierte una fila del lector en un ClienteDto (no se devuelve la contraseña)
        private static ClienteDto LeerCliente(SqlDataReader reader)
        {
            return new ClienteDto
            {
                IdCliente = (int)reader["IdCliente"],
                Correo = (string)reader["Correo"],
                Nombre = (string)reader["Nombre"],
                Apellido1 = (string)reader["Apellido1"],
                Apellido2 = (string)reader["Apellido2"],
                Telefono = (string)reader["Telefono"],
                Provincia = (string)reader["Provincia"],
                IdTipoSesion = (int)reader["IdTipoSesion"],
                NombreTipoSesion = (string)reader["NombreTipoSesion"],
                FechaRegistro = (DateTime)reader["FechaRegistro"]
            };
        }


        public Respuesta<ClienteDto> InsertarCliente(ClienteDto ObjCliente)
        {

            Respuesta<ClienteDto> respuesta = new Respuesta<ClienteDto>();

            try
            {

                string? ConexionDB = _Config.GetConnectionString("Conexion");

                using (SqlConnection connection = new SqlConnection(ConexionDB))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sp_InsertarCliente, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@pCorreo", SqlDbType.NVarChar, 100) { Value = ObjCliente.Correo });
                        command.Parameters.Add(new SqlParameter("@pContrasena", SqlDbType.NVarChar, 50) { Value = ObjCliente.Contrasena });
                        command.Parameters.Add(new SqlParameter("@pNombre", SqlDbType.NVarChar, 50) { Value = ObjCliente.Nombre });
                        command.Parameters.Add(new SqlParameter("@pApellido1", SqlDbType.NVarChar, 50) { Value = ObjCliente.Apellido1 });
                        command.Parameters.Add(new SqlParameter("@pApellido2", SqlDbType.NVarChar, 50) { Value = ObjCliente.Apellido2 });
                        command.Parameters.Add(new SqlParameter("@pTelefono", SqlDbType.NVarChar, 20) { Value = ObjCliente.Telefono });
                        command.Parameters.Add(new SqlParameter("@pProvincia", SqlDbType.NVarChar, 30) { Value = ObjCliente.Provincia });
                        command.Parameters.Add(new SqlParameter("@pIdTipoSesion", SqlDbType.Int) { Value = ObjCliente.IdTipoSesion });

                        // El procedimiento devuelve el Id del cliente nuevo, o -1 si el correo ya existe
                        int IdNuevo = Convert.ToInt32(command.ExecuteScalar());

                        if (IdNuevo > 0)
                        {
                            ObjCliente.IdCliente = IdNuevo;
                            ObjCliente.Contrasena = null;

                            respuesta.Ok = true;
                            respuesta.Mensaje = "El cliente ha sido registrado de manera exitosa";
                            respuesta.ValorRetorno = ObjCliente;
                        }
                        else
                        {
                            respuesta.Ok = false;
                            respuesta.Mensaje = "Ya existe un cliente registrado con ese correo";
                            respuesta.ValorRetorno = null;
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en el método InsertarCliente en la capa dapper {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }


        public Respuesta<List<ClienteDto>> ObtenerClientes()
        {

            Respuesta<List<ClienteDto>> respuesta = new Respuesta<List<ClienteDto>>();
            List<ClienteDto> ListaClientes = new List<ClienteDto>();

            try
            {

                string? ConexionDB = _Config.GetConnectionString("Conexion");

                using (SqlConnection connection = new SqlConnection(ConexionDB))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sp_ObtenerClientes, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ListaClientes.Add(LeerCliente(reader));
                            }
                        }

                        respuesta.Ok = true;
                        respuesta.Mensaje = $"Se obtuvieron {ListaClientes.Count} clientes de manera exitosa";
                        respuesta.ValorRetorno = ListaClientes;

                    }

                }

            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en el método ObtenerClientes en la capa dapper {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }


        public Respuesta<ClienteDto> ObtenerClientePorId(int IdCliente)
        {

            Respuesta<ClienteDto> respuesta = new Respuesta<ClienteDto>();

            try
            {

                string? ConexionDB = _Config.GetConnectionString("Conexion");

                using (SqlConnection connection = new SqlConnection(ConexionDB))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sp_ObtenerClientePorId, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@pIdCliente", SqlDbType.Int) { Value = IdCliente });

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                respuesta.Ok = true;
                                respuesta.Mensaje = "Se ha obtenido la información del cliente de manera exitosa";
                                respuesta.ValorRetorno = LeerCliente(reader);
                            }
                            else
                            {
                                respuesta.Ok = false;
                                respuesta.Mensaje = $"No se encontró ningún cliente con el Id {IdCliente}";
                                respuesta.ValorRetorno = null;
                            }

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en el método ObtenerClientePorId en la capa dapper {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }


        public Respuesta<ClienteDto> ActualizarCliente(ClienteDto ObjCliente)
        {

            Respuesta<ClienteDto> respuesta = new Respuesta<ClienteDto>();

            try
            {

                string? ConexionDB = _Config.GetConnectionString("Conexion");

                using (SqlConnection connection = new SqlConnection(ConexionDB))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sp_ActualizarCliente, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@pIdCliente", SqlDbType.Int) { Value = ObjCliente.IdCliente });
                        command.Parameters.Add(new SqlParameter("@pCorreo", SqlDbType.NVarChar, 100) { Value = ObjCliente.Correo });
                        command.Parameters.Add(new SqlParameter("@pContrasena", SqlDbType.NVarChar, 50) { Value = ObjCliente.Contrasena });
                        command.Parameters.Add(new SqlParameter("@pNombre", SqlDbType.NVarChar, 50) { Value = ObjCliente.Nombre });
                        command.Parameters.Add(new SqlParameter("@pApellido1", SqlDbType.NVarChar, 50) { Value = ObjCliente.Apellido1 });
                        command.Parameters.Add(new SqlParameter("@pApellido2", SqlDbType.NVarChar, 50) { Value = ObjCliente.Apellido2 });
                        command.Parameters.Add(new SqlParameter("@pTelefono", SqlDbType.NVarChar, 20) { Value = ObjCliente.Telefono });
                        command.Parameters.Add(new SqlParameter("@pProvincia", SqlDbType.NVarChar, 30) { Value = ObjCliente.Provincia });
                        command.Parameters.Add(new SqlParameter("@pIdTipoSesion", SqlDbType.Int) { Value = ObjCliente.IdTipoSesion });

                        // Devuelve las filas actualizadas (1), 0 si no existe el cliente, o -1 si el correo ya lo usa otro cliente
                        int Resultado = Convert.ToInt32(command.ExecuteScalar());

                        if (Resultado > 0)
                        {
                            respuesta.Ok = true;
                            respuesta.Mensaje = "El cliente ha sido actualizado de manera exitosa";
                            respuesta.ValorRetorno = null;
                        }
                        else if (Resultado == -1)
                        {
                            respuesta.Ok = false;
                            respuesta.Mensaje = "Ese correo ya está registrado por otro cliente";
                            respuesta.ValorRetorno = null;
                        }
                        else
                        {
                            respuesta.Ok = false;
                            respuesta.Mensaje = $"No se encontró ningún cliente con el Id {ObjCliente.IdCliente}";
                            respuesta.ValorRetorno = null;
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en el método ActualizarCliente en la capa dapper {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }


        public Respuesta<ClienteDto> EliminarCliente(int IdCliente)
        {

            Respuesta<ClienteDto> respuesta = new Respuesta<ClienteDto>();

            try
            {

                string? ConexionDB = _Config.GetConnectionString("Conexion");

                using (SqlConnection connection = new SqlConnection(ConexionDB))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sp_EliminarCliente, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@pIdCliente", SqlDbType.Int) { Value = IdCliente });

                        int FilasAfectadas = Convert.ToInt32(command.ExecuteScalar());

                        if (FilasAfectadas > 0)
                        {
                            respuesta.Ok = true;
                            respuesta.Mensaje = "El cliente ha sido eliminado de manera exitosa";
                            respuesta.ValorRetorno = null;
                        }
                        else
                        {
                            respuesta.Ok = false;
                            respuesta.Mensaje = $"No se encontró ningún cliente con el Id {IdCliente}";
                            respuesta.ValorRetorno = null;
                        }

                    }

                }

            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en el método EliminarCliente en la capa dapper {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

    }
}
