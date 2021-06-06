using APICapacitacion.Clases;
using APICapacitacion.IRepositorio;
using APICapacitacion.Model;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace APICapacitacion.Repositorio
{
    public class PeliculaRepositorio : IPeliculaRepositorio
    {
        private readonly ConexionBD _conexionBD;
        public PeliculaRepositorio(ConexionBD conexionBD)
        {
            this._conexionBD = conexionBD;
        }

        public async Task<List<Pelicula>> ObtenerPeliculasGenero()
        {
            List<Pelicula> listaPeliculas = new List<Pelicula>();
            Pelicula pelicula;
            Genero genero;
            DataSet dataset;
            OracleDataAdapter da;

            using (OracleConnection con = new OracleConnection(_conexionBD.StringConexion))
            {
                await con.OpenAsync();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SP_OBTENERPELICULAS";
                cmd.Parameters.Add("e_disp", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                dataset = new DataSet();
                da = new OracleDataAdapter(cmd);
                da.Fill(dataset);

                if (dataset.Tables[0].Rows.Count > 0) {
                    listaPeliculas = new List<Pelicula>();
                    DataSet datasetGeneros;
                    OracleDataAdapter dataAdapterGeneros;
                   // cmd = con.CreateCommand();
                    
                    foreach (DataRow row in dataset.Tables[0].Rows) {
                        
                        pelicula = new Pelicula();
                        pelicula.Id = Int32.Parse(row["id"].ToString());
                        pelicula.Titulo = row["titulo"].ToString();
                        pelicula.ListaGeneros = new List<Genero>();
                        cmd = con.CreateCommand();
                        dataAdapterGeneros = new OracleDataAdapter(cmd);
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.CommandText = "SP_OBTENER_GENEROS_PELICULA";
                        cmd.Parameters.Add("idP", row["id"]).Direction = ParameterDirection.Input;
                        cmd.Parameters.Add("e_disp", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                        datasetGeneros = new DataSet();
                        dataAdapterGeneros.Fill(datasetGeneros);

                        listaPeliculas.Add(pelicula);

                        if (datasetGeneros.Tables[0].Rows.Count > 0) {

                            foreach (DataRow rowGenero in datasetGeneros.Tables[0].Rows) {
                                genero = new Genero();
                                genero.Id = Int32.Parse(rowGenero["id"].ToString());
                                genero.Descripcion = rowGenero["descripcion"].ToString();
                                listaPeliculas[listaPeliculas.Count - 1].ListaGeneros.Add(genero);
                            }
                        }
                    }
                }

                await cmd.Connection.CloseAsync();
            }

            return listaPeliculas;
        }

        public async Task<IEnumerable<object>> ObtenerPeliculasDiccionario()
        {
            DataSet dataset;
            OracleDataAdapter da;
            using (OracleConnection con = new OracleConnection(_conexionBD.StringConexion))
            {
                await con.OpenAsync();
                OracleCommand cmd = con.CreateCommand();
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SP_OBTENERPELICULAS";
                cmd.Parameters.Add("e_disp", OracleDbType.RefCursor).Direction = ParameterDirection.Output;

                dataset = new DataSet();
                da = new OracleDataAdapter(cmd);
                da.Fill(dataset);

                await cmd.Connection.CloseAsync();
            }

            if (dataset.Tables[0] != null)
            {
                return Utilidades.ConvertirDataTableADiccionario(dataset.Tables[0]);
            }
            else {
                return null;
            }
        }

        public async Task<Boolean> RegistrarPelicula(Pelicula pelicula)
        {
            DataSet dataset;
            OracleDataAdapter da;
            using (OracleConnection con = new OracleConnection(_conexionBD.StringConexion))
            {

                await con.OpenAsync();
                OracleTransaction txn = con.BeginTransaction(IsolationLevel.ReadCommitted);
                OracleCommand cmd = con.CreateCommand();

                try
                {
                    cmd.Transaction = txn;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandText = "SP_Registrar_Pelicula";
                    cmd.Parameters.Add("titulo", pelicula.Titulo).Direction = ParameterDirection.Input;
                    cmd.Parameters.Add("e_disp", OracleDbType.RefCursor).Direction = ParameterDirection.Output;
                    dataset = new DataSet();
                    da = new OracleDataAdapter(cmd);
                    da.Fill(dataset);

                    int id = Int32.Parse(dataset.Tables[0].Rows[0]["id"].ToString());

                    if (pelicula.ListaGeneros != null && pelicula.ListaGeneros.Count > 0)
                    {
                        foreach (Genero genero in pelicula.ListaGeneros)
                        {
                            cmd = con.CreateCommand();
                            cmd.Transaction = txn;
                            cmd.CommandType = System.Data.CommandType.StoredProcedure;
                            cmd.CommandText = "SP_Registrar_Pelicula_Genero";
                            cmd.Parameters.Add("idPelicula", id);
                            cmd.Parameters.Add("idPelicula", genero.Id);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    txn.Commit();
                    await cmd.Connection.CloseAsync();
                    return true;
                }
                catch (Exception e)
                {
                    txn.Rollback();
                    await cmd.Connection.CloseAsync();
                    return false;
                }
            }
        }
    }
}
