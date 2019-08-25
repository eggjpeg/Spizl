using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SpazlUT
{
    [TestClass]
    public class UnitTest1
    {

        private string FS(string s)
        {
            return s.Replace("\r", "").Replace("\n", "");
        }

        [TestMethod]
        public void Bubble()
        {
            string actual = FS(SpazL.SpazL.Run("Tests/bubble.spaz"));
            string expected = "0123451098";
            Assert.AreEqual(expected, actual, "Bubble failed. spaz.");
        }

        [TestMethod]
        public void String()
        {
            string actual = FS(SpazL.SpazL.Run("Tests/string.spaz"));
            string expected = "spz123assspaz";
            Assert.AreEqual(expected, actual, "String failed. spaz.");
        }
    }
}
