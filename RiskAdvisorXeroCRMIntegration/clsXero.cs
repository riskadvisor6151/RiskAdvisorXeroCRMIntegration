using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using Xero.Api;
using Xero.Api.Core;
using Xero.Api.Core.Model;
using Xero.Api.Example.Applications;
using Xero.Api.Example.Applications.Private;
using Xero.Api.Infrastructure.OAuth;
using Xero.Api.Serialization;

namespace RiskAdvisorSalesProcess.RiskAdvisorXeroCRMIntegration
{
    class clsXero
    {
        public void Run()
        {
            X509Certificate2 cert = new X509Certificate2(@"C:\OpenSSL-Win64\bin\public_privatekey.pfx", "riskadvisor");
            var _app_api = new XeroCoreApi("https://api.xero.com", new PrivateAuthenticator(cert), new Consumer("ET6JDQAKKQPKOVS42LCSTH8MCS7PLJ", "6D6B3AV8SVPGZ5UX87HWFXJQYDXH7M"), null, new DefaultMapper(), new DefaultMapper());
            var _org = _app_api.Organisation;
            Console.WriteLine("Company name {0}", _org.Name);

            Invoice _invoice = new Invoice();
            
            // Sequence is: 
            // Connect to Dynamics CRM
            // Check if invoices have been sent to Xero
            // If not, send to Xero and store GUID back into Dynamics
            // If yes, Update status from Xero into Dynamics (Paid, etc)
            // Terminate


                   
                        
        }
                                       
    }
}
