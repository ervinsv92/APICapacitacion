using APICapacitacion.Clases;
using APICapacitacion.IRepositorio;
using APICapacitacion.Model;
using Oracle.ManagedDataAccess.Client;
using Oracle.ManagedDataAccess.Types;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace APICapacitacion.Repositorio
{
    public class GeneroRepositorio : IGeneroRepositorio
    {
        //TODO: 4- Repositorio que implementa la interfaz de repositorio de genero
        private readonly ConexionBD _conexionBD;
        private readonly LlaveUsuario _llaveUsuario;
        public GeneroRepositorio(ConexionBD conexionBD, LlaveUsuario llaveUsuario) {
            this._conexionBD = conexionBD;
            this._llaveUsuario = llaveUsuario;
        }

        async Task IGeneroRepositorio.ActualizarGenero(Genero genero)
        {
            using (OracleConnection con = new OracleConnection(_conexionBD.StringConexion))
            {
                await con.OpenAsync();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SP_ActualizarGenero";
                cmd.Parameters.Add(new OracleParameter("idP", genero.Id));
                cmd.Parameters.Add(new OracleParameter("descripcionP", genero.Descripcion));
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
            }
        }

        async Task<Genero> IGeneroRepositorio.ObtenerGeneroPorId(int Id)
        {
            Genero genero = null;
            DataSet dataset;
            OracleDataAdapter da;
            using (OracleConnection con = new OracleConnection(_conexionBD.StringConexion)) {
                
                await con.OpenAsync();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SP_ObtenerPeliculaPorId";
                cmd.Parameters.Add(new OracleParameter("idP", Id)).Direction = ParameterDirection.Input;
                cmd.Parameters.Add("e_disp", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                dataset = new DataSet();
                da = new OracleDataAdapter(cmd);
                da.Fill(dataset);

                await cmd.Connection.CloseAsync();

                if (dataset.Tables[0].Rows.Count > 0) {
                    genero = new Genero();
                    genero.Id = Int32.Parse(dataset.Tables[0].Rows[0]["id"].ToString());
                    genero.Descripcion = dataset.Tables[0].Rows[0]["descripcion"].ToString();
                }
            }

            return genero;
        }

        async Task<Genero> IGeneroRepositorio.ObtenerGeneroPorId_FN(int Id)
        {
            string llave1 = _llaveUsuario.LlaveUno;
            Genero genero = null;
            DataSet dataset;
            OracleDataAdapter da;
            using (OracleConnection con = new OracleConnection(_conexionBD.StringConexion))
            {

                await con.OpenAsync();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SP_ObtenerGeneroPorId_FN";
                cmd.Parameters.Add("e_disp", OracleDbType.RefCursor).Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add(new OracleParameter("idP", Id)).Direction = ParameterDirection.Input;
                dataset = new DataSet();
                da = new OracleDataAdapter(cmd);
                da.Fill(dataset);

                await cmd.Connection.CloseAsync();

                if (dataset.Tables[0].Rows.Count > 0)
                {
                    genero = new Genero();
                    genero.Id = Int32.Parse(dataset.Tables[0].Rows[0]["id"].ToString());
                    genero.Descripcion = dataset.Tables[0].Rows[0]["descripcion"].ToString();
                }
            }

            return genero;
        }

        async Task<List<Genero>> IGeneroRepositorio.ObtenerListaGeneros()
        {
            List<Genero> listaGeneros = null;
            Genero genero = null;
            DataSet dataset;
            OracleDataAdapter da;
            using (OracleConnection con = new OracleConnection(_conexionBD.StringConexion))
            {
                await con.OpenAsync();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SP_ObtenerGeneros";
                cmd.Parameters.Add("e_disp", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                dataset = new DataSet();
                da = new OracleDataAdapter(cmd);
                da.Fill(dataset);

                await cmd.Connection.CloseAsync();

                if (dataset.Tables[0].Rows.Count > 0)
                {
                    listaGeneros = new List<Genero>();
                    foreach (DataRow row in dataset.Tables[0].Rows)
                    {
                        genero = new Genero();
                        genero.Id = Int32.Parse(row["id"].ToString());
                        genero.Descripcion = row["descripcion"].ToString();
                        listaGeneros.Add(genero);
                    }    
                }
            }

            return listaGeneros;
        }

        async Task IGeneroRepositorio.RegistrarGenero(Genero genero)
        {
            using (OracleConnection con = new OracleConnection(_conexionBD.StringConexion))
            {
                await con.OpenAsync();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandText = "INSERT INTO GENEROS (descripcion) VALUES(:descripcion)";
                cmd.Parameters.Add(new OracleParameter("descripcion", genero.Descripcion));
                await cmd.ExecuteNonQueryAsync();
                await con.CloseAsync();
            }
        }

        async Task IGeneroRepositorio.EliminarGeneroPorId(int Id)
        {
            using (OracleConnection con = new OracleConnection(_conexionBD.StringConexion))
            {
                await con.OpenAsync();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SP_EliminarGeneroPorId";
                cmd.Parameters.Add(new OracleParameter("idP", Id));
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public async Task<List<string>> Test(int param)
        {
            List<string> listaExistencias = null;
            DataSet dataSet;
            OracleDataAdapter dataAdapter;

            using (OracleConnection con = new OracleConnection(_conexionBD.StringConexion))
            {
                await con.OpenAsync();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "TEST ";
                cmd.Parameters.Add("return", OracleDbType.RefCursor).Direction = ParameterDirection.ReturnValue;
                cmd.Parameters.Add("p_param ", param);
                //cmd.Parameters.Add("mensaje ", OracleDbType.NVarchar2,4000).Direction = ParameterDirection.Output;
                OracleParameter paramError = cmd.Parameters.Add("mensaje", OracleDbType.NVarchar2, 4000);
                paramError.Direction = ParameterDirection.Output;

                dataSet = new DataSet();
                dataAdapter = new OracleDataAdapter(cmd);
                dataAdapter.Fill(dataSet);
                //await cmd.ExecuteNonQueryAsync();
                await cmd.Connection.CloseAsync();

                //var error = cmd.Parameters["mensaje"].Value;
                var error = paramError.Value;

                if (dataSet.Tables[0].Rows.Count > 0)
                {

                    listaExistencias = new List<string>();

                    foreach (DataRow row in dataSet.Tables[0].Rows)
                    {
                        listaExistencias.Add(row["ID"].ToString());
                    }
                }

                return listaExistencias;
            }
        }
    }
}
