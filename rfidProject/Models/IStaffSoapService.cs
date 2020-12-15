

using System.ServiceModel;
using System.Data;

namespace Models
{
   [ServiceContract]
   public interface IStaffSoapService
   {
      [OperationContract]
      DataSet getAllStaffInfo();

      [OperationContract]
      void XmlMethod(System.Xml.Linq.XElement xml);

      [OperationContract]
      MyCustomModel TestCustomModel(MyCustomModel inputModel);
  }
}