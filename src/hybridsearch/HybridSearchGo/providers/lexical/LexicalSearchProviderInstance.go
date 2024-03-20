package lexical

import (
	"hybrid-search/webapi/models"
)

type LexicalSearchProviderInstance struct {
	Options OpenSearchOptions
}

func Create(options OpenSearchOptions) LexicalSearchProviderInstance {
	return LexicalSearchProviderInstance{
		Options: options,
	}
}

func (provider LexicalSearchProviderInstance) Search(query string, limit int) []models.SearchResultWithSummaryModel {
	return nil //TODO: complete this
}
