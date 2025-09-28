//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;

//using System.Web.Configuration;
//using System.Configuration;
//using OrdersPortal2.Models;
//using OrdersPortal2.Core.Repositories;


//namespace OrdersPortal2.Core.Services
//{


//    //private AnalyticsDbContext _datamanager;
//    public class IpTelService
//    {
      
//        private string server;
//        private string database;
//        private string uid;
//        private string password;

//        //string connectionString = ConfigurationManager.ConnectionStrings["IpTelSQLConnection"].ConnectionString;
//        //_SqlConnection = new SqlConnection(connectionString);
//        //_datamanager = datacontext;


//        public IpTelService()
//        {
//            Initialize();
//        }

//        //Initialize values
//        private void Initialize()
//        {
//            server = "192.168.200.2";
//            database = "asteriskcdrdb";
//            uid = "portal";
//            password = "1qa2ws3ed";
//            string connectionString;
//            connectionString = "SERVER=" + server + ";" + "DATABASE=" +
//            database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";

//            connection = new MySqlConnection(connectionString);
//        }

//        //open connection to database
//        private bool OpenConnection()
//        {

//            try
//            {
//                connection.Open();
//                return true;
//            }
//            catch (MySqlException ex)
//            {
//                //When handling errors, you can your application's response based 
//                //on the error number.
//                //The two most common error numbers when connecting are as follows:
//                //0: Cannot connect to server.
//                //1045: Invalid user name and/or password.
//                switch (ex.Number)
//                {
//                    case 0:
//                        // MessageBox.Show("Cannot connect to server.  Contact administrator");
//                        break;

//                    case 1045:
//                        //     MessageBox.Show("Invalid username/password, please try again");
//                        break;
//                }
//                return false;
//            }

//        }

//        //Close connection
//        private bool CloseConnection()
//        {
//            try
//            {
//                connection.Close();
//                return true;
//            }
//            catch (MySqlException ex)
//            {
//                //messages.Show(ex.Message);
//                return false;
//            }
//        }

//        public List<IpTelStats> GetTelStat(string tel, string fromdate, string todate)
//        {
//            List<IpTelStats> stat = new List<IpTelStats>();
//            DateTime fromDate = DateTime.Now.AddMonths(-1);
//            DateTime toDate = DateTime.Now;
//            if (tel != "")
//            {
//                if (fromdate != "")
//                {
//                    fromDate = Convert.ToDateTime(fromdate);
//                }
//                if (todate != "")
//                {
//                    toDate = Convert.ToDateTime(todate);
//                }


//                string sqlFromDate = fromDate.Year.ToString() + "-" + fromDate.Month.ToString() + "-" + fromDate.Day.ToString();
//                string sqlToDate = toDate.Year.ToString() + "-" + toDate.Month.ToString() + "-" + toDate.Day.ToString();


//                string query = "SELECT calldate, clid, dst,  disposition, TIME_FORMAT(SEC_TO_TIME(billsec),'%i:%s') as billsec FROM cdr WHERE (cnum like \"5%\" or cnum like \"0%\") and  dst=\"" + tel + "\" and calldate between \"" 
//                    + sqlFromDate + "\" and \"" + sqlToDate + "\" and disposition=\"ANSWERED\" "
//                       // Менеджер підняв требку
//                       + " UNION ALL "
//                    + "select tabb1.calldate, tabb1.clid, tabb1.dst, tabb1.disposition , tabb1.billsec from " +
//                    " (SELECT calldate, dst, clid, disposition, billsec, substring(uniqueid, 1, locate('.', uniqueid) - 1) as syss1 FROM cdr " +
//                    " where (cnum like \"5%\" or cnum like \"0%\") and dst = \"" + tel + "\" and calldate between \"" + sqlFromDate + "\" and \"" + sqlToDate + 
//                    "\" and disposition = \"NO ANSWER\" and " +
//                    "substring(uniqueid, 1, locate('.', uniqueid) - 1)  ) as tabb1 " +
//                    "LEFT JOIN " +
//                    "(select  table1.sysnum as culumn1 from(select calldate, substring(uniqueid, 1, locate('.', uniqueid) - 1) as sysnum, count(calldate)" +
//                    " as count1 FROM cdr   WHERE(cnum like \"5%\" or cnum like \"0%\") " +
//                    "and calldate between \"" + sqlFromDate + "\" and \"" + sqlToDate + "\" and dst like \"21%\"  and disposition = \"NO ANSWER\" group by calldate " +
//                    ") as table1 inner join " +
//                    "(select calldate, substring(uniqueid, 1, locate('.', uniqueid) - 1) as num2 FROM cdr WHERE(cnum like \"5%\" or cnum like \"0%\") " +
//                    "and calldate between \"" + sqlFromDate + "\" and \"" + sqlToDate + "\" and dst like \"21%\"   and disposition = \"ANSWERED\" group by calldate) as table2 " +
//                    " On table1.calldate = table2.calldate and table1.count1 > 2) as tabb2 On tabb1.syss1 = tabb2.culumn1 WHERE tabb2.culumn1 is null"
//                // Менеджер не підняв требку
//                + " UNION ALL "
//                    + " SELECT calldate, clid, dst,  \"DIALED\", TIME_FORMAT(SEC_TO_TIME(billsec),'%i:%s') FROM asteriskcdrdb.cdr WHERE src=\"" + tel + "\" and length(dst)>5  "
//                    + " and calldate between \"" + sqlFromDate + "\" and \"" + sqlToDate +
//                    "\" and disposition=\"ANSWERED\"";
//                // Менеджер дзвонив






////                "select tabb1.calldate, tabb1.syss1 from " +
////" (SELECT calldate, substring(uniqueid, 1, locate('.', uniqueid) - 1) as syss1 FROM asteriskcdrdb.cdr " +
////" where (cnum like \"5%\" or cnum like \"0%\") and dst = \"" + tel + "\" and calldate between \"" + sqlFromDate + "\" and \"" + sqlToDate + "\" and disposition = \"NO ANSWER\" and " +
////                    "substring(uniqueid, 1, locate('.', uniqueid) - 1)  ) as tabb1 " +
////                    "LEFT JOIN " +
////                    "(select  table1.sysnum as culumn1 from(select calldate, substring(uniqueid, 1, locate('.', uniqueid) - 1) as sysnum, count(calldate)" +
////                    " as count1 FROM asteriskcdrdb.cdr   WHERE(cnum like \"5%\" or cnum like \"0%\") " +
////                    "and calldate between \"" + sqlFromDate + "\" and \"" + sqlToDate + "\" and dst like \"21%\"  and disposition = \"NO ANSWER\" group by calldate " +
////                    ") as table1 inner join " +
////                    "(select calldate, substring(uniqueid, 1, locate('.', uniqueid) - 1) as num2 FROM asteriskcdrdb.cdr WHERE(cnum like \"5%\" or cnum like \"0%\")" +
////                    "and calldate between \"" + sqlFromDate + "\" and \"" + sqlToDate + "\" and dst like \"21%\"   and disposition = \"ANSWERED\" group by calldate) as table2" +
////                    "On table1.calldate = table2.calldate and table1.count1 > 2) as tabb2 On tabb1.syss1 = tabb2.culumn1 WHERE tabb2.culumn1 is null";





////                select tabb1.calldate, tabb1.syss1 from
////(SELECT calldate, substring(uniqueid, 1, locate('.', uniqueid) - 1) as syss1 FROM asteriskcdrdb.cdr
////where
////(cnum like "5%" or cnum like "0%") and dst = "2102" and calldate between "2017-02-25" and "2017-10-5" and disposition = "NO ANSWER"
////and substring(uniqueid, 1, locate('.', uniqueid) - 1)  ) as tabb1
////LEFT JOIN

//                //(select  table1.sysnum as culumn1 from(
//                //select calldate, substring(uniqueid, 1, locate('.', uniqueid) - 1) as sysnum, count(calldate) as count1 FROM asteriskcdrdb.cdr   WHERE(cnum like "5%" or cnum like "0%")
//                // and calldate between "2017-02-25" and "2017-10-5" and dst like "21%"  and disposition = "NO ANSWER"
//                // group by calldate
//                // ) as table1
//                // inner join
//                // (select calldate, substring(uniqueid, 1, locate('.', uniqueid) - 1) as num2 FROM asteriskcdrdb.cdr WHERE(cnum like "5%" or cnum like "0%")
//                // and calldate between "2017-02-25" and "2017-10-5" and dst like "21%"   and disposition = "ANSWERED"
//                // group by calldate) as table2
//                // On table1.calldate = table2.calldate
//                // and table1.count1 > 2) as tabb2

//                // On tabb1.syss1 = tabb2.culumn1


//                //  WHERE
//                //  tabb2.culumn1 is null





//                //Create a list to store the result


//                //Open connection
//                if (this.OpenConnection() == true)
//                {
//                    //Create Command
//                    MySqlCommand cmd = new MySqlCommand(query, connection);
//                    //Create a data reader and Execute the command
//                    MySqlDataReader dataReader = cmd.ExecuteReader();

//                    //Read the data and store them in the list
//                    while (dataReader.Read())
//                    {
//                        IpTelStats tmpIpTelStat = new IpTelStats();
//                        tmpIpTelStat.CallDateTime = Convert.ToDateTime(dataReader["calldate"]);
//                        tmpIpTelStat.TelNumber = dataReader["dst"].ToString();
//                        tmpIpTelStat.FromNumber = dataReader["clid"].ToString();
//                        tmpIpTelStat.ToNumber = dataReader["dst"].ToString();
//                        tmpIpTelStat.Disposition = dataReader["disposition"].ToString();
//                        tmpIpTelStat.Duration = System.Text.Encoding.UTF8.GetString((System.Byte[])dataReader["billsec"]);

//                        stat.Add(tmpIpTelStat);
//                    }

//                    //close Data Reader
//                    dataReader.Close();

//                    //close Connection
//                    this.CloseConnection();

//                    //return list to be displayed

//                }
//                else
//                {

//                }



//            }

//            return stat;

//        }


//    }





//}