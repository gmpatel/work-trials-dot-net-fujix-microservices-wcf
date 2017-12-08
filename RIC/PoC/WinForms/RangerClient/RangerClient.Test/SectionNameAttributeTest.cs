using System;
using System.Linq;
using System.Reflection;
using FujiXerox.RangerClient.Attributes;
using FujiXerox.RangerClient.Helpers;
using FujiXerox.RangerClient.Models.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RangerClient.Test
{
    [TestClass]
    public class SectionNameAttributeTest
    {
        [TestMethod]
        public void IqaGeneral_Get_Tag_Attributes()
        {
            // Arrange
            const string expected = "IQA.General";

            // Act
            var attrs = Attribute.GetCustomAttributes(typeof (IqaGeneral));

            // Assert
            foreach (var actual in attrs.OfType<SectionNameAttribute>().Select(tag => tag.GetName()))
            {
                Assert.AreEqual(expected, actual);
            }
        }

        [TestMethod]
        public void IqaGeneral_Get_Tag_Attribute()
        {
            // Arrange
            const string expected = "IQA.General";

            // Act
            var attr = Attribute.GetCustomAttribute(typeof (IqaGeneral), typeof (SectionNameAttribute));
            var a = (SectionNameAttribute)attr;
            var actual = a.GetName();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IqaGeneral_Get_Property_EnableIqa_Equals_EnableIQA()
        {
            // Arrange
            const string expected = "EnableIQA";

            // Act
            var props = typeof (IqaGeneral).GetProperty("EnableIqa");
            var attrs = props.GetCustomAttributes(true);
            foreach (var actual in attrs.OfType<ValueNameAttribute>().Select(elementTagAttr => elementTagAttr.GetName()))
           
            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IqaGeneral_Get_Property_EnableIqa_Equals_EnableIQA_attr()
        {
            // Arrange
            const string expected = "EnableIQA";

            // Act
            var props = typeof (IqaGeneral).GetProperty("EnableIqa");
            var attr = props.GetCustomAttribute(typeof (ValueNameAttribute));
            var a = (ValueNameAttribute) attr;
            var actual = a.GetName();

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void IqaGeneral_EnableIqa_true()
        {
            // Arrange
            var item = new IqaGeneral();
            var property = typeof (IqaGeneral).GetProperty("EnableIqa");

            // Act
            AttributeHelper.UpdatePropertyValueFromAttribute(item, property, TestFuncTrue);
            var actual = item.EnableIqa;

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void IqaGeneral_EnableIqa_false()
        {
            // Arrange
            var item = new IqaGeneral();
            var property = typeof(IqaGeneral).GetProperty("EnableIqa");

            // Act
            AttributeHelper.UpdatePropertyValueFromAttribute(item, property, TestFuncFalse);
            var actual = item.EnableIqa;

            // Assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void OptionalDevices_Get_All_Properties()
        {
            // Arrange
            var item = new OptionalDevices();

            // Act
            var props = typeof (OptionalDevices).GetProperties();
            foreach (var propertyInfo in props)
            {
                AttributeHelper.UpdatePropertyValueFromAttribute(item, propertyInfo, TestUpdateProperty);
            }

            // Assert
            Assert.IsTrue(item.NeedMicrEncoder);
        }

        private object TestUpdateProperty(string section, string value)
        {
            if (value == "BWTiffCompliance") return "Tiff";
            return section != value;
        }

        [SectionName("FRED")]
        public class Fred
        {
            public Fred()
            {
                BoolBlah = false;
            }

            public string Blah { get; set; }
            public bool BoolBlah { get; private set; }
        }

        public static class MyHelper
        {
            public static void DoStuff<T>(T item, PropertyInfo property, string value)
            {
                property.SetValue(item, value);
            }

            public static void DoStuff2<T>(T item, PropertyInfo property, Func<string, string, object> func)
            {
                var value = func.Invoke("result", "other");
                property.SetValue(item, value);
            }

        }

        [TestMethod]
        public void Mess_Around_1()
        {
            // Arrange
            const string expected = "result";
            var fred = new Fred {Blah = "init"};
            var prop = typeof (Fred).GetProperty("Blah");

            // Act
            prop.SetValue(fred, expected);
            var actual = fred.Blah;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Mess_Around_2()
        {
            // Arrange
            const string expected = "result";
            var fred = new Fred { Blah = "init" };
            var prop = typeof(Fred).GetProperty("Blah");
            // Act
            MyHelper.DoStuff(fred, prop, expected);
            var actual = fred.Blah;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Mess_Around_3()
        {
            // Arrange
            const string expected = "other";
            var fred = new Fred { Blah = "init" };
            var prop = typeof(Fred).GetProperty("Blah");

            // Act
            MyHelper.DoStuff2(fred, prop, TestFunc);
            var actual = fred.Blah;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Mess_Around_4()
        {
            // Arrange
            const bool expected = true;
            var fred = new Fred { Blah = "init" };
            var prop = typeof(Fred).GetProperty("BoolBlah");

            // Act
            MyHelper.DoStuff2(fred, prop, TestFuncTrue);
            var actual = fred.BoolBlah;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        public object TestFunc(string value, string other)
        {
            return other;
        }

        public object TestFuncFredBlahOther(string value, string other)
        {
            var result = "Error";
            if ((value == "Fred" || value == "FRED") && other == "Blah") result = "Other";
            return result;
        }

        public object TestFuncTrue(string value, string other)
        {
            return true;
        }

        public object TestFuncFalse(string value, string other)
        {
            return false;
        }

        [TestMethod]
        public void AttributeHelper_Test_string()
        {
            // Arrange
            const string expected = "Other";
            var fred = new Fred { Blah = "init" };
            var prop = typeof(Fred).GetProperty("Blah");

            // Act
            AttributeHelper.UpdatePropertyValueFromAttribute(fred, prop, TestFuncFredBlahOther);
            var actual = fred.Blah;

            // Assert
            Assert.AreEqual(expected, actual);

        }

        [TestMethod]
        public void AttributeHelper_Test_true()
        {
            // Arrange
            const bool expected = true;
            var fred = new Fred { Blah = "init" };
            var prop = typeof(Fred).GetProperty("BoolBlah");

            // Act
            AttributeHelper.UpdatePropertyValueFromAttribute(fred, prop, TestFuncTrue);
            var actual = fred.BoolBlah;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void AttributeHelper_Test_false()
        {
            // Arrange
            const bool expected = false;
            var fred = new Fred { Blah = "init" };
            var prop = typeof(Fred).GetProperty("BoolBlah");

            // Act
            AttributeHelper.UpdatePropertyValueFromAttribute(fred, prop, TestFuncFalse);
            var actual = fred.BoolBlah;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}
