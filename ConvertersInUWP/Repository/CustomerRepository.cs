using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace CustomerLib
{
    public class CustomerRepository : ICustomerRepository
    {
        private IList<Customer> customers;

        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            var file = await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Customers.xml"));
            var content = await FileIO.ReadTextAsync(file);
            var doc = XDocument.Parse(content);
            customers = new ObservableCollection<Customer>((from c in doc.Descendants("Customer")
                                                            select new Customer
                                                            {
                                                                CustomerId = GetValueOrDefault(c, "CustomerID"),
                                                                CompanyName = GetValueOrDefault(c, "CompanyName"),
                                                                ContactName = GetValueOrDefault(c, "ContactName"),
                                                                ContactTitle = GetValueOrDefault(c, "ContactTitle"),
                                                                Address = GetValueOrDefault(c, "Address"),
                                                                City = GetValueOrDefault(c, "City"),
                                                                Region = GetValueOrDefault(c, "Region"),
                                                                PostalCode = GetValueOrDefault(c, "PostalCode"),
                                                                Country = GetValueOrDefault(c, "Country"),
                                                                Phone = GetValueOrDefault(c, "Phone"),
                                                                Fax = GetValueOrDefault(c, "Fax"),
                                                                IsVip = bool.Parse(c.Element("IsVip")?.Value ?? "false")
                                                            }).ToList());
            return customers;
        }

        #region ICustomerRepository Members

        public bool Add(Customer customer)
        {
            if (customers.IndexOf(customer) < 0)
            {
                customers.Add(customer);
                return true;
            }
            return false;
        }

        public bool Remove(Customer customer)
        {
            if (customers.IndexOf(customer) >= 0)
            {
                customers.Remove(customer);
                return true;
            }
            return false;
        }

        public bool Commit()
        {
            try
            {
                var doc = new XDocument(new XDeclaration("1.0", "utf-8", "yes"));
                var root = new XElement("Customers");
                foreach (Customer customer in customers)
                {
                    root.Add(new XElement("Customer",
                                          new XElement("CustomerID", customer.CustomerId),
                                          new XElement("CompanyName", customer.CompanyName),
                                          new XElement("ContactName", customer.ContactName),
                                          new XElement("ContactTitle", customer.ContactTitle),
                                          new XElement("Address", customer.Address),
                                          new XElement("City", customer.City),
                                          new XElement("Region", customer.Region),
                                          new XElement("PostalCode", customer.PostalCode),
                                          new XElement("Country", customer.Country),
                                          new XElement("Phone", customer.Phone),
                                          new XElement("Fax", customer.Fax)
                                 ));
                }
                doc.Add(root);
                doc.Save("Customers.xml");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        private static string GetValueOrDefault(XContainer el, string propertyName)
        {
            return el.Element(propertyName)?.Value ?? string.Empty;
        }
    }
}