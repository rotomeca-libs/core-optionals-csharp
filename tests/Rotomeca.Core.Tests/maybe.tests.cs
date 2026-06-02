using Rotomeca.Core.Optionals;

namespace Rotomeca.Core.Tests.Optionals
{
    /// <summary>
    /// Tests unitaires pour <see cref="MayBe{T}"/>.
    /// Couvre : constructeurs, factories, propriétés, extraction, conversions, égalité, ToString.
    /// </summary>
    public class MayBeTests
    {
        // ────────────────────────────────────────────────────────────────────
        // Constructeur public MayBe(T? value)
        // ────────────────────────────────────────────────────────────────────

        public class Constructor
        {
            [Fact]
            public void WithNonNullValue_HasValueIsTrue()
            {
                var maybe = new MayBe<string>("hello");

                Assert.True(maybe.HasValue);
            }

            [Fact]
            public void WithNonNullValue_ValueIsSet()
            {
                var maybe = new MayBe<string>("hello");

                Assert.Equal("hello", maybe.Value);
            }

            [Fact]
            public void WithNullReference_HasValueIsTrue()
            {
                // null explicite : présent mais nul
                var maybe = new MayBe<string>(null);

                Assert.True(maybe.HasValue);
            }

            [Fact]
            public void WithNullReference_ValueIsNull()
            {
                var maybe = new MayBe<string>(null);

                Assert.Null(maybe.Value);
            }

            [Fact]
            public void WithValueType_HasValueIsTrue()
            {
                var maybe = new MayBe<int>(42);

                Assert.True(maybe.HasValue);
                Assert.Equal(42, maybe.Value);
            }

            [Fact]
            public void WithZeroValueType_HasValueIsTrue()
            {
                var maybe = new MayBe<int>(0);

                Assert.True(maybe.HasValue);
            }
        }

        // ────────────────────────────────────────────────────────────────────
        // default / Null
        // ────────────────────────────────────────────────────────────────────

        public class DefaultAndNull
        {
            [Fact]
            public void Default_HasValueIsFalse()
            {
                MayBe<string> maybe = default;

                Assert.False(maybe.HasValue);
            }

            [Fact]
            public void Default_IsEmptyIsTrue()
            {
                MayBe<string> maybe = default;

                Assert.True(maybe.IsEmpty);
            }

            [Fact]
            public void Default_ValueIsDefault()
            {
                MayBe<int> maybe = default;

                Assert.Equal(default, maybe.Value);
            }

            [Fact]
            public void NullField_HasValueIsFalse()
            {
                var maybe = MayBe<string>.Null;

                Assert.False(maybe.HasValue);
            }

            [Fact]
            public void NullField_EqualsDefault()
            {
                Assert.Equal(default, MayBe<string>.Null);
            }
        }

        // ────────────────────────────────────────────────────────────────────
        // Propriétés : HasValue / IsEmpty
        // ────────────────────────────────────────────────────────────────────

        public class Properties
        {
            [Fact]
            public void HasValue_IsFalse_WhenEmpty()
            {
                Assert.False(MayBe<int>.Null.HasValue);
            }

            [Fact]
            public void IsEmpty_IsTrue_WhenEmpty()
            {
                Assert.True(MayBe<int>.Null.IsEmpty);
            }

            [Fact]
            public void HasValue_IsTrue_WhenPresent()
            {
                var maybe = MayBe<int>.Some(1);

                Assert.True(maybe.HasValue);
                Assert.False(maybe.IsEmpty);
            }

            [Fact]
            public void HasValueAndIsEmpty_AreComplementary()
            {
                var present = MayBe<string>.Some("x");
                var empty = MayBe<string>.Null;

                Assert.NotEqual(present.HasValue, present.IsEmpty);
                Assert.NotEqual(empty.HasValue, empty.IsEmpty);
            }
        }

        // ────────────────────────────────────────────────────────────────────
        // Factory : Some
        // ────────────────────────────────────────────────────────────────────

        public class SomeFactory
        {
            [Fact]
            public void Some_WithValue_HasValueIsTrue()
            {
                var maybe = MayBe<string>.Some("test");

                Assert.True(maybe.HasValue);
                Assert.Equal("test", maybe.Value);
            }

            [Fact]
            public void Some_WithNull_HasValueIsTrue()
            {
                var maybe = MayBe<string>.Some(null);

                Assert.True(maybe.HasValue);
                Assert.Null(maybe.Value);
            }

            [Fact]
            public void Some_WithValueType_HasValueIsTrue()
            {
                var maybe = MayBe<double>.Some(3.14);

                Assert.True(maybe.HasValue);
                Assert.Equal(3.14, maybe.Value);
            }
        }

        // ────────────────────────────────────────────────────────────────────
        // GetValueOrDefault
        // ────────────────────────────────────────────────────────────────────

        public class GetValueOrDefaultTests
        {
            [Fact]
            public void WhenPresent_ReturnsEncapsulatedValue()
            {
                var maybe = MayBe<string>.Some("hello");

                Assert.Equal("hello", maybe.GetValueOrDefault("fallback"));
            }

            [Fact]
            public void WhenEmpty_ReturnsFallback()
            {
                var maybe = MayBe<string>.Null;

                Assert.Equal("fallback", maybe.GetValueOrDefault("fallback"));
            }

            [Fact]
            public void WhenEmpty_NoArgument_ReturnsTypeDefault()
            {
                var maybe = MayBe<int>.Null;

                Assert.Equal(0, maybe.GetValueOrDefault());
            }

            [Fact]
            public void WhenPresentWithNull_ReturnsNull_NotFallback()
            {
                var maybe = MayBe<string>.Some(null);

                // La valeur est présente (nulle) : on retourne null, pas le fallback
                Assert.Null(maybe.GetValueOrDefault("fallback"));
            }
        }

        // ────────────────────────────────────────────────────────────────────
        // GetValueOrElse
        // ────────────────────────────────────────────────────────────────────

        public class GetValueOrElseTests
        {
            [Fact]
            public void WhenPresent_ReturnsEncapsulatedValue()
            {
                var maybe = MayBe<string>.Some("hello");

                Assert.Equal("hello", maybe.GetValueOrElse(() => "fallback"));
            }

            [Fact]
            public void WhenEmpty_InvokesFallbackAndReturnsResult()
            {
                var maybe = MayBe<string>.Null;
                bool invoked = false;

                string? result = maybe.GetValueOrElse(() =>
                {
                    invoked = true;
                    return "from-fallback";
                });

                Assert.True(invoked);
                Assert.Equal("from-fallback", result);
            }

            [Fact]
            public void WhenPresent_FallbackIsNeverInvoked()
            {
                var maybe = MayBe<string>.Some("value");
                bool invoked = false;

                maybe.GetValueOrElse(() => { invoked = true; return "x"; });

                Assert.False(invoked);
            }

            [Fact]
            public void WhenPresentWithNull_ReturnNull_FallbackNotInvoked()
            {
                var maybe = MayBe<string>.Some(null);
                bool invoked = false;

                string? result = maybe.GetValueOrElse(() => { invoked = true; return "x"; });

                Assert.False(invoked);
                Assert.Null(result);
            }
        }

        // ────────────────────────────────────────────────────────────────────
        // Deconstruct
        // ────────────────────────────────────────────────────────────────────

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
            public void WhenEmpty_DestructuresCorrectly()
            {
                var maybe = MayBe<string>.Null;

                var (hasValue, value) = maybe;

                Assert.False(hasValue);
                Assert.Null(value);
            }

            [Fact]
            public void WhenPresentWithNull_DestructuresCorrectly()
            {
                var maybe = MayBe<string>.Some(null);

                var (hasValue, value) = maybe;

                Assert.True(hasValue);
                Assert.Null(value);
            }
        }

        // ────────────────────────────────────────────────────────────────────
        // Conversions implicites
        // ────────────────────────────────────────────────────────────────────

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
            public void FromNull_CreatesPresent_WithNullValue()
            {
                MayBe<string> maybe = (string?)null;

                Assert.True(maybe.HasValue);
                Assert.Null(maybe.Value);
            }

            [Fact]
            public void ToT_WhenPresent_ReturnsValue()
            {
                var maybe = MayBe<string>.Some("hello");

                string? result = maybe;

                Assert.Equal("hello", result);
            }

            [Fact]
            public void ToT_WhenEmpty_ReturnsDefault()
            {
                var maybe = MayBe<int>.Null;

                int result = maybe;

                Assert.Equal(default, result);
            }

            [Fact]
            public void FromValueType_CreatesPresent()
            {
                MayBe<int> maybe = 99;

                Assert.True(maybe.HasValue);
                Assert.Equal(99, maybe.Value);
            }
        }

        // ────────────────────────────────────────────────────────────────────
        // Égalité (IEquatable, ==, !=, GetHashCode, Equals(object?))
        // ────────────────────────────────────────────────────────────────────

        public class EqualityTests
        {
            // — Deux instances vides —

            [Fact]
            public void TwoEmpty_AreEqual()
            {
                Assert.Equal(MayBe<string>.Null, MayBe<string>.Null);
            }

            [Fact]
            public void TwoEmpty_OperatorEqual_IsTrue()
            {
                Assert.True(MayBe<string>.Null == MayBe<string>.Null);
            }

            [Fact]
            public void TwoEmpty_OperatorNotEqual_IsFalse()
            {
                Assert.False(MayBe<string>.Null != MayBe<string>.Null);
            }

            // — Deux présents avec la même valeur —

            [Fact]
            public void TwoPresent_SameValue_AreEqual()
            {
                var a = MayBe<string>.Some("hello");
                var b = MayBe<string>.Some("hello");

                Assert.Equal(a, b);
            }

            [Fact]
            public void TwoPresent_SameValue_OperatorEqual_IsTrue()
            {
                var a = MayBe<int>.Some(42);
                var b = MayBe<int>.Some(42);

                Assert.True(a == b);
            }

            // — Deux présents avec des valeurs différentes —

            [Fact]
            public void TwoPresent_DifferentValues_AreNotEqual()
            {
                var a = MayBe<string>.Some("hello");
                var b = MayBe<string>.Some("world");

                Assert.NotEqual(a, b);
            }

            [Fact]
            public void TwoPresent_DifferentValues_OperatorNotEqual_IsTrue()
            {
                var a = MayBe<int>.Some(1);
                var b = MayBe<int>.Some(2);

                Assert.True(a != b);
            }

            // — Présent vs vide —

            [Fact]
            public void Present_And_Empty_AreNotEqual()
            {
                var present = MayBe<string>.Some("hello");
                var empty = MayBe<string>.Null;

                Assert.NotEqual(present, empty);
            }

            [Fact]
            public void Present_And_Empty_OperatorNotEqual_IsTrue()
            {
                Assert.True(MayBe<int>.Some(0) != MayBe<int>.Null);
            }

            // — Présent avec valeur nulle —

            [Fact]
            public void TwoPresent_WithNull_AreEqual()
            {
                var a = MayBe<string>.Some(null);
                var b = MayBe<string>.Some(null);

                Assert.Equal(a, b);
            }

            [Fact]
            public void PresentWithNull_And_Empty_AreNotEqual()
            {
                var presentNull = MayBe<string>.Some(null);
                var empty = MayBe<string>.Null;

                Assert.NotEqual(presentNull, empty);
            }

            // — Equals(object?) —

            [Fact]
            public void EqualsObject_WithSameMayBe_IsTrue()
            {
                var a = MayBe<string>.Some("hello");
                object b = MayBe<string>.Some("hello");

                Assert.True(a.Equals(b));
            }

            [Fact]
            public void EqualsObject_WithDifferentType_IsFalse()
            {
                var maybe = MayBe<string>.Some("hello");

                Assert.False(maybe.Equals((object)"hello"));
            }

            [Fact]
            public void EqualsObject_WithNull_IsFalse()
            {
                var maybe = MayBe<string>.Some("hello");

                Assert.False(maybe.Equals(null));
            }

            // — GetHashCode —

            [Fact]
            public void GetHashCode_SameInstances_ReturnSameHash()
            {
                var a = MayBe<string>.Some("hello");
                var b = MayBe<string>.Some("hello");

                Assert.Equal(a.GetHashCode(), b.GetHashCode());
            }

            [Fact]
            public void GetHashCode_TwoEmpty_ReturnSameHash()
            {
                Assert.Equal(MayBe<string>.Null.GetHashCode(), MayBe<string>.Null.GetHashCode());
            }

            [Fact]
            public void GetHashCode_PresentVsEmpty_DifferentHash()
            {
                var present = MayBe<string>.Some("hello");
                var empty = MayBe<string>.Null;

                // Pas une garantie absolue, mais très attendu ici
                Assert.NotEqual(present.GetHashCode(), empty.GetHashCode());
            }
        }

        // ────────────────────────────────────────────────────────────────────
        // ToString
        // ────────────────────────────────────────────────────────────────────

        public class ToStringTests
        {
            [Fact]
            public void WhenPresent_ReturnsValueToString()
            {
                var maybe = MayBe<int>.Some(42);

                Assert.Equal("42", maybe.ToString());
            }

            [Fact]
            public void WhenPresentString_ReturnsValue()
            {
                var maybe = MayBe<string>.Some("hello");

                Assert.Equal("hello", maybe.ToString());
            }

            [Fact]
            public void WhenPresentWithNull_ReturnsLiteralNull()
            {
                var maybe = MayBe<string>.Some(null);

                Assert.Equal("null", maybe.ToString());
            }

            [Fact]
            public void WhenEmpty_ReturnsLiteralNull()
            {
                var maybe = MayBe<string>.Null;

                Assert.Equal("null", maybe.ToString());
            }
        }

        // ────────────────────────────────────────────────────────────────────
        // Scénarios combinés / types variés
        // ────────────────────────────────────────────────────────────────────

        public class Integration
        {
            [Fact]
            public void ValueType_Int_FullLifecycle()
            {
                MayBe<int> maybe = 7;

                Assert.True(maybe.HasValue);
                Assert.Equal(7, maybe.Value);
                Assert.Equal(7, maybe.GetValueOrDefault(-1));

                int raw = maybe;
                Assert.Equal(7, raw);
            }

            [Fact]
            public void NullableValueType_Struct()
            {
                var maybe = MayBe<DateTime>.Some(new DateTime(2024, 1, 1));

                Assert.True(maybe.HasValue);
                Assert.Equal(new DateTime(2024, 1, 1), maybe.Value);
            }

            [Fact]
            public void EmptyMaybe_ChainedFallback_ReturnsDefault()
            {
                var result = MayBe<string>.Null
                    .GetValueOrElse(() => MayBe<string>.Some("computed").GetValueOrDefault());

                Assert.Equal("computed", result);
            }

            [Fact]
            public void ImplicitConversion_ThenDeconstruct()
            {
                MayBe<string> maybe = "world";

                var (hasValue, value) = maybe;

                Assert.True(hasValue);
                Assert.Equal("world", value);
            }

            [Fact]
            public void SomeVsConstructor_BehaveIdentically()
            {
                var viaConstructor = new MayBe<string>("same");
                var viaSome = MayBe<string>.Some("same");

                Assert.Equal(viaConstructor, viaSome);
            }
        }
    }
}