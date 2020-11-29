using Extentions;
using ServiceSocials;
using COS;
using Connection_DB_Extension;
//using System.Windows.Documents;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.IO.Ports;
using System.Configuration;

namespace Connection_DB_Generalle
{
    public class ConnectionDemand
    {
        
        private static String constr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={System.IO.Directory.GetCurrentDirectory()}\COS Esi DB.mdf;Integrated Security=True;Connect Timeout=30";
        private static string table = " dbo.Demands ";
        private static int GetID()
        {
            int id= GenerateID.GetID(table);
            SqlConnection sql = new SqlConnection(constr);
            sql.Open();
            String command = "SELECT *  FROM " + table + " ;";
            SqlCommand sql1 = new SqlCommand(command, sql);
            SqlDataReader sqlDataReader = sql1.ExecuteReader();
            ;
            int ID = 1;
            while(sqlDataReader.Read())
            {
                ID = (int)sqlDataReader[0] + 1;
            }
            sqlDataReader.Close();
            sql.Close();
            sql1.Dispose();
            return ID;
            

        }
        public static void AddDemand(double somme, int Emp, int service, int type, string date)
        {
            int id =GetID();
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String Command = "INSERT INTO " + table + " VALUES (@Id , @somme , @Emp , @service , @date ," +
                " @type  ) ; ";
            SqlCommand sqlCommand = new SqlCommand(Command, sqlConnection);
            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@Id", id );
                sqlCommand.Parameters.AddWithValue("@somme", somme);
                sqlCommand.Parameters.AddWithValue("@Emp", Emp);
                sqlCommand.Parameters.AddWithValue("@service", service);
                sqlCommand.Parameters.AddWithValue("@date", date);
                sqlCommand.Parameters.AddWithValue("@type", type);
                sqlCommand.ExecuteNonQuery();

            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
         public static List<Demand> GetDemands()
        {
            List<Demand> Demands = new List<Demand>();

            SqlConnection conn = new SqlConnection(constr);
            conn.Open();

            string sql = "SELECT * FROM " + table + ";";
            SqlCommand cmd = new SqlCommand(sql, conn);

            if (conn.State != ConnectionState.Open) conn.Open();

            SqlDataReader dreader = cmd.ExecuteReader();

            while (dreader.Read())
            {
                Demand demand = new Demand();
                demand.ID =(int) dreader[0];
                demand.sommeAttribue = Convert.ToDouble( dreader[1] );
                demand.EmpID = (int)dreader[2];
                demand.ServiceID = (int)dreader[3];
                demand.Date = (string)dreader[4];
                demand.TypeID = (int)dreader[5];
                demand.accepted = false;
                Demands.Add(demand);

            }

            dreader.Close();
            cmd.Dispose();
            conn.Close();

            return Demands;
        }

        public static void DeleteDemand(int id)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String Command = "DELETE FROM " + table + " WHERE Id = @id ";
            SqlCommand sqlCommand = new SqlCommand(Command, sqlConnection);
            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
    }
    public class ConnectionPret
    {
        private static String constr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={System.IO.Directory.GetCurrentDirectory()}\COS Esi DB.mdf;Integrated Security=True;Connect Timeout=30";
        private static string table = " dbo.prets ";

     
        public static List< ServiceSocial > GetListPrets(int EmpID) 
        {
            List<ServiceSocial> serviceSocials = new List<ServiceSocial>();
            PV pv ;   Service service ; TypePret type ; Bon_Cmd bon_Cmd ; Facture facture ;
            ServiceSocial serviceSocial;
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String command = "SELECT * FROM " +  table + " WHERE EmpID = @emp ; ";
            SqlCommand sqlCommand = new SqlCommand(command , sqlConnection);
            using(sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@emp", EmpID);
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                while(dataReader.Read())
                {
                    service = ConnectionService.GetService( (int)dataReader[2]);
                    if (service.nom.Equals("Pret sociale"))
                    {
                        PretSociale pretSociale = new PretSociale();
                        type = new TypePret();
                        type = ConnectionTypes.GetTypePret((int)dataReader[26]);
                        pretSociale.setType(type);
                        serviceSocial = pretSociale;
                        
                    }
                    else if (service.nom.Equals("Dons"))
                    {
                        Dons dons = new Dons();
                        type = ConnectionTypes.GetTypePret((int)dataReader[26]);
                        dons.setType(type);
                        serviceSocial = dons;
                    }
                    else if (service.nom.Equals("Credit Eledtromenager"))
                    {
                        Electromenager electromenager = new Electromenager();
                       try
                        {
                            bon_Cmd = ConnectionBonCmd.GetBonCmd((int)dataReader[0]);
                            electromenager.setBon_cmd(bon_Cmd);
                            facture = ConnectionFacture.GetFacture((int)dataReader[0]);
                            electromenager.setFournissure(facture.GetFournisseur());

                           
                            electromenager.setFacture(facture);
                        }
                        catch { }
                       
                        serviceSocial = electromenager;

                    }
                    else
                    {
                        serviceSocial = new Pret();
                        // Cette cas n'est pas valide 
                        throw new Exception();
                    }
                    pv = ConnectionPV.GetPV((int) dataReader[3]);
                    serviceSocial.setPV(pv);
                    serviceSocial.setid((int)dataReader[0]);
                    serviceSocial.setEmployer((int)dataReader[1]);
                    serviceSocial.setPrix(Convert.ToDouble( dataReader[4] ));
                    if(dataReader[25] != DBNull.Value) serviceSocial.setobservation((string)dataReader[25]);
                    int indice = 5;
                    List<Double> prelev = new List<double>();
                    List < DateTime > dates = new List<DateTime>();
                    while(indice < 25)
                    {
                         prelev.Add(Convert.ToDouble(dataReader[indice]) );
                        indice++;
                        if (!(String.IsNullOrEmpty(dataReader[indice].ToString())) )
                            dates.Add(Convert.ToDateTime( dataReader[indice]) );
                        indice++;
                    }
                    serviceSocial.setDates(dates);
                    serviceSocial.setprelevements(prelev);
                    serviceSocials.Add(serviceSocial);
                    
                }
                dataReader.Close();

            }
            sqlCommand.Dispose();
            sqlConnection.Close();
            return serviceSocials;
        }
   
        public static void AddPret(ServiceSocial serviceSocial)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String Command = "INSERT INTO "+ table + " VALUES (@Id , @Emp , @service ," +
                "@pv , @somme , @remb1 , @date1 , @remb2 , @date2 , @remb3 , @date3" +
                ", @remb4 , @date4 , @remb5 , @date5" +
                ", @remb6 , @date6 , @remb7 , @date7 , @remb8 , @date8" +
                ", @remb9 , @date9 , @remb10 , @date10 ,@observation ,@type ) ; ";
            SqlCommand sqlCommand = new SqlCommand(Command, sqlConnection);
            using(sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@Id", serviceSocial.getid());
                sqlCommand.Parameters.AddWithValue("@Emp", serviceSocial.getEmployer());
                if(serviceSocial is PretSociale)
                    sqlCommand.Parameters.AddWithValue("@service", 1);
                else if (serviceSocial is Dons)
                    sqlCommand.Parameters.AddWithValue("@service", 2);
                else // Credit Electromenager
                    sqlCommand.Parameters.AddWithValue("@service", 3);
                sqlCommand.Parameters.AddWithValue("@pv",serviceSocial.getPV().getId());
                sqlCommand.Parameters.AddWithValue("@somme", serviceSocial.getPrix());
               
                int j;
                for (int i =0;i <serviceSocial.getprelevements().Count;i++)
                {
                    j = i + 1;
                    String str = "@remb" + j;
                    sqlCommand.Parameters.AddWithValue(str, serviceSocial.getprelevement(i));
                }
                for (int i = 0;i< serviceSocial.getDates().Count;i++)
                {
                    j = i + 1;
                    String str = "@date" + j; 
                    sqlCommand.Parameters.AddWithValue(str, serviceSocial.getDate(i));
                }
                for(int i = serviceSocial.getDates().Count; i<10;i++)
                {
                    j = i + 1;
                    String str = "@date" + j;
                    sqlCommand.Parameters.AddWithValue(str, DBNull.Value);
                }
                if(String.IsNullOrEmpty(serviceSocial.getobservation()))
                    sqlCommand.Parameters.AddWithValue("@observation", DBNull.Value);
                else
                    sqlCommand.Parameters.AddWithValue("@observation", serviceSocial.getobservation());
                if (serviceSocial is Pret)
                {
                    TypePret type = ((Pret)serviceSocial).getTypePret();
                    sqlCommand.Parameters.AddWithValue("@type", type.Id);

                }
                else
                    sqlCommand.Parameters.AddWithValue("@type", DBNull.Value);

                sqlCommand.ExecuteNonQuery();

            }
            sqlCommand.Dispose();
            sqlConnection.Close();
            try
            {
                if (serviceSocial is Electromenager)
                {
                    ConnectionBonCmd.AddBonCmd(((Electromenager)serviceSocial).GetBon_Cmd());
                    ConnectionFacture.AddFacture(((Electromenager)serviceSocial).GetFacture());
                }
            }
            catch { }

        }

        public static void DeletePret(ServiceSocial serviceSocial)
        {
            if (serviceSocial is Electromenager)
            {
                ConnectionFacture.DeleteFacture(((Electromenager)serviceSocial).GetFacture());
                ConnectionBonCmd.DeleteBonCmd(((Electromenager)serviceSocial).GetBon_Cmd());
            }
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String command = "DELETE FROM " + table + " WHERE Id = @id ;";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            using(sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@id", serviceSocial.getid());
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }

        public static void ModifyPret(ServiceSocial serviceSocial)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            string command = "UPDATE " + table + " SET remb1=@remb1 , date1=@date1, " +
                " remb2=@remb2 , date2=@date2," +
                "remb3=@remb3 , date3=@date3," +
                "remb10=@remb10 , date10=@date10," +
                "remb4=@remb4 , date4=@date4," +
                "remb5=@remb5 , date5=@date5," +
                "remb6=@remb6 , date6=@date6," +
                "remb7=@remb7 , date7=@date7," +
                "remb8=@remb8 , date8=@date8," +
                "remb9=@remb9 , date9=@date9 , observation=@observation " +
                " WHERE Id=@id ;";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            using(sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@id", serviceSocial.getid());
                int j;
                for (int i = 0; i < serviceSocial.getprelevements().Count; i++)
                {
                    j = i + 1;
                    String str = "@remb" + j;
                    sqlCommand.Parameters.AddWithValue(str, serviceSocial.getprelevement(i));
                }
                for (int i = 0; i < serviceSocial.getDates().Count; i++)
                {
                    j = i + 1;
                    String str = "@date" + j;
                    sqlCommand.Parameters.AddWithValue(str, serviceSocial.getDate(i));
                }
                for (int i = serviceSocial.getDates().Count; i < 10; i++)
                {
                    j = i + 1;
                    String str = "@date" + j;
                    sqlCommand.Parameters.AddWithValue(str, DBNull.Value);
                }
                if (String.IsNullOrEmpty(serviceSocial.getobservation()))
                    sqlCommand.Parameters.AddWithValue("@observation", DBNull.Value);
                else
                    sqlCommand.Parameters.AddWithValue("@observation", serviceSocial.getobservation());
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
    }
    public class ConnectionEmploye
    {
        private static String constr = $@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename={System.IO.Directory.GetCurrentDirectory()}\COS Esi DB.mdf;Integrated Security=True;Connect Timeout=30";
        private static string table = " dbo.Employes ";

       public static Employe GetEmploye(int id)
        {
            Employe employe = new Employe();
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String Command = "SELECT * FROM " + table + " WHERE Id = @id ; ";
            SqlCommand sqlCommand = new SqlCommand(Command, sqlConnection);
            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@id", id);
               
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.Read()) 
                {
                    employe.setID((int)dataReader[0]);
                    employe.setNom((String)dataReader[1]);
                    employe.setPrenom((String)dataReader[2]);
                    employe.setNSS((String)dataReader[3]);
                    employe.setService((String)dataReader[4]);
                    employe.setsalaire(Convert.ToDouble(dataReader[5]));
                    employe.setEtat((String)dataReader[8]);
                    employe.setMypret(ConnectionPret.GetListPrets((int)dataReader[0]));
                    
                }
                dataReader.Close();
               
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
            return employe;

        }
  
        public static List<Employe> GetEmployes() 
        {
            List<Employe> employes = new List<Employe>();
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String Command = "SELECT * FROM "+table+" ; ";
            SqlCommand sqlCommand = new SqlCommand(Command , sqlConnection);
            using(sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                while (dataReader.Read())
                {
                    Employe employe = new Employe();
                    employe.setID((int)dataReader[0]);
                    employe.setNom((String)dataReader[1]);
                    employe.setPrenom((String)dataReader[2]);
                    employe.setNSS((String)dataReader[3]);
                    employe.setService((String)dataReader[4]);
                    employe.setsalaire(Convert.ToDouble(dataReader[5]));
                    employe.setEtat((String)dataReader[8]);
                    employe.setMypret(ConnectionPret.GetListPrets((int)dataReader[0]));
                    employes.Add(employe);
                }
                dataReader.Close();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
            return employes;
        }

   
        public static void AddEmploye(Employe employe)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String command = "INSERT INTO "+table+ " VALUES ( @id,@nom,@prenom," +
                "@NSS,@service,@salaire," +
                "@dateDebut,@dateFin,@etat);";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            using(sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@id", employe.getID());
                sqlCommand.Parameters.AddWithValue("@nom", employe.getNom());
                sqlCommand.Parameters.AddWithValue("@prenom", employe.getPrenom());
                sqlCommand.Parameters.AddWithValue("@NSS", employe.getNSS());
                sqlCommand.Parameters.AddWithValue("@service", employe.getService());
                sqlCommand.Parameters.AddWithValue("@salaire", employe.getsalaire());
              
                sqlCommand.Parameters.AddWithValue("@dateDebut", DBNull.Value);
               
                sqlCommand.Parameters.AddWithValue("@dateFin", DBNull.Value);
                sqlCommand.Parameters.AddWithValue("@etat", employe.getEtat());
                sqlCommand.ExecuteNonQuery();
                }
            sqlCommand.Dispose();
            sqlConnection.Close();


        
        }
     
        public static void DeleteEmploye(Employe employe)
        {
            foreach(ServiceSocial serviceSocial in employe.myPret())
            {
                ConnectionPret.DeletePret(serviceSocial);
            }
            SqlConnection sqlConnection = new SqlConnection(constr);
            sqlConnection.Open();
            String Command = "DELETE FROM "+table +" WHERE Id = @id ";
            SqlCommand sqlCommand = new SqlCommand(Command, sqlConnection);
            using(sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@id",employe.getID());
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }

        public static void ModifyEtat(Employe employe)
        {
            SqlConnection sqlConnection = new SqlConnection(constr);
            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            string command = "UPDATE " + table + " SET Etat=@etat " +
                " WHERE Id=@id ;";
            SqlCommand sqlCommand = new SqlCommand(command, sqlConnection);
            using (sqlCommand)
            {
                sqlCommand.CommandType = CommandType.Text;
                sqlCommand.Parameters.AddWithValue("@etat", employe.getEtat());
                sqlCommand.Parameters.AddWithValue("@id", employe.getID());
                sqlCommand.ExecuteNonQuery();
            }
            sqlCommand.Dispose();
            sqlConnection.Close();
        }
        public static void ModifyEmploye(Employe employe)
        {
            foreach(ServiceSocial serviceSocial in employe.myPret() )
            {
                ConnectionPret.ModifyPret(serviceSocial);
            }
        }
    }
}