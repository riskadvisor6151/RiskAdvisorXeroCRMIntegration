using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;

using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace RiskAdvisorSalesProcess.RiskAdvisorXeroCRMIntegration
{
    class clsIntegration
    {
        public void Run()
        {
            
            // Sequence is: 
            // Connect to Dynamics CRM
            // Check if invoices have been sent to Xero
            // If not, send to Xero and store GUID back into Dynamics
            // If yes, Update status from Xero into Dynamics (Paid, etc)
            // Terminate

            ServerConnection serverConnect = new ServerConnection();
            ServerConnection.Configuration config = serverConnect.GetServerConfiguration();

            using (OrganizationServiceProxy _serviceProxy = new OrganizationServiceProxy(config.OrganizationUri, config.HomeRealmUri,
                                                                                        config.Credentials, config.DeviceCredentials))
            {
                // This statement is required to enable early-bound type support.
                _serviceProxy.EnableProxyTypes();
                IOrganizationService _service = (IOrganizationService)_serviceProxy;

                CrmServiceContext svcContext = new CrmServiceContext(_service);

                // Check if invoices have been sent to Xero

                var _crm_invs_to_submit = from tbl1 in svcContext.InvoiceSet
                                          where tbl1.rad_xero_id.Length == 0
                                          select tbl1;

                foreach (var _inv in _crm_invs_to_submit)
                {
                    
                    // Retrieve Account Information
                    Account _account = (Account)_service.Retrieve("account", new Guid(_inv.AccountId.ToString()), new ColumnSet(true));

                    // If Account not pushed to Xero, Push information
                    if (_account.rad_xero_contact_reference_id.Length == 0)
                    {
                        // Create Company (Xero Jargon is Contact) In Xero
                        Guid _new_xero_contact_reference_id = CreateXeroContact(_account);
                        _account.rad_xero_contact_reference_id = _new_xero_contact_reference_id.ToString();
                        _service.Update(_account);
                    }

                    // If Invoice_Xero Flag is Empty (We need to push the invoice over)

                    if (_inv.rad_xero_invoice_reference_id.Length == 0)
                    {
                        // Push invoice to Xero and grab Id to store
                        // Remember to add Tax at to elements and total at this point

                    }
                    // If Not, we want to get the latest updated (if it's outstanding, if it's paid, we ignore)
                    else
                    {
                        if (_inv.StateCode == InvoiceState.Active)
                        {
                           // Query Xero to find out where we are with payment
                           
                            
                        }
                    }    
                }

            }


        }

        private Guid CreateXeroContact(Account _account)
        {

            Xero.Api.Core.Model.Contact _contact = new Xero.Api.Core.Model.Contact();
            _contact.Name = _account.Name;

            Xero.Api.Core.Model.Address _address = new Xero.Api.Core.Model.Address();
            _address.AddressLine1 = _account.Address1_Line1;
            _address.AddressLine2 = _account.Address1_Line2;
            _address.AddressLine3 = _account.Address1_Line3;
            _address.City = _account.Address1_City;
            _address.Region = _account.rad_StateTerritory.ToString();
            _address.PostalCode = _account.Address1_PostalCode;
            _contact.Addresses.Add(_address);

            Xero.Api.Applications.Private.XeroConnector _xconn = new Xero.Api.Applications.Private.XeroConnector();
            return _xconn.create_contact(_contact);
            
        }

    }
}


