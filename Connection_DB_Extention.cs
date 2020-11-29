using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using Extentions;

namespace Connection_DB_Extension
{
     class ConnectionTypes 
    {
        private static String constr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={System.IO.Directory.GetCurrentDirectory()}\COS Esi DB.mdf;Integrated Security=True;Connect Timeout=30";
        private static string table = " dbo.types ";
        
        public static List<TypePret> TypesList()
        {
            List<TypePret> typePrets = new List<TypePret>();

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            string sql = "SELECT * FROM " + table + ";";
            SqlCommand cmd = new SqlCommand(sql, conn);

            if (conn.State != ConnectionState.Open) conn.Open();

            SqlDataReader dreader = cmd.ExecuteReader();

            while (dreader.Read())
            {
                TypePret type = new TypePret(Convert.ToInt32(dreader[0]), dreader[1].ToString());
                typePrets.Add(type);
            }

            dreader.Close();
            cmd.Dispose();
            conn.Close();

            return typePrets;

        }
        
        public static TypePret GetTypePret(int id)
        {
            TypePret type = new TypePret();

            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            String command = "SELECT * FROM " + table + " where Id = @id;";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@id", id);
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.Read())
                {
                    type.Id = (int)dataReader[0];
                    type.nom = (string)dataReader[1];
                }
                dataReader.Close();
            }

            sqlConnection.Close();
            sqlCommand.Dispose();

            return type;


        }
        
        public static void DeletType(TypePret typePret)
        {
            string _query = "DELETE FROM  dbo.types WHERE  Id=@ID ;";
            SqlConnection _con = new SqlConnection(constr);
            SqlCommand _command;
            _con.Open();
            using (_command = new SqlCommand(_query, _con))
            {
                _command.CommandType = CommandType.Text;
                _command.Parameters.AddWithValue("@ID", typePret.Id);
                _command.ExecuteNonQuery();
            }
            _con.Close();
        }
        
        public static void AddType(TypePret type)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            String command = "INSERT INTO dbo.types VALUES(@Id , @nom);";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlConnection.Open();
            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@Id", type.Id);
                sqlCommand.Parameters.AddWithValue("@nom", type.nom);
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
        
        public static void ModifyType(int id, string newValue)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            string command = "UPDATE " + table + " SET nom=@newVALUE WHERE Id=@id ;";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@newVALUE", newValue);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
    }
    class ConnectionFournisseur 
    {
        private static String constr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={System.IO.Directory.GetCurrentDirectory()}\COS Esi DB.mdf;Integrated Security=True;Connect Timeout=30";
        private static string table = " dbo.fournisseur ";
        
        public static List<Fournisseur> fournisseursList()
        {


            List<Fournisseur> fournisseursList = new List<Fournisseur>();

            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            String readCommand = "SELECT * from " + table + " ;";
            SqlCommand sqlCommand = new SqlCommand(readCommand, sqlConnection);

            SqlDataReader dataReader = sqlCommand.ExecuteReader();

            while (dataReader.Read())
            {
                Fournisseur fournisseur = new Fournisseur();
                fournisseur.Id = Convert.ToInt32(dataReader[0]);
                fournisseur.nom = dataReader[1].ToString();
                fournisseursList.Add(fournisseur);

            }
            sqlCommand.Dispose();
            dataReader.Close();
            sqlConnection.Close();

            return fournisseursList;
        }
        
        public static Fournisseur GetFournisseur(int id)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            String readCommand = "SELECT * from " + table + " WHERE Id=@id;";
            SqlCommand sqlCommand = new SqlCommand(readCommand, sqlConnection);
            SqlDataReader dataReader;
            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;

                sqlCommand.Parameters.AddWithValue("@id", id);

                dataReader = sqlCommand.ExecuteReader();
            }

            Fournisseur fournisseur = new Fournisseur();
            if (dataReader.Read())
            {

                fournisseur.Id = Convert.ToInt32(dataReader[0]);
                fournisseur.nom = dataReader[1].ToString();


            }
            //else throw new DBException();
            sqlCommand.Dispose();
            dataReader.Close();
            sqlConnection.Close();
            return fournisseur;

        }
        
        public static void DeleteFournisseur(Fournisseur fournisseur)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            String command = "DELETE FROM  " + table + " WHERE  Id=@ID";

            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;

                sqlCommand.Parameters.AddWithValue("@ID", fournisseur.Id);

                sqlCommand.ExecuteNonQuery();

            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
        
        public static void AddFournisseur(Fournisseur fournisseur)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            String command = "INSERT INTO dbo.fournisseur VALUES(@Id , @nom);";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@Id", fournisseur.Id);
                sqlCommand.Parameters.AddWithValue("@nom", fournisseur.nom);
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();

        }
        
        public static void ModifyFournisseur(int id, string newValue)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            string command = "UPDATE " + table + " SET nom=@newVALUE WHERE Id=@id ;";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@newVALUE", newValue);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
    }
    class ConnectionPV 
    {
        private static String constr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={System.IO.Directory.GetCurrentDirectory()}\COS Esi DB.mdf;Integrated Security=True;Connect Timeout=30";
        private static string table = " dbo.pv ";
       
        public static PV GetPV(int id)
        {
            PV pv = new PV();

            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            string command = "Select * from " + table + " Where Id =@id ;";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@id", id);
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.Read())
                {
                    pv.setDate((DateTime)dataReader[2]);
                    pv.setId(id);
                    pv.setNum((int)dataReader[1]);
                }
                dataReader.Close();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
            return pv;

        }
        
        public static void AddPV(PV pv)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            String command = "INSERT INTO " + table + "VALUES(@id,@num,@date);";

            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;

                sqlCommand.Parameters.AddWithValue("@id", pv.getId());
                sqlCommand.Parameters.AddWithValue("@num", pv.getNum());
                sqlCommand.Parameters.AddWithValue("@date", pv.getDate());

                sqlCommand.ExecuteNonQuery();

            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
        
        public static void DeletePV(PV pv)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            String command = "DELETE FROM  " + table + " WHERE Id = @ID"; ;


            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;

                sqlCommand.Parameters.AddWithValue("@ID", pv.getId());

                sqlCommand.ExecuteNonQuery();

            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
        
        public static void ModifyPV(int id, PV newPV)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            string command = "UPDATE " + table + " SET num=@num,date=@date WHERE Id=@id ;";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;

                sqlCommand.Parameters.AddWithValue("@num", newPV.getNum());
                sqlCommand.Parameters.AddWithValue("@date", newPV.getDate());
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
    }
    class ConnectionService  
   
    {

        private static String constr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={System.IO.Directory.GetCurrentDirectory()}\COS Esi DB.mdf;Integrated Security=True;Connect Timeout=30";
        private static string table = " dbo.services ";
        
        public static Service GetService(int id)
        {
            Service type = new Service();

            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            String command = "SELECT * FROM " + table + " where Id=@id;";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@id", id);
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.Read())
                {
                    type.id = (int)dataReader[0];
                    type.nom = (string)dataReader[1];
                }
                dataReader.Close();
            }

            sqlConnection.Close();
            sqlCommand.Dispose();

            return type;


        }
        public static void DeletService(Service service)  
        {
            string _query = "DELETE FROM  "+table+" WHERE  Id=@ID ;";
            SqlConnection _con = new SqlConnection(constr);
            SqlCommand _command;
            _con.Open();
            using (_command = new SqlCommand(_query, _con))
            {
                _command.CommandType = CommandType.Text;
                _command.Parameters.AddWithValue("@ID", service.id);
                _command.ExecuteNonQuery();
            }
            _con.Close();
        }
       
        public static void AddService(Service service)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            String command = "INSERT INTO "+table+" VALUES(@Id , @nom);";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            sqlConnection.Open();
            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@Id", service.id);
                sqlCommand.Parameters.AddWithValue("@nom", service.nom);
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
       
        public static void ModifyService(int id, string newValue)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();

            string command = "UPDATE " + table + " SET nom = @newVALUE WHERE Id=@id ;";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);

            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@newVALUE", newValue);
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
    }
    class ConnectionBonCmd  
    {
        private static String constr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={System.IO.Directory.GetCurrentDirectory()}\COS Esi DB.mdf;Integrated Security=True;Connect Timeout=30";
        private static string table = " dbo.BonCmd ";
        
        public static Bon_Cmd GetBonCmd(int PretID)
        {
            Bon_Cmd bon_cmd = new Bon_Cmd();
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String Command = "SELECT * FROM " + table + " WHERE pretsID = @id ;";
            SqlCommand sqlCommand = new SqlCommand(Command, sqlConnection);
            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@id", PretID);
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.Read())
                {
                    bon_cmd.setId((int)dataReader[0]);
                    bon_cmd.setNum((int)dataReader[1]);
                    bon_cmd.setDate((DateTime)dataReader[5]);
                    int id = (int)dataReader[2];
                    bon_cmd.setFournisseur(ConnectionFournisseur.GetFournisseur(id));
                
                    bon_cmd.setProduit((String)dataReader[3]);
                    bon_cmd.setPrix(Convert.ToDouble(dataReader[4]));
                    bon_cmd.setdemand_id((int)dataReader[7]);
                    bon_cmd.setprets_id((int)dataReader[6]);

                }
                dataReader.Close();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
            return bon_cmd;
        }
        public static void AddBonCmd(Bon_Cmd bon_Cmd)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String command = "INSERT INTO " + table + " VALUES(@Id , @num  ,@fournisseur " +
                ",@produit ,@prix,@date, @pretID" +
                ",@Demand);";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@Id", bon_Cmd.getId());
                sqlCommand.Parameters.AddWithValue("@num", bon_Cmd.getNum());
                sqlCommand.Parameters.AddWithValue("@date", bon_Cmd.getDate());
                sqlCommand.Parameters.AddWithValue("@fournisseur", bon_Cmd.GetFournisseur().Id);
                if (bon_Cmd.getprets_id() != 0)
                    sqlCommand.Parameters.AddWithValue("@pretID", bon_Cmd.getprets_id());
                else
                    sqlCommand.Parameters.AddWithValue("pretID", DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@produit", bon_Cmd.getproduit());
                if (bon_Cmd.getdemand_id() != 0)
                    sqlCommand.Parameters.AddWithValue("@Demand", bon_Cmd.getdemand_id());
                else
                    sqlCommand.Parameters.AddWithValue("@Demand",DBNull.Value );
                sqlCommand.Parameters.AddWithValue("@prix", bon_Cmd.getPrix());
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();

        }
        public static void DeleteBonCmd(Bon_Cmd facture)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String command = "DELETE FROM " + table + " WHERE Id=@id ;";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@id", facture.getId());
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
    }
    class ConnectionFacture
    {
        private static String constr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={System.IO.Directory.GetCurrentDirectory()}\COS Esi DB.mdf;Integrated Security=True;Connect Timeout=30";
        private static string table = " dbo.facture ";

        public static Facture GetFacture(int PretID)
        {
            Facture facture = new Facture();
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String Command = "SELECT * FROM " + table + " WHERE pretsID = @id ;";
            SqlCommand sqlCommand = new SqlCommand(Command, sqlConnection);
            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@id", PretID);
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.Read())
                {
                    facture.setid((int)dataReader[0]);
                    facture.setNum((int)dataReader[1]);
                    facture.setDate((DateTime)dataReader[2]);
                    int id = (int)dataReader[3]; 
                    facture.setFournisseur(ConnectionFournisseur.GetFournisseur(id));
                    facture.setProduit((String)dataReader[5]);
                    facture.setPrix(Convert.ToDouble( dataReader[7] ));
                    facture.setdemand_id((int) dataReader[6]);
                    facture.setprets_id((int)dataReader[4]);

                }
                
                dataReader.Close();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
            return facture;
        }
      
        public static void AddFacture(Facture facture)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String command = "INSERT INTO " + table + " VALUES(@Id , @num ,@date ,@fournisseur ," +
                " @pretID" +
                ",@produit,@Demand,@prix);";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@Id", facture.getid());
                sqlCommand.Parameters.AddWithValue("@num",facture.getNum());
                sqlCommand.Parameters.AddWithValue("@date",facture.getDate());
                sqlCommand.Parameters.AddWithValue("@fournisseur",facture.GetFournisseur().Id);
                if (facture.getprets_id() != 0)
                    sqlCommand.Parameters.AddWithValue("@pretID", facture.getprets_id());
                else
                    sqlCommand.Parameters.AddWithValue("@pretID", DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@produit",facture.getproduit());
                if (facture.getdemand_id() != 0)
                    sqlCommand.Parameters.AddWithValue("@Demand", facture.getdemand_id());
                else
                    sqlCommand.Parameters.AddWithValue("@Demand", DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@prix",facture.getprix());
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();

        }
        public static void DeleteFacture(Facture facture)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String command = "DELETE FROM " + table + " WHERE Id=@id ;";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@id", facture.getid());
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
        
    }
    class GenerateID 
    {
        private static String constr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={System.IO.Directory.GetCurrentDirectory()}\COS Esi DB.mdf;Integrated Security=True;Connect Timeout=30";


        public static int GetID(String table)
        {
            
            SqlConnection sql = new SqlConnection(constr);
            sql.Open();
            String command = "SELECT COUNT(*)  FROM " + table + " ;";
            SqlCommand sql1 = new SqlCommand(command, sql);
            int id = ((int)sql1.ExecuteScalar());
            sql.Close();
            sql1.Dispose();
            return id+1;
        }
    }
    
}
