using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using entities;

namespace app_2
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Sorted list: ".PadRight(60, '-'));
            List<Contact> cts = Contact.SampleData();
            cts.Sort((c1, c2) =>
                {
                    if (c1 != null && c2 != null)
                        return String.Compare(c1.LastName, c2.LastName);
                    return 0;
                });

            foreach (var item in cts)
            {
                Console.WriteLine(item.LastName);
            }

            Console.WriteLine();
            Console.WriteLine("Grouped: ".PadRight(60, '-'));

            {
                SortedDictionary<String, List<Contact>> groups = new SortedDictionary<string, List<Contact>>();
                foreach (var item in cts)
                {
                    Console.WriteLine(item.Phone);
                    if (groups.ContainsKey(item.State))
                    {
                        groups[item.State].Add(item);
                    }
                    else
                    {
                        List<Contact> new_contact = new List<Contact>();
                        groups.Add(item.State, new_contact);
                        new_contact.Add(item);
                    }
                }

                foreach (KeyValuePair<string, List<Contact>> item in groups)
                {
                    Console.WriteLine("State: {0}", item.Key);
                    foreach (Contact c in item.Value)
                    {
                        Console.WriteLine("  {0}", c.LastName);
                    }
                }
            }

            {
                Console.WriteLine();
                Console.WriteLine("Linq grouped: ".PadRight(60, '-'));
                var qry =
                    from i in cts
                    orderby i.State, i.LastName
                    group i by i.State;

                foreach (var item in qry)
                {
                    Console.WriteLine("State: {0}", item.Key);
                    foreach (Contact c in item)
                    {
                        Console.WriteLine("  {0}", c.LastName);
                    }
                }
            }

            {
                Console.WriteLine();
                Console.WriteLine("XML conversion - regular: ".PadRight(60, '-'));

                List<Contact> contacts = Contact.SampleData();
                List<CallLog> calls = CallLog.SampleData();

                Dictionary<string, List<CallLog>> callGroups = new Dictionary<string,List<CallLog>>();

                foreach (CallLog call in calls)
                {
                    if (callGroups.ContainsKey(call.Number))
                    {
                        if (call.Incoming == true)
                            callGroups[call.Number].Add(call);
                    }
                    else
                    {
                        if (call.Incoming == true)
                        {
                            List<CallLog> list = new List<CallLog>();
                            list.Add(call);
                            callGroups.Add(call.Number, list);
                        }
                    }
                }
                contacts.Sort( (c1, c2) => {
                    return c1.LastName.CompareTo(c2.LastName);
                });

                using (StringWriter s = new StringWriter())
                {
                    using (XmlTextWriter x = new XmlTextWriter(s))
                    {
                        x.Formatting = Formatting.Indented;
                        x.WriteStartDocument();
                        x.WriteComment("Summarized Incoming Call Stats");
                        x.WriteStartElement("contacts");
                        foreach (Contact con in contacts)
                        {
                            if (callGroups.ContainsKey(con.Phone))
                            {
                                List<CallLog> calls_ = callGroups[con.Phone];

                                long sum = 0;
                                foreach (CallLog call in calls_)
                                {
                                    sum += (long)call.Duration;
                                }

                                double avg = (double)sum / (double)calls_.Count();

                                x.WriteStartElement("contact");
                                x.WriteElementString("lastName", con.LastName);
                                x.WriteElementString("count", calls_.Count().ToString());
                                x.WriteElementString("totalDuration", sum.ToString());
                                x.WriteElementString("averageDuration", avg.ToString());
                                x.WriteEndElement();
                            }
                        }
                        x.WriteEndDocument();
                        x.Flush();

                        Console.WriteLine(s.ToString());
                    }
                }
            }


            {
                Console.WriteLine();
                Console.WriteLine("XML conversion - LINQ: ".PadRight(60, '-'));
                List<Contact> contacts = Contact.SampleData();
                List<CallLog> calls = CallLog.SampleData();

                XDocument doc = new XDocument(
                    new XComment("Summarized Incoming Call Stats"),
                    new XElement("contacts",
                        from call in calls
                        where call.Incoming == true
                        group call by call.Number into g
                        join contact in contacts on
                            g.Key equals contact.Phone
                        orderby contact.LastName
                        select new XElement("contact",
                            new XElement("lastName", contact.LastName),
                            new XElement("count", g.Count()),
                            new XElement("totalDuration", g.Sum(c => c.Duration)),
                            new XElement("averageDuration", g.Average(c => c.Duration))
                        )
                    )
                );

                Console.WriteLine(doc.ToString());
            }
        }
    }
}