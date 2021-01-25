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
    public class MateriaRepository : IMateriaRepository
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MateriaRepository> _logger;

        public MateriaRepository(IConfiguration configuration, ILogger<MateriaRepository> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<object> GetMateria()
        {
            object result = null;
            try
            {
                var dynamicParameter = new OracleDynamicParameters();
                dynamicParameter.Add("MATERIACURSOR", OracleDbType.RefCursor, ParameterDirection.Output);
                using (IDbConnection con = GetConnection())
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    if (con.State == ConnectionState.Open)
                    {
                        var query = "SP_GETMATERIAS";
                        result = await SqlMapper.QueryAsync<Materia>(con, query, param: dynamicParameter, commandType: CommandType.StoredProcedure);
                    }
                }
                _logger.LogInformation("Get Materia Repository");
            }
            catch (Exception error)
            {
                _logger.LogError(error, "Excepcion Capturada");
                throw new Exception(error.Message);
            }
            return result;
        }
        private IDbConnection GetConnection()
        {
            var connectionString = _configuration.GetConnectionString("OracleDBConnection");
            var conn = new OracleConnection(connectionString);
            return conn;
        }
    }
}
