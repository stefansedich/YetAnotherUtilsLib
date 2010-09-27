using NUnit.Framework;
using YetAnotherUtilsLib.Core.Common;

namespace YetAnotherUtilsLib.Core.Tests.Common
{
    [TestFixture]
    public class ExtensionsFixture
    {
        [Test]
        public void NullSafe_Returns_Leaf_Value_If_All_Objects_Along_Path_Not_Null()
        {
            // Arrange
            var foo = new Foo {Inner = new Inner{MoreInner = new MoreInner()}};
            
            // Act
            var result1 = foo.NullSafe(f => f.Inner);
            var result2 = foo.NullSafe(f => f.Inner.Name);
            var result3 = foo.NullSafe(f => f.Inner.GetBool());
            var result4 = foo.NullSafe(f => f.Inner.MoreInner.Age);

            // Assert  
            Assert.AreEqual(foo.Inner, result1);
            Assert.AreEqual(foo.Inner.Name, result2);
            Assert.AreEqual(foo.Inner.GetBool(), result3);
            Assert.AreEqual(foo.Inner.MoreInner.Age, result4);
        }

        [Test]
        public void NullSafe_Returns_Default_Value_For_Leaf_If_Any_Object_Along_Path_Is_Null()
        {
            // Arrange
            var foo = new Foo();

            // Act
            var result1 = foo.NullSafe(f => f.Inner);
            var result2 = foo.NullSafe(f => f.Inner.Name);
            var result3 = foo.NullSafe(f => f.Inner.GetBool());
            var result4 = foo.NullSafe(f => f.Inner.MoreInner.Age);

            // Assert  
            Assert.AreEqual(default(Foo), result1);
            Assert.AreEqual(default(string), result2);
            Assert.AreEqual(default(bool), result3);
            Assert.AreEqual(default(int), result4);
        }

        public class Foo
        {
            public Inner Inner { get; set; }
        }

        public class Inner
        {
            public string Name { get; set; }
            public MoreInner MoreInner { get; set; }

            public Inner()
            {
                Name = "Bob";
            }

            public bool GetBool()
            {
                return true;
            }
        }

        public class MoreInner
        {
            public int Age { get; set; }

            public MoreInner()
            {
                Age = 100;
            }
        }
    }
}