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

