using System;

namespace Extentions
{
    public class PV
    {
        private DateTime date;
        private int num;
        private int Id;

        public int getId()
        {
            return Id;
        }
        public void setId(int id)
        {
            this.Id = id;
        }

        /**---------------------------
         * 
         * 
         -----------------------------**/
        public DateTime getDate()
        {
            return date;
        }
        public int getNum()
        {
            return num;
        }
        /**-----------------------
         * 
         * 
         --------------------------**/
        public void setNum(int Num)
        {
            this.num = Num;
        }
        public void setDate(DateTime date)
        {
            this.date = date;
        }
     

    }
    public class Facture
    {
        private int id;//
        private int num;//
        private DateTime date;//
        private double prix;
        private Fournisseur fournisseur;
        private String produit;//
        private int demand_id;//
        private int prets_id;//
        public int getid()
        {
            return id;
        }
        public void setid(int id)
        {
            this.id = id;
        }
        public int getprets_id()
        {
            return prets_id;
        }
        public void setprets_id(int id)
        {
            this.prets_id = id;
        }
        public int getNum()
        {
            return num;
        }
        public void setNum(int Num)
        {
            this.num = Num;
        }


        public int getdemand_id()
        {
            return demand_id;
        }
        public void setdemand_id(int demand_id)
        {
            this.demand_id = demand_id;
        }
        public void setPrix(double prix)
        {
            this.prix = prix;
        }
        public double getprix()
        {
            return prix;
        }
        public DateTime getDate()
        {
            return date;
        }
        public void setDate(DateTime date)
        {
            this.date = date;
        }
        public Fournisseur GetFournisseur()
        {
            return fournisseur;
        }
        public void setFournisseur(Fournisseur fournisseur)
        {
            this.fournisseur = fournisseur;
        }

        public String getproduit()
        {
            return produit;
        }
        public void setProduit(String produit)
        {
            this.produit = produit;
        }
        public Boolean equals(Bon_Cmd bon_Cmd)
        {
            Boolean bln = true;
            string nom = bon_Cmd.GetFournisseur().nom;
            if (!(getDate().Equals(bon_Cmd.getDate()))) bln = false;
            else if (!(getproduit().Equals(bon_Cmd.getproduit()))) bln = false;
            else if (GetFournisseur().Id != bon_Cmd.GetFournisseur().Id) bln = false;
            else if (!GetFournisseur().nom.Equals(nom)) bln = false;
            else if (getprix() != bon_Cmd.getPrix()) bln = false;
            return bln;

        }


    }
    public class Bon_Cmd
    {
        private int ID;
        private int num;
        private DateTime date;
        private Fournisseur fournisseur;
        private String produit;
        private double prix;
        private int demand_id;
        private int prets_id;

        public int getId()
        {
            return ID;
        }
        public void setId(int id)
        {
            this.ID = id;
        }
        public int getprets_id()
        {
            return prets_id;
        }
        public void setprets_id(int id)
        {
            this.prets_id = id;
        }

        public int getdemand_id()
        {
            return demand_id;
        }
        public void setdemand_id(int demand_id)
        {
            this.demand_id = demand_id;
        }
        public int getNum()
        {
            return num;
        }
        public void setNum(int Num)
        {
            this.num = Num;
        }
        public DateTime getDate()
        {
            return date;
        }
        public void setDate(DateTime date)
        {
            this.date = date;
        }
        public Fournisseur GetFournisseur()
        {
            return fournisseur;
        }
        public void setFournisseur(Fournisseur fournisseur)
        {
            this.fournisseur = fournisseur;
        }
        public String getproduit()
        {
            return produit;
        }
        public void setProduit(String produit)
        {
            this.produit = produit;
        }
        public double getPrix()
        {
            return prix;
        }
        public void setPrix(double prix)
        {
            this.prix = prix;
        }


    }
    public struct Fournisseur
    {
        public int Id;
        public String nom;
        public Fournisseur(int Id, String nom)
        {
            this.nom = nom;
            this.Id = Id;
        }
       
    }
    public struct TypePret
    {
        public int Id;
        public String nom;
        public TypePret(int Id, String nom)
        {
            this.nom = nom;
            this.Id = Id;
        }
      
    }
    public struct Paires
    {
        public DateTime date;
        public double prelevement;
        public Boolean boulean;
    }
    public struct Service
    {
        public int id;
        public String nom;
        public int getid()
        {
            return id;
        }
        public void setid(int id)
        {
            this.id = id;
        }
        public String getnom()
        {
            return nom;
        }
        public void setnom(String nom)
        {
            this.nom = nom;
        }
    
    }
    public class LogiqueException : Exception
    {
        public String Message;
        public LogiqueException(String msg)
        {
           Message = msg;
        }
    }

}
