using System;

namespace OobDev.Search.Models;

[Flags]
public enum SearchTypes
{
    None,
    Semantic = 1,
    Lexical = 2,
    Hybrid = 3,
}
