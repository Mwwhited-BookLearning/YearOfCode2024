package lexical

import (
	opensearch "github.com/opensearch-project/opensearch-go"
	"hybrid-search/webapi/models"
)

type LexicalSearchProviderInstance struct {
	Client opensearch.Client
}

func Create(client opensearch.Client) LexicalSearchProviderInstance {
	return LexicalSearchProviderInstance{
		Client: client,
	}
}

func (provider LexicalSearchProviderInstance) Search(query string, limit int) []models.SearchResultWithSummaryModel {
	return nil //TODO: complete this
}
