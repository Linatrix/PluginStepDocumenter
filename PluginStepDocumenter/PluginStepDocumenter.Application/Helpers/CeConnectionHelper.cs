using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Tooling.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PluginStepDocumenter.Application
{
    public static class CeConnectionHelper
    {
        public static bool TestConnection(IOrganizationService service)
        {
            QueryExpression query = new QueryExpression("systemuser");
            query.TopCount = 1;

            EntityCollection results = service.RetrieveMultiple(query);

            return results?.Entities?.Count > 0;
        }

        public static IOrganizationService GetCeService()
        {
            IOrganizationService service = null;

            //Get CRM Configuration Details
            string connectionString = ConfigurationManager.ConnectionStrings["CRM"].ConnectionString;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CrmServiceClient conn = new CrmServiceClient(connectionString);
            // Cast the proxy client to the IOrganizationService interface.
            service = conn.OrganizationWebProxyClient != null ? conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            return service;
        }

        public static IOrganizationService GetCeService(string connectionString)
        {
            IOrganizationService service = null;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            CrmServiceClient conn = new CrmServiceClient(connectionString);

            //Cast the proxy client to the IOrganizationService interface.
            service = conn.OrganizationWebProxyClient != null ? conn.OrganizationWebProxyClient : (IOrganizationService)conn.OrganizationServiceProxy;

            return service;
        }
    }
}
