using Connection_DB_Extension;
using Connection_DB_Generalle;
using Extentions;
using ServiceSocials;
using System;
using System.Collections.Generic;

namespace COS
{
    public class Bereau
    {
        private List<Employe> employes;
        private List<Fournisseur> fournisseurs;
        private List<TypePret> typePrets;
        private Tresor tresor;
        public Bereau()
        {
            employes = new List<Employe>();
            fournisseurs = new List<Fournisseur>();
            typePrets = new List<TypePret>();
        }
        public void Initialise ()
        {
            fournisseurs = ConnectionFournisseur.fournisseursList();
            typePrets = ConnectionTypes.TypesList();
            employes = ConnectionEmploye.GetEmployes();
          
        }
        public void Modify()
        {
            foreach(Employe employe in employes)
            {
                ConnectionEmploye.ModifyEmploye(employe);
            }
        }
        public List<Employe> SerchNam(String sousNom)
        {
            sousNom =  sousNom.ToUpper();
            List<Employe> employesList = new List<Employe>();
            String Fullnam;
            foreach(Employe employe in getEmployes())
            {
                Fullnam = employe.getNom() + " " + employe.getPrenom()+" ";
                
                Fullnam =   Fullnam.ToUpper();
                if (Fullnam.Contains(sousNom))
                    employesList.Add(employe);
            }
            return employesList;

        }
        public void setEmployes(List<Employe> employes)
        {
            this.employes = employes;
        }
        public List<Employe> getEmployes()
        {
            return this.employes;
        }
        public Tresor GetTresor()
        {
            return this.tresor;
        }
        public void setTresor(Tresor tresor)
        {
            this.tresor = tresor;
        }
        public void addEmploye(Employe employe)
        {
            employes.Add(employe);
        }
        public void removeEmploye(Employe employe)
        {
            employes.Remove(employe);
        }
        public List<Fournisseur> GetFournisseurs()
        {
            return fournisseurs;
        }
        public List<TypePret> GetTypePrets()
        {
            return typePrets;
        }
        public void setTypePrets(List<TypePret> typePrets)
        {
            this.typePrets = typePrets;
        }
        public void setFournisseur(List<Fournisseur> fournisseur)
        {
            this.fournisseurs = fournisseur;
        }
        public void addTypePret(TypePret typePret)
        {
            GetTypePrets().Add(typePret);
        }
        public void addFournisseur(Fournisseur fournisseur)
        {
            GetFournisseurs().Add(fournisseur);
        }
        public void removeTypePrets(TypePret typePret)
        {
            GetTypePrets().Remove(typePret);
        }
        public void removeFournisseur(Fournisseur fournisseur)
        {
            GetFournisseurs().Remove(fournisseur);
        }

        public Dictionary<Employe /*Employe*/ , double /*montant de prelevement*/> getPrelevementList(DateTime date)
        {
            Dictionary<Employe, double> EmployPrelev = new Dictionary<Employe, double>();
            foreach (Employe employe in getEmployes())
            {
                employe.calculeDebt();
               if(employe.getDebt()>0)
                {
                    employe.finalePayement(date);
                    EmployPrelev.Add(employe, employe.getTranshe());
                }
               
            }
            return EmployPrelev;

        }
      
        
        public Dictionary<Employe /*Employe*/ , double /*montant de prelevement*/> getPrelevementList(int year , int month)
        {
            Dictionary<Employe, double> EmployPrelev = new Dictionary<Employe, double>();
            foreach (Employe employe in getEmployes())
            {
                 EmployPrelev.Add(employe, employe.GetTransh(year , month));
            }
            return EmployPrelev;

        }


    }
    public class Employe

    {
        private int ID; //saved 
        private String Nom;  //saved 
        private String Prenom;   //saved
        private String NSS;  //saved 
        private double salaire;
        private DateTime datedebut;
        private DateTime datefin;
        private String Etat; 
        private string Service;

        private List<ServiceSocial> MyPret; // La liste des prets accordes remboursse ou en cours
        private double MoneyToken; // somme totale de money  
        private double Debt; // somme qui rest a payer
        private double Transhe; // somme qu'on va enlever ce mois 
        /**----------------------------
       * 
       */
        public Employe()
        {
            MyPret = new List<ServiceSocial>();
       
        }


        /**----------------------------
         * 
         */
         public Boolean Equals(Employe employe)
        {

            return (getID() == employe.getID());
        }
        public void setEtat(String etat)
        {
            this.Etat = etat;
        }
        public String getEtat()
        {
            return Etat;
        }
        public void setService(String service)
        {
            this.Service = service;
        }
        public String getService()
        {
            return this.Service;
        }
        public int getID()
        {
            return ID;
        }
        public String getNSS()
        {
          
            return NSS;
        }
        public String getNom()
        {
            return Nom;
        }
        public String getPrenom()
        {
            return Prenom;
        }
      
        public DateTime getdatedebut()
        {
            return datedebut;
        }
        public DateTime getdatefin()
        {
            return datefin;
        }
        public double getsalaire()
        {
            return salaire;
        }
        public List<ServiceSocial> myPret()
        {
            return MyPret;
        }
     
        public double getTokenMoney()
        {
            return MoneyToken;
        }

        public double getDebt()
        {
            return Debt;
        }
        /**----------------------------
         * 
         * 
         ------------------------------**/
        public void setID(int id)
        {
            ID = id;
        }

        public void setNSS(String num_social)
        {
            this.NSS = num_social;
        }
        public void setNom(String nom)
        {
            this.Nom = nom;
        }
        public void setPrenom(String prenom)
        {
            this.Prenom = prenom;
        }
     
        public void setdatedebut(DateTime date1)
        {
            this.datedebut = date1;
        }
        public void setdatefin(DateTime date2)
        {
            this.datefin = date2;
        }
        public void setsalaire(double sal)
        {
            this.salaire = sal;
        }
        public void setMypret(List<ServiceSocial> newPrets)
        {
            this.MyPret = newPrets;
        }
      
        public void setDebt(double ARemboursse)
        {
            this.Debt = ARemboursse;
        }
        public void setMoneyToken(double moneyTOT)
        {
            this.MoneyToken = moneyTOT;
        }
        public void setTranshe(double transhe)
        {
            this.Transhe = transhe;
        }
        public double getTranshe()
        {
            return Transhe;
        }
        public void SetObservation(String remarques)
        {
            foreach(ServiceSocial serviceSocial in myPret())
            {
                serviceSocial.calculateRest();
                if(!(serviceSocial.isPayed()))
                serviceSocial.setobservation(remarques);
            }
        }
      
        public void addPret(ServiceSocial pret)
        {
            if (pret != null)
            {
                MyPret.Add(pret);
                MoneyToken += pret.getPrix();
                if (!(pret is Dons)) Debt += pret.getRestAPyer();
            }
        
        }
        public void removePret(ServiceSocial pret)
        {
            MyPret.Remove(pret);
        }
        public ServiceSocial getPret(int i)
        {
            return MyPret[i];
        }
        public void setPret(int i, ServiceSocial serviceSocial)
        {
            MyPret[i] = serviceSocial;
        }
        public void calculeMoneyToken()
        {
            setMoneyToken(0);
            if (myPret() != null)
            {
                foreach (ServiceSocial serviceSocial in myPret())
                {
                    setMoneyToken(getTokenMoney() + serviceSocial.getPrix());
                }
            }
           
        }
        public void calculeDebt()
        {
            setDebt(0);
            if (myPret() != null)
            {
                foreach (ServiceSocial serviceSocial in myPret())
                {
                    serviceSocial.calculateRest();
                    if (!serviceSocial.isPayed())
                        setDebt(getDebt() + serviceSocial.getRestAPyer());
                }
            }

        }
        public void finalePayement(DateTime date)
        {
            double preleveTot = 0;
            calculeDebt();
            if (getDebt() > 0)
            {

                foreach (ServiceSocial service in MyPret)
                {
                    if ((service is PretSociale))
                    {
                        PretSociale pret = (PretSociale)service;
                        pret.activePreleve(date);
                        preleveTot += pret.getTranshe();
                        }
                    else if ((service is Electromenager))
                    {
                        Electromenager Electro = (Electromenager)service;
                        Electro.activePreleve(date);
                        preleveTot += Electro.getTranshe();
                    }
                }
            }
            setTranshe(preleveTot);
        }
        public void DemandAnticiper(double purcentage, long nbmothes)
        {
            calculeDebt();

            if (getDebt() > 0)
            {

                foreach (ServiceSocial serviceSocial in myPret())
                {
                    if (!(serviceSocial is Dons))
                    {
                        if(serviceSocial is PretSociale)
                            ((PretSociale)serviceSocial).DemandAnticiper(purcentage, nbmothes);
                        else
                             serviceSocial.DemandAnticiper(purcentage, nbmothes);
                    }
                }
            }
            else throw new LogiqueException("Opération non valide ! \nCe salarié n'a aucune somme à rembourser\n ");



        }
        public void DemandDiffere(DateTime dt)
        {

            calculeDebt();

            if (getDebt() > 0)
            {
                List<ServiceSocial> services = new List<ServiceSocial>();
                foreach (ServiceSocial serviceSocial in myPret())
                {
                    if (serviceSocial is PretSociale)
                    {
                        PretSociale pret = (PretSociale)serviceSocial;
                        pret.demandDiffere(dt);
                        services.Add(pret);
                    }

                    else services.Add(serviceSocial);



                }

                setMypret(services);

            }
            else throw new LogiqueException("Opération non valide ! \nCe salarié n'a aucune somme à rembourser\n ");
        }
        public void CancelPrelevement()
        {
            calculeDebt();
            if (getDebt() > 0)
            {
                foreach (ServiceSocial serviceSocial in myPret())
                {
                    serviceSocial.calculateRest();
                    if ((!(serviceSocial is Dons)) && (!serviceSocial.isPayed()))
                        serviceSocial.CancelPrelevement();
                }
            }
            else throw new LogiqueException("Opération non valide ! \nCe salarié n'a aucune somme à rembourser\n ");
        }
        public void AnticiperPrelevement(double montant )
        {
            // Get last date de prelevement ;
            DateTime date = new DateTime(2000, 3, 3);
            foreach (ServiceSocial serviceSocial in myPret())
            {
               
                if (!(serviceSocial is Dons))
                {
                    DateTime dateTime = serviceSocial.getDate(serviceSocial.getDates().Count - 1);
                    if (date.CompareTo(dateTime) < 0)
                        date = dateTime;
                }

            }
            // Get Montant d'anticipation de chaque pret
            double purcentage = montant / GetTransh(date.Year, date.Month);
        
       
            foreach (ServiceSocial serviceSocial in myPret())
            {
                if ((!(serviceSocial is Dons)))
                {
                        double Montant = purcentage * serviceSocial.getprelevement(serviceSocial.getDates().Count - 1);
                        serviceSocial.AnticiperPrelevement(Montant);   
                }
                
            }
        }
        public double GetTransh(int year , int month)
        {
            double d = 0;
            foreach(ServiceSocial serviceSocial in myPret())
            {
                if (!(serviceSocial is Dons))
                d += serviceSocial.GetTransh(year, month);
            }
            return d;
        }
        public void cloture()
        {
            calculeDebt();
            if (getDebt() > 0)
            {
                foreach (ServiceSocial serviceSocial in myPret())
                {
                    serviceSocial.calculateRest();
                    if ((serviceSocial is PretSociale) && (!serviceSocial.isPayed()))
                        ((PretSociale)serviceSocial).Culture();
                }
            }
            else throw new LogiqueException("Opération non valide ! \nCe salarié n'a aucune somme à rembourser\n ");
        }
        public void AnticiperRest()
        {
            calculeDebt();
            if (getDebt() > 0)
            {
                foreach (ServiceSocial serviceSocial in myPret())
                {
                    serviceSocial.calculateRest();
                    if ((!(serviceSocial is Dons)) && (!serviceSocial.isPayed()))
                        serviceSocial.AnticiperRest();
                }
            }
            else throw new LogiqueException("Opération non valide ! \n Ce salarié n'a aucune somme à rembourser\n ");
        }
        public void AnticiperMois(int l)
        {
            calculeDebt();
            if (getDebt() > 0)
            {
                foreach (ServiceSocial serviceSocial in myPret())
                {
                    serviceSocial.calculateRest();
                    if ((!(serviceSocial is Dons)) && (!serviceSocial.isPayed()))
                        serviceSocial.AnticiperMois(l);
                }
            }
            else throw new LogiqueException("Opération non valide ! \nCe salarié n'a aucune somme à rembourser\n ");
        }

    }


    public class Tresor
    {
        private int id;
        private double compt;
        private DateTime annee;


        public double getCompt()
        {
            return compt;
        }
        public void setCompt(double compt)
        {
            this.compt = compt;
        }
        public DateTime getannee()
        {
            return annee;
        }
        public void setannee(DateTime ann)
        {
            this.annee = ann;
        }
        public int getid()
        {
            return id;
        }
        public void setid(int id)
        {
            this.id = id;
        }
        public void INC(double versCMP)
        {
            compt += versCMP;
        }
        public void DEC(double dec)
        {
            compt -= dec;
        }


    }

    public struct Demand
    {
        public int ID;
        public double sommeAttribue;
        public int EmpID;
        public int ServiceID;
        public int TypeID;
        public String Date;
        public Boolean accepted;
    }
}

