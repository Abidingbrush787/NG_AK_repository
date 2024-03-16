using UPojazd;

namespace TestPojazdClass
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            [TestMethod]
            void Opis_ZwracaPoprawnyOpisPojazdu()
            {
                // Arrange
                var pojazd = new Pojazd("Samoch�d", 120);

                // Act
                var opis = pojazd.Opis();

                // Assert
                Assert.AreEqual("Pojazd Samoch�d mo�e jecha� z maksymaln� pr�dko�ci� 120 km/h.", opis);
            }
        }

        [TestClass]
        public class SamochodTests
        {
            [TestMethod]
            public void Opis_ZwracaPoprawnyOpisSamochodu()
            {
                // Arrange
                var samochod = new Samochod("Sedan", 180, 4);

                // Act
                var opis = samochod.Opis();

                // Assert
                Assert.AreEqual("Pojazd Sedan mo�e jecha� z maksymaln� pr�dko�ci� 180 km/h. Ma 4 drzwi.", opis);
            }
        }
    }
}