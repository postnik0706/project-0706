using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace app_2
{
    class Contact
    {
        public string LastName { get; set; }
        public string State { get; set; }
        public string Phone { get; set; }
        public string Key { get { return State; } }

        public static List<Contact> SampleData()
        {
            return new List<Contact>() {
                new Contact() { LastName = "Postnikov", State="LA", Phone = "1111" },
                new Contact() { LastName = "Tkachenko", State="WA",  Phone = "2222" },
                new Contact() { LastName = "Ivanov", State = "WA", Phone = "3333" },
                new Contact() { LastName = "Petrov", State = "LA", Phone = "4444" },
                new Contact() {LastName = "Sidorov", State = "RU", Phone = "5555" },
                new Contact() {LastName = "Abdoulkhalikov", State = "LA", Phone = "6666" }
            };
        }
    }

    class CallLog
    {
        public string Phone { get; set; }
        public TimeSpan Duration { get; set; }
        public bool IsIncoming { get; set; }

        public static List<CallLog> SampleData()
        {
            return new List<CallLog>() {
                new CallLog() { Phone = "1111", Duration = new TimeSpan(0, 1, 99), IsIncoming = true },
                new CallLog() { Phone = "2222", Duration = new TimeSpan(1, 13, 22), IsIncoming = false },
                new CallLog() { Phone = "3333", Duration = new TimeSpan(0, 1, 32), IsIncoming = true },
                new CallLog() { Phone = "4444", Duration = new TimeSpan(1, 12, 89), IsIncoming = true },
                new CallLog() { Phone = "5555", Duration = new TimeSpan(0, 0, 23), IsIncoming = false },
                new CallLog() { Phone = "2222", Duration = new TimeSpan(0, 3, 19), IsIncoming = true },
                new CallLog() { Phone = "3333", Duration = new TimeSpan(0, 0, 5), IsIncoming = false },
                new CallLog() { Phone = "3333", Duration = new TimeSpan(0, 6, 19), IsIncoming = true },
                new CallLog() { Phone = "2222", Duration = new TimeSpan(1, 1, 23), IsIncoming = true },
                new CallLog() { Phone = "2222", Duration = new TimeSpan(1, 5, 24), IsIncoming = true }
            };
        }
    }

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
                    if (callGroups.ContainsKey(call.Phone))
                    {
                        if (call.IsIncoming == true)
                            callGroups[call.Phone].Add(call);
                    }
                    else
                    {
                        if (call.IsIncoming == true)
                        {
                            List<CallLog> list = new List<CallLog>();
                            list.Add(call);
                            callGroups.Add(call.Phone, list);
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
                                    sum += (long)call.Duration.TotalSeconds;
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
                        where call.IsIncoming == true
                        group call by call.Phone into g
                        join contact in contacts on
                            g.Key equals contact.Phone
                        orderby contact.LastName
                        select new XElement("contact",
                            new XElement("lastName", contact.LastName),
                            new XElement("count", g.Count()),
                            new XElement("totalDuration", g.Sum(c => c.Duration.TotalSeconds)),
                            new XElement("averageDuration", g.Average(c => c.Duration.TotalSeconds))
                        )
                    )
                );

                Console.WriteLine(doc.ToString());
            }
        }
    }
}