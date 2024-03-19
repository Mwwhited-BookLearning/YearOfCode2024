package lexical

import (
	"hybrid-search/webapi/models"
)

type LexicalSearchProviderInstance struct {
}

func Create() LexicalSearchProviderInstance {
	return LexicalSearchProviderInstance{}
}

func (provider LexicalSearchProviderInstance) Search(query string, limit int) []models.SearchResultWithSummaryModel {
	return nil //TODO: complete this
}
