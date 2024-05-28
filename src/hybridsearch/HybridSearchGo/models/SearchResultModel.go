package models

type SearchResultModel struct {
	Score    float32
	PathHash string
	File     string
	Content  string
	Type     SearchTypes
}
