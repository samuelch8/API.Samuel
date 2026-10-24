using API.Samuel.ENTIDADES;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace API.Samuel.DAPPER
{
    public class InicioSesionDAL
    {

        // Procedimientos almacenados

        private const string sp_IniciarSesion = "spIniciarSesionCliente";


        //Instancias

        private readonly IConfiguration _Config;


        public InicioSesionDAL(IConfiguration configuration)
        {
            _Config = configuration;
        }


        public Respuesta<InicioSesionDto> IniciarSesion(InicioSesionDto ObjCredenciales)
        {

            Respuesta<InicioSesionDto> respuesta = new Respuesta<InicioSesionDto>();

            try
            {

                string? ConexionDB = _Config.GetConnectionString("Conexion");

                using (SqlConnection connection = new SqlConnection(ConexionDB))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(sp_IniciarSesion, connection))
                    {

                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("@pCorreo", SqlDbType.NVarChar, 100) { Value = ObjCredenciales.Correo });
                        command.Parameters.Add(new SqlParameter("@pContrasena", SqlDbType.NVarChar, 50) { Value = ObjCredenciales.Contrasena });

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read() && (bool)reader["Existe"])
                            {
                                respuesta.Ok = true;
                                respuesta.Mensaje = (string)reader["Mensaje"];
                                respuesta.ValorRetorno = new InicioSesionDto
                                {
                                    IdCliente = (int)reader["IdCliente"],
                                    Correo = ObjCredenciales.Correo,
                                    NombreCompleto = (string)reader["NombreCompleto"]
                                };
                            }
                            else
                            {
                                respuesta.Ok = false;
                                respuesta.Mensaje = "Correo o contraseña incorrectos";
                                respuesta.ValorRetorno = null;
                            }

                        }

                    }

                }

            }
            catch (Exception ex)
            {
                respuesta.Ok = false;
                respuesta.Mensaje = $"Ha ocurrido un error en el método IniciarSesion en la capa dapper {ex.Message}";
                respuesta.ValorRetorno = null;
            }

            return respuesta;

        }

    }
}
