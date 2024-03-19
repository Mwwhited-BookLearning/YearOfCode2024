package models

type SearchResultWithSummaryModel struct {
}
type SearchResultModel struct {
	Score    float32
	PathHash string
	File     string
	Content  string
	Type     SearchTypes
}

type SearchTypes int
