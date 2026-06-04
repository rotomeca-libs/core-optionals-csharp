# Rotomeca.Core.Optionals — Librairie

Ce projet contient l'implémentation du type optionnel `MayBe<T>` pour C#.

---

## Structure du projet

```
src/Rotomeca.Core.Optionals/
├── classes/
│   └── MayBe{T}.cs       # Implémentation principale
├── assets/
│   └── icon.png           # Icône NuGet
├── Polyfills.cs            # Compatibilité netstandard2.0 / pré-net5
└── Rotomeca.Core.Optionals.csproj
```

---

## `MayBe<T>` — Référence complète

`MayBe<T>` est un `readonly struct` générique implémentant `IEquatable<MayBe<T>>`.

### Règle fondamentale

> `null` est toujours traité comme une valeur **absente**. `HasValue` sera `false`.

Ce comportement est uniforme quelle que soit la façon de créer l'instance : constructeur public, `Some`, ou conversion implicite depuis `T?`.

---

### Factories

```csharp
// Présent (uniquement si value != null)
MayBe<string> a = MayBe<string>.Some("hello");   // HasValue = true
MayBe<string> b = MayBe<string>.Some(null);      // HasValue = false

// Absent
MayBe<string> c = MayBe<string>.Null;            // statique
MayBe<string> d = default;                       // équivalent
```

---

### Propriétés

```csharp
bool HasValue  // true si une valeur non nulle est présente
bool IsEmpty   // true si absent — équivalent de !HasValue
T?   Value     // valeur encapsulée ; default si absent
```

---

### Extraction

```csharp
// Fallback statique — retourne defaultValue si absent
string result = maybe.GetValueOrDefault("fallback");

// Fallback lazy — le délégué n'est invoqué que si absent
string result = maybe.GetValueOrElse(() => ComputeFallback());

// Déconstruction
var (hasValue, value) = maybe;
```

---

### Conversions implicites

```csharp
// T? → MayBe<T>  (null → absent)
MayBe<string> a = "hello";          // HasValue = true
MayBe<string> b = (string?)null;    // HasValue = false

// MayBe<T> → T?  (extrait la valeur, default si absent)
string? s = MayBe<string>.Some("hi"); // → "hi"
int     i = MayBe<int>.Null;          // → 0
```

---

### Égalité

```csharp
// Deux absents sont toujours égaux
MayBe<string>.Null == MayBe<string>.Null           // true

// Deux présents égaux si leurs valeurs le sont (EqualityComparer<T>.Default)
MayBe<string>.Some("a") == MayBe<string>.Some("a") // true
MayBe<string>.Some("a") != MayBe<string>.Some("b") // true

// Présent ≠ Absent
MayBe<string>.Some("x") != MayBe<string>.Null      // true
```

---

### `ToString`

```csharp
MayBe<int>.Some(42).ToString()      // → "42"
MayBe<string>.Some("hi").ToString() // → "hi"
MayBe<string>.Null.ToString()       // → "null"
```

---

## Cibles et compatibilité

| Cible          | Support             | Notes                               |
| -------------- | ------------------- | ----------------------------------- |
| netstandard2.0 | ✅                  | `GetHashCode` via algorithme manuel (Polyfills.cs) |
| netstandard2.1 | ✅                  |                                     |
| net8.0         | ✅                  | `HashCode.Combine` natif            |
| net9.0         | ✅                  |                                     |
| net10.0        | ✅ (si SDK ≥ 10.0) | Ajout conditionnel au build         |

`Polyfills.cs` expose `IsExternalInit` pour les cibles pré-.NET 5 qui ne le fournissent pas nativement.

---

## Construire le projet

```bash
dotnet build src/Rotomeca.Core.Optionals/Rotomeca.Core.Optionals.csproj
```

Pour produire le package NuGet :

```bash
dotnet pack src/Rotomeca.Core.Optionals/Rotomeca.Core.Optionals.csproj \
    --configuration Release \
    --output ./artifacts
```

---

## Licence

[ISC](../../LICENSE) © Rotomeca