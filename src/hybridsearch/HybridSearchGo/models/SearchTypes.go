package models

type SearchTypes int

const (
	SearchTypeNone     SearchTypes = 0
	SearchTypeSemantic SearchTypes = 1
	SearchTypeLexical  SearchTypes = 2
	SearchTypeHybrid   SearchTypes = 3
)
