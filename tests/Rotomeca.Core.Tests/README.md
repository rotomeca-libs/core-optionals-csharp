# Rotomeca.Core.Tests — Suite de tests

Suite de tests xUnit pour `Rotomeca.Core.Optionals`. Couvre l'intégralité du comportement observable de `MayBe<T>` : construction, factories, propriétés, extraction, conversions implicites, égalité, affichage, et scénarios d'intégration.

---

## Prérequis

- [.NET 9 SDK](https://dotnet.microsoft.com/download) minimum (net10.0 si SDK disponible)
- Aucune dépendance externe hors framework de test

---

## Lancer les tests

Depuis la racine du dépôt :

```bash
dotnet test tests/Rotomeca.Core.Tests/Rotomeca.Core.Tests.csproj
```

Avec rapport de couverture (via coverlet) :

```bash
dotnet test tests/Rotomeca.Core.Tests/Rotomeca.Core.Tests.csproj \
    --collect:"XPlat Code Coverage"
```

---

## Dépendances de test

| Package                     | Version | Rôle                                 |
| --------------------------- | ------- | ------------------------------------ |
| `xunit`                     | 2.9.2   | Framework de tests                   |
| `xunit.runner.visualstudio` | 2.8.2   | Intégration IDE (VS, Rider, VS Code) |
| `Microsoft.NET.Test.Sdk`    | 17.12.0 | Hôte d'exécution des tests           |
| `coverlet.collector`        | 6.0.2   | Couverture de code                   |

---

## Structure des tests

Les tests sont organisés en classes imbriquées dans `MayBeTests`, une classe par domaine fonctionnel :

```
MayBeTests
├── Constructor            — new MayBe<T>(value) : présent si value != null, absent sinon
├── DefaultAndNull         — default et MayBe<T>.Null
├── Properties             — HasValue / IsEmpty et leur complémentarité
├── SomeFactory            — MayBe<T>.Some(value)
├── GetValueOrDefaultTests — retour de la valeur encapsulée ou du fallback statique
├── GetValueOrElseTests    — fallback lazy : invocation conditionnelle du délégué
├── DeconstructTests       — var (hasValue, value) = maybe;
├── ImplicitConversions    — T? → MayBe<T> et MayBe<T> → T?
├── EqualityTests          — ==, !=, Equals(MayBe<T>), Equals(object?), GetHashCode
├── ToStringTests          — représentation textuelle des différents états
└── Integration            — scénarios combinés sur types variés (int, string, DateTime…)
```

---

## ⚠️ Tests à mettre à jour

La sémantique de `null` a évolué : **`null` est désormais toujours absent** (`HasValue = false`).
Les tests suivants ont été écrits pour l'ancienne sémantique ("présent mais nul") et doivent être **supprimés ou réécrits** :

| Classe                  | Méthode                                   |
| ----------------------- | ----------------------------------------- |
| `Constructor`           | `WithNullReference_HasValueIsTrue`        |
| `Constructor`           | `WithNullReference_ValueIsNull`           |
| `SomeFactory`           | `Some_WithNull_HasValueIsTrue`            |
| `GetValueOrDefaultTests`| `WhenPresentWithNull_ReturnsNull_NotFallback` |
| `GetValueOrElseTests`   | `WhenPresentWithNull_ReturnNull_FallbackNotInvoked` |
| `DeconstructTests`      | `WhenPresentWithNull_DestructuresCorrectly` |
| `ImplicitConversions`   | `FromNull_CreatesPresent_WithNullValue`   |
| `EqualityTests`         | `TwoPresent_WithNull_AreEqual`            |
| `EqualityTests`         | `PresentWithNull_And_Empty_AreNotEqual`   |
| `ToStringTests`         | `WhenPresentWithNull_ReturnsLiteralNull`  |

Ces cas sont à remplacer par un seul test documentant le nouveau comportement :

```csharp
[Fact]
public void WithNull_TreatedAsAbsent()
{
    MayBe<string> viaConstructor = new(null);
    MayBe<string> viaSome        = MayBe<string>.Some(null);
    MayBe<string> viaImplicit    = (string?)null;

    Assert.False(viaConstructor.HasValue);
    Assert.False(viaSome.HasValue);
    Assert.False(viaImplicit.HasValue);
    Assert.Equal(MayBe<string>.Null, viaConstructor);
}
```

---

## Ajouter un test

Les tests suivent la convention `Contexte_RésultatAttendu` :

```csharp
public class MaNouvelleSectionTests
{
    [Fact]
    public void WhenXxx_DoesYyy()
    {
        // Arrange
        var maybe = MayBe<string>.Some("valeur");

        // Act
        var result = maybe.GetValueOrDefault("fallback");

        // Assert
        Assert.Equal("valeur", result);
    }
}
```

Placer la nouvelle classe dans `maybe.tests.cs` ou dans un fichier dédié au même namespace `Rotomeca.Core.Tests.Optionals`.

---

## Licence

[ISC](../../LICENSE) © Rotomeca