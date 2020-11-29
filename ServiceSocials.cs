using System;
using System.Collections.Generic;
using Extentions;

namespace ServiceSocials
{

    public abstract class ServiceSocial
    {
        private int id;//
        private int Employer_ID; // saved
        private double somme_attribue;/// la valeur de pret ; // saved
        private PV pv; // saved
        private Boolean payed; /// true if rembourssement is done 
        private double RestAPayer;/// the rest of the debt 
        private double Transh;
        private List<Double> prelevements; // saved
        private List<DateTime> dates; // saved
        private String observation; // saved
        /**---------------------
         * 
         * 
         ------------------------**/
        public ServiceSocial()
        {
            dates = new List<DateTime>();
            prelevements = new List<double>();

        }
        public List<Double> getprelevements()
        {
            return prelevements;
        }
        public int getid()
        {
            return id;
        }
        public void setid(int id)
        {
            this.id = id;
        }
      
      
        public String getobservation()
        {
            return observation;
        }
        public void setobservation(String o )
        {
            this.observation +="\n"+ o;
        }
        public int getEmployer()
        {
            return Employer_ID;
        }
        public void setEmployer(int id)
        {
            this.Employer_ID = id;
        }

        public double getTranshe()
        {
            return Transh;
        }
        public void setTranshe(double Transhe)
        {
            this.Transh = Transhe;
        }
        /**---------------------
         * 
         * 
         ------------------------**/
        public void affecterDate(DateTime date)
        {
            dates.Add(date);
        }
        public DateTime getDate(int i)
        {
            if ((i < dates.Count) && (i > -1))
                return dates[i];
            else
            {
               
                return new DateTime();
            }
        }
        public void setDate(DateTime date, int i)
        {
            if ((i < dates.Count) && (i > -1))
                dates[i] = date;
        }
        public double getPrix()
        {
            return this.somme_attribue;
        }
        public void setPrix(double price)
        {
            this.somme_attribue = price;
        }
        /**---------------------
         * 
         * 
         ------------------------**/
        public Boolean isPayed()
        {
            return payed;
        }
        public void setPayed(Boolean done)
        {
            this.payed = done;
        }
        /**---------------------
         * 
         * 
         ------------------------**/
        public PV getPV()
        {
            return pv;
        }
        public void setPV(PV pv)
        {
            this.pv = pv;
        }
        /**----------------------
         * 
         * 
         -----------------------**/
     
        public double getRestAPyer()
        {
            return RestAPayer;
        }
        public void setRestAPayer(double resttant)
        {
            this.RestAPayer = resttant;
        }
        /**---------------------------
         * 
         * */
        public void setprelevements(List<Double> transh)
        {
            this.prelevements = transh;
        }
        public void setprelevements(double prelev, int i)
        {
            prelevements[i] = prelev;
        }
        public double getprelevement(int i)
        {
            return prelevements[i];
        }
        public List<DateTime> getDates()
        {
            return dates;
        }
        public void setDates(List<DateTime> dates)
        {
            this.dates = dates;
        }

        /**----------------------
         * 
         * 
         * ---------------------------*/
       
     
        public void calculateRest() 
        {
            RestAPayer = somme_attribue;
            if ((prelevements != null) && (dates != null))
            {
                for (int i = 0; i < dates.Count; i++)
                {
                    if (prelevements[i] > 0) RestAPayer -= prelevements[i];

                }
                if ((getRestAPyer() > 0) && (getprelevement(dates.Count) == 0))
                    setPayed(true);

            }
           
            if (RestAPayer <= 0) setPayed(true);
        }
        public void AnticiperRest()
        {
            calculateRest();
            if (getRestAPyer() > 0)
            {
                int ln = getDates().Count;
                if ((ln > 0) && (getprelevement(ln - 1) < 0))
                    ln--;
                if (ln < 10)
                    setprelevements(getRestAPyer(), ln);
            }
        }
        public void AnticiperMois(int l)
        {
            calculateRest();
            if(getRestAPyer()>0)
            {
                int ln = getDates().Count;
                if ((ln > 0) && (getprelevement(ln - 1) < 0))
                    ln--;
                if (ln < 10)
                {
                    double newTransh = getprelevement(ln) * l;
                    setprelevements(newTransh, ln);
                }
            }
        }
        public void DemandAnticiper(double purcentage, long nbMonthes)
        {
            int lng = getprelevements().Count - getDates().Count;
            Boolean cond =  (purcentage <= 0)
                || (purcentage >= 1);
            calculateRest();
            if (cond)
            {
                throw new LogiqueException("Saisissez les informations Correctement \n ");
            }

            else if (!isPayed())
            {
                int ind = getDates().Count;
                double newValue = purcentage * getPrix();
                for (int i = ind; i < ind + nbMonthes; i++)
                {
                    try
                    {
                        if (getprelevement(i) > 0) setprelevements(newValue, i);
                        else setprelevements(-1 * newValue, i);
                    }
                    catch { }
                }
            }

        }
        public void initializePrelevement(double somme_attrb, double purcentage)
        {
            if (somme_attrb <= 0) throw new LogiqueException("Saisissez les informations Correctement \n ");
            if ((purcentage >= 1) || (purcentage <= 0)) throw new LogiqueException("Saisissez les informations Correctement \n ");
            setPrix(somme_attrb);
            for (int i = 0; i < 10; i++)
            {
                prelevements.Add(purcentage * somme_attrb);
            }
        }
        public List<Paires> EtatPrevsioelle()
        {
            List<Paires> paires = new List<Paires>();
            double somme = 0;
            for (int i = 0; i < getDates().Count; i++)
            {
                Paires paires1 = new Paires();
                paires1.date = getDate(i);
                if (getprelevement(i) > 0)
                {
                    paires1.prelevement = getprelevement(i);
                    paires1.boulean = true;
                }
                else
                {
                    paires1.prelevement = -1 * getprelevement(i);
                    paires1.boulean = false;
                }


                somme += paires1.prelevement;
                paires.Add(paires1);
            }
            int j = getDates().Count;
            int month, year, day;
            if (j > 0)
            {
                month = getDate(j - 1).Month;
                year = getDate(j - 1).Year;
                day = getDate(j - 1).Day;
            }
            else
            {
                month = getPV().getDate().Month;
                year = getPV().getDate().Year;
                day = getPV().getDate().Day;
            }
            while ((somme < getPrix())&&(j<10))
            {
                Paires paires1 = new Paires();
                if (getPrix() - somme > getprelevement(j))
                    paires1.prelevement = getprelevement(j);
                else
                    paires1.prelevement = getPrix() - somme;
                paires1.boulean = false;
                if (month < 12)
                {
                    month++;
                    DateTime dateTime = new DateTime(year, month, day);
                    paires1.date = dateTime;
                }
                else
                {
                    year++;
                    month = 1;
                    DateTime dateTime = new DateTime(year, month, day);
                    paires1.date = dateTime;
                }
                paires.Add(paires1);
                somme += getprelevement(j);
                j++;

            }
            return paires;
        }
        public void CancelPrelevement()
        {
            int L = getDates().Count - 1;
            if (getprelevement(L) > 0) getDates().RemoveAt(L);
        }
        public void AnticiperPrelevement(double montant)
        {
            int L = getDates().Count - 1;
            if (getprelevement(L) > 0) getprelevements()[L] = montant ;

        }
        public double GetTransh(int year, int month)
        {
            double d = 0;
            int L = getDates().Count;
            for (int i = 0; i < L; i++)
            {
                DateTime dateTime = getDates()[i];
                if ((dateTime.Year == year) && (dateTime.Month == month) && (getprelevement(i) > 0))
                {
                    d = getprelevement(i);
                }
            }
            return d;
        }

    }

    public class Pret : ServiceSocial
    {

        private TypePret type;
        public void setType(TypePret typePret)
        {
            type = typePret;
        }
        public TypePret getTypePret()
        {
            return type;
        }
    }

    public class PretSociale : Pret
    {
        private Boolean Differe;
        /**---------------------------
         * 
         */
        public void setDiffere(Boolean differe)
        {
            this.Differe = differe;
        }
        /**--------------------------
       * 
       */
        public Boolean getDiffere()
        {
            return Differe;
        }
        /**-------------------------
         * 
         */
        public void calculateDiffere()
        {
            List<double> prelev = getprelevements();
            setDiffere(false);
            if (prelev.Count > 0)
            {
                for (int i = 0; i < prelev.Count; i++)
                {
                    if (prelev[i] < 0)
                    {
                        setDiffere(true);
                        break;
                    }
                }
            }
        }
        /**------------------------
         * 
         * */
        public void demandDiffere(DateTime date2fin)
        {
            calculateRest();
            if (!isPayed())
            {
                List<DateTime> dates = getDates();
                List<double> prelev = getprelevements();
                int ind = dates.Count;
                prelev[ind] *= -1;
                dates.Add(date2fin);
                setprelevements(prelev);
                setDates(dates);
            }
        }
        public void DemandAnticiper(double purcentage, long monthes)
        {

            calculateDiffere();
            if (!getDiffere())
            {
                base.DemandAnticiper(purcentage, monthes);
            }
            else
            {
                int lng = getprelevements().Count - getDates().Count;
                Boolean cond = (lng < monthes) || (purcentage <= 0)
                    || (purcentage >= 1);
                calculateRest();
                if (cond)
                {
                    throw new LogiqueException("Veuillez saisir les informations correctement \n ");
                }

                else if (!isPayed())
                {
                    int ind = getDates().Count;
                    double newValue = purcentage * getPrix();
                    for (int i = ind - 1; i < ind + monthes - 1; i++)
                    {
                        if (getprelevement(i) > 0) setprelevements(newValue, i);
                        else setprelevements(-1 * newValue, i);
                    }
                }
            }
        }
        public void activePreleve(DateTime dt)
        {
            DateTime dateTime = getDate(getDates().Count - 1);
            
            calculateRest();
            if (!isPayed())
            {
                setTranshe(0);
                List<DateTime> dates = getDates();
                List<double> prelev = getprelevements();
                int ind = dates.Count;
                Boolean cond01 = ((ind > 0) && (prelev[ind - 1] > 0));
                if ((ind > 0) && ((prelev[ind - 1] < 0) && (dt.CompareTo(dates[ind - 1]) >= 0)))
                {
                   
                    prelev[ind - 1] *= -1;
                    dates[ind - 1] = dt;
                    if (prelev[ind - 1] > getRestAPyer())
                        prelev[ind - 1] = getRestAPyer();
                    setTranshe(prelev[ind - 1]);
                }
                else if (cond01 || (ind == 0))
                {
                   
                    dates.Add(dt);
                    if (prelev[ind] > getRestAPyer()) prelev[ind] = getRestAPyer();
                    setTranshe(prelev[ind]);
                }
                else setTranshe(0);
                setprelevements(prelev);
                setDates(dates);
            }
            // else throw new LogiqueException()");
        } 
        public void Culture()
        {
            calculateRest();
            if (getRestAPyer() > 0)
            {
                for (int i = getDates().Count; i < 10; i++)
                {
                    getprelevements()[i] = 0;
                }
                calculateDiffere();
                if ((getDiffere()) && (getDates().Count > 0))
                    getprelevements()[getDates().Count - 1] = 0;
            }
        }
    }
    public class Electromenager : ServiceSocial
    {

        private Fournisseur fournissureur;
        private Bon_Cmd BonCmd;
        private Facture facture;
        private String product;

        /*--------------------
         *
         *
         --------------------------**/
        public void setBon_cmd(Bon_Cmd bon_Cmd)
        {
            this.BonCmd = bon_Cmd;
        }
        public void setFacture(Facture facture)
        {
            this.facture = facture;
        }
        public void setFournissure(Fournisseur fournisseur)
        {
            this.fournissureur = fournisseur;
        }
        public void setProduit(String produit)
        {
            this.product = produit;
        }
        public Bon_Cmd GetBon_Cmd()
        {
            return BonCmd;
        }
        public Facture GetFacture()
        {
            return facture;

        }
        public Fournisseur GetFournisseur()
        {
            return fournissureur;
        }
        public string GetProduit()
        {
            return product;
        }
        public void activePreleve(DateTime dt)
        {
          
            setTranshe(0);
            calculateRest();
            if (!isPayed())
            {
                List<DateTime> dates = getDates();
                List<double> prelev = getprelevements();
                int ind = dates.Count;
                Boolean cond01 = ((ind >= 0) && (ind < 10));

                if (cond01)
                {
                    
                    dates.Add(dt);
                    if (getRestAPyer() < prelev[ind]) prelev[ind] = getRestAPyer();
                }


               
                setTranshe(prelev[ind]);
                setprelevements(prelev);
                setDates(dates);
            }
     
        }
      

    }
    public class Dons : Pret
    {
        public Dons()
        {
            setPayed(true);
            setRestAPayer(0);
        }
     

    }


}
