using Rotomeca.Core.Optionals;

namespace Rotomeca.Core.Tests.Optionals;

public class MayBeTests
{
    // ── Constructor ───────────────────────────────────────────────────────────

    public class Constructor
    {
        [Fact]
        public void WithValue_HasValueIsTrue()
        {
            var maybe = new MayBe<string>("hello");
            Assert.True(maybe.HasValue);
        }

        [Fact]
        public void WithValue_ValueIsCorrect()
        {
            var maybe = new MayBe<string>("hello");
            Assert.Equal("hello", maybe.Value);
        }

        [Fact]
        public void WithNull_HasValueIsFalse()
        {
            var maybe = new MayBe<string>(null);
            Assert.False(maybe.HasValue);
        }

        [Fact]
        public void WithNull_EqualsNullInstance()
        {
            var maybe = new MayBe<string>(null);
            Assert.Equal(MayBe<string>.Null, maybe);
        }

        [Fact]
        public void WithValueType_HasValueIsTrue()
        {
            var maybe = new MayBe<int>(42);
            Assert.True(maybe.HasValue);
            Assert.Equal(42, maybe.Value);
        }

        [Fact]
        public void WithValueType_Zero_HasValueIsTrue()
        {
            // 0 n'est pas null — HasValue doit être true
            var maybe = new MayBe<int>(0);
            Assert.True(maybe.HasValue);
            Assert.Equal(0, maybe.Value);
        }

        [Fact]
        public void WithNullNullableValueType_HasValueIsFalse()
        {
            int? nullableInt = null;
            var maybe = new MayBe<int?>(nullableInt);
            Assert.False(maybe.HasValue);
        }
    }

    // ── DefaultAndNull ────────────────────────────────────────────────────────

    public class DefaultAndNull
    {
        [Fact]
        public void Default_HasValueIsFalse()
        {
            var maybe = default(MayBe<string>);
            Assert.False(maybe.HasValue);
        }

        [Fact]
        public void Null_HasValueIsFalse()
        {
            Assert.False(MayBe<string>.Null.HasValue);
        }

        [Fact]
        public void Default_And_Null_AreEqual()
        {
            var defaultMaybe = default(MayBe<string>);
            Assert.Equal(defaultMaybe, MayBe<string>.Null);
        }

        [Fact]
        public void Null_ValueIsDefault()
        {
            Assert.Equal(default, MayBe<string>.Null.Value);
        }

        [Fact]
        public void Null_IsEmpty()
        {
            Assert.True(MayBe<string>.Null.IsEmpty);
        }
    }

    // ── Properties ────────────────────────────────────────────────────────────

    public class Properties
    {
        [Fact]
        public void WhenPresent_HasValueIsTrue_IsEmptyIsFalse()
        {
            var maybe = new MayBe<string>("hello");
            Assert.True(maybe.HasValue);
            Assert.False(maybe.IsEmpty);
        }

        [Fact]
        public void WhenAbsent_HasValueIsFalse_IsEmptyIsTrue()
        {
            var maybe = MayBe<string>.Null;
            Assert.False(maybe.HasValue);
            Assert.True(maybe.IsEmpty);
        }

        [Fact]
        public void IsEmpty_IsAlwaysNegationOfHasValue()
        {
            var present = new MayBe<int>(1);
            var absent = MayBe<int>.Null;

            Assert.Equal(!present.HasValue, present.IsEmpty);
            Assert.Equal(!absent.HasValue, absent.IsEmpty);
        }

        [Fact]
        public void WhenPresent_Value_ReturnsEncapsulatedValue()
        {
            var maybe = new MayBe<int>(99);
            Assert.Equal(99, maybe.Value);
        }

        [Fact]
        public void WhenAbsent_Value_ReturnsDefault()
        {
            var maybe = MayBe<string>.Null;
            Assert.Equal(default, maybe.Value);
        }
    }

    // ── SomeFactory ───────────────────────────────────────────────────────────

    public class SomeFactory
    {
        [Fact]
        public void WithValue_HasValueIsTrue()
        {
            var maybe = MayBe<string>.Some("hello");
            Assert.True(maybe.HasValue);
        }

        [Fact]
        public void WithValue_ValueIsCorrect()
        {
            var maybe = MayBe<string>.Some("hello");
            Assert.Equal("hello", maybe.Value);
        }

        [Fact]
        public void WithValueType_HasValueIsTrue()
        {
            var maybe = MayBe<int>.Some(42);
            Assert.True(maybe.HasValue);
            Assert.Equal(42, maybe.Value);
        }

        [Fact]
        public void WithNull_TreatedAsAbsent()
        {
            var maybe = MayBe<string>.Some(null);
            Assert.False(maybe.HasValue);
            Assert.Equal(MayBe<string>.Null, maybe);
        }
    }

    // ── GetValueOrDefault ─────────────────────────────────────────────────────

    public class GetValueOrDefaultTests
    {
        [Fact]
        public void WhenPresent_ReturnsValue()
        {
            var maybe = MayBe<string>.Some("hello");
            Assert.Equal("hello", maybe.GetValueOrDefault("fallback"));
        }

        [Fact]
        public void WhenAbsent_ReturnsFallback()
        {
            var maybe = MayBe<string>.Null;
            Assert.Equal("fallback", maybe.GetValueOrDefault("fallback"));
        }

        [Fact]
        public void WhenAbsent_NoParam_ReturnsDefault()
        {
            var maybe = MayBe<string>.Null;
            Assert.Equal(default, maybe.GetValueOrDefault());
        }

        [Fact]
        public void WhenAbsent_WithValueType_ReturnsFallback()
        {
            var maybe = MayBe<int>.Null;
            Assert.Equal(-1, maybe.GetValueOrDefault(-1));
        }
    }

    // ── GetValueOrElse ────────────────────────────────────────────────────────

    public class GetValueOrElseTests
    {
        [Fact]
        public void WhenPresent_ReturnsValue()
        {
            var maybe = MayBe<string>.Some("hello");
            Assert.Equal("hello", maybe.GetValueOrElse(() => "fallback"));
        }

        [Fact]
        public void WhenPresent_FallbackNotInvoked()
        {
            var maybe = MayBe<string>.Some("hello");
            var invoked = false;

            maybe.GetValueOrElse(() => { invoked = true; return "fallback"; });

            Assert.False(invoked);
        }

        [Fact]
        public void WhenAbsent_InvokesFallback()
        {
            var maybe = MayBe<string>.Null;
            var invoked = false;

            maybe.GetValueOrElse(() => { invoked = true; return "fallback"; });

            Assert.True(invoked);
        }

        [Fact]
        public void WhenAbsent_ReturnsFallbackResult()
        {
            var maybe = MayBe<string>.Null;
            Assert.Equal("fallback", maybe.GetValueOrElse(() => "fallback"));
        }
    }

    // ── Deconstruct ───────────────────────────────────────────────────────────

    public class DeconstructTests
    {
        [Fact]
        public void WhenPresent_DestructuresCorrectly()
        {
            var maybe = MayBe<string>.Some("hello");
            var (hasValue, value) = maybe;

            Assert.True(hasValue);
            Assert.Equal("hello", value);
        }

        [Fact]
        public void WhenAbsent_DestructuresCorrectly()
        {
            var maybe = MayBe<string>.Null;
            var (hasValue, value) = maybe;

            Assert.False(hasValue);
            Assert.Equal(default, value);
        }

        [Fact]
        public void WithNull_TreatedAsAbsent()
        {
            var maybe = new MayBe<string>(null);
            var (hasValue, _) = maybe;

            Assert.False(hasValue);
        }
    }

    // ── ImplicitConversions ───────────────────────────────────────────────────

    public class ImplicitConversions
    {
        [Fact]
        public void FromValue_CreatesPresent()
        {
            MayBe<string> maybe = "hello";
            Assert.True(maybe.HasValue);
            Assert.Equal("hello", maybe.Value);
        }

        [Fact]
        public void FromNull_TreatedAsAbsent()
        {
            MayBe<string> maybe = (string?)null;
            Assert.False(maybe.HasValue);
            Assert.Equal(MayBe<string>.Null, maybe);
        }

        [Fact]
        public void FromValueType_CreatesPresent()
        {
            MayBe<int> maybe = 42;
            Assert.True(maybe.HasValue);
            Assert.Equal(42, maybe.Value);
        }

        [Fact]
        public void ToT_WhenPresent_ReturnsValue()
        {
            MayBe<string> maybe = "hello";
            string? value = maybe;
            Assert.Equal("hello", value);
        }

        [Fact]
        public void ToT_WhenAbsent_ReturnsDefault()
        {
            MayBe<string> maybe = MayBe<string>.Null;
            string? value = maybe;
            Assert.Equal(default, value);
        }
    }

    // ── Equality ──────────────────────────────────────────────────────────────

    public class EqualityTests
    {
        [Fact]
        public void TwoAbsent_AreEqual()
        {
            Assert.Equal(MayBe<string>.Null, MayBe<string>.Null);
        }

        [Fact]
        public void TwoPresent_WithSameValue_AreEqual()
        {
            var a = MayBe<string>.Some("hello");
            var b = MayBe<string>.Some("hello");
            Assert.Equal(a, b);
        }

        [Fact]
        public void TwoPresent_WithDifferentValues_AreNotEqual()
        {
            var a = MayBe<string>.Some("hello");
            var b = MayBe<string>.Some("world");
            Assert.NotEqual(a, b);
        }

        [Fact]
        public void Present_And_Absent_AreNotEqual()
        {
            var present = MayBe<string>.Some("hello");
            var absent = MayBe<string>.Null;
            Assert.NotEqual(present, absent);
        }

        [Fact]
        public void OperatorEqual_WithSameValue_IsTrue()
        {
            var a = MayBe<string>.Some("hello");
            var b = MayBe<string>.Some("hello");
            Assert.True(a == b);
        }

        [Fact]
        public void OperatorNotEqual_WithDifferentValues_IsTrue()
        {
            var a = MayBe<string>.Some("hello");
            var b = MayBe<string>.Some("world");
            Assert.True(a != b);
        }

        [Fact]
        public void EqualsObject_WithSameType_IsTrue()
        {
            var a = MayBe<string>.Some("hello");
            object b = MayBe<string>.Some("hello");
            Assert.True(a.Equals(b));
        }

        [Fact]
        public void EqualsObject_WithDifferentType_IsFalse()
        {
            var maybe = MayBe<string>.Some("hello");
            Assert.False(maybe.Equals((object?)"hello"));
        }

        [Fact]
        public void EqualsObject_WithNull_IsFalse()
        {
            var maybe = MayBe<string>.Some("hello");
            Assert.False(maybe.Equals(null));
        }

        [Fact]
        public void GetHashCode_EqualInstances_HaveSameHash()
        {
            var a = MayBe<string>.Some("hello");
            var b = MayBe<string>.Some("hello");
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void GetHashCode_TwoAbsent_HaveSameHash()
        {
            var a = MayBe<string>.Null;
            var b = default(MayBe<string>);
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Fact]
        public void GetHashCode_PresentAndAbsent_HaveDifferentHash()
        {
            var present = MayBe<string>.Some("hello");
            var absent = MayBe<string>.Null;
            Assert.NotEqual(present.GetHashCode(), absent.GetHashCode());
        }
    }

    // ── ToString ──────────────────────────────────────────────────────────────

    public class ToStringTests
    {
        [Fact]
        public void WhenPresent_Int_ReturnsValueToString()
        {
            Assert.Equal("42", MayBe<int>.Some(42).ToString());
        }

        [Fact]
        public void WhenPresent_String_ReturnsValue()
        {
            Assert.Equal("hello", MayBe<string>.Some("hello").ToString());
        }

        [Fact]
        public void WhenAbsent_ReturnsNullString()
        {
            Assert.Equal("null", MayBe<string>.Null.ToString());
        }
    }

    // ── Integration ───────────────────────────────────────────────────────────

    public class Integration
    {
        [Fact]
        public void NullConsistency_AcrossCreationMethods()
        {
            var viaConstructor = new MayBe<string>(null);
            var viaSome = MayBe<string>.Some(null);
            var viaImplicit = (MayBe<string>)(string?)null;

            Assert.Equal(MayBe<string>.Null, viaConstructor);
            Assert.Equal(MayBe<string>.Null, viaSome);
            Assert.Equal(MayBe<string>.Null, viaImplicit);
        }

        [Fact]
        public void GenericMethod_WithPresentResult()
        {
            static MayBe<T> Find<T>(T[] source, Func<T, bool> predicate)
            {
                foreach (var item in source)
                    if (predicate(item)) return item;
                return MayBe<T>.Null;
            }

            var found = Find(new[] { 1, 2, 3 }, x => x == 2);
            Assert.True(found.HasValue);
            Assert.Equal(2, found.Value);
        }

        [Fact]
        public void GenericMethod_WithAbsentResult()
        {
            static MayBe<T> Find<T>(T[] source, Func<T, bool> predicate)
            {
                foreach (var item in source)
                    if (predicate(item)) return item;
                return MayBe<T>.Null;
            }

            var notFound = Find(new[] { 1, 2, 3 }, x => x == 99);
            Assert.False(notFound.HasValue);
        }

        [Fact]
        public void WithDateTime_WorksCorrectly()
        {
            var date = new DateTime(2024, 1, 1);
            var maybe = MayBe<DateTime>.Some(date);

            Assert.True(maybe.HasValue);
            Assert.Equal(date, maybe.Value);
        }

        [Fact]
        public void ChainGetValueOrElse_WithFallbackMayBe()
        {
            MayBe<string> primary = MayBe<string>.Null;
            MayBe<string> secondary = MayBe<string>.Some("secondary");

            string? result = primary.GetValueOrElse(() => secondary.GetValueOrDefault("fallback"));

            Assert.Equal("secondary", result);
        }
    }
}