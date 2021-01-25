using Dapper;
using Oracle.ManagedDataAccess.Client;
using System.Collections.Generic;
using System.Data;

namespace Backend.Infraestructure.Data
{
    public class OracleDynamicParameters : SqlMapper.IDynamicParameters
    {
        private readonly DynamicParameters dynamicParameters = new DynamicParameters();
        private readonly List<OracleParameter> oracleParameters = new List<OracleParameter>();

        public void Add(string name, OracleDbType oracleDbType, ParameterDirection parameterDirection, object value, int? size = null)
        {
            OracleParameter oracleParameter;
            if (size.HasValue)
            {
                oracleParameter = new OracleParameter(name, oracleDbType, size.Value, value, parameterDirection);
            }
            else
            {
                oracleParameter = new OracleParameter(name, oracleDbType, value, parameterDirection);
            }
            oracleParameters.Add(oracleParameter);
        }
        public void Add(string name, OracleDbType oracleDbType, ParameterDirection parameterDirection)
        {
            var oracleParameter = new OracleParameter(name, oracleDbType, parameterDirection);
            oracleParameters.Add(oracleParameter);

        }
        public void AddParameters(IDbCommand command, SqlMapper.Identity identity)
        {
            ((SqlMapper.IDynamicParameters)dynamicParameters).AddParameters(command, identity);
            var oracleCommand = command as OracleCommand;
            if (oracleCommand != null)
            {
                oracleCommand.Parameters.AddRange(oracleParameters.ToArray());
            }
        }
    }
}
