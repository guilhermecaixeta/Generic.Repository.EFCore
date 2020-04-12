using Generic.Repository.Exceptions;
using Generic.Repository.ThrowError;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Generic.RepositoryTest.Unit.ThrowError
{
    [TestFixture]
    public class ThrowErrorIfTest
    {
        [Test]
        public void ThrowErrorIf_FieldNoHasSameValue() =>
            Assert.Throws<NotEqualsFieldException>(() => ThrowErrorIf.FieldNoHasSameValue(1, 0));

        [Test]
        public void ThrowErrorIf_IsEmptyOrNullString() =>
            Assert.Throws<ArgumentNullException>(() => ThrowErrorIf.IsEmptyOrNullString("", string.Empty, string.Empty));

        [Test]
        public void ThrowErrorIf_IsLessThanOrEqualsZero() =>
            Assert.Throws<LessThanOrEqualsZeroException>(() =>
                {
                    ThrowErrorIf.IsLessThanOrEqualsZero(-1);
                }
            );

        [Test]
        public void ThrowErrorIf_IsEqualsZero() =>
            Assert.Throws<LessThanOrEqualsZeroException>(() =>
            {
                ThrowErrorIf.IsLessThanOrEqualsZero(0);
            });

        [Test]
        public void ThrowErrorIf_IsLessThanZero_InvalidValue()
        {
            try
            {
                ThrowErrorIf.IsLessThanZero(0);
                Assert.IsTrue(true);
            }
            catch
            {
                Assert.IsTrue(false);
            }
        }

        [Test]
        public void ThrowErrorIf_IsLessThanZero() =>
            Assert.Throws<LessThanZeroException>(() =>
                {
                    ThrowErrorIf.IsLessThanZero(-1);
                }
            );

        [Test]
        public void ThrowErrorIf_IsNullOrEmptyList() =>
            Assert.Throws<ListNullOrEmptyException>(() =>
            ThrowErrorIf.IsNullOrEmptyList(new List<string>(), string.Empty, string.Empty));

        [Test]
        public void ThrowErrorIf_IsNullValue() =>
            Assert.Throws<ArgumentNullException>(() =>
            ThrowErrorIf.IsNullValue(default, string.Empty, string.Empty));

        [Test]
        public void ThrowErrorIf_IsTypeNotEquals() =>
            Assert.Throws<InvalidTypeException>(() => ThrowErrorIf.IsTypeNotEquals<int>(string.Empty));

        [Test]
        public void ThrowErrorIf_TypeIsNotAllowed() =>
        Assert.Throws<InvalidTypeException>(() => ThrowErrorIf.TypeIsNotAllowed<string>(string.Empty));
    }
}