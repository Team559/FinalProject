using Models;
using System;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using System.Data;

public class StaffSoapService : IStaffSoapService
{
  
  public DataSet getAllStaffInfo()
  {

    string connStr = "server=localhost; port=3306; user=root; password=LSWPMGy825mv1u; database=Medical_Surgeries";
    MySqlConnection conn = new MySqlConnection(connStr);
    MySqlDataAdapter sqlAdapter = new MySqlDataAdapter("SELECT * FROM staff_info",conn);
    DataSet myResultSet = new DataSet();
    sqlAdapter.Fill(myResultSet, "staff_info");
    
    return myResultSet;

  }
  public void XmlMethod(XElement xml)
  {
    Console.WriteLine(xml.ToString());
  }
  public MyCustomModel TestCustomModel(MyCustomModel customModel)
  {
    return customModel;
  }
}