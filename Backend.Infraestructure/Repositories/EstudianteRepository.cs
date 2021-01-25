using Backend.Core.Entities;
using Backend.Core.Interfaces;
using Backend.Infraestructure.Data;
using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Threading.Tasks;

namespace Backend.Infraestructure.Repositories
{
    public class EstudianteRepository : IEstudianteRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EstudianteRepository> _logger;

        public EstudianteRepository(IConfiguration configuration, ILogger<EstudianteRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<object> GetEstudiantes()
        {
            object result = null;
            try
            {
                var dynamicParameter = new OracleDynamicParameters();
                dynamicParameter.Add("ESTUDIANTECURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                using (IDbConnection con = GetConnection())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (con.State == ConnectionState.Open)
                    {
                        var query = "SP_GETESTUDIANTES";
                        result = await SqlMapper.QueryAsync<Estudiante>(con, query, param: dynamicParameter, commandType: CommandType.StoredProcedure);
                    }
                }
                _logger.LogInformation("Get Estudiantes Repository");

            }
            catch (Exception error)
            {
                _logger.LogError(error, "Excepcion Capturada");
                throw new Exception(error.Message);
            }
            return result;
        }
        public async Task<bool> AddEstudiante(Estudiante estudiante)
        {
            int result = 0;
            try
            {
                var dynamicParameter = new OracleDynamicParameters();
                dynamicParameter.Add("nombres", OracleDbType.Varchar2, ParameterDirection.Input, estudiante.Nombres);
                dynamicParameter.Add("apellidos", OracleDbType.Varchar2, ParameterDirection.Input, estudiante.Apellidos);
                dynamicParameter.Add("ci", OracleDbType.Varchar2, ParameterDirection.Input, estudiante.Ci);
                dynamicParameter.Add("fecha_nacimiento", OracleDbType.Date, ParameterDirection.Input, estudiante.FechaNacimiento);

                using (IDbConnection con = GetConnection())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (con.State == ConnectionState.Open)
                    {
                        var query = "SP_INSERTESTUDIANTE";
                        result = await SqlMapper.ExecuteAsync(con, query, param: dynamicParameter, commandType: CommandType.StoredProcedure);
                    }
                }
                _logger.LogInformation("Add Estudiante Repository");
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Excepcion Capturada");
                throw new Exception(error.Message);
            }
            return true;
        }
        public async Task<bool> GenerarDatos()
        {
            var RandomNombres = new string[] { "Juan", "Maria", "Rodrigo", "Jose", "Marco", "Osvaldo", "Juana", "Rocio", "Lucia", "Mariel", "Tito", "Andres", "Roxana", "Leticia", "Ruth", "Mario", "Miriam", "Ruben", "Daniel", "Omar", "Carlos" };
            var RandomApellidos = new string[] { "Carvajal", "Lopez", "Conde", "Roque", "Rocha", "Martinez", "Lucana", "Arce", "Quintana", "Rodriguez", "Alvarez", "Quispe", "Mamani", "Rojas", "Vaca","Barba", "Soria", "Gonzales", "Palacios", "Reyes" };
            int contador = 0;
            int result;
            try
            {
                while (contador < 10)
                {
                    var Nombres = RandomNombres[new Random().Next(0, RandomNombres.Length)];
                    var Apellidos = RandomApellidos[new Random().Next(0, RandomApellidos.Length)];
                    var Ci = new Random().Next(1000000, 9999999).ToString();
                    var FechaNacimiento = new DateTime(new Random().Next(1984, 2000), new Random().Next(1, 12), new Random().Next(1, 30));
                    var dynamicParameter = new OracleDynamicParameters();
                    dynamicParameter.Add("nombres", OracleDbType.Varchar2, ParameterDirection.Input, Nombres);
                    dynamicParameter.Add("apellidos", OracleDbType.Varchar2, ParameterDirection.Input, Apellidos);
                    dynamicParameter.Add("ci", OracleDbType.Varchar2, ParameterDirection.Input, Ci);
                    dynamicParameter.Add("fecha_nacimiento", OracleDbType.Date, ParameterDirection.Input, FechaNacimiento);
                    using (IDbConnection con = GetConnection())
                    {
                        if (con.State == ConnectionState.Closed)
                        {
                            con.Open();
                        }
                        if (con.State == ConnectionState.Open)
                        {
                            var query = "SP_INSERTESTUDIANTE";
                            result = await SqlMapper.ExecuteAsync(con, query, param: dynamicParameter, commandType: CommandType.StoredProcedure);
                        }
                    }
                    contador++;
                }
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Excepcion Capturada");
                throw new Exception(error.Message);
            }
            return true;

        }
        private IDbConnection GetConnection()
        {
            var connectionString = _configuration.GetConnectionString("OracleDBConnection");
            var conn = new OracleConnection(connectionString);
            return conn;
        }
    }
}