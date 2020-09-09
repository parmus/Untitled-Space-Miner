using NUnit.Framework;
using SpaceGame.Utility;

namespace SpaceGame._Project.Systems.Utility.Tests.EditMode
{
    [Author("Martin Parm", "parmus@gmail.com")]
    public class ObservableTests
    {
        private const float VALUE1 = 42f;
        
        [Test]
        public void InvokesCallbackExactlyOncePerChange()
        {
            var callbackCount = 0;
            var o = new Observable<float>();
            
            o.OnChange += f => callbackCount++;
            Assert.AreEqual(callbackCount, 0, "The callback has not been called yet");
            Assert.AreEqual(o.Value, default(float), "Value contains the default value");

            o.Set(VALUE1);
            o.Set(VALUE1);
            
            Assert.AreEqual(callbackCount, 1, "The callback has been called once");
            Assert.AreEqual(o.Value, VALUE1, "Value contains VALUE1");
        }

        [Test]
        public void ValuePropertySetter()
        {
            var callbackCount = 0;
            var o = new Observable<float>();
            
            o.OnChange += f => callbackCount++;
            Assert.AreEqual(callbackCount, 0);
            Assert.AreEqual(o.Value, default(float));

            o.Value = VALUE1;
            
            Assert.AreEqual(callbackCount, 1);
            Assert.AreEqual(o.Value, VALUE1);
        }
    }
}
