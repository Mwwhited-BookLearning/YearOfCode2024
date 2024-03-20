package models

type SearchResultWithSummaryModel struct {
	Score    float32
	PathHash string
	File     string
	Content  string
	Type     SearchTypes

	Summary string
}
