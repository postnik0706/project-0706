using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace entities
{
    public class Contact
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string State { get; set; }

        public override string ToString()
        {
            return String.Format("Contact {0} {1}", FirstName, LastName) + Environment.NewLine +
                String.Format("\tEmail: {0}", Email) + Environment.NewLine +
                String.Format("\tPhone: {0}", Phone) + Environment.NewLine +
                String.Format("\tDateOfBirth: {0}", DateOfBirth.ToString("dd/MM/yyyy")) + Environment.NewLine +
                String.Format("\tState: {0}", State);
        }

        public string ToScript()
        {
            return String.Format("new Contact() {{ FirstName = \"{0}\", LastName = \"{1}\", Phone = \"{2}\", DateOfBirth = DateTime.Parse(\"{3}\"), State = \"{4}\" }}",
                FirstName, LastName, Phone, DateOfBirth.ToShortDateString(), State);
        }

        public static List<Contact> SampleData()
        {
            return new List<Contact>() {
              new Contact() { FirstName = "Barney", LastName = "Gottshall", Phone = "885 983 8858", DateOfBirth = DateTime.Parse("10/19/1945"), State = "CA" },
              new Contact() { FirstName = "Armando", LastName = "Valdes", Phone = "848 553 8487", DateOfBirth = DateTime.Parse("12/9/1973"), State = "WA" },
              new Contact() { FirstName = "Adam", LastName = "Gauwain", Phone = "115 999 1154", DateOfBirth = DateTime.Parse("10/3/1959"), State = "AK" },
              new Contact() { FirstName = "Jeffery", LastName = "Deane", Phone = "677 602 6774", DateOfBirth = DateTime.Parse("12/16/1950"), State = "CA" },
              new Contact() { FirstName = "Collin", LastName = "Zeeman", Phone = "603 303 6030", DateOfBirth = DateTime.Parse("2/10/1935"), State = "FL" },
              new Contact() { FirstName = "Stewart", LastName = "Kagel", Phone = "546 607 5462", DateOfBirth = DateTime.Parse("2/20/1950"), State = "WA" },
              new Contact() { FirstName = "Chance", LastName = "Lard", Phone = "278 918 2789", DateOfBirth = DateTime.Parse("10/21/1951"), State = "WA" },
              new Contact() { FirstName = "Blaine", LastName = "Reifsteck", Phone = "715 920 7157", DateOfBirth = DateTime.Parse("5/18/1946"), State = "TX" },
              new Contact() { FirstName = "Mack", LastName = "Kamph", Phone = "364 202 3644", DateOfBirth = DateTime.Parse("9/17/1977"), State = "TX" },
              new Contact() { FirstName = "Ariel", LastName = "Hazelgrove", Phone = "165 737 1656", DateOfBirth = DateTime.Parse("5/23/1922"), State = "OR" },
              new Contact() { FirstName = "Alex", LastName = "Postnikov", Phone = "165 123 4545", DateOfBirth = DateTime.Parse("1/05/1971"), State = "OR" }
            };
        }
    }

    public class CallLog
    {
        public string Number { get; set; }
        public int Duration { get; set; }
        public bool Incoming { get; set; }
        public DateTime When { get; set; }

        public override string ToString()
        {
            return String.Format("Call {0}", Number) + Environment.NewLine +
                String.Format("\tDuration: {0}", Duration) + Environment.NewLine +
                String.Format("\tIncoming: {0}", Incoming) + Environment.NewLine +
                String.Format("\tWhen: {0}", When);
        }

        public string ToScript()
        {
            return String.Format("new CallLog() {{ Number = \"{0}\", Duration = {1}, Incoming = {2}, When = DateTime.Parse(\"{3}\") }}",
                Number, Duration, Incoming.ToString().ToLower(), When);
        }

        public static List<CallLog> SampleData()
        {
            return new List<CallLog>() {
              new CallLog() { Number = "885 983 8858", Duration = 2, Incoming = true, When = DateTime.Parse("8/7/2006 8:12:00 AM") },
              new CallLog() { Number = "165 737 1656", Duration = 15, Incoming = true, When = DateTime.Parse("8/7/2006 9:23:00 AM") },
              new CallLog() { Number = "364 202 3644", Duration = 1, Incoming = false, When = DateTime.Parse("8/7/2006 10:05:00 AM") },
              new CallLog() { Number = "603 303 6030", Duration = 2, Incoming = false, When = DateTime.Parse("8/7/2006 10:35:00 AM") },
              new CallLog() { Number = "546 607 5462", Duration = 4, Incoming = true, When = DateTime.Parse("8/7/2006 11:15:00 AM") },
              new CallLog() { Number = "885 983 8858", Duration = 15, Incoming = false, When = DateTime.Parse("8/7/2006 1:12:00 PM") },
              new CallLog() { Number = "885 983 8858", Duration = 3, Incoming = true, When = DateTime.Parse("8/7/2006 1:47:00 PM") },
              new CallLog() { Number = "546 607 5462", Duration = 1, Incoming = false, When = DateTime.Parse("8/7/2006 8:34:00 PM") },
              new CallLog() { Number = "546 607 5462", Duration = 3, Incoming = false, When = DateTime.Parse("8/8/2006 10:10:00 AM") },
              new CallLog() { Number = "603 303 6030", Duration = 23, Incoming = false, When = DateTime.Parse("8/8/2006 10:40:00 AM") },
              new CallLog() { Number = "848 553 8487", Duration = 3, Incoming = false, When = DateTime.Parse("8/8/2006 2:00:00 PM") },
              new CallLog() { Number = "848 553 8487", Duration = 7, Incoming = true, When = DateTime.Parse("8/8/2006 2:37:00 PM") },
              new CallLog() { Number = "278 918 2789", Duration = 6, Incoming = true, When = DateTime.Parse("8/8/2006 3:23:00 PM") },
              new CallLog() { Number = "364 202 3644", Duration = 20, Incoming = true, When = DateTime.Parse("8/8/2006 5:12:00 PM") }
            };
        }
    }

    static class ReadAndGenerateTestData
    {
        public static void GenerateScript()
        {
            List<Contact> contacts = new List<Contact>();
            List<CallLog> calls = new List<CallLog>();

            string[] lines = File.ReadAllLines(@"..\..\..\Input.txt");
            foreach (var item in lines)
            {
                if (item.Trim() == "")
                    continue;

                string[] buf = item.Split(' ');
                short num;
                if (!Int16.TryParse(buf[0], out num))
                {
                    contacts.Add(new Contact()
                    {
                        FirstName = buf[0],
                        LastName = buf[1],
                        DateOfBirth = DateTime.Parse(buf[2]),
                        Phone = buf[3] + " " + buf[4] + " " + buf[5],
                        State = buf[6]
                    });
                }
                else
                {
                    calls.Add(new CallLog()
                    {
                        Number = buf[0] + " " + buf[1] + " " + buf[2],
                        Duration = Int16.Parse(buf[3]),
                        Incoming = Boolean.Parse(buf[4]),
                        When = DateTime.Parse(buf[5] + " " + buf[6])
                    });
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("new List<Contact>() {");
            foreach (var item in contacts)
            {
                string postfix = "";
                if (item != contacts.Last())
                    postfix = ",";
                sb.AppendLine("  " + item.ToScript() + postfix);
            }
            sb.AppendLine("};");

            sb.AppendLine("new List<CallLog>() {");
            foreach (var item in calls)
            {
                string postfix = "";
                if (item != calls.Last())
                    postfix = ",";
                sb.AppendLine("  " + item.ToScript() + postfix);
            }
            sb.AppendLine("};");

            File.WriteAllText("out.txt", sb.ToString());
        }
    }
}
