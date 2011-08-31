using System.IO;
using System.Text;
using CDTag.Common.Json;
using CDTag.Common.Tests.Json.Model;
using NUnit.Framework;

namespace CDTag.Common.Tests.Json
{
    [TestFixture]
    public class JsonSerializerTest
    {
        [Test]
        public void DeserializePerson()
        {
            string json = Properties.Resources.person_json;

            Person person;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                person = JsonSerializer.ReadObject<Person>(stream);
            }

            AssertPerson(person);
        }

        [Test]
        public void SerializePerson()
        {
            Person person = new Person();
            person.FirstName = "John";
            person.LastName = "Smith";
            person.Age = 25;
            person.Address = new Address { StreetAddress = "21 2nd Street", City = "New York", State = "NY", PostalCode = "10021" };
            person.PhoneNumber = new[] {
                new PhoneNumber { Type = "home", Number = "212 555-1234" },
                new PhoneNumber { Type = "fax", Number = "646 555-4567" },
            };

            AssertPerson(person);

            string json = JsonSerializer.SerializeObject(person);

            Person deserializedPerson;
            using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                deserializedPerson = JsonSerializer.ReadObject<Person>(stream);
            }

            AssertPerson(deserializedPerson);
        }

        private static void AssertPerson(Person person)
        {
            Assert.IsNotNull(person);
            Assert.IsNotNull(person.PhoneNumber);
            Assert.IsNotNull(person.Address);

            Assert.AreEqual("John", person.FirstName);
            Assert.AreEqual("Smith", person.LastName);
            Assert.AreEqual(25, person.Age);

            Assert.AreEqual("21 2nd Street", person.Address.StreetAddress);
            Assert.AreEqual("New York", person.Address.City);
            Assert.AreEqual("NY", person.Address.State);
            Assert.AreEqual("10021", person.Address.PostalCode);

            Assert.AreEqual(2, person.PhoneNumber.Length);

            Assert.AreEqual("home", person.PhoneNumber[0].Type);
            Assert.AreEqual("212 555-1234", person.PhoneNumber[0].Number);

            Assert.AreEqual("fax", person.PhoneNumber[1].Type);
            Assert.AreEqual("646 555-4567", person.PhoneNumber[1].Number);
        }
    }
}
