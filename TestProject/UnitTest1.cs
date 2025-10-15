using PizzaConsole_5;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestProject
{
    [TestClass]
    public class PizzaTest
    {
        [TestMethod]
        [DataRow(0, "aaa", 1, 1)]
        [DataRow(1, "aaa", 1, 1)]
        [DataRow(1, "abcabcabcabc", 1, 1)]
        [DataRow(1, "aaabbb", 1, 1)]
        [DataRow(2, "aaa", 100f, 1)]
        [DataRow(2, "abcabcabcabc", 10.5f, 1)]
        [DataRow(2, "aaabbb", 0.75f, 1)]
        [DataRow(3, "aaa", 100, 9.999)]
        [DataRow(3, "abcabcabcabc", 10.5f, 1.5)]
        [DataRow(3, "aaabbb", 0.75f, 0.625)]
        public void PizzaPrice_Test_correct_data(int type, string name, float price, double weight)
        {
            //Arrange
            PizzaClass? pizza = null;

            switch (type)
            {
                case 0:
                    pizza = new PizzaClass();
                    break;
                case 1:
                    pizza = new PizzaClass(name);
                    break;
                case 2:
                    pizza = new PizzaClass(name, price);
                    break;
                case 3:
                    pizza = new PizzaClass(name, price, weight);
                    break;
                default:
                    break;
            }

            //Act+Assert

            Assert.IsNotNull(pizza);

            if (type > 0) Assert.AreEqual(pizza.Name, name);
            if (type > 1) Assert.AreEqual(pizza.Price, price);
            if (type > 2) Assert.AreEqual(pizza.Weight, weight);
        }

        [TestMethod]
        [ExpectedException (typeof(ArgumentNullException))]
        [DataRow(0, "", 1, 1)]
        [DataRow(0, "a", 1, 1)]
        [DataRow(0, "aa", 1, 1)]
        [DataRow(0, "aaaaaaaaaaaaa", 1, 1)]
        [DataRow(0, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 1, 1)]
        [DataRow(1, "", 1, 1)]
        [DataRow(1, "a", 1, 1)]
        [DataRow(1, "aa", 1, 1)]
        [DataRow(1, "aaaaaaaaaaaaa", 1, 1)]
        [DataRow(1, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 1, 1)]
        [DataRow(2, "", 1, 1)]
        [DataRow(2, "a", 1, 1)]
        [DataRow(2, "aa", 1, 1)]
        [DataRow(2, "aaaaaaaaaaaaa", 1, 1)]
        [DataRow(2, "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", 1, 1)]
        public void PizzaName_Test_length_less_or_more_than_need(int type, string name, float price, double weight)
        {
            //Arrange
            PizzaClass pizza;
            switch (type)
            {
                case 0:
                    pizza = new PizzaClass(name);
                    break;
                case 1: 
                    pizza = new PizzaClass(name, price);
                    break;
                case 2:
                    pizza = new PizzaClass(name, price, weight);
                    break;
            }

            //Act+Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(1, "aaa", -50, 1)]
        [DataRow(1, "aaa", -9999999999999, 1)]
        [DataRow(1, "aaa", 0, 1)]
        [DataRow(1, "aaa", 10000, 1)]
        [DataRow(1, "aaa", 999999999999999, 1)]
        [DataRow(2, "aaa", -50, 1)]
        [DataRow(2, "aaa", -9999999999999, 1)]
        [DataRow(2, "aaa", 0, 1)]
        [DataRow(2, "aaa", 10000, 1)]
        [DataRow(2, "aaa", 999999999999999, 1)]
        public void PizzaPrice_Test_amount_less_or_more_than_need(int type, string name, float price, double weight)
        {
            //Arrange
            PizzaClass pizza;
            switch (type)
            {
                case 0:
                    pizza = new PizzaClass(name);
                    break;
                case 1:
                    pizza = new PizzaClass(name, price);
                    break;
                case 2:
                    pizza = new PizzaClass(name, price, weight);
                    break;
            }

            //Act+Assert
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        [DataRow(2, "aaa", 1, -0.5)]
        [DataRow(2, "aaa", 1, -99999999999999)]
        [DataRow(2, "aaa", 1, 0)]
        [DataRow(2, "aaa", 1, 10)]
        [DataRow(2, "aaa", 1, 9999999999999999)]
        public void PizzaWeight_Test_amount_less_or_more_than_need(int type, string name, float price, double weight)
        {
            //Arrange
            PizzaClass pizza;
            switch (type)
            {
                case 0:
                    pizza = new PizzaClass(name);
                    break;
                case 1:
                    pizza = new PizzaClass(name, price);
                    break;
                case 2:
                    pizza = new PizzaClass(name, price, weight);
                    break;
            }

            //Act+Assert
        }

        [TestMethod]
        [DataRow("", true)]
        [DataRow(" ", true)]
        [DataRow("0", false)]
        [DataRow("0bc", false)]
        [DataRow("abc ", true)]
        [DataRow(" abc", true)]
        [DataRow(" abc -8", false)]
        [DataRow("abc 0", false)]
        [DataRow(" abc 10", true)]
        [DataRow("abc 10000", false)]
        [DataRow("abc 5d", false)]
        [DataRow("abc 5 -6", false)]
        [DataRow("abc 5 0", false)]
        [DataRow("abc 5 0.8", true)]
        [DataRow("abc 5 9.999", true)]
        [DataRow("abc 5 5b", false)]
        [DataRow("abc 5 10", false)]
        [DataRow("abc 5 1000", false)]
        [DataRow("abc 5 100 7", false)]
        [DataRow("abc 5 1000 abc", false)]
        public void PizzaFromString_Test(string line, bool expected)
        {
            //Arrange
            PizzaClass? pizza;

            //Act

            bool actual = PizzaClass.TryParse(line, out pizza);

            //Assert

            Assert.AreEqual(expected, actual);
        }
    }
}