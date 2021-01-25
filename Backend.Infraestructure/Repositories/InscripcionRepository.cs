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
    public class InscripcionRepository : IInscripcionRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<InscripcionRepository> _logger;

        public InscripcionRepository(IConfiguration configuration, ILogger<InscripcionRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<bool> AddInscripcion(Inscripcion inscripcion)
        {
            int result = 0;
            try
            {
                var dynamicParameter = new OracleDynamicParameters();
                dynamicParameter.Add("descripcion", OracleDbType.Varchar2, ParameterDirection.Input, inscripcion.Descripcion);
                dynamicParameter.Add("id_materia", OracleDbType.Int32, ParameterDirection.Input, inscripcion.IdMateria);
                dynamicParameter.Add("id_estudiante", OracleDbType.Int32, ParameterDirection.Input, inscripcion.IdEstudiante);
                using (IDbConnection con = GetConnection())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (con.State == ConnectionState.Open)
                    {
                        var query = "SP_INSERTINSCRIPCION";
                        result = await SqlMapper.ExecuteAsync(con, query, param: dynamicParameter, commandType: CommandType.StoredProcedure);
                    }
                }
                _logger.LogInformation("Add Inscripcion Repository");
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Excepcion Capturada");
                throw new Exception(error.Message);
            }
            return true;
        }
        public async Task<bool> DeleteInscripcion(int id)
        {
            int result = 0;
            try
            {
                var dynamicParameter = new OracleDynamicParameters();
                dynamicParameter.Add("id_inscripcion", OracleDbType.Int32, ParameterDirection.Input, id);
                using (IDbConnection con = GetConnection())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (con.State == ConnectionState.Open)
                    {
                        var query = "delete inscripcion where id_inscripcion=:id_inscripcion";
                        result = await SqlMapper.ExecuteAsync(con, query, param: dynamicParameter, commandType: CommandType.Text);
                    }
                }
                _logger.LogInformation("Delete Inscripcion Repository");
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Excepcion Capturada");
                throw new Exception(error.Message);
            }
            return true;
        }

        public async Task<object> GetEstudiantesPorMateria(int idMateria)
        {
            object result = null;
            try
            {
                var dynamicParameter = new OracleDynamicParameters();
                dynamicParameter.Add("id_materia", OracleDbType.Int32, ParameterDirection.Input, idMateria);

                using (IDbConnection con = GetConnection())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (con.State == ConnectionState.Open)
                    {
                        var query = "select Id_Inscripcion as IdInscripcion, i.id_Materia as IdMateria, Sigla , nombre, i.id_estudiante as IdEstudiante, ci, nombres, apellidos from inscripcion i, estudiante e, materia m where i.id_materia =:id_materia and i.id_estudiante = e.id_estudiante and i.id_materia = m.id_materia";
                        result = await SqlMapper.QueryAsync<Object>(con, query, param: dynamicParameter, commandType: CommandType.Text);
                    }
                }
                _logger.LogInformation("Get Estudiantes Materia Repository");
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Excepcion Capturada");
                throw new Exception(error.Message);
            }
            return result;
        }

        public async Task<object> Getinscripciones()
        {
            object result = null;
            try
            {
                var dynamicParameter = new OracleDynamicParameters();
                dynamicParameter.Add("INSCRIPCIONCURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                using (IDbConnection con = GetConnection())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (con.State == ConnectionState.Open)
                    {
                        var query = "SP_GETINSCRIPCIONES";
                        result = await SqlMapper.QueryAsync<Inscripcion>(con, query, param: dynamicParameter, commandType: CommandType.StoredProcedure);
                    }
                }
                _logger.LogInformation("Get Inscripciones Repository");
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Excepcion Capturada");
                throw new Exception(error.Message);
            }
            return result;
        }

        public async Task<object> GetMateriasPorEstudiante(int idEstudiante)
        {
            object result = null;
            try
            {
                var dynamicParameter = new OracleDynamicParameters();
                dynamicParameter.Add("id_estudiante", OracleDbType.Int32, ParameterDirection.Input, idEstudiante);

                using (IDbConnection con = GetConnection())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (con.State == ConnectionState.Open)
                    {
                        var query = "select Id_Inscripcion as IdInscripcion, i.id_Materia as IdMateria, " +
                            "Sigla , nombre, i.id_estudiante as IdEstudiante, ci, nombres, apellidos " +
                            "from inscripcion i, estudiante e, materia m " +
                            "where i.id_estudiante =:id_estudiante and i.id_estudiante = e.id_estudiante and i.id_materia = m.id_materia";
                        result = await SqlMapper.QueryAsync<Object>(con, query, param: dynamicParameter, commandType: CommandType.Text);
                    }
                }
                _logger.LogInformation("Get Materias por estudiante Repository");
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Excepcion Capturada");
                throw new Exception(error.Message);
            }
            return result;

        }

        public async Task<bool> UpdateInscripcion(Inscripcion inscripcion)
        {
            int result = 0;
            try
            {
                var dynamicParameter = new OracleDynamicParameters();
                dynamicParameter.Add("descripcion", OracleDbType.Varchar2, ParameterDirection.Input, inscripcion.Descripcion);
                dynamicParameter.Add("id_materia", OracleDbType.Decimal, ParameterDirection.Input, inscripcion.IdMateria);
                dynamicParameter.Add("id_estudiante", OracleDbType.Decimal, ParameterDirection.Input, inscripcion.IdEstudiante);
                dynamicParameter.Add("id_inscripcion", OracleDbType.Decimal, ParameterDirection.Input, inscripcion.IdInscripcion);

                using (IDbConnection con = GetConnection())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (con.State == ConnectionState.Open)
                    {
                        var query = "update inscripcion " +
                            "set descripcion=:descripcion, id_materia=:id_materia, id_estudiante=:id_estudiante " +
                            "where id_inscripcion=:id_inscripcion";
                        result = await SqlMapper.ExecuteAsync(con, query, param: dynamicParameter, commandType: CommandType.Text);
                    }
                }
                _logger.LogInformation("Update inscripcion Repository");
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
