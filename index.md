---
_layout: landing
---

<div align="center">
<img height="90" alt="rotomeca-lib-ts" src="https://raw.githubusercontent.com/rotomeca-libs/projects-pictures/494cadd82673710c227e454c08cabc41d089e0f8/rotomeca-lib-csharp.svg" />

# Rotomeca.Core.Optionals

[![NuGet version](https://img.shields.io/nuget/v/Rotomeca.Core.Optionals)](https://www.nuget.org/packages/Rotomeca.Core.Optionals)
[![CI](https://github.com/rotomeca-libs/core-optionals-csharp/actions/workflows/ci.yml/badge.svg)](https://github.com/rotomeca-libs/core-optionals-csharp/actions)
[![License: ISC](https://img.shields.io/badge/License-ISC-blue.svg)](https://opensource.org/licenses/ISC)
[![.NET](https://img.shields.io/badge/.NET-netstandard2.0%20%7C%20netstandard2.1%20%7C%20net8%20%7C%20net9%20%7C%20net10-512BD4)](https://dotnet.microsoft.com)
[![Docs](https://img.shields.io/badge/docs-github.io-blue)](https://rotomeca-libs.github.io/core-optionals-csharp/index.html)

Type optionnel `MayBe<T>` pour C# qui représente une valeur qui peut être **présente** ou **absente**, uniformément pour tous les types, y compris dans les contextes génériques sans contrainte où `T?` ne permet pas de tester l'absence.

Une valeur `null` est toujours traitée comme **absente** : `MayBe<T>` se comporte comme `Nullable<T>`, mais pour n'importe quel `T`.

Conçu pour s'aligner avec son équivalent TypeScript `MayBe<T>` dans [`@rotomeca/utils`](https://www.npmjs.com/package/@rotomeca/utils).

</div>

## Installation

```bash
dotnet add package Rotomeca.Core.Optionals
```

---

## Compatibilité

| Environnement | Support                                          |
| ------------- | ------------------------------------------------ |
| .NET Standard | 2.0, 2.1                                         |
| .NET          | 8.0, 9.0 (10.0 si SDK disponible)               |
| Source Link   | ✅ (navigation vers le code depuis le débogueur) |
| Symbols       | ✅ (`.snupkg`)                                   |
| Nullable      | ✅ (activé)                                      |

---

## Pourquoi `MayBe<T>` ?

Dans un contexte générique sans contrainte, il est impossible de tester l'absence de valeur via `null` :

```csharp
// ❌ Ne compile pas sans contrainte where T : class
T? Find<T>(int id) { ... }
if (result == null) { ... }

// ✅ Sémantique claire, fonctionne pour tout T
MayBe<T> Find<T>(int id) { ... }
if (result.IsEmpty) { ... }
```

---

## Démarrage rapide

```csharp
using Rotomeca.Core.Optionals;

// Créer une instance présente
MayBe<string> some  = MayBe<string>.Some("hello"); // HasValue = true
MayBe<string> fromT = "hello";                      // conversion implicite

// null → toujours absent
MayBe<string> empty = MayBe<string>.Some(null);    // HasValue = false
MayBe<string> empty = (string?)null;               // HasValue = false
MayBe<string> empty = MayBe<string>.Null;          // HasValue = false

// Vérifier la présence
if (some.HasValue)
    Console.WriteLine(some.Value); // → hello

// Extraire avec fallback
string result = empty.GetValueOrDefault("fallback"); // → fallback
string lazy   = empty.GetValueOrElse(() => Compute()); // évaluation différée

// Déconstruction
var (hasValue, value) = some;
```

---

## Référence de l'API publique

```csharp
// Factories
MayBe<T>.Some(value)   // présent si value != null, absent sinon
MayBe<T>.Null          // absent (= default)

// Propriétés
.HasValue              // true si une valeur non nulle est présente
.IsEmpty               // true si absent (!HasValue)
.Value                 // valeur encapsulée (default si absent)

// Extraction
.GetValueOrDefault(defaultValue?)   // valeur ou fallback statique
.GetValueOrElse(Func<T?> fallback)  // valeur ou résultat du délégué (lazy)
.Deconstruct(out bool, out T?)      // var (has, val) = maybe;

// Conversions implicites
(MayBe<T>) ← T?      // null → absent, sinon présent
(T?)        ← MayBe<T> // extrait la valeur (default si absent)

// Égalité
IEquatable<MayBe<T>>  // ==, !=, Equals, GetHashCode
```

---

## Structure du dépôt

```
Rotomeca.Core.Optionals/
├── src/
│   └── Rotomeca.Core.Optionals/   # Librairie principale (MayBe<T>)
└── tests/
    └── Rotomeca.Core.Tests/       # Suite de tests xUnit
```

---

## Ecosystème Rotomeca

>  Voir la liste complète sur [rotomeca-libs](https://github.com/rotomeca-libs) et les packages C# sur [github](https://github.com/orgs/rotomeca-libs/repositories?q=csharp) ou sur [nuget](https://www.nuget.org/packages?q=Rotomeca&includeComputedFrameworks=true&prerel=true&sortby=relevance)

---

## Contribuer

```bash
git clone https://github.com/rotomeca-libs/core-optionals-csharp.git
cd core-optionals-csharp
dotnet restore
dotnet test
```

Les contributions sont les bienvenues via Pull Request sur la branche `dev`.

---

## Note sur l'utilisation de l'IA

L'intégralité du code de ce projet a d'abord été écrite à la main en essayant d'avoir le C# le plus propre possible. L'IA a ensuite été utilisée pour :

- **Proposer des axes d'amélioration et de refactorisation** si besoin, après relecture de ses modifications par mes soins
- **La documentation et les README** -> j'ai toujours été une bille en documentation, je trouve celle de l'IA lisible et explicite ; elle a toujours été relue et validée par mes soins
- **Les tests unitaires** -> tester, c'est facile, mais présenter des tests unitaires, c'est complexe (de mon point de vue) ; l'IA a dans un premier temps généré les tests, je les ai parcourus pour les comprendre et les corriger au besoin
- **La CI/CD** -> vu que ce n'est pas mon domaine, mais ça permet d'apprendre beaucoup 👍

Sa principale contribution a donc été de m'accompagner sur les points qui me sont lacunaires.

---

## Licence

[ISC](LICENSE) © Rotomeca